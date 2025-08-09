using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using GuildsOfArcanaTerra.Combat.Effects;

namespace GuildsOfArcanaTerra.Combat
{
    /// <summary>
    /// Global system that manages all status effects across all combatants
    /// Integrates with TurnOrderSystem to tick effects at appropriate times
    /// </summary>
    public class StatusEffectSystem : MonoBehaviour
    {
        [Header("System Settings")]
        [SerializeField] private bool debugMode = false;
        [SerializeField] private bool autoTickEffects = true;
        
        [Header("Integration")]
        [SerializeField] private TurnOrderSystem turnOrderSystem;
        
        // Events
        public UnityEvent<ICombatant, IStatusEffect> OnEffectApplied;
        public UnityEvent<ICombatant, IStatusEffect> OnEffectRemoved;
        public UnityEvent<ICombatant, IStatusEffect> OnEffectTicked;
        public UnityEvent<ICombatant> OnCombatantEffectsCleared;
        
        // Tracking
        private Dictionary<ICombatant, StatusEffectManager> combatantManagers = new Dictionary<ICombatant, StatusEffectManager>();
        
        // Properties
        public bool IsActive { get; private set; } = false;
        public int TotalActiveEffects => combatantManagers.Values.Sum(manager => manager.EffectCount);
        
        private void Start()
        {
            // Find turn order system if not assigned
            if (turnOrderSystem == null)
            {
                turnOrderSystem = FindObjectOfType<TurnOrderSystem>();
            }
            
            // Subscribe to turn order events
            if (turnOrderSystem != null)
            {
                turnOrderSystem.OnCombatantTurnStart.AddListener(OnCombatantTurnStart);
                turnOrderSystem.OnCombatantTurnEnd.AddListener(OnCombatantTurnEnd);
                turnOrderSystem.OnCombatStart.AddListener(OnCombatStart);
                turnOrderSystem.OnCombatEnd.AddListener(OnCombatEnd);
            }
            else
            {
                Debug.LogWarning("StatusEffectSystem: No TurnOrderSystem found - effects will not tick automatically");
            }
        }
        
        /// <summary>
        /// Register a combatant with the status effect system
        /// </summary>
        public void RegisterCombatant(ICombatant combatant)
        {
            if (combatant == null) return;
            
            if (combatant is MonoBehaviour mb)
            {
                var statusManager = mb.GetComponent<StatusEffectManager>();
                if (statusManager == null)
                {
                    statusManager = mb.gameObject.AddComponent<StatusEffectManager>();
                }
                
                combatantManagers[combatant] = statusManager;
                
                // Subscribe to manager events (with null checks)
                if (statusManager.OnEffectApplied != null)
                    statusManager.OnEffectApplied.AddListener((effect) => OnEffectApplied?.Invoke(combatant, effect));
                if (statusManager.OnEffectRemoved != null)
                    statusManager.OnEffectRemoved.AddListener((effect) => OnEffectRemoved?.Invoke(combatant, effect));
                if (statusManager.OnEffectTicked != null)
                    statusManager.OnEffectTicked.AddListener((effect) => OnEffectTicked?.Invoke(combatant, effect));
                
                if (debugMode)
                {
                    Debug.Log($"StatusEffectSystem: Registered {combatant.Name} for status effect management");
                }
            }
        }
        
        /// <summary>
        /// Unregister a combatant from the status effect system
        /// </summary>
        public void UnregisterCombatant(ICombatant combatant)
        {
            if (combatant == null) return;
            
            if (combatantManagers.TryGetValue(combatant, out var manager))
            {
                // Unsubscribe from events (with null checks)
                if (manager.OnEffectApplied != null)
                    manager.OnEffectApplied.RemoveAllListeners();
                if (manager.OnEffectRemoved != null)
                    manager.OnEffectRemoved.RemoveAllListeners();
                if (manager.OnEffectTicked != null)
                    manager.OnEffectTicked.RemoveAllListeners();
                
                combatantManagers.Remove(combatant);
                
                if (debugMode)
                {
                    Debug.Log($"StatusEffectSystem: Unregistered {combatant.Name} from status effect management");
                }
            }
        }
        
        /// <summary>
        /// Apply a status effect to a combatant
        /// </summary>
        public bool ApplyEffect(ICombatant target, IStatusEffect effect)
        {
            if (target == null || effect == null) return false;
            
            if (combatantManagers.TryGetValue(target, out var manager))
            {
                bool success = manager.ApplyEffect(effect);
                
                if (success && debugMode)
                {
                    Debug.Log($"StatusEffectSystem: Applied {effect.Name} to {target.Name}");
                }
                
                return success;
            }
            
            Debug.LogError($"StatusEffectSystem: Cannot apply effect to {target.Name} - not registered");
            return false;
        }
        
        /// <summary>
        /// Remove a specific effect from a combatant
        /// </summary>
        public bool RemoveEffect(ICombatant target, string effectName)
        {
            if (target == null) return false;
            
            if (combatantManagers.TryGetValue(target, out var manager))
            {
                return manager.RemoveEffect(effectName);
            }
            
            return false;
        }
        
        /// <summary>
        /// Check if a combatant has a specific effect
        /// </summary>
        public bool HasEffect(ICombatant target, string effectName)
        {
            if (target == null) return false;
            
            if (combatantManagers.TryGetValue(target, out var manager))
            {
                return manager.HasEffect(effectName);
            }
            
            return false;
        }
        
        /// <summary>
        /// Get all active effects on a combatant
        /// </summary>
        public List<IStatusEffect> GetCombatantEffects(ICombatant target)
        {
            if (target == null) return new List<IStatusEffect>();
            
            if (combatantManagers.TryGetValue(target, out var manager))
            {
                return manager.ActiveEffects;
            }
            
            return new List<IStatusEffect>();
        }
        
        /// <summary>
        /// Clear all effects from a combatant
        /// </summary>
        public void ClearCombatantEffects(ICombatant target)
        {
            if (target == null) return;
            
            if (combatantManagers.TryGetValue(target, out var manager))
            {
                manager.RemoveAllEffects();
                OnCombatantEffectsCleared?.Invoke(target);
                
                if (debugMode)
                {
                    Debug.Log($"StatusEffectSystem: Cleared all effects from {target.Name}");
                }
            }
        }
        
        /// <summary>
        /// Clear all effects from all combatants
        /// </summary>
        public void ClearAllEffects()
        {
            foreach (var kvp in combatantManagers)
            {
                kvp.Value.RemoveAllEffects();
                OnCombatantEffectsCleared?.Invoke(kvp.Key);
            }
            
            if (debugMode)
            {
                Debug.Log("StatusEffectSystem: Cleared all effects from all combatants");
            }
        }
        
        #region Effect Factory Methods (Based on GDD)
        
        /// <summary>
        /// Create a Burn effect (25% INT damage, does not stack)
        /// </summary>
        public IStatusEffect CreateBurnEffect(ICombatant target, ICombatant caster, int duration = 2)
        {
            return new BurnEffect(target, caster, duration);
        }
        
        /// <summary>
        /// Create a Bleed effect (20% AGI damage, stacks up to 3 times)
        /// </summary>
        public IStatusEffect CreateBleedEffect(ICombatant target, ICombatant caster, int duration = 3)
        {
            return new BleedEffect(target, caster, duration);
        }
        
        /// <summary>
        /// Create a Poison effect (15% AGI damage + 25% healing reduction)
        /// </summary>
        public IStatusEffect CreatePoisonEffect(ICombatant target, ICombatant caster, int duration = 3)
        {
            return new PoisonEffect(target, caster, duration);
        }
        
        /// <summary>
        /// Create a Stun effect (skip next turn, immunity for 1 turn after)
        /// </summary>
        public IStatusEffect CreateStunEffect(ICombatant target, ICombatant caster, int duration = 1)
        {
            return new StunEffect(target, caster, duration);
        }
        
        /// <summary>
        /// Create a Slow effect (AGI -10%)
        /// </summary>
        public IStatusEffect CreateSlowEffect(ICombatant target, ICombatant caster, int duration = 2)
        {
            return new SlowEffect(target, caster, duration);
        }
        
        /// <summary>
        /// Create a Shield effect (absorbs incoming damage)
        /// </summary>
        public IStatusEffect CreateShieldEffect(ICombatant target, ICombatant caster, int shieldValue, int duration = 2)
        {
            // If shieldValue is 0, create INT-based shield, otherwise flat-value shield
            if (shieldValue <= 0)
            {
                return new ShieldEffect(target, caster, duration, 1.0f);
            }
            return new ShieldEffect(target, caster, shieldValue, duration);
        }
        
        #endregion
        
        #region Turn Order Integration
        
        /// <summary>
        /// Called when a combatant's turn starts - tick their effects
        /// </summary>
        private void OnCombatantTurnStart(ICombatant combatant)
        {
            if (!autoTickEffects) return;
            
            if (combatantManagers.TryGetValue(combatant, out var manager))
            {
                manager.TickAllEffects();
                
                if (debugMode)
                {
                    Debug.Log($"StatusEffectSystem: Ticked effects for {combatant.Name}");
                }
            }
        }
        
        /// <summary>
        /// Called when a combatant's turn ends - reduce effect durations
        /// </summary>
        private void OnCombatantTurnEnd(ICombatant combatant)
        {
            if (combatantManagers.TryGetValue(combatant, out var manager))
            {
                manager.ReduceAllEffectDurations();
                
                if (debugMode)
                {
                    Debug.Log($"StatusEffectSystem: Reduced effect durations for {combatant.Name}");
                }
            }
        }
        
        /// <summary>
        /// Called when combat starts
        /// </summary>
        private void OnCombatStart()
        {
            IsActive = true;
            
            if (debugMode)
            {
                Debug.Log("StatusEffectSystem: Combat started - status effects active");
            }
        }
        
        /// <summary>
        /// Called when combat ends
        /// </summary>
        private void OnCombatEnd()
        {
            IsActive = false;
            
            // Clear all effects when combat ends
            ClearAllEffects();
            
            if (debugMode)
            {
                Debug.Log("StatusEffectSystem: Combat ended - cleared all status effects");
            }
        }
        
        #endregion
        
        #region Utility Methods
        
        /// <summary>
        /// Get a summary of all active effects across all combatants
        /// </summary>
        public string GetGlobalEffectsSummary()
        {
            var summaries = new List<string>();
            
            foreach (var kvp in combatantManagers)
            {
                var combatant = kvp.Key;
                var manager = kvp.Value;
                
                if (manager.EffectCount > 0)
                {
                    summaries.Add($"{combatant.Name}: {manager.GetEffectsSummary()}");
                }
            }
            
            return summaries.Count > 0 ? string.Join("\n", summaries) : "No active effects";
        }
        
        /// <summary>
        /// Get all combatants with active effects
        /// </summary>
        public List<ICombatant> GetCombatantsWithEffects()
        {
            return combatantManagers
                .Where(kvp => kvp.Value.EffectCount > 0)
                .Select(kvp => kvp.Key)
                .ToList();
        }
        
        /// <summary>
        /// Get the total number of effects of a specific type
        /// </summary>
        public int GetEffectCount(string effectName)
        {
            return combatantManagers.Values
                .Count(manager => manager.HasEffect(effectName));
        }
        
        /// <summary>
        /// Manually tick effects for a specific combatant
        /// </summary>
        public void TickCombatantEffects(ICombatant combatant)
        {
            if (combatantManagers.TryGetValue(combatant, out var manager))
            {
                manager.TickAllEffects();
            }
        }
        
        /// <summary>
        /// Manually reduce effect durations for a specific combatant
        /// </summary>
        public void ReduceCombatantEffectDurations(ICombatant combatant)
        {
            if (combatantManagers.TryGetValue(combatant, out var manager))
            {
                manager.ReduceAllEffectDurations();
            }
        }
        
        #endregion
        
        #region Unity Lifecycle
        
        private void OnDestroy()
        {
            // Unsubscribe from turn order events
            if (turnOrderSystem != null)
            {
                turnOrderSystem.OnCombatantTurnStart.RemoveListener(OnCombatantTurnStart);
                turnOrderSystem.OnCombatantTurnEnd.RemoveListener(OnCombatantTurnEnd);
                turnOrderSystem.OnCombatStart.RemoveListener(OnCombatStart);
                turnOrderSystem.OnCombatEnd.RemoveListener(OnCombatEnd);
            }
            
            // Clean up events
            OnEffectApplied?.RemoveAllListeners();
            OnEffectRemoved?.RemoveAllListeners();
            OnEffectTicked?.RemoveAllListeners();
            OnCombatantEffectsCleared?.RemoveAllListeners();
            
            // Clear all effects
            ClearAllEffects();
        }
        
        #endregion
    }
} 