using UnityEngine;
using GuildsOfArcanaTerra.Combat;

public class TestKillAllEnemies : MonoBehaviour
{
    public DungeonRunManager dungeonRunManager;

    void Awake()
    {
        if (dungeonRunManager == null)
            dungeonRunManager = FindObjectOfType<DungeonRunManager>();
    }

    // Call this from the Inspector or another script to kill all enemies
    [ContextMenu("Kill All Enemies")]
    public void KillAllEnemies()
    {
        if (dungeonRunManager == null)
        {
            Debug.LogWarning("[TestKillAllEnemies] DungeonRunManager not found!");
            return;
        }
        Debug.Log($"[TestKillAllEnemies] SpawnedEnemies count at button press: {dungeonRunManager.SpawnedEnemies.Count}");
        foreach (var enemy in dungeonRunManager.SpawnedEnemies)
        {
            Debug.Log($"[TestKillAllEnemies] Enemy in list: {enemy?.name}");
        }
        int killed = 0;
        foreach (var enemyGO in dungeonRunManager.SpawnedEnemies)
        {
            if (enemyGO != null)
            {
                var combatant = enemyGO.GetComponent<EnemyCombatant>();
                if (combatant != null)
                {
                    combatant.TakeDamage(99999); // Overkill
                    killed++;
                }
            }
        }
        Debug.Log($"[TestKillAllEnemies] Killed {killed} enemies in current encounter.");
    }
} 