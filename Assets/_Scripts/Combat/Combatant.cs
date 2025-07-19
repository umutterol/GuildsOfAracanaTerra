using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GuildsOfArcanaTerra.ScriptableObjects.Classes;
using GuildsOfArcanaTerra.Combat.Effects;

namespace GuildsOfArcanaTerra.Combat
{
    /// <summary>
    /// Enhanced combatant that loads skills from CharacterClassDefinition ScriptableObjects
    /// Integrates with TurnOrderSystem, SkillSystem, and StatusEffectSystem
    /// </summary>
    public class Combatant : MonoBehaviour, ICombatant
    {
        [Header("Class Definition")]
        [SerializeField] private CharacterClassDefinition classDefinition;
        
        [Header("Combat Stats")]
        [SerializeField] private string characterName = "Unnamed";
        [SerializeField] private int level = 1;
        [SerializeField] private int currentHealth;
        [SerializeField] private int maxHealth;
        
        [Header("Base Stats (from class + level scaling)")]
        [SerializeField] private int strength;
        [SerializeField] private int agility;
        [SerializeField] private int intelligence;
        [SerializeField] private int defense;
        [SerializeField] private int vitality;
        
        [Header("Systems")]
        [SerializeField] private SkillSet skillSet;
        [SerializeField] private StatusEffectManager statusEffectManager;
        
        [Header("Debug")]
        [SerializeField] private bool debugMode = true;
        
        // Events
        public UnityEvent<Combatant> OnDamageTaken;
        public UnityEvent<Combatant> OnHealed;
        public UnityEvent<Combatant> OnTurnStarted;
        public UnityEvent<Combatant> OnTurnEnded;
        public UnityEvent<Combatant> OnSkillUsed;
        public UnityEvent<Combatant> OnDeath;
        
        // ICombatant Implementation
        public string Name => characterName;
        public int AGI => agility;
        public int INT => intelligence;
        public bool IsAlive => currentHealth > 0;
        
        // Cooldowns (for ICombatant interface)
        private Dictionary<string, int> cooldowns = new Dictionary<string, int>();
        public Dictionary<string, int> Cooldowns => cooldowns;
        
        // Properties
        public CharacterClassDefinition ClassDefinition => classDefinition;
        public int CurrentHealth => currentHealth;
        public int MaxHealth => maxHealth;
        public float HealthPercentage => maxHealth > 0 ? (float)currentHealth / maxHealth : 0f;
        public int Level => level;
        public int Strength => strength;
        public int Defense => defense;
        public int Vitality => vitality;
        public SkillSet SkillSet => skillSet;
        public StatusEffectManager StatusEffectManager => statusEffectManager;
        
        private void Awake()
        {
            // Ensure required components exist
            if (skillSet == null)
                skillSet = GetComponent<SkillSet>();
            if (skillSet == null)
                skillSet = gameObject.AddComponent<SkillSet>();
                
            if (statusEffectManager == null)
                statusEffectManager = GetComponent<StatusEffectManager>();
            if (statusEffectManager == null)
                statusEffectManager = gameObject.AddComponent<StatusEffectManager>();
        }
        
        private void Start()
        {
            // Load class definition if assigned
            if (classDefinition != null)
            {
                LoadClassDefinition();
            }
            else
            {
                Debug.LogWarning($"Combatant {characterName}: No class definition assigned!");
            }
        }
        
        /// <summary>
        /// Load skills and stats from the assigned CharacterClassDefinition
        /// </summary>
        public void LoadClassDefinition()
        {
            if (classDefinition == null)
            {
                Debug.LogError($"Combatant {characterName}: Cannot load null class definition!");
                return;
            }
            
            // Set character name if not already set
            if (string.IsNullOrEmpty(characterName) || characterName == "Unnamed")
            {
                characterName = classDefinition.ClassName;
            }
            
            // Load base stats from class definition
            LoadBaseStats();
            
            // Load skills from class definition
            LoadSkills();
            
            if (debugMode)
            {
                Debug.Log($"Combatant {characterName}: Loaded {classDefinition.ClassName} class with {skillSet.SkillCount} skills");
            }
        }
        
        /// <summary>
        /// Load base stats from the class definition with level scaling
        /// </summary>
        private void LoadBaseStats()
        {
            // Base stats from class definition
            strength = classDefinition.BaseStrength;
            agility = classDefinition.BaseAgility;
            intelligence = classDefinition.BaseIntelligence;
            defense = classDefinition.BaseDefense;
            vitality = classDefinition.BaseVitality;
            
            // Apply level scaling (simple linear scaling)
            float levelMultiplier = 1f + (level - 1) * 0.1f; // 10% increase per level
            
            strength = Mathf.RoundToInt(strength * levelMultiplier);
            agility = Mathf.RoundToInt(agility * levelMultiplier);
            intelligence = Mathf.RoundToInt(intelligence * levelMultiplier);
            defense = Mathf.RoundToInt(defense * levelMultiplier);
            vitality = Mathf.RoundToInt(vitality * levelMultiplier);
            
            // Set health based on vitality and class base health
            maxHealth = classDefinition.BaseHealth + (vitality * 10);
            currentHealth = maxHealth;
            
            if (debugMode)
            {
                Debug.Log($"Combatant {characterName}: Stats loaded - STR:{strength} AGI:{agility} INT:{intelligence} DEF:{defense} VIT:{vitality} HP:{maxHealth}");
            }
        }
        
        /// <summary>
        /// Load skills from the class definition into the SkillSet
        /// </summary>
        private void LoadSkills()
        {
            if (skillSet == null)
            {
                Debug.LogError($"Combatant {characterName}: SkillSet component not found!");
                return;
            }
            
            // Clear existing skills
            skillSet.ClearSkills();
            
            // Add basic attack
            if (classDefinition.BasicAttack != null)
            {
                var basicSkill = CreateSkillFromDefinition(classDefinition.BasicAttack);
                skillSet.AddSkill(basicSkill);
            }
            
            // Add active skills
            foreach (var skillDef in classDefinition.ActiveSkills)
            {
                if (skillDef != null)
                {
                    var activeSkill = CreateSkillFromDefinition(skillDef);
                    skillSet.AddSkill(activeSkill);
                }
            }
            
            // Note: Passive skills are handled separately through the class definition
            // They don't go into the SkillSet since they're always active
            
            if (debugMode)
            {
                Debug.Log($"Combatant {characterName}: Loaded {skillSet.SkillCount} skills from {classDefinition.ClassName}");
            }
        }
        
        /// <summary>
        /// Create a Skill object from a SkillDefinition
        /// </summary>
        private Skill CreateSkillFromDefinition(SkillDefinition skillDef)
        {
            var skillType = skillDef.Type == GuildsOfArcanaTerra.ScriptableObjects.Classes.SkillType.Basic ? Combat.SkillType.Basic : Combat.SkillType.Active;
            var skill = new Skill(skillDef.SkillName, skillType, skillDef.Cooldown);
            
            // Subscribe to skill events
            skill.OnSkillUsed.AddListener((usedSkill) => OnSkillUsed?.Invoke(this));
            
            return skill;
        }
        
        /// <summary>
        /// Use a skill by name
        /// </summary>
        public bool UseSkill(string skillName)
        {
            if (!IsAlive)
            {
                if (debugMode)
                    Debug.LogWarning($"Combatant {characterName}: Cannot use skill {skillName} - not alive!");
                return false;
            }
            
            if (skillSet == null)
            {
                if (debugMode)
                    Debug.LogError($"Combatant {characterName}: No SkillSet component!");
                return false;
            }
            
            bool success = skillSet.UseSkill(skillName);
            
            if (success && debugMode)
            {
                Debug.Log($"Combatant {characterName}: Used skill {skillName}");
            }
            
            return success;
        }
        
        /// <summary>
        /// Use a skill by index
        /// </summary>
        public bool UseSkill(int index)
        {
            if (!IsAlive) return false;
            
            if (skillSet == null) return false;
            
            bool success = skillSet.UseSkill(index);
            
            if (success && debugMode)
            {
                var skill = skillSet.GetSkill(index);
                if (skill != null)
                {
                    Debug.Log($"Combatant {characterName}: Used skill {skill.SkillName} at index {index}");
                }
            }
            
            return success;
        }
        
        /// <summary>
        /// Get available skills (not on cooldown)
        /// </summary>
        public List<Skill> GetAvailableSkills()
        {
            if (skillSet == null) return new List<Skill>();
            
            return skillSet.GetAvailableSkills();
        }
        
        /// <summary>
        /// Get all skills including those on cooldown
        /// </summary>
        public List<Skill> GetAllSkills()
        {
            if (skillSet == null) return new List<Skill>();
            
            return skillSet.GetAllSkills();
        }
        
        /// <summary>
        /// Take damage from attacks or effects
        /// </summary>
        public void TakeDamage(int damage)
        {
            if (!IsAlive) return;
            
            // Apply defense reduction
            int actualDamage = Mathf.Max(1, damage - defense);
            
            currentHealth = Mathf.Max(0, currentHealth - actualDamage);
            
            if (debugMode)
            {
                Debug.Log($"{characterName} takes {actualDamage} damage (reduced from {damage} by {defense} DEF)! Health: {currentHealth}/{maxHealth}");
            }
            
            OnDamageTaken?.Invoke(this);
            
            if (!IsAlive)
            {
                if (debugMode)
                {
                    Debug.Log($"{characterName} has been defeated!");
                }
                OnDeath?.Invoke(this);
            }
        }
        
        /// <summary>
        /// Heal the combatant
        /// </summary>
        public void Heal(int amount)
        {
            if (!IsAlive) return;
            
            int oldHealth = currentHealth;
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            int actualHeal = currentHealth - oldHealth;
            
            if (actualHeal > 0)
            {
                if (debugMode)
                {
                    Debug.Log($"{characterName} heals {actualHeal} health! Health: {currentHealth}/{maxHealth}");
                }
                
                OnHealed?.Invoke(this);
            }
        }
        
        /// <summary>
        /// Called when this unit's turn begins
        /// </summary>
        public void OnTurnStart()
        {
            if (debugMode)
            {
                Debug.Log($"{characterName}'s turn starts!");
            }
            
            OnTurnStarted?.Invoke(this);
        }
        
        /// <summary>
        /// Called when this unit's turn ends
        /// </summary>
        public void OnTurnEnd()
        {
            if (debugMode)
            {
                Debug.Log($"{characterName}'s turn ends!");
            }
            
            OnTurnEnded?.Invoke(this);
        }
        
        /// <summary>
        /// Set a skill cooldown (for ICombatant interface compatibility)
        /// </summary>
        public void SetCooldown(string skillName, int turns)
        {
            cooldowns[skillName] = turns;
            
            // Also update the SkillSet
            if (skillSet != null)
            {
                var skill = skillSet.GetSkill(skillName);
                if (skill != null)
                {
                    skill.SetCooldown(turns);
                }
            }
        }
        
        /// <summary>
        /// Reset health to full
        /// </summary>
        public void ResetHealth()
        {
            currentHealth = maxHealth;
            if (debugMode)
            {
                Debug.Log($"{characterName} health reset to {currentHealth}/{maxHealth}");
            }
        }
        
        /// <summary>
        /// Get a string representation of the combatant's status
        /// </summary>
        public override string ToString()
        {
            return $"{characterName} ({classDefinition?.ClassName ?? "No Class"}) - HP: {currentHealth}/{maxHealth}, AGI: {AGI}, INT: {INT}, Alive: {IsAlive}";
        }
        
        /// <summary>
        /// Get a detailed status string for UI display
        /// </summary>
        public string GetStatusString()
        {
            var status = $"{characterName} ({classDefinition?.ClassName ?? "No Class"})\n";
            status += $"HP: {currentHealth}/{maxHealth} ({HealthPercentage:P0})\n";
            status += $"STR: {strength} | AGI: {agility} | INT: {intelligence}\n";
            status += $"DEF: {defense} | VIT: {vitality} | Level: {level}";
            
            return status;
        }
        
        /// <summary>
        /// Get skill information for UI display
        /// </summary>
        public string GetSkillInfoString()
        {
            if (skillSet == null) return "No skills loaded";
            
            var info = "Skills:\n";
            var skills = skillSet.GetAllSkills();
            
            foreach (var skill in skills)
            {
                info += $"• {skill.SkillName}";
                if (skill.IsOnCooldown)
                {
                    info += $" (CD: {skill.CurrentCooldown})";
                }
                info += "\n";
            }
            
            return info;
        }
        
        #region Unity Lifecycle
        
        private void OnDestroy()
        {
            // Clean up events
            OnDamageTaken?.RemoveAllListeners();
            OnHealed?.RemoveAllListeners();
            OnTurnStarted?.RemoveAllListeners();
            OnTurnEnded?.RemoveAllListeners();
            OnSkillUsed?.RemoveAllListeners();
            OnDeath?.RemoveAllListeners();
        }
        
        #endregion
    }
} 