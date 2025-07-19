using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GuildsOfArcanaTerra.Combat.Skills.Interfaces;
using GuildsOfArcanaTerra.Combat.Effects;

namespace GuildsOfArcanaTerra.Combat.UI
{
    /// <summary>
    /// Manages all UI elements for the combat system
    /// Connects buttons, text, and other UI elements to combat mechanics
    /// </summary>
    public class CombatUIManager : MonoBehaviour
    {
        [Header("Combat System")]
        [SerializeField] private TurnOrderSystem turnOrderSystem;
        [SerializeField] private StatusEffectSystem statusEffectSystem;
        
        [Header("Combatants")]
        [SerializeField] private Combatant playerCombatant;
        [SerializeField] private Combatant enemyCombatant;
        
        [Header("Combatant Info Panels")]
        [SerializeField] private GameObject playerInfoPanel;
        [SerializeField] private GameObject enemyInfoPanel;
        
        [Header("Player Info UI")]
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI playerHealthText;
        [SerializeField] private Slider playerHealthBar;
        [SerializeField] private TextMeshProUGUI playerStatsText;
        [SerializeField] private TextMeshProUGUI playerClassText;
        
        [Header("Enemy Info UI")]
        [SerializeField] private TextMeshProUGUI enemyNameText;
        [SerializeField] private TextMeshProUGUI enemyHealthText;
        [SerializeField] private Slider enemyHealthBar;
        [SerializeField] private TextMeshProUGUI enemyStatsText;
        
        [Header("Skill Buttons")]
        [SerializeField] private Button[] skillButtons = new Button[4];
        [SerializeField] private TextMeshProUGUI[] skillButtonTexts = new TextMeshProUGUI[4];
        [SerializeField] private TextMeshProUGUI[] skillCooldownTexts = new TextMeshProUGUI[4];
        
        [Header("Action Buttons")]
        [SerializeField] private Button endTurnButton;
        [SerializeField] private Button autoAttackButton;
        [SerializeField] private Button healButton;
        [SerializeField] private Button resetButton;
        
        [Header("Combat Log")]
        [SerializeField] private TextMeshProUGUI combatLogText;
        [SerializeField] private ScrollRect combatLogScrollRect;
        [SerializeField] private int maxLogLines = 20;
        
        [Header("Status Effects")]
        [SerializeField] private Transform playerStatusEffectsContainer;
        [SerializeField] private Transform enemyStatusEffectsContainer;
        [SerializeField] private GameObject statusEffectPrefab;
        
        [Header("Turn Info")]
        [SerializeField] private TextMeshProUGUI turnInfoText;
        [SerializeField] private TextMeshProUGUI turnOrderText;
        
        [Header("Settings")]
        [SerializeField] private bool autoUpdateUI = true;
        [SerializeField] private float updateInterval = 0.1f;
        
        // Private variables
        private List<string> combatLog = new List<string>();
        private float lastUpdateTime;
        private bool isPlayerTurn = true;
        
        void Start()
        {
            InitializeUI();
            SetupButtonListeners();
            UpdateAllUI();
        }
        
        void Update()
        {
            if (autoUpdateUI && Time.time - lastUpdateTime > updateInterval)
            {
                UpdateAllUI();
                lastUpdateTime = Time.time;
            }
        }
        
        /// <summary>
        /// Initialize the UI system
        /// </summary>
        void InitializeUI()
        {
            // Find systems if not assigned
            if (turnOrderSystem == null)
                turnOrderSystem = FindObjectOfType<TurnOrderSystem>();
            
            if (statusEffectSystem == null)
                statusEffectSystem = FindObjectOfType<StatusEffectSystem>();
            
            // Subscribe to combat events
            if (playerCombatant != null)
            {
                playerCombatant.OnDamageTaken.AddListener((combatant) => OnCombatantDamaged(combatant));
                playerCombatant.OnHealed.AddListener((combatant) => OnCombatantHealed(combatant));
                playerCombatant.OnTurnStarted.AddListener((combatant) => OnPlayerTurnStart(combatant));
            }
            
            if (enemyCombatant != null)
            {
                enemyCombatant.OnDamageTaken.AddListener((combatant) => OnCombatantDamaged(combatant));
                enemyCombatant.OnHealed.AddListener((combatant) => OnCombatantHealed(combatant));
            }
            
            // Initialize combat log
            AddToCombatLog("Combat started!");
        }
        
        /// <summary>
        /// Set up button click listeners
        /// </summary>
        void SetupButtonListeners()
        {
            // Skill buttons
            for (int i = 0; i < skillButtons.Length; i++)
            {
                int skillIndex = i; // Capture the index
                if (skillButtons[i] != null)
                {
                    skillButtons[i].onClick.AddListener(() => UseSkill(skillIndex));
                }
            }
            
            // Action buttons
            if (endTurnButton != null)
                endTurnButton.onClick.AddListener(EndTurn);
            
            if (autoAttackButton != null)
                autoAttackButton.onClick.AddListener(UseAutoAttack);
            
            if (healButton != null)
                healButton.onClick.AddListener(UseHeal);
            
            if (resetButton != null)
                resetButton.onClick.AddListener(ResetCombat);
        }
        
        /// <summary>
        /// Update all UI elements
        /// </summary>
        public void UpdateAllUI()
        {
            UpdateCombatantInfo();
            UpdateSkillButtons();
            UpdateStatusEffects();
            UpdateTurnInfo();
        }
        
        /// <summary>
        /// Update combatant information displays
        /// </summary>
        void UpdateCombatantInfo()
        {
            // Update player info
            if (playerCombatant != null)
            {
                if (playerNameText != null)
                    playerNameText.text = playerCombatant.Name;
                
                if (playerHealthText != null)
                    playerHealthText.text = $"{playerCombatant.CurrentHealth}/{playerCombatant.MaxHealth}";
                
                if (playerHealthBar != null)
                {
                    playerHealthBar.maxValue = playerCombatant.MaxHealth;
                    playerHealthBar.value = playerCombatant.CurrentHealth;
                }
                
                if (playerStatsText != null)
                    playerStatsText.text = $"STR: {playerCombatant.Strength} | AGI: {playerCombatant.AGI} | INT: {playerCombatant.INT} | DEF: {playerCombatant.Defense}";
                
                if (playerClassText != null)
                    playerClassText.text = playerCombatant.ClassDefinition?.ClassName ?? "No Class";
            }
            
            // Update enemy info
            if (enemyCombatant != null)
            {
                if (enemyNameText != null)
                    enemyNameText.text = enemyCombatant.Name;
                
                if (enemyHealthText != null)
                    enemyHealthText.text = $"{enemyCombatant.CurrentHealth}/{enemyCombatant.MaxHealth}";
                
                if (enemyHealthBar != null)
                {
                    enemyHealthBar.maxValue = enemyCombatant.MaxHealth;
                    enemyHealthBar.value = enemyCombatant.CurrentHealth;
                }
                
                if (enemyStatsText != null)
                    enemyStatsText.text = $"STR: {enemyCombatant.Strength} | AGI: {enemyCombatant.AGI} | INT: {enemyCombatant.INT} | DEF: {enemyCombatant.Defense}";
            }
        }
        
        /// <summary>
        /// Update skill buttons with current skills and cooldowns
        /// </summary>
        void UpdateSkillButtons()
        {
            if (playerCombatant == null) return;
            
            var skills = playerCombatant.GetAllSkills();
            
            for (int i = 0; i < skillButtons.Length; i++)
            {
                if (skillButtons[i] == null) continue;
                
                if (i < skills.Count)
                {
                    var skill = skills[i];
                    bool canUse = skill.CanUse(playerCombatant) && isPlayerTurn;
                    
                    // Update button text
                    if (skillButtonTexts[i] != null)
                        skillButtonTexts[i].text = skill.SkillName;
                    
                    // Update cooldown text
                    if (skillCooldownTexts[i] != null)
                    {
                        if (skill.CurrentCooldown > 0)
                            skillCooldownTexts[i].text = $"{skill.CurrentCooldown}";
                        else
                            skillCooldownTexts[i].text = "";
                    }
                    
                    // Update button interactability
                    skillButtons[i].interactable = canUse;
                    
                    // Update button color based on availability
                    var buttonImage = skillButtons[i].GetComponent<Image>();
                    if (buttonImage != null)
                    {
                        if (canUse)
                            buttonImage.color = Color.white;
                        else if (skill.CurrentCooldown > 0)
                            buttonImage.color = Color.gray;
                        else
                            buttonImage.color = Color.yellow;
                    }
                }
                else
                {
                    // No skill for this slot
                    if (skillButtonTexts[i] != null)
                        skillButtonTexts[i].text = "Empty";
                    
                    if (skillCooldownTexts[i] != null)
                        skillCooldownTexts[i].text = "";
                    
                    skillButtons[i].interactable = false;
                }
            }
        }
        
        /// <summary>
        /// Update status effects display
        /// </summary>
        void UpdateStatusEffects()
        {
            // Clear existing status effect displays
            ClearStatusEffects(playerStatusEffectsContainer);
            ClearStatusEffects(enemyStatusEffectsContainer);
            
            // Update player status effects
            if (playerCombatant != null && playerCombatant.StatusEffectManager != null)
            {
                var effects = playerCombatant.StatusEffectManager.ActiveEffects;
                foreach (var effect in effects)
                {
                    CreateStatusEffectDisplay(effect, playerStatusEffectsContainer);
                }
            }
            
            // Update enemy status effects
            if (enemyCombatant != null && enemyCombatant.StatusEffectManager != null)
            {
                var effects = enemyCombatant.StatusEffectManager.ActiveEffects;
                foreach (var effect in effects)
                {
                    CreateStatusEffectDisplay(effect, enemyStatusEffectsContainer);
                }
            }
        }
        
        /// <summary>
        /// Update turn information
        /// </summary>
        void UpdateTurnInfo()
        {
            if (turnInfoText != null)
            {
                turnInfoText.text = isPlayerTurn ? "Your Turn" : "Enemy Turn";
            }
            
            if (turnOrderText != null && turnOrderSystem != null)
            {
                var turnOrder = turnOrderSystem.AllCombatants;
                string orderText = "Turn Order: ";
                for (int i = 0; i < turnOrder.Count; i++)
                {
                    if (i > 0) orderText += " → ";
                    orderText += turnOrder[i].Name;
                }
                turnOrderText.text = orderText;
            }
        }
        
        /// <summary>
        /// Use a skill by index
        /// </summary>
        void UseSkill(int skillIndex)
        {
            if (playerCombatant == null || !isPlayerTurn) return;
            
            var skills = playerCombatant.GetAllSkills();
            if (skillIndex >= skills.Count) return;
            
            var skill = skills[skillIndex];
            if (!skill.CanUse(playerCombatant)) return;
            
            // Execute the skill
            var targets = new List<ICombatant> { enemyCombatant };
            SkillEffects.ExecuteSkill(skill, playerCombatant, targets, statusEffectSystem);
            
            AddToCombatLog($"{playerCombatant.Name} uses {skill.SkillName}!");
            
            // Update UI
            UpdateAllUI();
        }
        
        /// <summary>
        /// Use auto attack
        /// </summary>
        void UseAutoAttack()
        {
            if (playerCombatant == null || !isPlayerTurn) return;
            
            var basicAttack = playerCombatant.GetAllSkills().Find(s => s.SkillName.ToLower().Contains("attack"));
            if (basicAttack != null)
            {
                var targets = new List<ICombatant> { enemyCombatant };
                SkillEffects.ExecuteSkill(basicAttack, playerCombatant, targets, statusEffectSystem);
                
                AddToCombatLog($"{playerCombatant.Name} performs a basic attack!");
            }
            
            UpdateAllUI();
        }
        
        /// <summary>
        /// Use heal skill
        /// </summary>
        void UseHeal()
        {
            if (playerCombatant == null || !isPlayerTurn) return;
            
            var healSkill = playerCombatant.GetAllSkills().Find(s => s.SkillName.ToLower().Contains("heal"));
            if (healSkill != null)
            {
                var targets = new List<ICombatant> { playerCombatant };
                SkillEffects.ExecuteSkill(healSkill, playerCombatant, targets, statusEffectSystem);
                
                AddToCombatLog($"{playerCombatant.Name} heals themselves!");
            }
            
            UpdateAllUI();
        }
        
        /// <summary>
        /// End the current turn
        /// </summary>
        void EndTurn()
        {
            if (!isPlayerTurn) return;
            
            isPlayerTurn = false;
            AddToCombatLog("Player ends their turn.");
            
            // Simulate enemy turn
            Invoke(nameof(EnemyTurn), 1f);
        }
        
        /// <summary>
        /// Simulate enemy turn
        /// </summary>
        void EnemyTurn()
        {
            if (enemyCombatant == null) return;
            
            AddToCombatLog("Enemy's turn!");
            
            // Simple AI: use a random skill or basic attack
            var skills = enemyCombatant.GetAllSkills();
            if (skills.Count > 0)
            {
                var randomSkill = skills[Random.Range(0, skills.Count)];
                var targets = new List<ICombatant> { playerCombatant };
                SkillEffects.ExecuteSkill(randomSkill, enemyCombatant, targets, statusEffectSystem);
                
                AddToCombatLog($"{enemyCombatant.Name} uses {randomSkill.SkillName}!");
            }
            
            // End enemy turn
            Invoke(nameof(StartPlayerTurn), 1f);
        }
        
        /// <summary>
        /// Start player turn
        /// </summary>
        void StartPlayerTurn()
        {
            isPlayerTurn = true;
            AddToCombatLog("Your turn!");
            UpdateAllUI();
        }
        
        /// <summary>
        /// Reset combat to initial state
        /// </summary>
        void ResetCombat()
        {
            if (playerCombatant != null)
                playerCombatant.ResetHealth();
            
            if (enemyCombatant != null)
                enemyCombatant.ResetHealth();
            
            isPlayerTurn = true;
            AddToCombatLog("Combat reset!");
            UpdateAllUI();
        }
        
        /// <summary>
        /// Add message to combat log
        /// </summary>
        void AddToCombatLog(string message)
        {
            combatLog.Add($"[{System.DateTime.Now:HH:mm:ss}] {message}");
            
            // Keep only the last maxLogLines entries
            while (combatLog.Count > maxLogLines)
            {
                combatLog.RemoveAt(0);
            }
            
            // Update log text
            if (combatLogText != null)
            {
                combatLogText.text = string.Join("\n", combatLog);
            }
            
            // Scroll to bottom
            if (combatLogScrollRect != null)
            {
                Canvas.ForceUpdateCanvases();
                combatLogScrollRect.verticalNormalizedPosition = 0f;
            }
        }
        
        /// <summary>
        /// Handle combatant taking damage
        /// </summary>
        void OnCombatantDamaged(Combatant combatant)
        {
            AddToCombatLog($"{combatant.Name} takes damage!");
        }
        
        /// <summary>
        /// Handle combatant being healed
        /// </summary>
        void OnCombatantHealed(Combatant combatant)
        {
            AddToCombatLog($"{combatant.Name} is healed!");
        }
        
        /// <summary>
        /// Handle player turn start
        /// </summary>
        void OnPlayerTurnStart(Combatant combatant)
        {
            isPlayerTurn = true;
            AddToCombatLog("Your turn begins!");
            UpdateAllUI();
        }
        
        /// <summary>
        /// Clear status effects from a container
        /// </summary>
        void ClearStatusEffects(Transform container)
        {
            if (container == null) return;
            
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }
        }
        
        /// <summary>
        /// Create a status effect display
        /// </summary>
        void CreateStatusEffectDisplay(IStatusEffect effect, Transform container)
        {
            if (statusEffectPrefab == null || container == null) return;
            
            var effectGO = Instantiate(statusEffectPrefab, container);
            var effectText = effectGO.GetComponentInChildren<TextMeshProUGUI>();
            
            if (effectText != null)
            {
                effectText.text = $"{effect.Name} ({effect.Duration})";
            }
        }
        
        /// <summary>
        /// Public method to manually update UI
        /// </summary>
        public void ForceUpdateUI()
        {
            UpdateAllUI();
        }
    }
} 