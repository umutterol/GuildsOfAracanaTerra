using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GuildsOfArcanaTerra.Combat.Effects
{
    /// <summary>
    /// Manages status effects for a single combatant
    /// </summary>
    public class StatusEffectManager : MonoBehaviour
    {
        [Header("Status Effects")]
        [SerializeField] private List<IStatusEffect> activeEffects = new List<IStatusEffect>();
        
        [Header("Debug")]
        [SerializeField] private bool debugMode = false;
        
        // Events
        public UnityEvent<IStatusEffect> OnEffectApplied = new UnityEvent<IStatusEffect>();
        public UnityEvent<IStatusEffect> OnEffectRemoved = new UnityEvent<IStatusEffect>();
        public UnityEvent<IStatusEffect> OnEffectTicked = new UnityEvent<IStatusEffect>();
        
        // Properties
        public List<IStatusEffect> ActiveEffects => new List<IStatusEffect>(activeEffects);
        public int EffectCount => activeEffects.Count;
        
        /// <summary>
        /// Apply a status effect to this combatant
        /// </summary>
        public bool ApplyEffect(IStatusEffect effect)
        {
            if (effect == null)
            {
                Debug.LogError("StatusEffectManager: Cannot apply null effect");
                return false;
            }
            
            // Check if effect is already applied (for non-stacking effects)
            if (HasEffect(effect.Name))
            {
                if (debugMode)
                {
                    Debug.Log($"StatusEffectManager: {effect.Name} already applied to {name}");
                }
                return false;
            }
            
            // Add effect to list
            activeEffects.Add(effect);
            
            // Apply the effect
            effect.ApplyEffect();
            
            // Notify listeners
            OnEffectApplied?.Invoke(effect);
            
            if (debugMode)
            {
                Debug.Log($"StatusEffectManager: Applied {effect.Name} to {name} for {effect.Duration} turns");
            }
            
            return true;
        }
        
        /// <summary>
        /// Remove a specific status effect
        /// </summary>
        public bool RemoveEffect(string effectName)
        {
            var effect = activeEffects.FirstOrDefault(e => e.Name.Equals(effectName, System.StringComparison.OrdinalIgnoreCase));
            if (effect != null)
            {
                return RemoveEffect(effect);
            }
            return false;
        }
        
        /// <summary>
        /// Remove a specific status effect
        /// </summary>
        public bool RemoveEffect(IStatusEffect effect)
        {
            if (effect == null) return false;
            
            if (activeEffects.Remove(effect))
            {
                effect.RemoveEffect();
                OnEffectRemoved?.Invoke(effect);
                
                if (debugMode)
                {
                    Debug.Log($"StatusEffectManager: Removed {effect.Name} from {name}");
                }
                
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Check if this combatant has a specific effect
        /// </summary>
        public bool HasEffect(string effectName)
        {
            return activeEffects.Any(e => e.Name.Equals(effectName, System.StringComparison.OrdinalIgnoreCase));
        }
        
        /// <summary>
        /// Get a specific effect by name
        /// </summary>
        public IStatusEffect GetEffect(string effectName)
        {
            return activeEffects.FirstOrDefault(e => e.Name.Equals(effectName, System.StringComparison.OrdinalIgnoreCase));
        }
        
        /// <summary>
        /// Get all effects of a specific type
        /// </summary>
        public List<IStatusEffect> GetEffectsByType(System.Type effectType)
        {
            return activeEffects.Where(e => e.GetType() == effectType).ToList();
        }
        
        /// <summary>
        /// Remove all status effects
        /// </summary>
        public void RemoveAllEffects()
        {
            var effectsToRemove = new List<IStatusEffect>(activeEffects);
            
            foreach (var effect in effectsToRemove)
            {
                RemoveEffect(effect);
            }
            
            if (debugMode)
            {
                Debug.Log($"StatusEffectManager: Removed all effects from {name}");
            }
        }
        
        /// <summary>
        /// Remove all expired effects
        /// </summary>
        public void RemoveExpiredEffects()
        {
            var expiredEffects = activeEffects.Where(e => e.IsExpired).ToList();
            
            foreach (var effect in expiredEffects)
            {
                RemoveEffect(effect);
            }
            
            if (debugMode && expiredEffects.Count > 0)
            {
                Debug.Log($"StatusEffectManager: Removed {expiredEffects.Count} expired effects from {name}");
            }
        }
        
        /// <summary>
        /// Tick all active effects (called at the start of the combatant's turn)
        /// </summary>
        public void TickAllEffects()
        {
            var effectsToTick = new List<IStatusEffect>(activeEffects);
            
            foreach (var effect in effectsToTick)
            {
                if (!effect.IsExpired)
                {
                    effect.TickEffect();
                    OnEffectTicked?.Invoke(effect);
                    
                    if (debugMode)
                    {
                        Debug.Log($"StatusEffectManager: Ticked {effect.Name} on {name}");
                    }
                }
            }
            
            // Remove any effects that expired during ticking
            RemoveExpiredEffects();
        }
        
        /// <summary>
        /// Reduce duration of all effects by 1 turn (called at the end of the combatant's turn)
        /// </summary>
        public void ReduceAllEffectDurations()
        {
            var effectsToReduce = new List<IStatusEffect>(activeEffects);
            
            foreach (var effect in effectsToReduce)
            {
                effect.ReduceDuration();
                
                if (debugMode)
                {
                    Debug.Log($"StatusEffectManager: Reduced duration of {effect.Name} on {name} to {effect.RemainingDuration}");
                }
            }
            
            // Remove any effects that expired after duration reduction
            RemoveExpiredEffects();
        }
        
        /// <summary>
        /// Extend the duration of a specific effect
        /// </summary>
        public bool ExtendEffectDuration(string effectName, int additionalTurns)
        {
            var effect = GetEffect(effectName);
            if (effect != null)
            {
                effect.ExtendDuration(additionalTurns);
                
                if (debugMode)
                {
                    Debug.Log($"StatusEffectManager: Extended {effectName} on {name} by {additionalTurns} turns");
                }
                
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Get a list of all active effect names
        /// </summary>
        public List<string> GetActiveEffectNames()
        {
            return activeEffects.Select(e => e.Name).ToList();
        }
        
        /// <summary>
        /// Get a string representation of all active effects
        /// </summary>
        public string GetEffectsSummary()
        {
            if (activeEffects.Count == 0)
            {
                return "No effects";
            }
            
            var effectStrings = activeEffects.Select(e => $"{e.Name} ({e.RemainingDuration}/{e.Duration})");
            return string.Join(", ", effectStrings);
        }
        
        #region Unity Lifecycle
        
        private void OnDestroy()
        {
            // Clean up events
            OnEffectApplied?.RemoveAllListeners();
            OnEffectRemoved?.RemoveAllListeners();
            OnEffectTicked?.RemoveAllListeners();
            
            // Remove all effects
            RemoveAllEffects();
        }
        
        #endregion
    }
} 