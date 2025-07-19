using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GuildsOfArcanaTerra.ScriptableObjects.Classes;
using GuildsOfArcanaTerra.Combat.Effects;
using GuildsOfArcanaTerra.Traits;
using GuildsOfArcanaTerra.Combat.Core;
using GuildsOfArcanaTerra.Combat.Skills;
using GuildsOfArcanaTerra.Combat.Skills.Interfaces;

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
        
        [Header("IRL Trait")]
        [SerializeField] private ScriptableObject traitAsset; // For designer assignment (optional)
        private IRLTrait irlTrait;

        // Trait modifiers
        private float defenseModifier = 1f;
        private float damageModifier = 1f;
        private float critModifier = 1f;
        private bool alwaysActsLast = false;
        private bool overdrive = false;
        
        // Events
        public UnityEvent<Combatant> OnDamageTaken;
        public UnityEvent<Combatant> OnHealed;
        public UnityEvent<Combatant> OnTurnStarted;
        public UnityEvent<Combatant> OnTurnEnded;
        public UnityEvent<Combatant> OnSkillUsed;
        public UnityEvent<Combatant> OnDeath;
        
        // ICombatant Implementation
        public string Name => characterName;
        public string CharacterName => characterName;
        public int AGI => agility;
        public int INT => intelligence;
        public int Agility => agility;
        public int Intelligence => intelligence;
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
        public IRLTrait Trait => irlTrait;
        public float DefenseModifier => defenseModifier;
        public float DamageModifier => damageModifier;
        public float CritModifier => critModifier;
        public bool AlwaysActsLast => alwaysActsLast;
        public bool Overdrive => overdrive;
        
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

            // Assign trait (for now, create manually or from asset)
            if (traitAsset is IRLTrait trait)
                irlTrait = trait;
            // For testing, you can assign a trait here:
            // irlTrait = new GlassCannonMainTrait();
            // irlTrait = new AFKFarmerTrait();
            // irlTrait = new DramaQueenTrait();

            irlTrait?.ApplyPreCombat(this);
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
            
            if (classDefinition == null)
            {
                Debug.LogError($"Combatant {characterName}: Class definition is null!");
                return;
            }
            
            // Clear existing skills
            skillSet.ClearSkills();
            
            // Add basic attack
            if (classDefinition.BasicAttack != null)
            {
                var basicSkill = CreateSkillFromDefinition(classDefinition.BasicAttack);
                if (basicSkill != null)
                {
                    skillSet.AddSkill(basicSkill);
                }
            }
            
            // Add active skills
            if (classDefinition.ActiveSkills != null)
            {
                foreach (var skillDef in classDefinition.ActiveSkills)
                {
                    if (skillDef != null)
                    {
                        var activeSkill = CreateSkillFromDefinition(skillDef);
                        if (activeSkill != null)
                        {
                            skillSet.AddSkill(activeSkill);
                        }
                    }
                }
            }
            
            // Note: Passive skills are handled separately through the class definition
            // They don't go into the SkillSet since they're always active
            
            if (debugMode)
            {
                Debug.Log($"Combatant {characterName}: Loaded {skillSet.SkillCount} skills from {classDefinition?.ClassName ?? "Unknown Class"}");
            }
        }
        
        /// <summary>
        /// Create a Skill object from a SkillDefinition
        /// </summary>
        private IBaseSkill CreateSkillFromDefinition(SkillDefinition skillDef)
        {
            if (skillDef == null)
            {
                Debug.LogWarning($"Combatant {characterName}: Cannot create skill from null definition!");
                return null;
            }
            
            var skill = SkillFactory.CreateSkillFromDefinition(skillDef);
            
            if (skill != null && skillSet != null)
            {
                // Subscribe to skill events
                skillSet.OnSkillUsed.AddListener((usedSkill) => OnSkillUsed?.Invoke(this));
            }
            
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
            
            irlTrait?.OnSkillUsed(this, skillSet.GetSkill(skillName));
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
        public List<IBaseSkill> GetAvailableSkills()
        {
            if (skillSet == null) return new List<IBaseSkill>();
            
            return skillSet.GetAvailableSkills();
        }
        
        /// <summary>
        /// Get all skills including those on cooldown
        /// </summary>
        public List<IBaseSkill> GetAllSkills()
        {
            if (skillSet == null) return new List<IBaseSkill>();
            
            return skillSet.GetAllSkills();
        }
        
        /// <summary>
        /// Take damage from attacks or effects
        /// </summary>
        public void TakeDamage(int damage)
        {
            if (!IsAlive) return;
            
            // Apply defense modifier
            float finalDamage = damage * (1f - (defenseModifier - 1f));
            int actualDamage = Mathf.RoundToInt(finalDamage);
            
            currentHealth = Mathf.Max(0, currentHealth - actualDamage);
            
            if (debugMode)
            {
                Debug.Log($"Combatant {characterName}: Took {actualDamage} damage (base: {damage}, defense mod: {defenseModifier:F2}) - HP: {currentHealth}/{maxHealth}");
            }
            
            OnDamageTaken?.Invoke(this);
            
            // Check for death
            if (currentHealth <= 0)
            {
                OnDeath?.Invoke(this);
                if (debugMode)
                {
                    Debug.Log($"Combatant {characterName}: Died!");
                }
            }
        }
        
        /// <summary>
        /// Heal the combatant
        /// </summary>
        public void Heal(int healAmount)
        {
            if (!IsAlive) return;
            
            int oldHealth = currentHealth;
            currentHealth = Mathf.Min(maxHealth, currentHealth + healAmount);
            int actualHeal = currentHealth - oldHealth;
            
            if (debugMode)
            {
                Debug.Log($"Combatant {characterName}: Healed for {actualHeal} HP - HP: {currentHealth}/{maxHealth}");
            }
            
            OnHealed?.Invoke(this);
        }
        
        /// <summary>
        /// Called when this unit's turn begins
        /// </summary>
        public virtual void OnTurnStart()
        {
            if (debugMode)
            {
                Debug.Log($"{characterName}'s turn starts!");
            }
            
            irlTrait?.OnTurnStart(this);
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
                    skill.CurrentCooldown = turns;
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
                if (skill.CurrentCooldown > 0)
                {
                    info += $" (CD: {skill.CurrentCooldown})";
                }
                info += "\n";
            }
            
            return info;
        }

        // --- Trait Modifier Setters ---
        public void SetDefenseModifier(float value) => defenseModifier = value;
        public void SetDamageModifier(float value) => damageModifier = value;
        public void SetCritModifier(float value) => critModifier = value;
        public void SetAlwaysActsLast(bool value) => alwaysActsLast = value;
        public void SetOverdrive(bool value) => overdrive = value;

        // --- Combat Flow Integration ---
        // Example: apply defense modifier in damage calculation
        public void TakeDamageWithModifiers(int damage)
        {
            int actualDamage = Mathf.Max(1, Mathf.RoundToInt((damage - defense) * defenseModifier));
            currentHealth = Mathf.Max(0, currentHealth - actualDamage);
            // ... rest of TakeDamage logic ...
        }

        // Example: apply damage modifier in skill use
        public int CalculateSkillDamage(int baseDamage)
        {
            return Mathf.RoundToInt(baseDamage * damageModifier * (overdrive ? 1.5f : 1f));
        }

        // Example: apply crit modifier in crit chance calculation
        public float CalculateCritChance(float baseCrit)
        {
            return baseCrit * critModifier;
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