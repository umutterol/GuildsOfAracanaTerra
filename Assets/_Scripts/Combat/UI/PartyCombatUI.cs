using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GuildsOfArcanaTerra.Characters;
using GuildsOfArcanaTerra.Combat.Core;
using GuildsOfArcanaTerra.Combat.Skills.Interfaces;

namespace GuildsOfArcanaTerra.Combat.UI
{
    /// <summary>
    /// Enhanced combat UI for party-based combat with proper targeting
    /// </summary>
    public class PartyCombatUI : MonoBehaviour
    {
        [Header("Party Management")]
        [SerializeField] private PartyManager partyManager;
        
        [Header("Combat Systems")]
        [SerializeField] private TurnOrderSystem turnOrderSystem;
        [SerializeField] private StatusEffectSystem statusEffectSystem;
        
        [Header("Party Display")]
        [SerializeField] private Transform playerPartyContainer;
        [SerializeField] private GameObject playerCombatantCardPrefab;
        [SerializeField] private List<PlayerCombatantCard> playerCards = new List<PlayerCombatantCard>();
        
        [Header("Enemy Display")]
        [SerializeField] private Transform enemyPartyContainer;
        [SerializeField] private GameObject enemyCombatantCardPrefab;
        [SerializeField] private List<EnemyCombatantCard> enemyCards = new List<EnemyCombatantCard>();
        
        [Header("Combatant Selection")]
        [SerializeField] private PlayerCombatantCard selectedPlayerCard;
        [SerializeField] private List<ICombatant> selectedTargets = new List<ICombatant>();
        
        [Header("Skill UI")]
        [SerializeField] private Transform skillButtonContainer;
        [SerializeField] private GameObject skillButtonPrefab;
        [SerializeField] private List<SkillButton> skillButtons = new List<SkillButton>();
        
        [Header("Targeting UI")]
        [SerializeField] private GameObject targetingPanel;
        [SerializeField] private Transform targetButtonContainer;
        [SerializeField] private GameObject targetButtonPrefab;
        [SerializeField] private List<TargetButton> targetButtons = new List<TargetButton>();
        
        [Header("Combat Info")]
        [SerializeField] private TextMeshProUGUI turnInfoText;
        [SerializeField] private TextMeshProUGUI combatLogText;
        [SerializeField] private TextMeshProUGUI targetingInfoText;
        
        [Header("Action Buttons")]
        [SerializeField] private Button endTurnButton;
        [SerializeField] private Button autoAttackButton;
        [SerializeField] private Button cancelTargetingButton;
        
        [Header("Debug")]
        [SerializeField, System.Diagnostics.CodeAnalysis.SuppressMessage("Compiler", "CS0414")] private bool debugMode = false;
        
        // Note: debugMode is available for future debug logging
        
        private string combatLog = "";
        private bool isTargetingMode = false;
        private IBaseSkill selectedSkill;
        
        private void Start()
        {
            // Find references if not assigned
            if (partyManager == null)
                partyManager = FindObjectOfType<PartyManager>();
            
            if (turnOrderSystem == null)
                turnOrderSystem = FindObjectOfType<TurnOrderSystem>();
            
            if (statusEffectSystem == null)
                statusEffectSystem = FindObjectOfType<StatusEffectSystem>();
            
            // Setup UI
            SetupButtons();
            SetupCombatSystems();
            
            // Initial setup
            RefreshUI();
        }
        
        /// <summary>
        /// Setup button click listeners
        /// </summary>
        private void SetupButtons()
        {
            if (endTurnButton != null)
                endTurnButton.onClick.AddListener(EndTurn);
            
            if (autoAttackButton != null)
                autoAttackButton.onClick.AddListener(UseAutoAttack);
            
            if (cancelTargetingButton != null)
                cancelTargetingButton.onClick.AddListener(CancelTargeting);
        }
        
        /// <summary>
        /// Setup combat system event listeners
        /// </summary>
        private void SetupCombatSystems()
        {
            if (turnOrderSystem != null)
            {
                turnOrderSystem.OnTurnChanged.AddListener(OnTurnChanged);
                turnOrderSystem.OnCombatantTurnStart.AddListener(OnCombatantTurnStart);
                turnOrderSystem.OnCombatantTurnEnd.AddListener(OnCombatantTurnEnd);
            }
        }
        
        /// <summary>
        /// Refresh all UI elements
        /// </summary>
        public void RefreshUI()
        {
            UpdatePartyDisplay();
            UpdateEnemyDisplay();
            UpdateSkillButtons();
            UpdateCombatInfo();
            UpdateButtonStates();
        }
        
        /// <summary>
        /// Update player party display
        /// </summary>
        private void UpdatePartyDisplay()
        {
            if (playerPartyContainer == null) return;
            
            // Clear existing cards
            foreach (var card in playerCards)
            {
                if (card != null)
                    DestroyImmediate(card.gameObject);
            }
            playerCards.Clear();
            
            // Create cards for each party member
            var partyCombatants = partyManager.ActiveCombatants;
            foreach (var combatant in partyCombatants)
            {
                var cardGO = Instantiate(playerCombatantCardPrefab, playerPartyContainer);
                var card = cardGO.GetComponent<PlayerCombatantCard>();
                
                if (card != null)
                {
                    card.Initialize(combatant, this);
                    playerCards.Add(card);
                }
            }
        }
        
        /// <summary>
        /// Update enemy party display
        /// </summary>
        private void UpdateEnemyDisplay()
        {
            if (enemyPartyContainer == null) return;
            
            // Clear existing cards
            foreach (var card in enemyCards)
            {
                if (card != null)
                    DestroyImmediate(card.gameObject);
            }
            enemyCards.Clear();
            
            // TODO: Get enemy combatants from combat system
            // For now, create placeholder enemies
            CreatePlaceholderEnemies();
        }
        
        /// <summary>
        /// Create placeholder enemies for testing
        /// </summary>
        private void CreatePlaceholderEnemies()
        {
            // Create 2-3 placeholder enemies
            for (int i = 0; i < Random.Range(2, 4); i++)
            {
                var cardGO = Instantiate(enemyCombatantCardPrefab, enemyPartyContainer);
                var card = cardGO.GetComponent<EnemyCombatantCard>();
                
                if (card != null)
                {
                    // Create a placeholder enemy combatant
                    var enemyGO = new GameObject($"Enemy_{i}");
                    var enemy = enemyGO.AddComponent<Combatant>();
                    // Note: CharacterName is read-only, so we'll set it through reflection or use a different approach
                    // For now, the name will be set by the GameObject name
                    
                    card.Initialize(enemy, this);
                    enemyCards.Add(card);
                }
            }
        }
        
        /// <summary>
        /// Update skill buttons for selected combatant
        /// </summary>
        private void UpdateSkillButtons()
        {
            if (skillButtonContainer == null) return;
            
            // Clear existing buttons
            foreach (var button in skillButtons)
            {
                if (button != null)
                    DestroyImmediate(button.gameObject);
            }
            skillButtons.Clear();
            
            if (selectedPlayerCard == null) return;
            
            var combatant = selectedPlayerCard.Combatant;
            var skills = combatant.GetAllSkills();
            
            foreach (var skill in skills)
            {
                var buttonGO = Instantiate(skillButtonPrefab, skillButtonContainer);
                var button = buttonGO.GetComponent<SkillButton>();
                
                if (button != null)
                {
                    button.Initialize(skill, this);
                    skillButtons.Add(button);
                }
            }
        }
        
        /// <summary>
        /// Update combat information display
        /// </summary>
        private void UpdateCombatInfo()
        {
            if (turnInfoText != null)
            {
                var currentCombatant = turnOrderSystem?.CurrentCombatant;
                turnInfoText.text = currentCombatant != null ? 
                    $"{currentCombatant.Name}'s Turn" : "Combat Not Started";
            }
            
            if (targetingInfoText != null)
            {
                if (isTargetingMode && selectedSkill != null)
                {
                    targetingInfoText.text = $"Select target for {selectedSkill.SkillName}";
                }
                else
                {
                    targetingInfoText.text = "";
                }
            }
        }
        
        /// <summary>
        /// Update button states
        /// </summary>
        private void UpdateButtonStates()
        {
            if (endTurnButton != null)
                endTurnButton.interactable = !isTargetingMode;
            
            if (autoAttackButton != null)
                autoAttackButton.interactable = selectedPlayerCard != null && !isTargetingMode;
            
            if (cancelTargetingButton != null)
                cancelTargetingButton.gameObject.SetActive(isTargetingMode);
        }
        
        /// <summary>
        /// Select a player combatant
        /// </summary>
        public void SelectPlayerCombatant(PlayerCombatantCard card)
        {
            // Deselect previous card
            if (selectedPlayerCard != null)
                selectedPlayerCard.SetSelected(false);
            
            selectedPlayerCard = card;
            
            if (selectedPlayerCard != null)
                selectedPlayerCard.SetSelected(true);
            
            UpdateSkillButtons();
            UpdateButtonStates();
        }
        
        /// <summary>
        /// Start targeting mode for a skill
        /// </summary>
        public void StartTargeting(IBaseSkill skill)
        {
            if (selectedPlayerCard == null) return;
            
            selectedSkill = skill;
            isTargetingMode = true;
            selectedTargets.Clear();
            
            ShowTargetingUI();
            UpdateCombatInfo();
            UpdateButtonStates();
        }
        
        /// <summary>
        /// Show targeting UI
        /// </summary>
        private void ShowTargetingUI()
        {
            if (targetingPanel != null)
                targetingPanel.SetActive(true);
            
            if (targetButtonContainer == null) return;
            
            // Clear existing target buttons
            foreach (var button in targetButtons)
            {
                if (button != null)
                    DestroyImmediate(button.gameObject);
            }
            targetButtons.Clear();
            
            // Get valid targets based on skill type
            var validTargets = GetValidTargets(selectedSkill);
            
            foreach (var target in validTargets)
            {
                var buttonGO = Instantiate(targetButtonPrefab, targetButtonContainer);
                var button = buttonGO.GetComponent<TargetButton>();
                
                if (button != null)
                {
                    button.Initialize(target, this);
                    targetButtons.Add(button);
                }
            }
        }
        
        /// <summary>
        /// Get valid targets for a skill
        /// </summary>
        private List<ICombatant> GetValidTargets(IBaseSkill skill)
        {
            var validTargets = new List<ICombatant>();
            var caster = selectedPlayerCard?.Combatant;
            
            if (caster == null) return validTargets;
            
            // Get all combatants
            var allCombatants = new List<ICombatant>();
            allCombatants.AddRange(partyManager.ActiveCombatants);
            allCombatants.AddRange(enemyCards.Select(c => c.Combatant));
            
            // Filter based on skill target type
            foreach (var combatant in allCombatants)
            {
                if (!combatant.IsAlive) continue;
                
                bool isEnemy = IsEnemy(caster, combatant);
                bool isValidTarget = IsValidTargetForSkill(skill.TargetType, isEnemy);
                
                if (isValidTarget)
                {
                    validTargets.Add(combatant);
                }
            }
            
            return validTargets;
        }
        
        /// <summary>
        /// Check if a target is an enemy of the caster
        /// </summary>
        private bool IsEnemy(Combatant caster, ICombatant target)
        {
            // Player party members are not enemies of each other
            bool casterIsPlayer = partyManager.ActiveCombatants.Contains(caster);
            bool targetIsPlayer = partyManager.ActiveCombatants.Contains(target as Combatant);
            
            return casterIsPlayer != targetIsPlayer;
        }
        
        /// <summary>
        /// Check if a target is valid for the given skill target type
        /// </summary>
        private bool IsValidTargetForSkill(SkillTargetType targetType, bool isEnemy)
        {
            switch (targetType)
            {
                case SkillTargetType.SingleEnemy:
                    return isEnemy;
                case SkillTargetType.SingleAlly:
                    return !isEnemy;
                case SkillTargetType.AllEnemies:
                    return isEnemy;
                case SkillTargetType.AllAllies:
                    return !isEnemy;
                case SkillTargetType.Self:
                    return false; // Self-targeting handled separately
                case SkillTargetType.SingleAny:
                    return true;
                case SkillTargetType.AllAny:
                    return true;
                default:
                    return false;
            }
        }
        
        /// <summary>
        /// Select a target
        /// </summary>
        public void SelectTarget(ICombatant target)
        {
            if (selectedSkill == null) return;
            
            // Handle different targeting types
            switch (selectedSkill.TargetType)
            {
                case SkillTargetType.SingleEnemy:
                case SkillTargetType.SingleAlly:
                case SkillTargetType.SingleAny:
                    selectedTargets.Clear();
                    selectedTargets.Add(target);
                    ExecuteSkill();
                    break;
                    
                case SkillTargetType.AllEnemies:
                case SkillTargetType.AllAllies:
                case SkillTargetType.AllAny:
                    // For AoE skills, execute immediately
                    var validTargets = GetValidTargets(selectedSkill);
                    selectedTargets.Clear();
                    selectedTargets.AddRange(validTargets);
                    ExecuteSkill();
                    break;
                    
                case SkillTargetType.Self:
                    selectedTargets.Clear();
                    selectedTargets.Add(selectedPlayerCard.Combatant);
                    ExecuteSkill();
                    break;
            }
        }
        
        /// <summary>
        /// Execute the selected skill
        /// </summary>
        private void ExecuteSkill()
        {
            if (selectedSkill == null || selectedPlayerCard == null || selectedTargets.Count == 0)
                return;
            
            var caster = selectedPlayerCard.Combatant;
            
            // Execute the skill
            SkillEffects.ExecuteSkill(selectedSkill, caster, selectedTargets, statusEffectSystem);
            
            // Add to combat log
            AddToCombatLog($"{caster.Name} uses {selectedSkill.SkillName}!");
            
            // Exit targeting mode
            CancelTargeting();
            
            // Refresh UI
            RefreshUI();
        }
        
        /// <summary>
        /// Cancel targeting mode
        /// </summary>
        private void CancelTargeting()
        {
            isTargetingMode = false;
            selectedSkill = null;
            selectedTargets.Clear();
            
            if (targetingPanel != null)
                targetingPanel.SetActive(false);
            
            UpdateCombatInfo();
            UpdateButtonStates();
        }
        
        /// <summary>
        /// Use auto attack
        /// </summary>
        private void UseAutoAttack()
        {
            if (selectedPlayerCard == null) return;
            
            var caster = selectedPlayerCard.Combatant;
            var basicAttack = caster.GetAllSkills().Find(s => s.SkillName.ToLower().Contains("attack"));
            
            if (basicAttack != null)
            {
                // Auto-attack targets the first enemy
                var enemies = enemyCards.Select(c => c.Combatant).Where(c => c.IsAlive).ToList();
                if (enemies.Count > 0)
                {
                    var targets = new List<ICombatant> { enemies[0] };
                    SkillEffects.ExecuteSkill(basicAttack, caster, targets, statusEffectSystem);
                    
                    AddToCombatLog($"{caster.Name} attacks {enemies[0].Name}!");
                    RefreshUI();
                }
            }
        }
        
        /// <summary>
        /// End the current turn
        /// </summary>
        private void EndTurn()
        {
            if (turnOrderSystem != null)
            {
                turnOrderSystem.EndCurrentTurn();
            }
        }
        
        /// <summary>
        /// Add message to combat log
        /// </summary>
        private void AddToCombatLog(string message)
        {
            combatLog += $"\n{message}";
            
            // Keep log manageable
            if (combatLog.Length > 1000)
            {
                combatLog = combatLog.Substring(combatLog.Length - 1000);
            }
            
            if (combatLogText != null)
            {
                combatLogText.text = combatLog;
            }
        }
        
        #region Event Handlers
        
        private void OnTurnChanged(ICombatant combatant)
        {
            UpdateCombatInfo();
        }
        
        private void OnCombatantTurnStart(ICombatant combatant)
        {
            AddToCombatLog($"{combatant.Name}'s turn starts!");
            UpdateCombatInfo();
        }
        
        private void OnCombatantTurnEnd(ICombatant combatant)
        {
            AddToCombatLog($"{combatant.Name}'s turn ends!");
            UpdateCombatInfo();
        }
        
        #endregion
        
        #region Unity Lifecycle
        
        private void OnDestroy()
        {
            // Clean up event listeners
            if (turnOrderSystem != null)
            {
                turnOrderSystem.OnTurnChanged.RemoveListener(OnTurnChanged);
                turnOrderSystem.OnCombatantTurnStart.RemoveListener(OnCombatantTurnStart);
                turnOrderSystem.OnCombatantTurnEnd.RemoveListener(OnCombatantTurnEnd);
            }
            
            // Clean up button listeners
            if (endTurnButton != null)
                endTurnButton.onClick.RemoveListener(EndTurn);
            
            if (autoAttackButton != null)
                autoAttackButton.onClick.RemoveListener(UseAutoAttack);
            
            if (cancelTargetingButton != null)
                cancelTargetingButton.onClick.RemoveListener(CancelTargeting);
        }
        
        #endregion
    }
    
    /// <summary>
    /// UI component for displaying a player combatant
    /// </summary>
    public class PlayerCombatantCard : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Image selectionIndicator;
        [SerializeField] private Button selectButton;
        
        private Combatant combatant;
        private PartyCombatUI combatUI;
        private bool isSelected = false;
        
        public Combatant Combatant => combatant;
        
        public void Initialize(Combatant combatant, PartyCombatUI ui)
        {
            this.combatant = combatant;
            this.combatUI = ui;
            
            if (selectButton != null)
                selectButton.onClick.AddListener(OnSelect);
            
            UpdateUI();
        }
        
        public void SetSelected(bool selected)
        {
            isSelected = selected;
            
            if (selectionIndicator != null)
                selectionIndicator.gameObject.SetActive(selected);
        }
        
        private void OnSelect()
        {
            combatUI.SelectPlayerCombatant(this);
        }
        
        private void UpdateUI()
        {
            if (combatant == null) return;
            
            if (nameText != null)
                nameText.text = combatant.Name;
            
            if (healthText != null)
                healthText.text = $"{combatant.CurrentHealth}/{combatant.MaxHealth}";
            
            if (healthSlider != null)
                healthSlider.value = combatant.HealthPercentage;
        }
    }
    
    /// <summary>
    /// UI component for displaying an enemy combatant
    /// </summary>
    public class EnemyCombatantCard : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private Slider healthSlider;
        
        private Combatant combatant;
        private PartyCombatUI combatUI;
        
        public Combatant Combatant => combatant;
        
        public void Initialize(Combatant combatant, PartyCombatUI ui)
        {
            this.combatant = combatant;
            this.combatUI = ui;
            
            UpdateUI();
        }
        
        private void UpdateUI()
        {
            if (combatant == null) return;
            
            if (nameText != null)
                nameText.text = combatant.Name;
            
            if (healthText != null)
                healthText.text = $"{combatant.CurrentHealth}/{combatant.MaxHealth}";
            
            if (healthSlider != null)
                healthSlider.value = combatant.HealthPercentage;
        }
    }
    
    /// <summary>
    /// UI component for skill buttons
    /// </summary>
    public class SkillButton : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI skillNameText;
        [SerializeField] private TextMeshProUGUI cooldownText;
        [SerializeField] private Button button;
        [SerializeField] private Image cooldownOverlay;
        
        private IBaseSkill skill;
        private PartyCombatUI combatUI;
        
        public void Initialize(IBaseSkill skill, PartyCombatUI ui)
        {
            this.skill = skill;
            this.combatUI = ui;
            
            if (button != null)
                button.onClick.AddListener(OnClick);
            
            UpdateUI();
        }
        
        private void OnClick()
        {
            combatUI.StartTargeting(skill);
        }
        
        private void UpdateUI()
        {
            if (skill == null) return;
            
            if (skillNameText != null)
                skillNameText.text = skill.SkillName;
            
            if (cooldownText != null)
                cooldownText.text = skill.CurrentCooldown > 0 ? skill.CurrentCooldown.ToString() : "";
            
            if (button != null)
                button.interactable = skill.CanUse(null);
            
            if (cooldownOverlay != null)
                cooldownOverlay.gameObject.SetActive(skill.CurrentCooldown > 0);
        }
    }
    
    /// <summary>
    /// UI component for target selection buttons
    /// </summary>
    public class TargetButton : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private Button button;
        
        private ICombatant target;
        private PartyCombatUI combatUI;
        
        public void Initialize(ICombatant target, PartyCombatUI ui)
        {
            this.target = target;
            this.combatUI = ui;
            
            if (button != null)
                button.onClick.AddListener(OnClick);
            
            UpdateUI();
        }
        
        private void OnClick()
        {
            combatUI.SelectTarget(target);
        }
        
        private void UpdateUI()
        {
            if (target == null) return;
            
            if (nameText != null)
                nameText.text = target.Name;
            
            if (healthText != null)
            {
                // ICombatant doesn't have CurrentHealth/MaxHealth, so we'll use a placeholder
                healthText.text = "HP: ???/???";
            }
        }
    }
} 