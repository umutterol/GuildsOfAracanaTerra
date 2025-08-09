using UnityEngine;

namespace GuildsOfArcanaTerra.Combat.Effects
{
    /// <summary>
    /// Bleed status effect that deals damage over time and can stack
    /// Based on GDD: "Bleed – 20% AGI per turn. Stacks up to 3x."
    /// </summary>
    public class BleedEffect : IStatusEffect
    {
        [Header("Bleed Effect Properties")]
        private string effectName = "Bleed";
        private int duration;
        private int remainingDuration;
        private ICombatant target;
        private ICombatant caster;
        private float damageMultiplier = 0.20f; // 20% of AGI as per GDD
        private int stackLevel = 1; // Current stack level (1-3)
        private const int maxStacks = 3; // Maximum stacks allowed
        
        // Properties
        public string Name => effectName;
        public int Duration => duration;
        public int RemainingDuration => remainingDuration;
        public bool IsExpired => remainingDuration <= 0;
        public ICombatant Target => target;
        public int StackLevel => stackLevel;
        
        /// <summary>
        /// Constructor for creating a bleed effect
        /// </summary>
        /// <param name="target">The combatant to apply bleed to</param>
        /// <param name="caster">The combatant who caused the bleed</param>
        /// <param name="duration">How many turns the bleed lasts</param>
        /// <param name="stackLevel">Stack level (1-3)</param>
        public BleedEffect(ICombatant target, ICombatant caster, int duration = 3, int stackLevel = 1)
        {
            this.target = target;
            this.caster = caster;
            this.duration = duration;
            this.remainingDuration = duration;
            this.stackLevel = Mathf.Clamp(stackLevel, 1, maxStacks);
        }
        
        /// <summary>
        /// Called when the bleed effect is first applied
        /// </summary>
        public void ApplyEffect()
        {
            Debug.Log($"BleedEffect: {target.Name} is now bleeding (Stack {stackLevel}) for {duration} turns!");
        }
        
        /// <summary>
        /// Called each turn to deal bleed damage
        /// </summary>
        public void TickEffect()
        {
            if (IsExpired || target == null || !target.IsAlive)
            {
                return;
            }
            
            // Calculate bleed damage (20% of caster's AGI × stack level)
            int bleedDamage = CalculateBleedDamage();
            
            // Apply damage to target
            if (target is ICombatant combatant)
            {
                combatant.TakeDamage(bleedDamage);
                
                Debug.Log($"BleedEffect: {target.Name} takes {bleedDamage} bleed damage! (Stack {stackLevel}, {remainingDuration} turns remaining)");
            }
        }
        
        /// <summary>
        /// Called when the bleed effect is removed
        /// </summary>
        public void RemoveEffect()
        {
            Debug.Log($"BleedEffect: {target.Name} is no longer bleeding.");
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
                
                Debug.Log($"BleedEffect: {target.Name}'s bleed extended by {additionalTurns} turns. New duration: {remainingDuration}");
            }
        }
        
        /// <summary>
        /// Stack this bleed effect with another one (up to max stacks)
        /// </summary>
        public bool TryStack(BleedEffect newBleed)
        {
            if (stackLevel >= maxStacks)
            {
                // Refresh duration instead of stacking
                remainingDuration = Mathf.Max(remainingDuration, newBleed.remainingDuration);
                Debug.Log($"BleedEffect: {target.Name}'s bleed refreshed (already at max stacks)");
                return false;
            }
            
            // Increase stack level and refresh duration
            stackLevel++;
            remainingDuration = Mathf.Max(remainingDuration, newBleed.remainingDuration);
            
            Debug.Log($"BleedEffect: {target.Name}'s bleed stacked to level {stackLevel}!");
            return true;
        }
        
        /// <summary>
        /// Calculate the bleed damage based on caster's AGI stat and stack level
        /// </summary>
        private int CalculateBleedDamage()
        {
            if (caster == null)
            {
                Debug.LogWarning("BleedEffect: No caster found, using default damage of 3 per stack");
                return 3 * stackLevel;
            }
            
            // Get caster's AGI stat
            int casterAgi = GetCasterAgility();
            
            // Calculate 20% of AGI × stack level as per GDD
            int damage = Mathf.RoundToInt(casterAgi * damageMultiplier * stackLevel);
            
            // Ensure minimum damage of 1 per stack
            return Mathf.Max(stackLevel, damage);
        }
        
        /// <summary>
        /// Get the caster's agility stat
        /// </summary>
        private int GetCasterAgility()
        {
            if (caster == null)
            {
                Debug.LogWarning("BleedEffect: No caster found, using default AGI of 15");
                return 15;
            }
            
            return caster.AGI;
        }
        
        /// <summary>
        /// Get a string representation of the bleed effect
        /// </summary>
        public override string ToString()
        {
            return $"Bleed x{stackLevel} ({remainingDuration}/{duration} turns)";
        }
    }
}
