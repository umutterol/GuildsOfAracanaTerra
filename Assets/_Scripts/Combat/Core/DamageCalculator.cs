using UnityEngine;

namespace GuildsOfArcanaTerra.Combat.Core
{
    /// <summary>
    /// Centralized damage and healing calculation system
    /// Extracted from SkillEffects to provide reusable calculation logic
    /// </summary>
    public static class DamageCalculator
    {
        /// <summary>
        /// Calculate physical damage based on attacker stat and target defense
        /// </summary>
        /// <param name="attackerStat">Attacker's primary stat (STR/AGI)</param>
        /// <param name="targetDefense">Target's defense value</param>
        /// <param name="multiplier">Damage multiplier (default 1.0f)</param>
        /// <returns>Calculated damage amount</returns>
        public static int CalculatePhysicalDamage(int attackerStat, int targetDefense, float multiplier = 1.0f)
        {
            // Base damage calculation
            float baseDamage = attackerStat * multiplier;
            
            // Defense reduction (10% of defense reduces damage)
            float defenseReduction = targetDefense * 0.10f;
            
            // Final damage calculation
            float finalDamage = Mathf.Max(1, baseDamage - defenseReduction);
            
            return Mathf.RoundToInt(finalDamage);
        }
        
        /// <summary>
        /// Calculate magical damage based on attacker intelligence and target defense
        /// </summary>
        /// <param name="attackerInt">Attacker's intelligence stat</param>
        /// <param name="targetDefense">Target's defense value</param>
        /// <param name="multiplier">Damage multiplier (default 1.0f)</param>
        /// <returns>Calculated magical damage amount</returns>
        public static int CalculateMagicalDamage(int attackerInt, int targetDefense, float multiplier = 1.0f)
        {
            // Base magical damage calculation
            float baseDamage = attackerInt * multiplier;
            
            // Defense reduction for magic (5% of defense reduces damage)
            float defenseReduction = targetDefense * 0.05f;
            
            // Final damage calculation
            float finalDamage = Mathf.Max(1, baseDamage - defenseReduction);
            
            return Mathf.RoundToInt(finalDamage);
        }
        
        /// <summary>
        /// Calculate healing amount based on healer's intelligence
        /// </summary>
        /// <param name="healerInt">Healer's intelligence stat</param>
        /// <param name="multiplier">Healing multiplier (default 1.0f)</param>
        /// <returns>Calculated healing amount</returns>
        public static int CalculateHealing(int healerInt, float multiplier = 1.0f)
        {
            float healingAmount = healerInt * multiplier;
            return Mathf.RoundToInt(healingAmount);
        }
        
        /// <summary>
        /// Calculate critical hit damage
        /// </summary>
        /// <param name="baseDamage">Base damage amount</param>
        /// <param name="critMultiplier">Critical damage multiplier (default 1.5f)</param>
        /// <returns>Critical damage amount</returns>
        public static int CalculateCriticalDamage(int baseDamage, float critMultiplier = 1.5f)
        {
            return Mathf.RoundToInt(baseDamage * critMultiplier);
        }
        
        /// <summary>
        /// Calculate damage with critical hit chance
        /// </summary>
        /// <param name="baseDamage">Base damage amount</param>
        /// <param name="critChance">Critical hit chance (0.0f to 1.0f)</param>
        /// <param name="critMultiplier">Critical damage multiplier</param>
        /// <returns>Final damage amount (may be critical)</returns>
        public static int CalculateDamageWithCrit(int baseDamage, float critChance, float critMultiplier = 1.5f)
        {
            if (Random.Range(0f, 1f) <= critChance)
            {
                return CalculateCriticalDamage(baseDamage, critMultiplier);
            }
            
            return baseDamage;
        }
    }
} 