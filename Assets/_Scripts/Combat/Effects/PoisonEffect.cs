using UnityEngine;

namespace GuildsOfArcanaTerra.Combat.Effects
{
    /// <summary>
    /// Poison status effect that deals damage over time and reduces healing
    /// Based on GDD: "Poison – Deals 15% AGI and -25% healing (3 turns)."
    /// </summary>
    public class PoisonEffect : IStatusEffect
    {
        [Header("Poison Effect Properties")]
        private string effectName = "Poison";
        private int duration;
        private int remainingDuration;
        private ICombatant target;
        private ICombatant caster;
        private float damageMultiplier = 0.15f; // 15% of AGI as per GDD
        private float healingReduction = 0.25f; // 25% healing reduction as per GDD
        
        // Properties
        public string Name => effectName;
        public int Duration => duration;
        public int RemainingDuration => remainingDuration;
        public bool IsExpired => remainingDuration <= 0;
        public ICombatant Target => target;
        public float HealingReduction => healingReduction;
        
        /// <summary>
        /// Constructor for creating a poison effect
        /// </summary>
        /// <param name="target">The combatant to apply poison to</param>
        /// <param name="caster">The combatant who caused the poison</param>
        /// <param name="duration">How many turns the poison lasts (default 3 per GDD)</param>
        public PoisonEffect(ICombatant target, ICombatant caster, int duration = 3)
        {
            this.target = target;
            this.caster = caster;
            this.duration = duration;
            this.remainingDuration = duration;
        }
        
        /// <summary>
        /// Called when the poison effect is first applied
        /// </summary>
        public void ApplyEffect()
        {
            Debug.Log($"PoisonEffect: {target.Name} is now poisoned for {duration} turns! (-25% healing effectiveness)");
            
            // Check if target already has poison (does not stack per GDD)
            if (target is MonoBehaviour mb)
            {
                var statusManager = mb.GetComponent<StatusEffectManager>();
                if (statusManager != null && statusManager.HasEffect("Poison"))
                {
                    Debug.Log($"PoisonEffect: {target.Name} already has poison - refreshing duration");
                    return;
                }
            }
        }
        
        /// <summary>
        /// Called each turn to deal poison damage
        /// </summary>
        public void TickEffect()
        {
            if (IsExpired || target == null || !target.IsAlive)
            {
                return;
            }
            
            // Calculate poison damage (15% of caster's AGI)
            int poisonDamage = CalculatePoisonDamage();
            
            // Apply damage to target
            if (target is ICombatant combatant)
            {
                combatant.TakeDamage(poisonDamage);
                
                Debug.Log($"PoisonEffect: {target.Name} takes {poisonDamage} poison damage! ({remainingDuration} turns remaining, -25% healing)");
            }
        }
        
        /// <summary>
        /// Called when the poison effect is removed
        /// </summary>
        public void RemoveEffect()
        {
            Debug.Log($"PoisonEffect: {target.Name} is no longer poisoned. Healing effectiveness restored.");
        }
        
        /// <summary>
        /// Reduce the remaining duration by 1 turn
        /// </summary>
        public void ReduceDuration()
        {
            if (remainingDuration > 0)
            {
                remainingDuration--;
                
                if (IsExpired)
                {
                    RemoveEffect();
                }
            }
        }
        
        /// <summary>
        /// Extend the duration by additional turns
        /// </summary>
        public void ExtendDuration(int additionalTurns)
        {
            if (additionalTurns > 0)
            {
                remainingDuration += additionalTurns;
                duration += additionalTurns;
                
                Debug.Log($"PoisonEffect: {target.Name}'s poison extended by {additionalTurns} turns. New duration: {remainingDuration}");
            }
        }
        
        /// <summary>
        /// Calculate the poison damage based on caster's AGI stat
        /// </summary>
        private int CalculatePoisonDamage()
        {
            if (caster == null)
            {
                Debug.LogWarning("PoisonEffect: No caster found, using default damage of 4");
                return 4;
            }
            
            // Get caster's AGI stat
            int casterAgi = GetCasterAgility();
            
            // Calculate 15% of AGI as per GDD
            int damage = Mathf.RoundToInt(casterAgi * damageMultiplier);
            
            // Ensure minimum damage of 1
            return Mathf.Max(1, damage);
        }
        
        /// <summary>
        /// Get the caster's agility stat
        /// </summary>
        private int GetCasterAgility()
        {
            if (caster == null)
            {
                Debug.LogWarning("PoisonEffect: No caster found, using default AGI of 20");
                return 20;
            }
            
            return caster.AGI;
        }
        
        /// <summary>
        /// Apply healing reduction to incoming healing
        /// </summary>
        /// <param name="originalHealing">The original healing amount</param>
        /// <returns>Reduced healing amount</returns>
        public int ApplyHealingReduction(int originalHealing)
        {
            if (IsExpired) return originalHealing;
            
            int reducedHealing = Mathf.RoundToInt(originalHealing * (1f - healingReduction));
            
            Debug.Log($"PoisonEffect: {target.Name}'s healing reduced from {originalHealing} to {reducedHealing} (poison effect)");
            
            return reducedHealing;
        }
        
        /// <summary>
        /// Get a string representation of the poison effect
        /// </summary>
        public override string ToString()
        {
            return $"Poison ({remainingDuration}/{duration} turns, -25% healing)";
        }
    }
}
