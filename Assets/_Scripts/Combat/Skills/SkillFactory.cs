using System.Collections.Generic;
using UnityEngine;
using GuildsOfArcanaTerra.Combat.Core;
using GuildsOfArcanaTerra.ScriptableObjects.Classes;
using GuildsOfArcanaTerra.Combat.Skills.Implementations;
using GuildsOfArcanaTerra.Combat.Skills.Effects;
using GuildsOfArcanaTerra.Combat.Skills.Interfaces;

namespace GuildsOfArcanaTerra.Combat.Skills
{
    /// <summary>
    /// Factory for creating different types of skills
    /// Centralizes skill creation logic and reduces hardcoded skill instantiation
    /// </summary>
    public static class SkillFactory
    {
        /// <summary>
        /// Create a skill by name with default parameters
        /// </summary>
        public static IBaseSkill CreateSkill(string skillName)
        {
            return CreateSkillByName(skillName);
        }
        
        /// <summary>
        /// Create a skill with full parameters (legacy method for compatibility)
        /// </summary>
        public static IBaseSkill CreateSkill(string skillName, Core.SkillType type, int cooldown, Core.SkillTargetType targetType)
        {
            return CreateSkillByName(skillName);
        }
        
        /// <summary>
        /// Create a skill by name using the new implementation system
        /// </summary>
        public static IBaseSkill CreateSkillByName(string skillName)
        {
            switch (skillName.ToLower())
            {
                case "shield bash":
                    return new ShieldBashSkill();
                case "cleave":
                    return new CleaveSkill();
                case "fireball":
                    return new FireballSkill();
                case "arcane bolt":
                    return new ArcaneBoltSkill();
                case "heal":
                    return new HealSkill();
                case "quick stab":
                    return new QuickStabSkill();
                case "shadowstep":
                    return new ShadowstepSkill();
                case "precise shot":
                    return new PreciseShotSkill();
                default:
                    Debug.LogWarning($"SkillFactory: Unknown skill '{skillName}', creating basic attack");
                    return CreateBasicAttack();
            }
        }
        
        /// <summary>
        /// Create a basic attack skill
        /// </summary>
        public static IBaseSkill CreateBasicAttack(string skillName = "Basic Attack")
        {
            return new BasicAttackSkill(skillName);
        }
        
        /// <summary>
        /// Create a warrior skill set
        /// </summary>
        public static List<IBaseSkill> CreateWarriorSkillSet()
        {
            var skills = new List<IBaseSkill>();
            
            // Basic attack
            skills.Add(CreateBasicAttack("Slash"));
            
            // Active skills
            skills.Add(CreateSkillByName("Shield Bash"));
            skills.Add(CreateSkillByName("Cleave"));
            skills.Add(CreateBasicAttack("Taunt")); // Placeholder for Taunt
            skills.Add(CreateBasicAttack("Defensive Stance")); // Placeholder for Defensive Stance
            
            return skills;
        }
        
        /// <summary>
        /// Create a mage skill set
        /// </summary>
        public static List<IBaseSkill> CreateMageSkillSet()
        {
            var skills = new List<IBaseSkill>();
            
            // Basic attack
            skills.Add(CreateBasicAttack("Arcane Bolt"));
            
            // Active skills
            skills.Add(CreateSkillByName("Fireball"));
            skills.Add(CreateBasicAttack("Ice Storm")); // Placeholder for Ice Storm
            skills.Add(CreateSkillByName("Heal"));
            skills.Add(CreateBasicAttack("Mana Shield")); // Placeholder for Mana Shield
            
            return skills;
        }
        
        /// <summary>
        /// Create a rogue skill set
        /// </summary>
        public static List<IBaseSkill> CreateRogueSkillSet()
        {
            var skills = new List<IBaseSkill>();
            
            // Basic attack
            skills.Add(CreateBasicAttack("Quick Stab"));
            
            // Active skills
            skills.Add(CreateSkillByName("Shadowstep"));
            skills.Add(CreateBasicAttack("Poison Dart")); // Placeholder for Poison Dart
            skills.Add(CreateBasicAttack("Stealth")); // Placeholder for Stealth
            skills.Add(CreateBasicAttack("Backstab")); // Placeholder for Backstab
            
            return skills;
        }
        
        /// <summary>
        /// Create a cleric skill set
        /// </summary>
        public static List<IBaseSkill> CreateClericSkillSet()
        {
            var skills = new List<IBaseSkill>();
            
            // Basic attack
            skills.Add(CreateBasicAttack("Divine Strike"));
            
            // Active skills
            skills.Add(CreateSkillByName("Heal"));
            skills.Add(CreateBasicAttack("Group Heal")); // Placeholder for Group Heal
            skills.Add(CreateBasicAttack("Smite")); // Placeholder for Smite
            skills.Add(CreateBasicAttack("Divine Protection")); // Placeholder for Divine Protection
            
            return skills;
        }
        
        /// <summary>
        /// Create a skill set based on class name
        /// </summary>
        public static List<IBaseSkill> CreateSkillSetByClass(string className)
        {
            switch (className.ToLower())
            {
                case "warrior":
                    return CreateWarriorSkillSet();
                case "mage":
                    return CreateMageSkillSet();
                case "rogue":
                    return CreateRogueSkillSet();
                case "cleric":
                    return CreateClericSkillSet();
                default:
                    Debug.LogWarning($"SkillFactory: Unknown class '{className}', creating basic skill set");
                    return CreateBasicSkillSet();
            }
        }
        
        /// <summary>
        /// Create a basic skill set for testing
        /// </summary>
        public static List<IBaseSkill> CreateBasicSkillSet()
        {
            var skills = new List<IBaseSkill>();
            skills.Add(CreateBasicAttack());
            skills.Add(CreateSkillByName("Fireball")); // Use an existing skill for testing
            return skills;
        }
        
        /// <summary>
        /// Create a skill from a skill definition (for compatibility with existing systems)
        /// </summary>
        public static IBaseSkill CreateSkillFromDefinition(SkillDefinition skillDef)
        {
            if (skillDef == null)
            {
                Debug.LogError("SkillFactory: Cannot create skill from null definition!");
                return null;
            }
            
            // Primary path: create by name using typed implementations
            var skill = CreateSkillByName(skillDef.SkillName);
            if (skill != null) return skill;

            // Secondary path: try to build a GenericSkill from linked effect assets via Resources
            // Convention: Resources/Skills/<SkillName> contains one or more SkillEffectSO assets
            var loadedEffects = new System.Collections.Generic.List<SkillEffectSO>();
            var effectAssets = Resources.LoadAll<SkillEffectSO>($"Skills/{skillDef.SkillName}");
            if (effectAssets != null && effectAssets.Length > 0)
            {
                loadedEffects.AddRange(effectAssets);
            }

            if (loadedEffects.Count > 0)
            {
                var generic = new GenericSkill(
                    skillDef.SkillName,
                    skillDef.Description,
                    skillDef.Cooldown,
                    ConvertSkillTargetType(skillDef.TargetType),
                    1,
                    loadedEffects
                );
                return generic;
            }

            // Fallback to basic attack for unknown skills
            Debug.LogWarning($"SkillFactory: Unknown skill '{skillDef.SkillName}', creating basic attack");
            return new BasicAttackSkill(skillDef.SkillName);
        }
        
        /// <summary>
        /// Convert ScriptableObjects SkillTargetType to Core SkillTargetType
        /// </summary>
        private static Core.SkillTargetType ConvertSkillTargetType(GuildsOfArcanaTerra.ScriptableObjects.Classes.SkillTargetType scriptableTargetType)
        {
            switch (scriptableTargetType)
            {
                case GuildsOfArcanaTerra.ScriptableObjects.Classes.SkillTargetType.SingleEnemy:
                    return Core.SkillTargetType.SingleEnemy;
                case GuildsOfArcanaTerra.ScriptableObjects.Classes.SkillTargetType.SingleAlly:
                    return Core.SkillTargetType.SingleAlly;
                case GuildsOfArcanaTerra.ScriptableObjects.Classes.SkillTargetType.AllEnemies:
                    return Core.SkillTargetType.AllEnemies;
                case GuildsOfArcanaTerra.ScriptableObjects.Classes.SkillTargetType.AllAllies:
                    return Core.SkillTargetType.AllAllies;
                case GuildsOfArcanaTerra.ScriptableObjects.Classes.SkillTargetType.Self:
                    return Core.SkillTargetType.Self;
                case GuildsOfArcanaTerra.ScriptableObjects.Classes.SkillTargetType.Hybrid:
                    return Core.SkillTargetType.SingleAny;
                default:
                    return Core.SkillTargetType.SingleEnemy;
            }
        }
    }
} 