using System.Collections.Generic;
using UnityEngine;
using GuildsOfArcanaTerra.Traits;

namespace GuildsOfArcanaTerra.Combat
{
    /// <summary>
    /// Sets up a test arena with player and enemy parties, assigns kits/traits, and starts combat
    /// </summary>
    public class TestArenaManager : MonoBehaviour
    {
        [Header("Player Party")] 
        public List<CharacterData> playerPartyData;
        [Header("Enemy Party")] 
        public List<CharacterData> enemyPartyData;
        public List<AIType> enemyAITypes;

        [Header("Managers")]
        public TurnOrderSystem turnOrderSystem;
        public StatusEffectSystem statusEffectSystem;
        public PartyTraitManager partyTraitManager;

        [Header("Debug")]
        public bool autoStart = true;

        private List<Combatant> playerParty = new List<Combatant>();
        private List<EnemyCombatant> enemyParty = new List<EnemyCombatant>();

        private void Start()
        {
            if (autoStart)
                SetupAndStartCombat();
        }

        public void SetupAndStartCombat()
        {
            // 1. Spawn player party
            playerParty.Clear();
            for (int i = 0; i < playerPartyData.Count; i++)
            {
                var data = playerPartyData[i];
                if (data == null || data.classId < 0 || data.traitId < 0)
                {
                    Debug.LogWarning($"Skipping player at index {i}: missing CharacterData, invalid classId, or invalid traitId.");
                    continue;
                }
                var go = new GameObject($"Player_{data.name}");
                var combatant = go.AddComponent<Combatant>();
                combatant.InitializeFromCharacterData(data);
                playerParty.Add(combatant);
            }

            // 2. Spawn enemy party
            enemyParty.Clear();
            int enemyCount = Mathf.Min(enemyPartyData.Count, enemyAITypes.Count);
            for (int i = 0; i < enemyCount; i++)
            {
                var data = enemyPartyData[i];
                if (data == null || data.classId < 0 || data.traitId < 0)
                {
                    Debug.LogWarning($"Skipping enemy at index {i}: missing CharacterData, invalid classId, or invalid traitId.");
                    continue;
                }
                var go = new GameObject($"Enemy_{data.name}");
                var enemy = go.AddComponent<EnemyCombatant>();
                enemy.InitializeFromCharacterData(data);
                enemy.aiType = enemyAITypes[i];
                enemyParty.Add(enemy);
            }

            // 3. Assign party references for AI
            foreach (var enemy in enemyParty)
            {
                enemy.enemyParty = playerParty;
                enemy.allyParty = new List<Combatant>(enemyParty);
            }

            // 4. Assign parties to managers
            if (partyTraitManager != null)
                partyTraitManager.SetPartyMembers(playerParty);

            // 5. Start combat
            var allCombatants = new List<ICombatant>();
            allCombatants.AddRange(playerParty);
            allCombatants.AddRange(enemyParty);
            if (turnOrderSystem != null)
                turnOrderSystem.StartCombat(allCombatants);
        }
    }

    // --- Extension method for initializing from CharacterData ---
    public static class CombatantExtensions
    {
        public static void InitializeFromCharacterData(this Combatant combatant, CharacterData data)
        {
            // Set basic info
            combatant.GetType().GetField("characterName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(combatant, data.name);
            combatant.GetType().GetField("level", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(combatant, data.level);
            
            // Load class definition and trait by ID using the lookup system
            var classDef = CharacterDataManager.Instance.GetClassById(data.classId);
            var trait = CharacterDataManager.Instance.GetTraitById(data.traitId);
            
            if (classDef != null)
            {
                combatant.GetType().GetField("classDefinition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(combatant, classDef);
            }
            
            if (trait != null)
            {
                combatant.GetType().GetField("irlTrait", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(combatant, trait);
            }
            
            // Initialize the combatant with the loaded data
            if (classDef != null)
            {
                combatant.LoadClassDefinition();
            }
            
            if (trait != null)
            {
                trait.ApplyPreCombat(combatant);
            }
        }
    }
} 