using System.Collections.Generic;
using UnityEngine;
using GuildsOfArcanaTerra.Combat;
using System.Linq; // Added for .Select() and .Where()

public class DungeonRunManager : MonoBehaviour
{
    [Header("Dungeon Setup")]
    public DungeonDefinition dungeonDefinition;

    private Transform[] enemySpawnPoints = new Transform[5];
    public Transform[] playerSpawnPoints = new Transform[5];

    [Header("Visuals & Audio")]
    public UnityEngine.UI.Image backgroundImage; // Assign in Inspector
    public UnityEngine.AudioSource musicSource;  // Assign in Inspector
    private Sprite currentBackground;
    private AudioClip currentMusic;

    // New fields for SpriteRenderer and AudioSource
    public SpriteRenderer dungeonBackgroundRenderer; // Assign in Inspector
    public AudioSource dungeonMusicSource; // Assign in Inspector

    [Header("Player Party")]
    public List<GuildsOfArcanaTerra.Combat.Combatant> playerPartyCombatants = new List<GuildsOfArcanaTerra.Combat.Combatant>(); // Assign or populate at dungeon start

    [Header("Runtime State")]
    private int currentEncounterIndex = 0;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    public List<GameObject> SpawnedEnemies => spawnedEnemies;

    [Header("Results Screen")]
    public ResultsScreenUI resultsScreenUI; // Assign in Inspector

    public GuildsOfArcanaTerra.Combat.TurnOrderSystem turnOrderSystem; // Assign in Inspector

    void Awake()
    {
        // Auto-find enemy spawn points by name
        for (int i = 0; i < 5; i++)
        {
            var go = GameObject.Find($"EnemySpawn_{i}");
            if (go != null)
            {
                enemySpawnPoints[i] = go.transform;
                Debug.Log($"[DungeonRunManager] Found EnemySpawn_{i} at {go.transform.position}");
            }
            else
            {
                Debug.LogWarning($"[DungeonRunManager] EnemySpawn_{i} not found in scene!");
            }
        }
    }

    void Start()
    {
        // If not already assigned, get from SceneFlowManager
        if (dungeonDefinition == null && SceneFlowManager.Instance != null)
            dungeonDefinition = SceneFlowManager.Instance.currentDungeon;

        // Spawn player party before starting the dungeon
        SpawnPlayerParty();

        if (dungeonDefinition != null)
        {
            StartDungeon();
        }
        else
        {
            Debug.LogWarning("DungeonRunManager: No DungeonDefinition assigned!");
        }

        // Start combat after both parties are spawned
        Invoke(nameof(StartCombatIfReady), 0.1f); // Small delay to ensure all objects are initialized
    }

    private void StartCombatIfReady()
    {
        if (turnOrderSystem == null)
        {
            Debug.LogWarning("[DungeonRunManager] TurnOrderSystem not assigned!");
            return;
        }
        var allCombatants = new List<GuildsOfArcanaTerra.Combat.ICombatant>();
        allCombatants.AddRange(playerPartyCombatants);
        allCombatants.AddRange(spawnedEnemies.Select(go => go.GetComponent<GuildsOfArcanaTerra.Combat.ICombatant>()).Where(c => c != null));
        Debug.Log($"[DungeonRunManager] Starting combat with {allCombatants.Count} combatants.");
        turnOrderSystem.StartCombat(allCombatants);
    }

    public void StartDungeon()
    {
        currentEncounterIndex = 0;
        SpawnCurrentEncounter();
    }

    public void SpawnCurrentEncounter()
    {
        Debug.Log($"[DungeonRunManager] Spawning encounter {currentEncounterIndex}...");
        ClearEnemies();
        if (dungeonDefinition == null || dungeonDefinition.Encounters.Count <= currentEncounterIndex)
        {
            Debug.LogWarning("[DungeonRunManager] No more encounters or DungeonDefinition missing.");
            return;
        }
        var encounter = dungeonDefinition.Encounters[currentEncounterIndex];
        Debug.Log($"[DungeonRunManager] Encounter: {encounter.EncounterName}, Enemies: {encounter.Enemies.Count}");

        // Set background and music (SpriteRenderer/AudioSource)
        Sprite bg = encounter.EncounterBackground != null ? encounter.EncounterBackground : dungeonDefinition.DefaultBackground;
        AudioClip music = encounter.EncounterMusic != null ? encounter.EncounterMusic : dungeonDefinition.DefaultMusic;
        if (dungeonBackgroundRenderer != null && bg != dungeonBackgroundRenderer.sprite)
        {
            Debug.Log($"[DungeonRunManager] Setting background sprite: {bg?.name ?? "NULL"}");
            dungeonBackgroundRenderer.sprite = bg;
        }
        if (dungeonMusicSource != null && music != dungeonMusicSource.clip)
        {
            Debug.Log($"[DungeonRunManager] Setting music: {music?.name ?? "NULL"}");
            dungeonMusicSource.clip = music;
            dungeonMusicSource.Play();
        }

        // (Keep previous UI.Image/AudioSource logic for compatibility)
        if (backgroundImage != null && bg != currentBackground)
        {
            backgroundImage.sprite = bg;
            currentBackground = bg;
        }
        if (musicSource != null && music != currentMusic)
        {
            musicSource.clip = music;
            musicSource.Play();
            currentMusic = music;
        }

        for (int i = 0; i < encounter.Enemies.Count; i++)
        {
            var enemyDef = encounter.Enemies[i];
            int slot = Mathf.Clamp(enemyDef.spawnSlotIndex, 0, enemySpawnPoints.Length - 1);
            Transform spawnPoint = enemySpawnPoints[slot];
            if (enemyDef.Prefab == null)
            {
                Debug.LogWarning($"[DungeonRunManager] Enemy prefab missing for enemy {i} in encounter {encounter.EncounterName}");
                continue;
            }
            if (spawnPoint == null)
            {
                Debug.LogWarning($"[DungeonRunManager] Spawn point {slot} missing for enemy {i} in encounter {encounter.EncounterName}");
                continue;
            }
            Debug.Log($"[DungeonRunManager] Spawning enemy '{enemyDef.Prefab.name}' at slot {slot} ({spawnPoint.position})");
            GameObject enemyGO = Instantiate(enemyDef.Prefab, spawnPoint.position, Quaternion.identity);
            var combatant = enemyGO.GetComponent<EnemyCombatant>();
            if (combatant != null && enemyDef.characterData != null)
            {
                combatant.characterData = enemyDef.characterData;
                // You may need to call a method to apply the data:
                // combatant.LoadCharacterData();
            }
            spawnedEnemies.Add(enemyGO);
        }
        Debug.Log($"[DungeonRunManager] Spawned encounter: {encounter.EncounterName}");
        Debug.Log($"[DungeonRunManager] SpawnedEnemies count after spawn: {spawnedEnemies.Count}");
        foreach (var enemy in spawnedEnemies)
        {
            Debug.Log($"[DungeonRunManager] Spawned enemy: {enemy?.name}");
        }
    }

    public void OnPlayerVictory()
    {
        // Check if at least one player is alive
        bool anyPlayerAlive = playerPartyCombatants.Exists(c => c != null && c.IsAlive);
        if (!anyPlayerAlive)
        {
            ShowResultsScreen(false);
            return;
        }
        currentEncounterIndex++;
        if (currentEncounterIndex < dungeonDefinition.Encounters.Count)
        {
            SpawnCurrentEncounter();
        }
        else
        {
            ShowResultsScreen(true);
        }
    }

    public void OnPlayerDefeat()
    {
        // If all player characters are dead, defeat
        bool allPlayersDead = playerPartyCombatants.TrueForAll(c => c == null || !c.IsAlive);
        if (allPlayersDead)
        {
            ShowResultsScreen(false);
        }
    }

    private void ShowResultsScreen(bool victory)
    {
        Debug.Log(victory ? "Dungeon Complete!" : "Dungeon Failed!");
        if (resultsScreenUI != null)
        {
            if (victory)
                resultsScreenUI.ShowVictory();
            else
                resultsScreenUI.ShowDefeat();
        }
    }

    private void ClearEnemies()
    {
        Debug.Log($"[DungeonRunManager] Clearing {spawnedEnemies.Count} enemies...");
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null) Destroy(enemy);
        }
        spawnedEnemies.Clear();
    }

    public void SpawnPlayerParty()
    {
        playerPartyCombatants.Clear();
        var party = SceneFlowManager.Instance?.selectedParty;
        if (party == null)
        {
            Debug.LogWarning("[DungeonRunManager] No selected party found in SceneFlowManager.");
            return;
        }
        for (int i = 0; i < party.Count && i < playerSpawnPoints.Length; i++)
        {
            var charDef = party[i];
            if (charDef == null || charDef.prefab == null)
            {
                Debug.LogWarning($"[DungeonRunManager] CharacterDefinition or prefab missing for party slot {i}.");
                continue;
            }
            var spawnPoint = playerSpawnPoints[i];
            if (spawnPoint == null)
            {
                Debug.LogWarning($"[DungeonRunManager] PlayerSpawn_{i} not found or not assigned.");
                continue;
            }
            var playerGO = Instantiate(charDef.prefab, spawnPoint.position, Quaternion.identity);
            var combatant = playerGO.GetComponent<GuildsOfArcanaTerra.Combat.Combatant>();
            if (combatant != null)
            {
                combatant.characterData = charDef;
                // Set classDefinition from characterData.characterClass using reflection
                if (charDef.characterClass != null)
                {
                    var classDefField = typeof(GuildsOfArcanaTerra.Combat.Combatant)
                        .GetField("classDefinition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (classDefField != null)
                    {
                        classDefField.SetValue(combatant, charDef.characterClass);
                    }
                    combatant.LoadClassDefinition();
                }
                playerPartyCombatants.Add(combatant);
                Debug.Log($"[DungeonRunManager] Spawned player '{charDef.characterName}' at PlayerSpawn_{i} ({spawnPoint.position})");
            }
            else
            {
                Debug.LogWarning($"[DungeonRunManager] No Combatant component found on player prefab for '{charDef.characterName}'.");
            }
        }
        Debug.Log($"[DungeonRunManager] Spawned {playerPartyCombatants.Count} player party members.");
    }

    // Optionally, add a helper to check defeat after any player death
    public void CheckForPartyDefeat()
    {
        bool allPlayersDead = playerPartyCombatants.TrueForAll(c => c == null || !c.IsAlive);
        if (allPlayersDead)
        {
            ShowResultsScreen(false);
        }
    }
} 