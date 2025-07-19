using UnityEngine;

namespace GuildsOfArcanaTerra.ScriptableObjects.Classes
{
    /// <summary>
    /// Represents a single skill with its properties
    /// </summary>
    [System.Serializable]
    public class SkillDefinition
    {
        [Header("Skill Info")]
        [SerializeField] private string skillName;
        [SerializeField] private int cooldown;
        [SerializeField] private string description;
        [SerializeField] private SkillType skillType;
        [SerializeField] private SkillTargetType targetType;
        [SerializeField] private SkillEffectType effectType;
        
        // Properties
        public string SkillName => skillName;
        public int Cooldown => cooldown;
        public string Description => description;
        public SkillType Type => skillType;
        public SkillTargetType TargetType => targetType;
        public SkillEffectType EffectType => effectType;
        
        /// <summary>
        /// Constructor for creating skill definitions
        /// </summary>
        public SkillDefinition(string name, int cd, string desc, SkillType type, SkillTargetType target, SkillEffectType effect)
        {
            skillName = name;
            cooldown = cd;
            description = desc;
            skillType = type;
            targetType = target;
            effectType = effect;
        }
    }
    
    /// <summary>
    /// Types of skills
    /// </summary>
    public enum SkillType
    {
        Basic,      // 0-CD basic attack
        Active,     // Skills with cooldowns
        Passive     // Always active effects
    }
    
    /// <summary>
    /// Target types for skills
    /// </summary>
    public enum SkillTargetType
    {
        SingleEnemy,    // One enemy
        MultipleEnemies, // Multiple enemies (AoE)
        SingleAlly,     // One ally
        MultipleAllies, // Multiple allies
        Self,           // Self only
        AllEnemies,     // All enemies
        AllAllies,      // All allies
        Hybrid          // Mixed targeting (both enemies and allies)
    }
    
    /// <summary>
    /// Effect types for skills
    /// </summary>
    public enum SkillEffectType
    {
        Damage,         // Direct damage
        Heal,           // Healing
        Buff,           // Positive status effect
        Debuff,         // Negative status effect
        Utility,        // Utility effects (positioning, etc.)
        Hybrid          // Multiple effect types
    }
    
    /// <summary>
    /// ScriptableObject that defines a character class with all its skills and properties
    /// Based on GDD class definitions
    /// </summary>
    [CreateAssetMenu(fileName = "NewClass", menuName = "Guilds of Arcana Terra/Character Class")]
    public class CharacterClassDefinition : ScriptableObject
    {
        [Header("Class Identity")]
        [SerializeField] private string className;
        [SerializeField] private Sprite classIcon;
        [SerializeField] private string classDescription;
        [SerializeField] private Color classColor = Color.white;
        
        [Header("Base Stats")]
        [SerializeField] private int baseStrength = 10;
        [SerializeField] private int baseAgility = 10;
        [SerializeField] private int baseIntelligence = 10;
        [SerializeField] private int baseDefense = 5;
        [SerializeField] private int baseVitality = 10;
        [SerializeField] private int baseHealth = 100;
        
        [Header("Skills")]
        [SerializeField] private SkillDefinition basicAttack;
        [SerializeField] private SkillDefinition[] activeSkills = new SkillDefinition[4];
        [SerializeField] private SkillDefinition passiveSkill;
        
        [Header("Class Specialization")]
        [SerializeField] private string primaryStat = "STR";
        [SerializeField] private string secondaryStat = "AGI";
        [SerializeField] private string classRole = "DPS";
        
        // Properties
        public string ClassName => className;
        public Sprite ClassIcon => classIcon;
        public string ClassDescription => classDescription;
        public Color ClassColor => classColor;
        
        public int BaseStrength => baseStrength;
        public int BaseAgility => baseAgility;
        public int BaseIntelligence => baseIntelligence;
        public int BaseDefense => baseDefense;
        public int BaseVitality => baseVitality;
        public int BaseHealth => baseHealth;
        
        public SkillDefinition BasicAttack => basicAttack;
        public SkillDefinition[] ActiveSkills => activeSkills;
        public SkillDefinition PassiveSkill => passiveSkill;
        
        public string PrimaryStat => primaryStat;
        public string SecondaryStat => secondaryStat;
        public string ClassRole => classRole;
        
        /// <summary>
        /// Get all skills for this class (basic + active + passive)
        /// </summary>
        public SkillDefinition[] GetAllSkills()
        {
            var allSkills = new System.Collections.Generic.List<SkillDefinition>();
            
            if (basicAttack != null)
                allSkills.Add(basicAttack);
            
            foreach (var skill in activeSkills)
            {
                if (skill != null)
                    allSkills.Add(skill);
            }
            
            if (passiveSkill != null)
                allSkills.Add(passiveSkill);
            
            return allSkills.ToArray();
        }
        
        /// <summary>
        /// Get a skill by name
        /// </summary>
        public SkillDefinition GetSkillByName(string skillName)
        {
            if (basicAttack != null && basicAttack.SkillName == skillName)
                return basicAttack;
            
            foreach (var skill in activeSkills)
            {
                if (skill != null && skill.SkillName == skillName)
                    return skill;
            }
            
            if (passiveSkill != null && passiveSkill.SkillName == skillName)
                return passiveSkill;
            
            return null;
        }
        
        /// <summary>
        /// Validate that the class definition is complete
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(className))
                return false;
            
            if (basicAttack == null)
                return false;
            
            // Check that we have at least 1 active skill
            bool hasActiveSkill = false;
            foreach (var skill in activeSkills)
            {
                if (skill != null)
                {
                    hasActiveSkill = true;
                    break;
                }
            }
            
            if (!hasActiveSkill)
                return false;
            
            if (passiveSkill == null)
                return false;
            
            return true;
        }
        
        /// <summary>
        /// Get a summary of the class for debugging
        /// </summary>
        public string GetClassSummary()
        {
            var summary = $"Class: {className}\n";
            summary += $"Role: {classRole}\n";
            summary += $"Primary Stat: {primaryStat}\n";
            summary += $"Base Stats - STR:{baseStrength} AGI:{baseAgility} INT:{baseIntelligence} DEF:{baseDefense} VIT:{baseVitality}\n";
            summary += $"Skills:\n";
            
            if (basicAttack != null)
                summary += $"  Basic: {basicAttack.SkillName} (CD: {basicAttack.Cooldown})\n";
            
            foreach (var skill in activeSkills)
            {
                if (skill != null)
                    summary += $"  Active: {skill.SkillName} (CD: {skill.Cooldown})\n";
            }
            
            if (passiveSkill != null)
                summary += $"  Passive: {passiveSkill.SkillName}\n";
            
            return summary;
        }
    }
} 