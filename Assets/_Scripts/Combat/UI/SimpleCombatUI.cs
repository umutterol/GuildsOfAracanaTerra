using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic; // Added for List

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
        
        [Header("Basic UI Elements")]
        [SerializeField] private TextMeshProUGUI playerHealthText;
        [SerializeField] private TextMeshProUGUI enemyHealthText;
        [SerializeField] private TextMeshProUGUI combatLogText;
        
        [Header("Action Buttons")]
        [SerializeField] private Button attackButton;
        [SerializeField] private Button healButton;
        [SerializeField] private Button resetButton;
        
        [Header("Skill Buttons")]
        [SerializeField] private Button[] skillButtons = new Button[4];
        [SerializeField] private TextMeshProUGUI[] skillTexts = new TextMeshProUGUI[4];
        
        private string combatLog = "";
        
        void Start()
        {
            SetupButtons();
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
                    bool canUse = skill.CanUse(player);
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
        /// Use basic attack
        /// </summary>
        void UseBasicAttack()
        {
            if (player == null || enemy == null) return;
            
            // Find basic attack skill
            var basicAttack = player.GetAllSkills().Find(s => s.SkillName.ToLower().Contains("attack"));
            if (basicAttack != null)
            {
                var targets = new List<ICombatant> { enemy };
                SkillEffects.ExecuteSkill(basicAttack, player, targets, null);
                
                AddToLog($"{player.Name} attacks {enemy.Name}!");
            }
        }
        
        /// <summary>
        /// Use heal skill
        /// </summary>
        void UseHeal()
        {
            if (player == null) return;
            
            // Find heal skill
            var healSkill = player.GetAllSkills().Find(s => s.SkillName.ToLower().Contains("heal"));
            if (healSkill != null)
            {
                var targets = new List<ICombatant> { player };
                SkillEffects.ExecuteSkill(healSkill, player, targets, null);
                
                AddToLog($"{player.Name} heals themselves!");
            }
        }
        
        /// <summary>
        /// Use skill by index
        /// </summary>
        void UseSkill(int skillIndex)
        {
            if (player == null || enemy == null) return;
            
            var skills = player.GetAllSkills();
            if (skillIndex >= skills.Count) return;
            
            var skill = skills[skillIndex];
            if (!skill.CanUse(player)) return;
            
            // Execute the skill
            var targets = new List<ICombatant> { enemy };
            SkillEffects.ExecuteSkill(skill, player, targets, null);
            
            AddToLog($"{player.Name} uses {skill.SkillName}!");
        }
        
        /// <summary>
        /// Reset combat to initial state
        /// </summary>
        void ResetCombat()
        {
            if (player != null)
                player.ResetHealth();
            
            if (enemy != null)
                enemy.ResetHealth();
            
            AddToLog("Combat reset!");
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