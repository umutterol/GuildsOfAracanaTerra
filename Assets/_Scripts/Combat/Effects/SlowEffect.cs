using UnityEngine;

namespace GuildsOfArcanaTerra.Combat.Effects
{
    /// <summary>
    /// Slow status effect that reduces AGI and affects dodge & turn order
    /// Based on GDD: "Slow – -10% AGI for 2 turns. Affects dodge & turn order."
    /// </summary>
    public class SlowEffect : IStatusEffect
    {
        [Header("Slow Effect Properties")]
        private string effectName = "Slow";
        private int duration;
        private int remainingDuration;
        private ICombatant target;
        private ICombatant caster;
        private float agiReduction = 0.10f; // 10% AGI reduction as per GDD
        private int originalAgi; // Store original AGI to restore later
        
        // Properties
        public string Name => effectName;
        public int Duration => duration;
        public int RemainingDuration => remainingDuration;
        public bool IsExpired => remainingDuration <= 0;
        public ICombatant Target => target;
        public float AgilityReduction => agiReduction;
        
        /// <summary>
        /// Constructor for creating a slow effect
        /// </summary>
        /// <param name="target">The combatant to apply slow to</param>
        /// <param name="caster">The combatant who caused the slow</param>
        /// <param name="duration">How many turns the slow lasts (default 2 per GDD)</param>
        public SlowEffect(ICombatant target, ICombatant caster, int duration = 2)
        {
            this.target = target;
            this.caster = caster;
            this.duration = duration;
            this.remainingDuration = duration;
        }
        
        /// <summary>
        /// Called when the slow effect is first applied
        /// </summary>
        public void ApplyEffect()
        {
            if (target == null) return;
            
            // Store original AGI before modification
            originalAgi = target.AGI;
            
            // Calculate and apply AGI reduction
            int agiReductionAmount = Mathf.RoundToInt(originalAgi * agiReduction);
            int newAgi = Mathf.Max(1, originalAgi - agiReductionAmount); // Ensure minimum AGI of 1
            
            // Apply the AGI reduction (this would need to be implemented in the combatant interface)
            if (target is ICombatantWithModifiableStats modifiableTarget)
            {
                modifiableTarget.ModifyAGI(-agiReductionAmount);
            }
            
            Debug.Log($"SlowEffect: {target.Name} is slowed for {duration} turns! AGI reduced from {originalAgi} to {newAgi} (-{agiReductionAmount})");
            
            // Check if target already has slow (does not stack per typical implementation)
            if (target is MonoBehaviour mb)
            {
                var statusManager = mb.GetComponent<StatusEffectManager>();
                if (statusManager != null && statusManager.HasEffect("Slow"))
                {
                    Debug.Log($"SlowEffect: {target.Name} already has slow - refreshing duration");
                    return;
                }
            }
        }
        
        /// <summary>
        /// Called each turn - for slow, this maintains the AGI reduction
        /// </summary>
        public void TickEffect()
        {
            if (IsExpired || target == null || !target.IsAlive)
            {
                return;
            }
            
            Debug.Log($"SlowEffect: {target.Name} remains slowed ({remainingDuration} turns remaining, -10% AGI)");
            
            // The AGI reduction is persistent while the effect is active
            // No additional action needed on tick
        }
        
        /// <summary>
        /// Called when the slow effect is removed
        /// </summary>
        public void RemoveEffect()
        {
            if (target == null) return;
            
            // Restore original AGI
            int currentAgi = target.AGI;
            int agiReductionAmount = Mathf.RoundToInt(originalAgi * agiReduction);
            
            if (target is ICombatantWithModifiableStats modifiableTarget)
            {
                modifiableTarget.ModifyAGI(agiReductionAmount); // Add back the reduction
            }
            
            Debug.Log($"SlowEffect: {target.Name} is no longer slowed. AGI restored from {currentAgi} to {originalAgi}");
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
                
                Debug.Log($"SlowEffect: {target.Name}'s slow extended by {additionalTurns} turns. New duration: {remainingDuration}");
            }
        }
        
        /// <summary>
        /// Get the AGI modifier applied by this slow effect
        /// </summary>
        public int GetAgilityModifier()
        {
            if (IsExpired) return 0;
            
            return -Mathf.RoundToInt(originalAgi * agiReduction);
        }
        
        /// <summary>
        /// Check if this slow effect affects turn order calculation
        /// </summary>
        public bool AffectsTurnOrder()
        {
            return !IsExpired;
        }
        
        /// <summary>
        /// Check if this slow effect affects dodge calculation
        /// </summary>
        public bool AffectsDodge()
        {
            return !IsExpired;
        }
        
        /// <summary>
        /// Get a string representation of the slow effect
        /// </summary>
        public override string ToString()
        {
            return $"Slow ({remainingDuration}/{duration} turns, -10% AGI)";
        }
    }
    
    /// <summary>
    /// Interface extension for combatants that can have their stats modified
    /// This would need to be implemented by the actual combatant classes
    /// </summary>
    public interface ICombatantWithModifiableStats : ICombatant
    {
        /// <summary>
        /// Modify the AGI stat by the specified amount (can be negative)
        /// </summary>
        void ModifyAGI(int modifier);
        
        /// <summary>
        /// Modify the INT stat by the specified amount (can be negative)
        /// </summary>
        void ModifyINT(int modifier);
        
        /// <summary>
        /// Modify the STR stat by the specified amount (can be negative)
        /// </summary>
        void ModifySTR(int modifier);
    }
}
