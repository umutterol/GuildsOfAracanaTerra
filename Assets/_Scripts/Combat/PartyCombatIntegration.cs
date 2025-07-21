using System.Collections.Generic;
using UnityEngine;
using GuildsOfArcanaTerra.Characters;
using GuildsOfArcanaTerra.Combat;
using GuildsOfArcanaTerra.Combat.UI;
using System.Reflection;

namespace GuildsOfArcanaTerra.Combat
{
    /// <summary>
    /// Integrates party management with the combat system
    /// Handles the transition from party setup to combat
    /// </summary>
    public class PartyCombatIntegration : MonoBehaviour
    {
        [Header("System References")]
        [SerializeField] private PartyManager partyManager;
        [SerializeField] private CombatUIManager combatUIManager;
        [SerializeField] private MonoBehaviour turnOrderSystem;
        [SerializeField] private MonoBehaviour statusEffectSystem;
        
        [Header("Combat Settings")]
        [SerializeField] private int maxEnemies = 3;
        [SerializeField] private bool autoGenerateEnemies = true;
        [SerializeField] private List<EnemyCombatant> enemyPrefabs = new List<EnemyCombatant>();
        
        [Header("UI References")]
        [SerializeField] private GameObject partyManagementUI;
        [SerializeField] private GameObject combatUI;
        [SerializeField] private PartyCombatUI partyCombatUI;
        
        [Header("Debug")]
        [SerializeField] private bool debugMode = false;
        
        private List<Combatant> playerCombatants = new List<Combatant>();
        private List<Combatant> enemyCombatants = new List<Combatant>();
        private bool isInCombat = false;
        
        private void Start()
        {
            // Find references if not assigned
            if (partyManager == null)
                partyManager = FindObjectOfType<PartyManager>();
            
            if (combatUIManager == null)
                combatUIManager = FindObjectOfType<CombatUIManager>();
            
            if (turnOrderSystem == null)
                turnOrderSystem = FindObjectOfType<MonoBehaviour>();
            
            if (statusEffectSystem == null)
                statusEffectSystem = FindObjectOfType<MonoBehaviour>();
            
            if (partyCombatUI == null)
                partyCombatUI = FindObjectOfType<PartyCombatUI>();
            
            // Ensure combat systems exist
            EnsureCombatSystems();
            
            // Hide combat UI initially
            if (combatUI != null)
                combatUI.SetActive(false);
        }
        
        /// <summary>
        /// Start combat with the current party
        /// </summary>
        [ContextMenu("Start Combat")]
        public void StartCombat()
        {
            if (isInCombat)
            {
                Debug.LogWarning("Combat is already in progress!");
                return;
            }
            
            if (partyManager == null || partyManager.PartySize == 0)
            {
                Debug.LogWarning("No party members available for combat!");
                return;
            }
            
            Debug.Log("=== STARTING COMBAT ===");
            
            // 1. Create player combatants from party
            CreatePlayerCombatants();
            
            // 2. Create enemy combatants
            CreateEnemyCombatants();
            
            // 3. Initialize combat systems
            InitializeCombatSystems();
            
            // 4. Switch UI
            SwitchToCombatUI();
            
            // 5. Start combat
            BeginCombat();
            
            isInCombat = true;
            
            Debug.Log("=== COMBAT STARTED ===");
        }
        
        /// <summary>
        /// End combat and return to party management
        /// </summary>
        [ContextMenu("End Combat")]
        public void EndCombat()
        {
            if (!isInCombat)
            {
                Debug.LogWarning("No combat in progress!");
                return;
            }
            
            Debug.Log("=== ENDING COMBAT ===");
            
            // 1. Stop combat systems
            StopCombatSystems();
            
            // 2. Clean up combatants
            CleanupCombatants();
            
            // 3. Switch back to party management UI
            SwitchToPartyManagementUI();
            
            // 4. Update party with combat results
            UpdatePartyAfterCombat();
            
            isInCombat = false;
            
            Debug.Log("=== COMBAT ENDED ===");
        }
        
        /// <summary>
        /// Create combatants from party members
        /// </summary>
        private void CreatePlayerCombatants()
        {
            playerCombatants.Clear();

            // Use PartyManager's built-in combatant creation
            var combatants = partyManager.CreateCombatants();

            // Get party character data list
            var partyDataList = partyManager.GetPartyDataList();

            for (int i = 0; i < combatants.Count; i++)
            {
                var combatant = combatants[i];
                CharacterDefinition charData = null;
                if (partyDataList != null && i < partyDataList.Count)
                    charData = partyDataList[i];

                // Assign class definition directly from SO
                if (charData != null && charData.characterClass != null)
                {
                    var classDefField = typeof(Combatant).GetField("classDefinition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (classDefField != null)
                    {
                        classDefField.SetValue(combatant, charData.characterClass);
                        combatant.LoadClassDefinition();
                    }
                }
                playerCombatants.Add(combatant);
            }

            if (debugMode)
            {
                Debug.Log($"Created {playerCombatants.Count} player combatants:");
                foreach (var combatant in playerCombatants)
                {
                    Debug.Log($"- {combatant.name} (HP: {combatant.CurrentHealth}/{combatant.MaxHealth})");
                }
            }
        }
        
        /// <summary>
        /// Create enemy combatants
        /// </summary>
        private void CreateEnemyCombatants()
        {
            enemyCombatants.Clear();
            
            if (autoGenerateEnemies)
            {
                GenerateRandomEnemies();
            }
            else
            {
                CreateEnemiesFromPrefabs();
            }
            
            if (debugMode)
            {
                Debug.Log($"Created {enemyCombatants.Count} enemy combatants:");
                foreach (var combatant in enemyCombatants)
                {
                    Debug.Log($"- {combatant.name} (HP: {combatant.CurrentHealth}/{combatant.MaxHealth})");
                }
            }
        }
        
        /// <summary>
        /// Generate random enemies based on party level
        /// </summary>
        private void GenerateRandomEnemies()
        {
            int partyLevel = partyManager.GetTotalPartyLevel();
            int enemyCount = Mathf.Min(maxEnemies, Random.Range(1, 4));

            // Load all available class definitions
            var classDefs = new List<GuildsOfArcanaTerra.ScriptableObjects.Classes.CharacterClassDefinition>();
            classDefs.Add(Resources.Load<GuildsOfArcanaTerra.ScriptableObjects.Classes.CharacterClassDefinition>("Classes/Warrior"));
            classDefs.Add(Resources.Load<GuildsOfArcanaTerra.ScriptableObjects.Classes.CharacterClassDefinition>("Classes/Mage"));
            classDefs.Add(Resources.Load<GuildsOfArcanaTerra.ScriptableObjects.Classes.CharacterClassDefinition>("Classes/Rogue"));
            classDefs.Add(Resources.Load<GuildsOfArcanaTerra.ScriptableObjects.Classes.CharacterClassDefinition>("Classes/Cleric"));
            classDefs.Add(Resources.Load<GuildsOfArcanaTerra.ScriptableObjects.Classes.CharacterClassDefinition>("Classes/Ranger"));

            for (int i = 0; i < enemyCount; i++)
            {
                var enemyGO = new GameObject($"Enemy_{i + 1}");
                var enemy = enemyGO.AddComponent<EnemyCombatant>();

                // Assign a random class definition
                int classIndex = Random.Range(0, classDefs.Count);
                var classDef = classDefs[classIndex];
                string[] classNames = { "Warrior", "Mage", "Rogue", "Cleric", "Ranger" };
                string classAssetPath = $"Classes/{classNames[classIndex]}";
                if (classDef == null)
                {
                    Debug.LogError($"[PartyCombatIntegration] Could not load class definition asset at path: Resources/{classAssetPath}.asset. Please ensure the asset exists and is in a Resources folder.");
                }
                var classDefField = typeof(Combatant).GetField("classDefinition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (classDefField != null && classDef != null)
                {
                    classDefField.SetValue(enemy, classDef);
                }

                // Generate random enemy stats based on party level
                int enemyLevel = Mathf.Max(1, partyLevel / partyManager.PartySize + Random.Range(-2, 3));
                var levelField = typeof(Combatant).GetField("level", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (levelField != null)
                {
                    levelField.SetValue(enemy, enemyLevel);
                }

                // Load stats from class definition
                enemy.LoadClassDefinition();

                // Add basic skills if SkillSet exists
                var skillSetType = System.Type.GetType("GuildsOfArcanaTerra.Combat.Skills.SkillSet");
                if (skillSetType != null)
                {
                    var skillSet = enemyGO.GetComponent(skillSetType);
                    if (skillSet == null)
                    {
                        skillSet = enemyGO.AddComponent(skillSetType);
                    }

                    // Try to add a basic attack skill if available
                    var basicAttackType = System.Type.GetType("GuildsOfArcanaTerra.Combat.Skills.Implementations.BasicAttackSkill");
                    var iBaseSkillType = System.Type.GetType("GuildsOfArcanaTerra.Combat.Skills.Interfaces.IBaseSkill");

                    if (basicAttackType != null && iBaseSkillType != null)
                    {
                        var basicAttack = System.Activator.CreateInstance(basicAttackType);
                        if (basicAttack != null && iBaseSkillType.IsAssignableFrom(basicAttack.GetType()))
                        {
                            var addSkillMethod = skillSet.GetType().GetMethod("AddSkill");
                            if (addSkillMethod != null)
                            {
                                addSkillMethod.Invoke(skillSet, new object[] { basicAttack });
                            }
                        }
                    }
                }

                enemyCombatants.Add(enemy);
            }
        }
        
        /// <summary>
        /// Create enemies from prefabs
        /// </summary>
        private void CreateEnemiesFromPrefabs()
        {
            if (enemyPrefabs.Count == 0)
            {
                Debug.LogWarning("No enemy prefabs available, generating random enemies instead");
                GenerateRandomEnemies();
                return;
            }
            
            int enemyCount = Mathf.Min(maxEnemies, enemyPrefabs.Count);
            
            for (int i = 0; i < enemyCount; i++)
            {
                var enemyPrefab = enemyPrefabs[i % enemyPrefabs.Count];
                var enemyGO = Instantiate(enemyPrefab.gameObject);
                var enemy = enemyGO.GetComponent<EnemyCombatant>();
                
                if (enemy != null)
                {
                    enemyCombatants.Add(enemy);
                }
            }
        }
        
        /// <summary>
        /// Initialize all combat systems
        /// </summary>
        private void InitializeCombatSystems()
        {
            // Initialize turn order system (if available)
            if (turnOrderSystem != null)
            {
                var allCombatants = new List<Combatant>();
                allCombatants.AddRange(playerCombatants);
                allCombatants.AddRange(enemyCombatants);
                
                // Try to call Initialize method if it exists
                var initializeMethod = turnOrderSystem.GetType().GetMethod("Initialize");
                if (initializeMethod != null)
                {
                    initializeMethod.Invoke(turnOrderSystem, new object[] { allCombatants });
                }
                
                if (debugMode)
                {
                    Debug.Log("Turn order initialized:");
                    for (int i = 0; i < allCombatants.Count; i++)
                    {
                        Debug.Log($"{i + 1}. {allCombatants[i].name}");
                    }
                }
            }
            
            // Initialize status effect system (if available)
            if (statusEffectSystem != null)
            {
                var initializeMethod = statusEffectSystem.GetType().GetMethod("Initialize");
                if (initializeMethod != null)
                {
                    initializeMethod.Invoke(statusEffectSystem, null);
                }
            }
            
            // Initialize combat UI
            if (combatUIManager != null)
            {
                // Try to set player combatants using reflection
                var playerCombatantField = combatUIManager.GetType().GetField("playerCombatant", 
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (playerCombatantField != null && playerCombatants.Count > 0)
                {
                    playerCombatantField.SetValue(combatUIManager, playerCombatants[0]);
                }
                
                // Try to set enemy combatants using reflection
                var enemyCombatantField = combatUIManager.GetType().GetField("enemyCombatant", 
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (enemyCombatantField != null && enemyCombatants.Count > 0)
                {
                    enemyCombatantField.SetValue(combatUIManager, enemyCombatants[0]);
                }
                
                // Try to call Initialize method
                var initializeMethod = combatUIManager.GetType().GetMethod("Initialize");
                if (initializeMethod != null)
                {
                    initializeMethod.Invoke(combatUIManager, null);
                }
            }
            
            // Initialize party combat UI if available
            if (partyCombatUI != null)
            {
                // Try to call methods using reflection
                var setPartyMethod = partyCombatUI.GetType().GetMethod("SetPartyCombatants");
                if (setPartyMethod != null)
                {
                    setPartyMethod.Invoke(partyCombatUI, new object[] { playerCombatants });
                }
                
                var setEnemyMethod = partyCombatUI.GetType().GetMethod("SetEnemyCombatants");
                if (setEnemyMethod != null)
                {
                    setEnemyMethod.Invoke(partyCombatUI, new object[] { enemyCombatants });
                }
                
                var initializeMethod = partyCombatUI.GetType().GetMethod("Initialize");
                if (initializeMethod != null)
                {
                    initializeMethod.Invoke(partyCombatUI, null);
                }
            }
        }
        
        /// <summary>
        /// Switch UI to combat mode
        /// </summary>
        private void SwitchToCombatUI()
        {
            if (partyManagementUI != null)
                partyManagementUI.SetActive(false);
            
            if (combatUI != null)
                combatUI.SetActive(true);
        }
        
        /// <summary>
        /// Switch UI back to party management
        /// </summary>
        private void SwitchToPartyManagementUI()
        {
            if (combatUI != null)
                combatUI.SetActive(false);
            
            if (partyManagementUI != null)
                partyManagementUI.SetActive(true);
        }
        
        /// <summary>
        /// Begin the combat sequence
        /// </summary>
        private void BeginCombat()
        {
            if (turnOrderSystem != null)
            {
                var startCombatMethod = turnOrderSystem.GetType().GetMethod("StartCombat");
                if (startCombatMethod != null)
                {
                    startCombatMethod.Invoke(turnOrderSystem, null);
                }
            }
            
            Debug.Log("Combat has begun!");
        }
        
        /// <summary>
        /// Stop all combat systems
        /// </summary>
        private void StopCombatSystems()
        {
            if (turnOrderSystem != null)
            {
                var stopCombatMethod = turnOrderSystem.GetType().GetMethod("StopCombat");
                if (stopCombatMethod != null)
                {
                    stopCombatMethod.Invoke(turnOrderSystem, null);
                }
            }
            
            if (statusEffectSystem != null)
            {
                var clearEffectsMethod = statusEffectSystem.GetType().GetMethod("ClearAllEffects");
                if (clearEffectsMethod != null)
                {
                    clearEffectsMethod.Invoke(statusEffectSystem, null);
                }
            }
        }
        
        /// <summary>
        /// Clean up combatant objects
        /// </summary>
        private void CleanupCombatants()
        {
            // Clean up enemy combatants
            foreach (var enemy in enemyCombatants)
            {
                if (enemy != null)
                {
                    DestroyImmediate(enemy.gameObject);
                }
            }
            enemyCombatants.Clear();
            
            // Clear player combatants (they're managed by PartyManager)
            playerCombatants.Clear();
        }
        
        /// <summary>
        /// Update party with combat results
        /// </summary>
        private void UpdatePartyAfterCombat()
        {
            // This could include:
            // - Experience gain
            // - Level ups
            // - Item drops
            // - Status effects
            // - Death/resurrection
            
            Debug.Log("Party updated after combat");
        }
        
        /// <summary>
        /// Ensure all required combat systems exist
        /// </summary>
        private void EnsureCombatSystems()
        {
            // Create placeholder systems if they don't exist
            if (turnOrderSystem == null)
            {
                var turnOrderGO = new GameObject("TurnOrderSystem");
                turnOrderSystem = turnOrderGO.AddComponent<MonoBehaviour>();
            }
            
            if (statusEffectSystem == null)
            {
                var statusEffectGO = new GameObject("StatusEffectSystem");
                statusEffectSystem = statusEffectGO.AddComponent<MonoBehaviour>();
            }
        }
        
        /// <summary>
        /// Get current combat status
        /// </summary>
        public bool IsInCombat => isInCombat;
        
        /// <summary>
        /// Get player combatants
        /// </summary>
        public List<Combatant> PlayerCombatants => new List<Combatant>(playerCombatants);
        
        /// <summary>
        /// Get enemy combatants
        /// </summary>
        public List<Combatant> EnemyCombatants => new List<Combatant>(enemyCombatants);
        
        /// <summary>
        /// Check if combat is over
        /// </summary>
        public bool IsCombatOver()
        {
            if (!isInCombat) return true;
            
            bool allPlayersDead = playerCombatants.TrueForAll(c => c.CurrentHealth <= 0);
            bool allEnemiesDead = enemyCombatants.TrueForAll(c => c.CurrentHealth <= 0);
            
            return allPlayersDead || allEnemiesDead;
        }
        
        /// <summary>
        /// Get combat result
        /// </summary>
        public string GetCombatResult()
        {
            if (!IsCombatOver()) return "Combat in progress";
            
            bool allPlayersDead = playerCombatants.TrueForAll(c => c.CurrentHealth <= 0);
            bool allEnemiesDead = enemyCombatants.TrueForAll(c => c.CurrentHealth <= 0);
            
            if (allPlayersDead && allEnemiesDead)
                return "Draw";
            else if (allPlayersDead)
                return "Defeat";
            else if (allEnemiesDead)
                return "Victory";
            else
                return "Unknown";
        }
        
        /// <summary>
        /// Auto-end combat when it's over
        /// </summary>
        private void Update()
        {
            if (isInCombat && IsCombatOver())
            {
                Debug.Log($"Combat ended: {GetCombatResult()}");
                EndCombat();
            }
        }
    }
} 