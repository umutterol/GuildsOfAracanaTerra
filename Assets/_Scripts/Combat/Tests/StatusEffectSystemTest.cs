using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using GuildsOfArcanaTerra.Combat.Effects;

namespace GuildsOfArcanaTerra.Combat
{
    /// <summary>
    /// Test script for the Status Effect System
    /// Provides UI buttons and debug output to test all functionality
    /// </summary>
    public class StatusEffectSystemTest : MonoBehaviour
    {
        [Header("Test Combatants")]
        [SerializeField] private TestCombatant warrior;
        [SerializeField] private TestCombatant mage;
        [SerializeField] private TestCombatant rogue;
        
        [Header("Systems")]
        [SerializeField] private TurnOrderSystem turnOrderSystem;
        [SerializeField] private StatusEffectSystem statusEffectSystem;
        
        [Header("UI Elements")]
        [SerializeField] private Text statusText;
        [SerializeField] private Button applyBurnButton;
        [SerializeField] private Button applyBleedButton;
        [SerializeField] private Button startCombatButton;
        [SerializeField] private Button endTurnButton;
        [SerializeField] private Button clearEffectsButton;
        [SerializeField] private Button resetHealthButton;
        
        [Header("UI Manager")]
        [SerializeField] private UI.StatusEffectTestUI uiManager;
        
        [Header("Debug")]
        [SerializeField] private bool debugMode = true;
        
        private List<ICombatant> testCombatants = new List<ICombatant>();
        
        private void Start()
        {
            // Find systems if not assigned
            if (turnOrderSystem == null)
                turnOrderSystem = FindObjectOfType<TurnOrderSystem>();
            
            if (statusEffectSystem == null)
                statusEffectSystem = FindObjectOfType<StatusEffectSystem>();
            
            if (uiManager == null)
                uiManager = FindObjectOfType<UI.StatusEffectTestUI>();
            
            // Log system status
            if (debugMode)
            {
                Debug.Log($"StatusEffectSystemTest: Systems found - TurnOrder: {turnOrderSystem != null}, StatusEffect: {statusEffectSystem != null}, UI: {uiManager != null}");
            }
            
            // Setup test combatants
            SetupTestCombatants();
            
            // Setup UI buttons (only if UI manager not found)
            if (uiManager == null)
            {
                SetupUIButtons();
            }
            
            // Subscribe to events
            SubscribeToEvents();
            
            // Initial status update
            UpdateStatusDisplay();
            
            if (debugMode)
            {
                Debug.Log("StatusEffectSystemTest: Test system initialized!");
                Debug.Log($"Found {testCombatants.Count} test combatants");
            }
        }
        
        /// <summary>
        /// Setup test combatants
        /// </summary>
        private void SetupTestCombatants()
        {
            testCombatants.Clear();
            
            // Add combatants if assigned
            if (warrior != null) testCombatants.Add(warrior);
            if (mage != null) testCombatants.Add(mage);
            if (rogue != null) testCombatants.Add(rogue);
            
            // Create default combatants if none assigned
            if (testCombatants.Count == 0)
            {
                CreateDefaultCombatants();
            }
            
            // Register with status effect system
            if (statusEffectSystem != null)
            {
                foreach (var combatant in testCombatants)
                {
                    statusEffectSystem.RegisterCombatant(combatant);
                }
            }
            else
            {
                Debug.LogWarning("StatusEffectSystemTest: No StatusEffectSystem found - combatants will not be registered for status effects");
            }
        }
        
        /// <summary>
        /// Create default test combatants if none are assigned
        /// </summary>
        private void CreateDefaultCombatants()
        {
            // Create Warrior
            var warriorGO = new GameObject("Test Warrior");
            warrior = warriorGO.AddComponent<TestCombatant>();
            warrior.name = "Test Warrior";
            testCombatants.Add(warrior);
            
            // Create Mage
            var mageGO = new GameObject("Test Mage");
            mage = mageGO.AddComponent<TestCombatant>();
            mage.name = "Test Mage";
            testCombatants.Add(mage);
            
            // Create Rogue
            var rogueGO = new GameObject("Test Rogue");
            rogue = rogueGO.AddComponent<TestCombatant>();
            rogue.name = "Test Rogue";
            testCombatants.Add(rogue);
            
            if (debugMode)
            {
                Debug.Log("StatusEffectSystemTest: Created default test combatants");
            }
        }
        
        /// <summary>
        /// Setup UI button listeners
        /// </summary>
        private void SetupUIButtons()
        {
            if (applyBurnButton != null)
                applyBurnButton.onClick.AddListener(TestApplyBurn);
            
            if (applyBleedButton != null)
                applyBleedButton.onClick.AddListener(TestApplyBleed);
            
            if (startCombatButton != null)
                startCombatButton.onClick.AddListener(TestStartCombat);
            
            if (endTurnButton != null)
                endTurnButton.onClick.AddListener(TestEndTurn);
            
            if (clearEffectsButton != null)
                clearEffectsButton.onClick.AddListener(TestClearAllEffects);
            
            if (resetHealthButton != null)
                resetHealthButton.onClick.AddListener(TestResetHealth);
        }
        
        /// <summary>
        /// Subscribe to system events
        /// </summary>
        private void SubscribeToEvents()
        {
            if (statusEffectSystem != null)
            {
                statusEffectSystem.OnEffectApplied.AddListener(OnEffectApplied);
                statusEffectSystem.OnEffectRemoved.AddListener(OnEffectRemoved);
                statusEffectSystem.OnEffectTicked.AddListener(OnEffectTicked);
            }
            
            if (turnOrderSystem != null)
            {
                turnOrderSystem.OnTurnChanged.AddListener(OnTurnChanged);
                turnOrderSystem.OnCombatStart.AddListener(OnCombatStart);
                turnOrderSystem.OnCombatEnd.AddListener(OnCombatEnd);
            }
        }
        
        #region Test Methods
        
        /// <summary>
        /// Test applying burn effect
        /// </summary>
        public void TestApplyBurn()
        {
            if (statusEffectSystem == null || testCombatants.Count < 2) return;
            
            var caster = testCombatants[0]; // Mage
            var target = testCombatants[1]; // Warrior
            
            var burnEffect = statusEffectSystem.CreateBurnEffect(target, caster, 3);
            bool success = statusEffectSystem.ApplyEffect(target, burnEffect);
            
            if (debugMode)
            {
                Debug.Log($"TestApplyBurn: Applied burn effect to {target.Name} from {caster.Name}. Success: {success}");
            }
            
            UpdateStatusDisplay();
        }
        
        /// <summary>
        /// Test applying bleed effect (placeholder)
        /// </summary>
        public void TestApplyBleed()
        {
            if (statusEffectSystem == null || testCombatants.Count < 2) return;
            
            var caster = testCombatants[2]; // Rogue
            var target = testCombatants[0]; // Mage
            
            var bleedEffect = statusEffectSystem.CreateBleedEffect(target, caster, 2);
            bool success = statusEffectSystem.ApplyEffect(target, bleedEffect);
            
            if (debugMode)
            {
                Debug.Log($"TestApplyBleed: Attempted to apply bleed effect to {target.Name} from {caster.Name}. Success: {success}");
            }
            
            UpdateStatusDisplay();
        }
        
        /// <summary>
        /// Test starting combat
        /// </summary>
        public void TestStartCombat()
        {
            if (turnOrderSystem == null) return;
            
            turnOrderSystem.StartCombat(testCombatants);
            
            if (debugMode)
            {
                Debug.Log("TestStartCombat: Started combat with test combatants");
            }
            
            UpdateStatusDisplay();
        }
        
        /// <summary>
        /// Test ending current turn
        /// </summary>
        public void TestEndTurn()
        {
            if (turnOrderSystem == null) return;
            
            turnOrderSystem.EndCurrentTurn();
            
            if (debugMode)
            {
                Debug.Log("TestEndTurn: Ended current turn");
            }
            
            UpdateStatusDisplay();
        }
        
        /// <summary>
        /// Test clearing all effects
        /// </summary>
        public void TestClearAllEffects()
        {
            if (statusEffectSystem == null) return;
            
            statusEffectSystem.ClearAllEffects();
            
            if (debugMode)
            {
                Debug.Log("TestClearAllEffects: Cleared all status effects");
            }
            
            UpdateStatusDisplay();
        }
        
        /// <summary>
        /// Test resetting health
        /// </summary>
        public void TestResetHealth()
        {
            foreach (var combatant in testCombatants)
            {
                if (combatant is TestCombatant testCombatant)
                {
                    testCombatant.ResetHealth();
                }
            }
            
            if (debugMode)
            {
                Debug.Log("TestResetHealth: Reset all combatant health");
            }
            
            UpdateStatusDisplay();
        }
        
        #endregion
        
        #region Event Handlers
        
        private void OnEffectApplied(ICombatant combatant, IStatusEffect effect)
        {
            if (debugMode)
            {
                Debug.Log($"Event: {effect.Name} applied to {combatant.Name}");
            }
            UpdateStatusDisplay();
        }
        
        private void OnEffectRemoved(ICombatant combatant, IStatusEffect effect)
        {
            if (debugMode)
            {
                Debug.Log($"Event: {effect.Name} removed from {combatant.Name}");
            }
            UpdateStatusDisplay();
        }
        
        private void OnEffectTicked(ICombatant combatant, IStatusEffect effect)
        {
            if (debugMode)
            {
                Debug.Log($"Event: {effect.Name} ticked on {combatant.Name}");
            }
            UpdateStatusDisplay();
        }
        
        private void OnTurnChanged(ICombatant combatant)
        {
            if (debugMode)
            {
                Debug.Log($"Event: Turn changed to {combatant.Name}");
            }
            UpdateStatusDisplay();
        }
        
        private void OnCombatStart()
        {
            if (debugMode)
            {
                Debug.Log("Event: Combat started");
            }
            UpdateStatusDisplay();
        }
        
        private void OnCombatEnd()
        {
            if (debugMode)
            {
                Debug.Log("Event: Combat ended");
            }
            UpdateStatusDisplay();
        }
        
        #endregion
        
        /// <summary>
        /// Update the status display text
        /// </summary>
        private void UpdateStatusDisplay()
        {
            var statusLines = new List<string>();
            
            // Combat status
            if (turnOrderSystem != null)
            {
                statusLines.Add($"Combat Active: {turnOrderSystem.IsCombatActive}");
                if (turnOrderSystem.IsCombatActive)
                {
                    statusLines.Add($"Current Turn: {turnOrderSystem.CurrentCombatant?.Name ?? "None"}");
                }
            }
            
            // Combatant status
            statusLines.Add("\nCombatants:");
            foreach (var combatant in testCombatants)
            {
                var effects = statusEffectSystem?.GetCombatantEffects(combatant) ?? new List<IStatusEffect>();
                var effectText = effects.Count > 0 ? $" [{string.Join(", ", effects.Select(e => e.ToString()))}]" : "";
                statusLines.Add($"  {combatant}{effectText}");
            }
            
            // Global effects summary
            if (statusEffectSystem != null)
            {
                statusLines.Add($"\nTotal Active Effects: {statusEffectSystem.TotalActiveEffects}");
                var summary = statusEffectSystem.GetGlobalEffectsSummary();
                if (summary != "No active effects")
                {
                    statusLines.Add($"Effects: {summary}");
                }
            }
            
            var statusText = string.Join("\n", statusLines);
            
            // Update UI
            if (uiManager != null)
            {
                uiManager.UpdateStatusText(statusText);
            }
            else if (this.statusText != null)
            {
                this.statusText.text = statusText;
            }
        }
        
        /// <summary>
        /// Manual test method that can be called from Inspector
        /// </summary>
        [ContextMenu("Run Full Test")]
        public void RunFullTest()
        {
            if (debugMode)
            {
                Debug.Log("=== RUNNING FULL STATUS EFFECT SYSTEM TEST ===");
            }
            
            // Reset everything
            TestResetHealth();
            TestClearAllEffects();
            
            // Start combat
            TestStartCombat();
            
            // Apply effects
            TestApplyBurn();
            TestApplyBleed();
            
            // End a few turns to see effects tick
            for (int i = 0; i < 3; i++)
            {
                TestEndTurn();
            }
            
            if (debugMode)
            {
                Debug.Log("=== FULL TEST COMPLETED ===");
            }
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            if (statusEffectSystem != null)
            {
                statusEffectSystem.OnEffectApplied.RemoveAllListeners();
                statusEffectSystem.OnEffectRemoved.RemoveAllListeners();
                statusEffectSystem.OnEffectTicked.RemoveAllListeners();
            }
            
            if (turnOrderSystem != null)
            {
                turnOrderSystem.OnTurnChanged.RemoveAllListeners();
                turnOrderSystem.OnCombatStart.RemoveAllListeners();
                turnOrderSystem.OnCombatEnd.RemoveAllListeners();
            }
        }
    }
} 