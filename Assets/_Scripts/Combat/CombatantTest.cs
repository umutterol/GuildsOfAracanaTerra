using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GuildsOfArcanaTerra.ScriptableObjects.Classes;
using GuildsOfArcanaTerra.Combat.UI;

namespace GuildsOfArcanaTerra.Combat
{
    /// <summary>
    /// Test script for the new Combatant system with CharacterClassDefinition integration
    /// </summary>
    public class CombatantTest : MonoBehaviour
    {
        [Header("Test Combatants")]
        [SerializeField] private Combatant warriorCombatant;
        [SerializeField] private Combatant mageCombatant;
        [SerializeField] private Combatant clericCombatant;
        
        [Header("Class Definitions")]
        [SerializeField] private CharacterClassDefinition warriorClass;
        [SerializeField] private CharacterClassDefinition mageClass;
        [SerializeField] private CharacterClassDefinition clericClass;
        
        [Header("Systems")]
        [SerializeField] private TurnOrderSystem turnOrderSystem;
        [SerializeField] private StatusEffectSystem statusEffectSystem;
        
        [Header("UI Manager")]
        [SerializeField] private CombatantTestUI uiManager;
        
        [Header("UI Elements (Auto-assigned via UI Manager)")]
        [SerializeField] private Text statusText;
        [SerializeField] private Text skillText;
        [SerializeField] private Button loadClassesButton;
        [SerializeField] private Button startCombatButton;
        [SerializeField] private Button endTurnButton;
        [SerializeField] private Button useSkillButton;
        [SerializeField] private Button applyBurnButton;
        [SerializeField] private Button resetHealthButton;
        
        [Header("Skill Selection")]
        [SerializeField] private Dropdown skillDropdown;
        [SerializeField] private Dropdown targetDropdown;
        
        [Header("Debug")]
        [SerializeField] private bool debugMode = true;
        
        private List<Combatant> testCombatants = new List<Combatant>();
        private Combatant currentTarget;
        
        private void Start()
        {
            // Initialize test combatants list
            if (warriorCombatant != null) testCombatants.Add(warriorCombatant);
            if (mageCombatant != null) testCombatants.Add(mageCombatant);
            if (clericCombatant != null) testCombatants.Add(clericCombatant);
            
            // Find systems if not assigned
            if (turnOrderSystem == null)
                turnOrderSystem = FindObjectOfType<TurnOrderSystem>();
            if (statusEffectSystem == null)
                statusEffectSystem = FindObjectOfType<StatusEffectSystem>();
            
            // Find UI manager if not assigned
            if (uiManager == null)
                uiManager = FindObjectOfType<CombatantTestUI>();
            
            // Setup UI
            SetupUI();
            
            // Subscribe to events
            SubscribeToEvents();
            
            if (debugMode)
            {
                Debug.Log("CombatantTest: Initialized test system");
            }
        }
        
        /// <summary>
        /// Setup UI buttons and dropdowns
        /// </summary>
        private void SetupUI()
        {
            // Get UI elements from UI manager if available
            if (uiManager != null)
            {
                statusText = uiManager.StatusText;
                skillText = uiManager.SkillText;
                loadClassesButton = uiManager.LoadClassesButton;
                startCombatButton = uiManager.StartCombatButton;
                endTurnButton = uiManager.EndTurnButton;
                useSkillButton = uiManager.UseSkillButton;
                applyBurnButton = uiManager.ApplyBurnButton;
                resetHealthButton = uiManager.ResetHealthButton;
                skillDropdown = uiManager.SkillDropdown;
                targetDropdown = uiManager.TargetDropdown;
            }
            
            // Setup button listeners
            if (loadClassesButton != null)
                loadClassesButton.onClick.AddListener(LoadAllClasses);
            
            if (startCombatButton != null)
                startCombatButton.onClick.AddListener(StartCombat);
            
            if (endTurnButton != null)
                endTurnButton.onClick.AddListener(EndCurrentTurn);
            
            if (useSkillButton != null)
                useSkillButton.onClick.AddListener(UseSelectedSkill);
            
            if (applyBurnButton != null)
                applyBurnButton.onClick.AddListener(ApplyBurnToCurrentTarget);
            
            if (resetHealthButton != null)
                resetHealthButton.onClick.AddListener(ResetAllHealth);
            
            // Setup skill dropdown
            if (skillDropdown != null)
            {
                skillDropdown.onValueChanged.AddListener(OnSkillSelected);
            }
            
            // Setup target dropdown
            if (targetDropdown != null)
            {
                targetDropdown.onValueChanged.AddListener(OnTargetSelected);
                UpdateTargetDropdown();
            }
            
            if (debugMode)
            {
                Debug.Log("CombatantTest: UI setup completed");
            }
        }
        
        /// <summary>
        /// Subscribe to combatant events
        /// </summary>
        private void SubscribeToEvents()
        {
            foreach (var combatant in testCombatants)
            {
                if (combatant != null)
                {
                    combatant.OnDamageTaken.AddListener(OnCombatantDamaged);
                    combatant.OnHealed.AddListener(OnCombatantHealed);
                    combatant.OnTurnStarted.AddListener(OnCombatantTurnStarted);
                    combatant.OnTurnEnded.AddListener(OnCombatantTurnEnded);
                    combatant.OnSkillUsed.AddListener(OnCombatantSkillUsed);
                    combatant.OnDeath.AddListener(OnCombatantDeath);
                }
            }
            
            // Subscribe to turn order events
            if (turnOrderSystem != null)
            {
                turnOrderSystem.OnTurnChanged.AddListener(OnTurnChanged);
                turnOrderSystem.OnCombatantTurnStart.AddListener(OnCombatantTurnStart);
                turnOrderSystem.OnCombatantTurnEnd.AddListener(OnCombatantTurnEnd);
            }
        }
        
        /// <summary>
        /// Load all class definitions into combatants
        /// </summary>
        public void LoadAllClasses()
        {
            if (warriorCombatant != null && warriorClass != null)
            {
                // Set the class definition
                SetPrivateField(warriorCombatant, "classDefinition", warriorClass);
                warriorCombatant.LoadClassDefinition();
            }
            
            if (mageCombatant != null && mageClass != null)
            {
                SetPrivateField(mageCombatant, "classDefinition", mageClass);
                mageCombatant.LoadClassDefinition();
            }
            
            if (clericCombatant != null && clericClass != null)
            {
                SetPrivateField(clericCombatant, "classDefinition", clericClass);
                clericCombatant.LoadClassDefinition();
            }
            
            UpdateUI();
            
            if (debugMode)
            {
                Debug.Log("CombatantTest: Loaded all class definitions");
            }
        }
        
        /// <summary>
        /// Start combat with the test combatants
        /// </summary>
        public void StartCombat()
        {
            if (turnOrderSystem == null)
            {
                Debug.LogError("CombatantTest: No TurnOrderSystem found!");
                return;
            }
            
            var combatants = new List<ICombatant>();
            foreach (var combatant in testCombatants)
            {
                if (combatant != null && combatant.IsAlive)
                {
                    combatants.Add(combatant);
                }
            }
            
            if (combatants.Count > 0)
            {
                turnOrderSystem.StartCombat(combatants);
                
                if (debugMode)
                {
                    Debug.Log($"CombatantTest: Started combat with {combatants.Count} combatants");
                }
            }
            else
            {
                Debug.LogWarning("CombatantTest: No alive combatants to start combat with!");
            }
        }
        
        /// <summary>
        /// End the current turn
        /// </summary>
        public void EndCurrentTurn()
        {
            if (turnOrderSystem != null && turnOrderSystem.IsCombatActive)
            {
                turnOrderSystem.EndCurrentTurn();
            }
        }
        
        /// <summary>
        /// Use the selected skill on the selected target
        /// </summary>
        public void UseSelectedSkill()
        {
            if (turnOrderSystem?.CurrentCombatant is Combatant currentCombatant)
            {
                if (skillDropdown != null && skillDropdown.options.Count > 0)
                {
                    string selectedSkillName = skillDropdown.options[skillDropdown.value].text;
                    bool success = currentCombatant.UseSkill(selectedSkillName);
                    
                    if (success && debugMode)
                    {
                        Debug.Log($"CombatantTest: {currentCombatant.Name} used {selectedSkillName}");
                    }
                }
            }
            else
            {
                Debug.LogWarning("CombatantTest: No current combatant or combat not active!");
            }
        }
        
        /// <summary>
        /// Apply burn effect to the current target
        /// </summary>
        public void ApplyBurnToCurrentTarget()
        {
            if (currentTarget != null && statusEffectSystem != null)
            {
                var burnEffect = statusEffectSystem.CreateBurnEffect(currentTarget, currentTarget, 2);
                bool success = statusEffectSystem.ApplyEffect(currentTarget, burnEffect);
                
                if (success && debugMode)
                {
                    Debug.Log($"CombatantTest: Applied burn to {currentTarget.Name}");
                }
            }
        }
        
        /// <summary>
        /// Reset all combatant health
        /// </summary>
        public void ResetAllHealth()
        {
            foreach (var combatant in testCombatants)
            {
                if (combatant != null)
                {
                    combatant.ResetHealth();
                }
            }
            
            UpdateUI();
            
            if (debugMode)
            {
                Debug.Log("CombatantTest: Reset all health");
            }
        }
        
        /// <summary>
        /// Force refresh UI connections (for debugging)
        /// </summary>
        [ContextMenu("Refresh UI Connections")]
        public void RefreshUIConnections()
        {
            SetupUI();
            UpdateUI();
            
            if (debugMode)
            {
                Debug.Log("CombatantTest: UI connections refreshed");
            }
        }
        
        /// <summary>
        /// Update the UI with current status
        /// </summary>
        public void UpdateUI()
        {
            UpdateStatusText();
            UpdateSkillDropdown();
            UpdateTargetDropdown();
        }
        
        /// <summary>
        /// Update the status text
        /// </summary>
        private void UpdateStatusText()
        {
            if (statusText == null) return;
            
            var status = "Combatant Status:\n\n";
            
            foreach (var combatant in testCombatants)
            {
                if (combatant != null)
                {
                    status += combatant.GetStatusString() + "\n\n";
                }
            }
            
            if (turnOrderSystem != null && turnOrderSystem.IsCombatActive)
            {
                status += $"Current Turn: {turnOrderSystem.CurrentCombatant?.Name ?? "None"}\n";
                status += $"Turn Queue: {turnOrderSystem.TurnQueueCount} remaining";
            }
            else
            {
                status += "Combat not active";
            }
            
            statusText.text = status;
        }
        
        /// <summary>
        /// Update the skill dropdown with current combatant's skills
        /// </summary>
        private void UpdateSkillDropdown()
        {
            if (skillDropdown == null) return;
            
            skillDropdown.ClearOptions();
            
            if (turnOrderSystem?.CurrentCombatant is Combatant currentCombatant)
            {
                var skills = currentCombatant.GetAvailableSkills();
                var options = new List<string>();
                
                foreach (var skill in skills)
                {
                    options.Add(skill.SkillName);
                }
                
                skillDropdown.AddOptions(options);
            }
        }
        
        /// <summary>
        /// Update the target dropdown
        /// </summary>
        private void UpdateTargetDropdown()
        {
            if (targetDropdown == null) return;
            
            targetDropdown.ClearOptions();
            var options = new List<string>();
            
            foreach (var combatant in testCombatants)
            {
                if (combatant != null && combatant.IsAlive)
                {
                    options.Add(combatant.Name);
                }
            }
            
            targetDropdown.AddOptions(options);
            
            // Set current target to first option
            if (options.Count > 0)
            {
                OnTargetSelected(0);
            }
        }
        
        /// <summary>
        /// Handle skill selection
        /// </summary>
        private void OnSkillSelected(int index)
        {
            UpdateSkillText();
        }
        
        /// <summary>
        /// Handle target selection
        /// </summary>
        private void OnTargetSelected(int index)
        {
            if (index >= 0 && index < testCombatants.Count)
            {
                currentTarget = testCombatants[index];
                UpdateSkillText();
            }
        }
        
        /// <summary>
        /// Update skill information text
        /// </summary>
        private void UpdateSkillText()
        {
            if (skillText == null) return;
            
            if (turnOrderSystem?.CurrentCombatant is Combatant currentCombatant)
            {
                skillText.text = currentCombatant.GetSkillInfoString();
            }
            else
            {
                skillText.text = "No current combatant";
            }
        }
        
        #region Event Handlers
        
        private void OnCombatantDamaged(Combatant combatant)
        {
            UpdateUI();
        }
        
        private void OnCombatantHealed(Combatant combatant)
        {
            UpdateUI();
        }
        
        private void OnCombatantTurnStarted(Combatant combatant)
        {
            UpdateUI();
        }
        
        private void OnCombatantTurnEnded(Combatant combatant)
        {
            UpdateUI();
        }
        
        private void OnCombatantSkillUsed(Combatant combatant)
        {
            UpdateUI();
        }
        
        private void OnCombatantDeath(Combatant combatant)
        {
            UpdateUI();
        }
        
        private void OnTurnChanged(ICombatant combatant)
        {
            UpdateUI();
        }
        
        private void OnCombatantTurnStart(ICombatant combatant)
        {
            UpdateUI();
        }
        
        private void OnCombatantTurnEnd(ICombatant combatant)
        {
            UpdateUI();
        }
        
        #endregion
        
        /// <summary>
        /// Helper method to set private fields using reflection
        /// </summary>
        private void SetPrivateField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName, 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(obj, value);
            }
        }
        
        #region Unity Lifecycle
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            foreach (var combatant in testCombatants)
            {
                if (combatant != null)
                {
                    combatant.OnDamageTaken.RemoveListener(OnCombatantDamaged);
                    combatant.OnHealed.RemoveListener(OnCombatantHealed);
                    combatant.OnTurnStarted.RemoveListener(OnCombatantTurnStarted);
                    combatant.OnTurnEnded.RemoveListener(OnCombatantTurnEnded);
                    combatant.OnSkillUsed.RemoveListener(OnCombatantSkillUsed);
                    combatant.OnDeath.RemoveListener(OnCombatantDeath);
                }
            }
            
            if (turnOrderSystem != null)
            {
                turnOrderSystem.OnTurnChanged.RemoveListener(OnTurnChanged);
                turnOrderSystem.OnCombatantTurnStart.RemoveListener(OnCombatantTurnStart);
                turnOrderSystem.OnCombatantTurnEnd.RemoveListener(OnCombatantTurnEnd);
            }
        }
        
        #endregion
    }
} 