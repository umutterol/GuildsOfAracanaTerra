using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic; // Added for List
using GuildsOfArcanaTerra.Combat.Skills.Interfaces;

namespace GuildsOfArcanaTerra.Combat.UI
{
    /// <summary>
    /// Simple example of how to connect basic UI elements to the combat system
    /// This is a minimal implementation for quick testing
    /// </summary>
    public class SimpleCombatUI : MonoBehaviour
    {
        [Header("Combatants")]
        [SerializeField] private Combatant player;
        [SerializeField] private Combatant enemy;
        
        [Header("Systems")]
        [SerializeField] private StatusEffectSystem statusEffectSystem;
        
        [Header("Basic UI Elements")]
        [SerializeField] private TextMeshProUGUI playerHealthText;
        [SerializeField] private TextMeshProUGUI enemyHealthText;
        [SerializeField] private TextMeshProUGUI combatLogText;
        
        [Header("Action Buttons")]
        [SerializeField] private Button attackButton;
        [SerializeField] private Button healButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private Button endTurnButton;
        
        [Header("Skill Buttons")]
        [SerializeField] private Button[] skillButtons = new Button[4];
        [SerializeField] private TextMeshProUGUI[] skillTexts = new TextMeshProUGUI[4];
        
        private string combatLog = "";
        private bool isPlayerTurn = true;
        private bool hasActedThisTurn = false;
        
        void Start()
        {
            SetupButtons();
            // Auto-find systems if not wired
            if (statusEffectSystem == null)
                statusEffectSystem = FindObjectOfType<StatusEffectSystem>();
            // Register combatants for status effects
            if (statusEffectSystem != null)
            {
                if (player != null) statusEffectSystem.RegisterCombatant(player);
                if (enemy != null) statusEffectSystem.RegisterCombatant(enemy);
            }
            UpdateUI();
        }
        
        void Update()
        {
            // Update UI every frame (for simple testing)
            UpdateUI();
        }
        
        /// <summary>
        /// Set up button click listeners
        /// </summary>
        void SetupButtons()
        {
            // Action buttons
            if (attackButton != null)
                attackButton.onClick.AddListener(UseBasicAttack);
            
            if (healButton != null)
                healButton.onClick.AddListener(UseHeal);
            
            if (resetButton != null)
                resetButton.onClick.AddListener(ResetCombat);
            
            if (endTurnButton != null)
                endTurnButton.onClick.AddListener(EndTurn);
            
            // Skill buttons
            for (int i = 0; i < skillButtons.Length; i++)
            {
                int index = i; // Capture the index
                if (skillButtons[i] != null)
                {
                    skillButtons[i].onClick.AddListener(() => UseSkill(index));
                }
            }
        }
        
        /// <summary>
        /// Update all UI elements
        /// </summary>
        void UpdateUI()
        {
            UpdateHealthTexts();
            UpdateSkillButtons();
            UpdateActionButtonsInteractivity();
        }
        
        /// <summary>
        /// Update health text displays
        /// </summary>
        void UpdateHealthTexts()
        {
            if (playerHealthText != null && player != null)
            {
                playerHealthText.text = $"Player: {player.CurrentHealth}/{player.MaxHealth}";
            }
            
            if (enemyHealthText != null && enemy != null)
            {
                enemyHealthText.text = $"Enemy: {enemy.CurrentHealth}/{enemy.MaxHealth}";
            }
        }
        
        /// <summary>
        /// Update skill buttons with current skills
        /// </summary>
        void UpdateSkillButtons()
        {
            if (player == null) return;
            
            var skills = player.GetAllSkills();
            
            for (int i = 0; i < skillButtons.Length; i++)
            {
                if (skillButtons[i] == null) continue;
                
                if (i < skills.Count)
                {
                    var skill = skills[i];
                    
                    // Update button text
                    if (skillTexts[i] != null)
                    {
                        skillTexts[i].text = skill.SkillName;
                    }
                    
                    // Update button interactability
                    bool canUse = isPlayerTurn && !hasActedThisTurn && skill.CanUse(player);
                    skillButtons[i].interactable = canUse;
                    
                    // Update button color
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
                    if (skillTexts[i] != null)
                        skillTexts[i].text = "Empty";
                    
                    skillButtons[i].interactable = false;
                }
            }
        }

        /// <summary>
        /// Update action buttons interactivity based on turn state
        /// </summary>
        void UpdateActionButtonsInteractivity()
        {
            bool canAct = isPlayerTurn && !hasActedThisTurn;
            if (attackButton != null) attackButton.interactable = canAct;
            if (healButton != null) healButton.interactable = canAct;
            if (resetButton != null) resetButton.interactable = true;
            if (endTurnButton != null) endTurnButton.interactable = isPlayerTurn; // only player ends their turn
        }
        
        /// <summary>
        /// Use basic attack
        /// </summary>
        void UseBasicAttack()
        {
            if (player == null || enemy == null) return;
            if (!isPlayerTurn || hasActedThisTurn) return;
            
            // Find basic attack skill
            var basicAttack = player.GetAllSkills().Find(s => s.SkillName.ToLower().Contains("attack"));
            if (basicAttack != null)
            {
                var targets = new List<ICombatant> { enemy };
                SkillEffects.ExecuteSkill(basicAttack, player, targets, statusEffectSystem);
                
                AddToLog($"{player.Name} attacks {enemy.Name}!");
                hasActedThisTurn = true;
                UpdateUI();
            }
        }
        
        /// <summary>
        /// Use heal skill
        /// </summary>
        void UseHeal()
        {
            if (player == null) return;
            if (!isPlayerTurn || hasActedThisTurn) return;
            
            // Find heal skill
            var healSkill = player.GetAllSkills().Find(s => s.SkillName.ToLower().Contains("heal"));
            if (healSkill != null)
            {
                var targets = new List<ICombatant> { player };
                SkillEffects.ExecuteSkill(healSkill, player, targets, statusEffectSystem);
                
                AddToLog($"{player.Name} heals themselves!");
                hasActedThisTurn = true;
                UpdateUI();
            }
        }
        
        /// <summary>
        /// Use skill by index
        /// </summary>
        void UseSkill(int skillIndex)
        {
            if (player == null || enemy == null) return;
            if (!isPlayerTurn || hasActedThisTurn) return;
            
            var skills = player.GetAllSkills();
            if (skillIndex >= skills.Count) return;
            
            var skill = skills[skillIndex];
            if (!skill.CanUse(player)) return;
            
            // Execute the skill
            var targets = new List<ICombatant> { enemy };
            SkillEffects.ExecuteSkill(skill, player, targets, statusEffectSystem);
            
            AddToLog($"{player.Name} uses {skill.SkillName}!");
            hasActedThisTurn = true;
            UpdateUI();
        }
        
        /// <summary>
        /// End the current turn and reduce cooldowns
        /// </summary>
        void EndTurn()
        {
            if (player == null) return;
            if (!isPlayerTurn) return;
            
            // Reduce cooldowns for the player
            if (player.SkillSet != null)
            {
                player.SkillSet.ReduceAllCooldowns();
                AddToLog($"{player.Name}'s turn ends - cooldowns reduced!");
            }

            // Tick effects on the enemy at start of their turn and reduce durations on player
            if (statusEffectSystem != null)
            {
                if (enemy != null) statusEffectSystem.TickCombatantEffects(enemy);
                if (player != null) statusEffectSystem.ReduceCombatantEffectDurations(player);
            }
            
            // Switch to enemy turn
            isPlayerTurn = false;
            hasActedThisTurn = false; // reset for next turn later

            // Enemy action
            EnemyTakeAction();

            // After enemy action, start player's next turn
            if (statusEffectSystem != null)
            {
                if (player != null) statusEffectSystem.TickCombatantEffects(player);
                if (enemy != null) statusEffectSystem.ReduceCombatantEffectDurations(enemy);
            }

            isPlayerTurn = true;
            hasActedThisTurn = false;
            AddToLog("Your turn begins!");

            // Update UI to reflect new states
            UpdateUI();
        }
        
        /// <summary>
        /// Reset combat to initial state
        /// </summary>
        void ResetCombat()
        {
            if (player != null)
            {
                player.ResetHealth();
                
                // Reset all cooldowns
                if (player.SkillSet != null)
                {
                    player.SkillSet.ResetAllCooldowns();
                }
            }
            
            if (enemy != null)
                enemy.ResetHealth();
            
            AddToLog("Combat reset!");
            UpdateUI();
        }

        /// <summary>
        /// Very simple enemy AI: choose any usable skill or fallback to basic attack
        /// </summary>
        void EnemyTakeAction()
        {
            if (enemy == null || player == null) return;

            // Reduce enemy cooldowns at start of their action if needed
            // (we already ticked effects above)

            var skills = enemy.GetAllSkills();
            IBaseSkill chosen = null;
            // Prefer any skill that can be used
            foreach (var s in skills)
            {
                if (s.CanUse(enemy)) { chosen = s; break; }
            }
            // Fallback to a basic attack-like skill
            if (chosen == null)
            {
                chosen = skills.Find(s => s.SkillName.ToLower().Contains("attack"));
            }

            if (chosen != null)
            {
                var targets = new List<ICombatant> { player };
                SkillEffects.ExecuteSkill(chosen, enemy, targets, statusEffectSystem);
                AddToLog($"Enemy uses {chosen.SkillName}!");
            }

            // Reduce enemy cooldowns at end of their action
            if (enemy.SkillSet != null)
            {
                enemy.SkillSet.ReduceAllCooldowns();
            }
        }
        
        /// <summary>
        /// Add message to combat log
        /// </summary>
        void AddToLog(string message)
        {
            combatLog += $"\n{message}";
            
            // Keep log manageable
            if (combatLog.Length > 500)
            {
                combatLog = combatLog.Substring(combatLog.Length - 500);
            }
            
            if (combatLogText != null)
            {
                combatLogText.text = combatLog;
            }
        }
        
        /// <summary>
        /// Public method to manually update UI
        /// </summary>
        public void ForceUpdate()
        {
            UpdateUI();
        }
    }
} 