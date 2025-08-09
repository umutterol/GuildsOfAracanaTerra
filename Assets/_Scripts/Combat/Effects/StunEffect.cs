using UnityEngine;

namespace GuildsOfArcanaTerra.Combat.Effects
{
    /// <summary>
    /// Stun status effect that causes target to skip their next turn
    /// Based on GDD: "Stun – Target skips their next turn. Immune for 1 round after."
    /// </summary>
    public class StunEffect : IStatusEffect
    {
        [Header("Stun Effect Properties")]
        private string effectName = "Stun";
        private int duration;
        private int remainingDuration;
        private ICombatant target;
        private ICombatant caster;
        
        // Properties
        public string Name => effectName;
        public int Duration => duration;
        public int RemainingDuration => remainingDuration;
        public bool IsExpired => remainingDuration <= 0;
        public ICombatant Target => target;
        
        /// <summary>
        /// Constructor for creating a stun effect
        /// </summary>
        /// <param name="target">The combatant to apply stun to</param>
        /// <param name="caster">The combatant who caused the stun</param>
        /// <param name="duration">How many turns the stun lasts (default 1 per GDD)</param>
        public StunEffect(ICombatant target, ICombatant caster, int duration = 1)
        {
            this.target = target;
            this.caster = caster;
            this.duration = duration;
            this.remainingDuration = duration;
        }
        
        /// <summary>
        /// Called when the stun effect is first applied
        /// </summary>
        public void ApplyEffect()
        {
            Debug.Log($"StunEffect: {target.Name} is stunned and will skip their next turn!");
            
            // Check if target is immune to stun
            if (target is MonoBehaviour mb)
            {
                var statusManager = mb.GetComponent<StatusEffectManager>();
                if (statusManager != null && statusManager.HasEffect("Stun Immunity"))
                {
                    Debug.Log($"StunEffect: {target.Name} is immune to stun - effect not applied");
                    remainingDuration = 0; // Immediately expire
                    return;
                }
            }
        }
        
        /// <summary>
        /// Called each turn - for stun, this is when the turn is skipped
        /// </summary>
        public void TickEffect()
        {
            if (IsExpired || target == null || !target.IsAlive)
            {
                return;
            }
            
            Debug.Log($"StunEffect: {target.Name} is stunned and skips their turn! ({remainingDuration} turns remaining)");
            
            // Note: The actual turn skipping logic should be handled by the combat system
            // This effect just indicates that the target should skip their turn
        }
        
        /// <summary>
        /// Called when the stun effect is removed
        /// </summary>
        public void RemoveEffect()
        {
            Debug.Log($"StunEffect: {target.Name} is no longer stunned.");
            
            // Apply stun immunity for 1 round as per GDD
            ApplyStunImmunity();
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
                
                Debug.Log($"StunEffect: {target.Name}'s stun extended by {additionalTurns} turns. New duration: {remainingDuration}");
            }
        }
        
        /// <summary>
        /// Apply stun immunity effect for 1 round after stun expires (per GDD)
        /// </summary>
        private void ApplyStunImmunity()
        {
            if (target is MonoBehaviour mb)
            {
                var statusManager = mb.GetComponent<StatusEffectManager>();
                if (statusManager != null)
                {
                    // Create a temporary immunity effect
                    var immunity = new StunImmunityEffect(target, 1);
                    statusManager.ApplyEffect(immunity);
                    
                    Debug.Log($"StunEffect: {target.Name} gains stun immunity for 1 round");
                }
            }
        }
        
        /// <summary>
        /// Check if this combatant can act this turn (used by combat system)
        /// </summary>
        public bool CanAct()
        {
            return IsExpired || remainingDuration <= 0;
        }
        
        /// <summary>
        /// Get a string representation of the stun effect
        /// </summary>
        public override string ToString()
        {
            return $"Stun ({remainingDuration}/{duration} turns)";
        }
    }
    
    /// <summary>
    /// Temporary immunity effect applied after stun expires
    /// </summary>
    public class StunImmunityEffect : IStatusEffect
    {
        private string effectName = "Stun Immunity";
        private int duration;
        private int remainingDuration;
        private ICombatant target;
        
        public string Name => effectName;
        public int Duration => duration;
        public int RemainingDuration => remainingDuration;
        public bool IsExpired => remainingDuration <= 0;
        public ICombatant Target => target;
        
        public StunImmunityEffect(ICombatant target, int duration = 1)
        {
            this.target = target;
            this.duration = duration;
            this.remainingDuration = duration;
        }
        
        public void ApplyEffect()
        {
            Debug.Log($"StunImmunityEffect: {target.Name} is immune to stun for {duration} round(s)");
        }
        
        public void TickEffect()
        {
            // No special tick behavior needed
        }
        
        public void RemoveEffect()
        {
            Debug.Log($"StunImmunityEffect: {target.Name} is no longer immune to stun");
        }
        
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
        
        public void ExtendDuration(int additionalTurns)
        {
            if (additionalTurns > 0)
            {
                remainingDuration += additionalTurns;
                duration += additionalTurns;
            }
        }
        
        public override string ToString()
        {
            return $"Stun Immunity ({remainingDuration}/{duration} rounds)";
        }
    }
}
