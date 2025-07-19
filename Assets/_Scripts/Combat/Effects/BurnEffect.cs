using UnityEngine;

namespace GuildsOfArcanaTerra.Combat.Effects
{
    /// <summary>
    /// Burn status effect that deals damage over time
    /// Based on GDD: "Burn – Deals 25% INT as damage at the start of the target's turn for X turns. Does not stack."
    /// </summary>
    public class BurnEffect : IStatusEffect
    {
        [Header("Burn Effect Properties")]
        private string effectName = "Burn";
        private int duration;
        private int remainingDuration;
        private ICombatant target;
        private ICombatant caster;
        private float damageMultiplier = 0.25f; // 25% of INT as per GDD
        
        // Properties
        public string Name => effectName;
        public int Duration => duration;
        public int RemainingDuration => remainingDuration;
        public bool IsExpired => remainingDuration <= 0;
        public ICombatant Target => target;
        
        /// <summary>
        /// Constructor for creating a burn effect
        /// </summary>
        /// <param name="target">The combatant to apply burn to</param>
        /// <param name="caster">The combatant who cast the burn</param>
        /// <param name="duration">How many turns the burn lasts</param>
        public BurnEffect(ICombatant target, ICombatant caster, int duration = 2)
        {
            this.target = target;
            this.caster = caster;
            this.duration = duration;
            this.remainingDuration = duration;
        }
        
        /// <summary>
        /// Called when the burn effect is first applied
        /// </summary>
        public void ApplyEffect()
        {
            Debug.Log($"BurnEffect: {target.Name} is now burning for {duration} turns!");
            
            // Check if target already has burn (does not stack per GDD)
            if (target is MonoBehaviour mb)
            {
                var statusManager = mb.GetComponent<StatusEffectManager>();
                if (statusManager != null && statusManager.HasEffect("Burn"))
                {
                    Debug.Log($"BurnEffect: {target.Name} already has burn - effect not applied (no stacking)");
                    return;
                }
            }
        }
        
        /// <summary>
        /// Called each turn to deal burn damage
        /// </summary>
        public void TickEffect()
        {
            if (IsExpired || target == null || !target.IsAlive)
            {
                return;
            }
            
            // Calculate burn damage (25% of caster's INT)
            int burnDamage = CalculateBurnDamage();
            
            // Apply damage to target
            if (target is ICombatant combatant)
            {
                combatant.TakeDamage(burnDamage);
                
                Debug.Log($"BurnEffect: {target.Name} takes {burnDamage} burn damage! ({remainingDuration} turns remaining)");
            }
        }
        
        /// <summary>
        /// Called when the burn effect is removed
        /// </summary>
        public void RemoveEffect()
        {
            Debug.Log($"BurnEffect: {target.Name} is no longer burning.");
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
                
                Debug.Log($"BurnEffect: {target.Name}'s burn extended by {additionalTurns} turns. New duration: {remainingDuration}");
            }
        }
        
        /// <summary>
        /// Calculate the burn damage based on caster's INT stat
        /// </summary>
        private int CalculateBurnDamage()
        {
            if (caster == null)
            {
                Debug.LogWarning("BurnEffect: No caster found, using default damage of 5");
                return 5;
            }
            
            // Get caster's INT stat (assuming it's accessible via a property or method)
            int casterInt = GetCasterIntelligence();
            
            // Calculate 25% of INT as per GDD
            int damage = Mathf.RoundToInt(casterInt * damageMultiplier);
            
            // Ensure minimum damage of 1
            return Mathf.Max(1, damage);
        }
        
        /// <summary>
        /// Get the caster's intelligence stat
        /// </summary>
        private int GetCasterIntelligence()
        {
            if (caster == null)
            {
                Debug.LogWarning("BurnEffect: No caster found, using default INT of 20");
                return 20;
            }
            
            return caster.INT;
        }
        
        /// <summary>
        /// Get a string representation of the burn effect
        /// </summary>
        public override string ToString()
        {
            return $"Burn ({remainingDuration}/{duration} turns)";
        }
    }
} 