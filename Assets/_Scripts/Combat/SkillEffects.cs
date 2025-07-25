using System.Collections.Generic;
using UnityEngine;
using GuildsOfArcanaTerra.Combat.Effects;
using GuildsOfArcanaTerra.ScriptableObjects.Classes;
using GuildsOfArcanaTerra.Combat.Core;
using GuildsOfArcanaTerra.Combat.Skills;
using GuildsOfArcanaTerra.Combat.Skills.Interfaces;

namespace GuildsOfArcanaTerra.Combat
{
    /// <summary>
    /// Handles the execution of skill effects including damage calculation, targeting, and status effects
    /// </summary>
    public static class SkillEffects
    {
        /// <summary>
        /// Execute a skill with the given parameters
        /// </summary>
        public static void ExecuteSkill(IBaseSkill skill, Combatant caster, List<ICombatant> targets, StatusEffectSystem statusEffectSystem)
        {
            if (skill == null || caster == null || targets == null || targets.Count == 0)
            {
                Debug.LogWarning("SkillEffects: Invalid parameters for skill execution");
                return;
            }

            Debug.Log($"SkillEffects: {caster.CharacterName} uses {skill.SkillName}");

            // Execute the skill using its implementation
            skill.Execute(caster, targets, statusEffectSystem);
        }

        // Individual skill implementations have been moved to separate classes in Skills/Implementations/
        // This class now serves as a simple delegate to the skill implementations

        // Primary stat calculation is now handled within individual skill implementations

        #region Target Selection

        /// <summary>
        /// Get valid targets based on skill target type
        /// </summary>
        public static List<ICombatant> GetValidTargets(IBaseSkill skill, Combatant caster, List<ICombatant> allCombatants, bool isEnemySkill)
        {
            var validTargets = new List<ICombatant>();

            foreach (var combatant in allCombatants)
            {
                if (!combatant.IsAlive) continue;

                bool isEnemy = IsEnemy(caster, combatant);
                bool isValidTarget = IsValidTargetForSkill(skill.TargetType, isEnemy, isEnemySkill);

                if (isValidTarget)
                {
                    validTargets.Add(combatant);
                }
            }

            return validTargets;
        }

        /// <summary>
        /// Check if a target is valid for the given skill target type
        /// </summary>
        private static bool IsValidTargetForSkill(Core.SkillTargetType targetType, bool isEnemy, bool isEnemySkill)
        {
            switch (targetType)
            {
                case Core.SkillTargetType.SingleEnemy:
                    return isEnemy;
                case Core.SkillTargetType.SingleAlly:
                    return !isEnemy;
                case Core.SkillTargetType.AllEnemies:
                    return isEnemy;
                case Core.SkillTargetType.AllAllies:
                    return !isEnemy;
                case Core.SkillTargetType.Self:
                    return false; // Self-targeting handled separately
                case Core.SkillTargetType.SingleAny:
                    return true; // Can target any single combatant
                case Core.SkillTargetType.AllAny:
                    return true; // Can target all combatants
                default:
                    return false;
            }
        }

        /// <summary>
        /// Check if a combatant is an enemy of the caster
        /// </summary>
        private static bool IsEnemy(Combatant caster, ICombatant target)
        {
            // This is a simplified check - you might want to implement proper faction system
            // For now, assume different types are enemies
            return caster.GetType() != target.GetType();
        }

        #endregion
    }
} 