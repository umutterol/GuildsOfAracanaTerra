using System;

namespace GuildsOfArcanaTerra.Combat.Effects
{
    /// <summary>
    /// Interface for all status effects that can be applied to combatants
    /// </summary>
    public interface IStatusEffect
    {
        /// <summary>
        /// The name of the status effect
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// How many turns this effect will last
        /// </summary>
        int Duration { get; }
        
        /// <summary>
        /// Current remaining duration
        /// </summary>
        int RemainingDuration { get; }
        
        /// <summary>
        /// Whether this effect has expired (duration = 0)
        /// </summary>
        bool IsExpired { get; }
        
        /// <summary>
        /// The combatant this effect is applied to
        /// </summary>
        ICombatant Target { get; }
        
        /// <summary>
        /// Called when the effect is first applied to a target
        /// </summary>
        void ApplyEffect();
        
        /// <summary>
        /// Called each turn to process the effect (damage, healing, etc.)
        /// </summary>
        void TickEffect();
        
        /// <summary>
        /// Called when the effect is removed (expired or dispelled)
        /// </summary>
        void RemoveEffect();
        
        /// <summary>
        /// Reduce duration by 1 turn
        /// </summary>
        void ReduceDuration();
        
        /// <summary>
        /// Extend the duration by the specified number of turns
        /// </summary>
        void ExtendDuration(int additionalTurns);
    }
} 