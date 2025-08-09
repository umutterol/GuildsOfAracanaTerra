using UnityEngine;

namespace GuildsOfArcanaTerra.Combat.Effects
{
    /// <summary>
    /// Shield status effect that absorbs incoming damage
    /// Based on GDD: "Shield – Absorbs damage. Based on caster INT or flat value."
    /// </summary>
    public class ShieldEffect : IStatusEffect
    {
        [Header("Shield Effect Properties")]
        private string effectName = "Shield";
        private int duration;
        private int remainingDuration;
        private ICombatant target;
        private ICombatant caster;
        private int shieldAmount; // Current shield points
        private int maxShieldAmount; // Original shield points
        private float intMultiplier = 1.0f; // Multiplier for INT-based shields
        
        // Properties
        public string Name => effectName;
        public int Duration => duration;
        public int RemainingDuration => remainingDuration;
        public bool IsExpired => remainingDuration <= 0 || shieldAmount <= 0;
        public ICombatant Target => target;
        public int ShieldAmount => shieldAmount;
        public int MaxShieldAmount => maxShieldAmount;
        
        /// <summary>
        /// Constructor for creating an INT-based shield effect
        /// </summary>
        /// <param name="target">The combatant to apply shield to</param>
        /// <param name="caster">The combatant who cast the shield</param>
        /// <param name="duration">How many turns the shield lasts (0 = until depleted)</param>
        /// <param name="intMultiplier">Multiplier for caster's INT (default 1.0)</param>
        public ShieldEffect(ICombatant target, ICombatant caster, int duration = 0, float intMultiplier = 1.0f)
        {
            this.target = target;
            this.caster = caster;
            this.duration = duration;
            this.remainingDuration = duration;
            this.intMultiplier = intMultiplier;
            
            // Calculate shield amount based on caster's INT
            this.shieldAmount = CalculateShieldAmount();
            this.maxShieldAmount = this.shieldAmount;
        }
        
        /// <summary>
        /// Constructor for creating a flat value shield effect
        /// </summary>
        /// <param name="target">The combatant to apply shield to</param>
        /// <param name="caster">The combatant who cast the shield</param>
        /// <param name="flatAmount">Flat shield amount</param>
        /// <param name="duration">How many turns the shield lasts (0 = until depleted)</param>
        public ShieldEffect(ICombatant target, ICombatant caster, int flatAmount, int duration = 0)
        {
            this.target = target;
            this.caster = caster;
            this.duration = duration;
            this.remainingDuration = duration;
            this.shieldAmount = flatAmount;
            this.maxShieldAmount = flatAmount;
        }
        
        /// <summary>
        /// Called when the shield effect is first applied
        /// </summary>
        public void ApplyEffect()
        {
            if (duration > 0)
            {
                Debug.Log($"ShieldEffect: {target.Name} gains a {shieldAmount} point shield for {duration} turns!");
            }
            else
            {
                Debug.Log($"ShieldEffect: {target.Name} gains a {shieldAmount} point shield until depleted!");
            }
            
            // Check if target already has a shield (replace with stronger one)
            if (target is MonoBehaviour mb)
            {
                var statusManager = mb.GetComponent<StatusEffectManager>();
                if (statusManager != null)
                {
                    var existingShield = statusManager.GetEffect("Shield") as ShieldEffect;
                    if (existingShield != null)
                    {
                        if (this.shieldAmount > existingShield.shieldAmount)
                        {
                            statusManager.RemoveEffect("Shield");
                            Debug.Log($"ShieldEffect: Replacing weaker shield ({existingShield.shieldAmount}) with stronger one ({this.shieldAmount})");
                        }
                        else
                        {
                            Debug.Log($"ShieldEffect: Keeping existing stronger shield ({existingShield.shieldAmount})");
                            this.shieldAmount = 0; // Invalidate this shield
                            return;
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Called each turn - shields don't have tick effects typically
        /// </summary>
        public void TickEffect()
        {
            if (IsExpired || target == null || !target.IsAlive)
            {
                return;
            }
            
            if (duration > 0)
            {
                Debug.Log($"ShieldEffect: {target.Name}'s shield persists ({shieldAmount}/{maxShieldAmount} points, {remainingDuration} turns remaining)");
            }
            else
            {
                Debug.Log($"ShieldEffect: {target.Name}'s shield persists ({shieldAmount}/{maxShieldAmount} points)");
            }
        }
        
        /// <summary>
        /// Called when the shield effect is removed
        /// </summary>
        public void RemoveEffect()
        {
            if (shieldAmount > 0)
            {
                Debug.Log($"ShieldEffect: {target.Name}'s shield expires with {shieldAmount} points remaining");
            }
            else
            {
                Debug.Log($"ShieldEffect: {target.Name}'s shield is depleted");
            }
        }
        
        /// <summary>
        /// Reduce the remaining duration by 1 turn
        /// </summary>
        public void ReduceDuration()
        {
            if (duration > 0 && remainingDuration > 0)
            {
                remainingDuration--;
                
                if (remainingDuration <= 0)
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
            if (additionalTurns > 0 && duration > 0)
            {
                remainingDuration += additionalTurns;
                duration += additionalTurns;
                
                Debug.Log($"ShieldEffect: {target.Name}'s shield extended by {additionalTurns} turns. New duration: {remainingDuration}");
            }
        }
        
        /// <summary>
        /// Absorb incoming damage with the shield
        /// </summary>
        /// <param name="incomingDamage">The amount of damage to absorb</param>
        /// <returns>The amount of damage that gets through the shield</returns>
        public int AbsorbDamage(int incomingDamage)
        {
            if (IsExpired || shieldAmount <= 0)
            {
                return incomingDamage; // Shield can't absorb anything
            }
            
            int damageAbsorbed = Mathf.Min(shieldAmount, incomingDamage);
            int remainingDamage = incomingDamage - damageAbsorbed;
            
            shieldAmount -= damageAbsorbed;
            
            if (damageAbsorbed > 0)
            {
                Debug.Log($"ShieldEffect: {target.Name}'s shield absorbs {damageAbsorbed} damage! Shield: {shieldAmount}/{maxShieldAmount}");
            }
            
            if (shieldAmount <= 0)
            {
                Debug.Log($"ShieldEffect: {target.Name}'s shield is depleted!");
            }
            
            return remainingDamage;
        }
        
        /// <summary>
        /// Calculate the shield amount based on caster's INT stat
        /// </summary>
        private int CalculateShieldAmount()
        {
            if (caster == null)
            {
                Debug.LogWarning("ShieldEffect: No caster found, using default shield amount of 20");
                return 20;
            }
            
            // Get caster's INT stat
            int casterInt = GetCasterIntelligence();
            
            // Calculate shield amount based on INT and multiplier
            int amount = Mathf.RoundToInt(casterInt * intMultiplier);
            
            // Ensure minimum shield of 5
            return Mathf.Max(5, amount);
        }
        
        /// <summary>
        /// Get the caster's intelligence stat
        /// </summary>
        private int GetCasterIntelligence()
        {
            if (caster == null)
            {
                Debug.LogWarning("ShieldEffect: No caster found, using default INT of 20");
                return 20;
            }
            
            return caster.INT;
        }
        
        /// <summary>
        /// Get the shield's effectiveness percentage
        /// </summary>
        public float GetShieldPercentage()
        {
            if (maxShieldAmount <= 0) return 0f;
            
            return (float)shieldAmount / maxShieldAmount;
        }
        
        /// <summary>
        /// Check if the shield is still active
        /// </summary>
        public bool IsActive()
        {
            return !IsExpired && shieldAmount > 0;
        }
        
        /// <summary>
        /// Get a string representation of the shield effect
        /// </summary>
        public override string ToString()
        {
            if (duration > 0)
            {
                return $"Shield ({shieldAmount}/{maxShieldAmount} points, {remainingDuration}/{duration} turns)";
            }
            else
            {
                return $"Shield ({shieldAmount}/{maxShieldAmount} points)";
            }
        }
    }
}
