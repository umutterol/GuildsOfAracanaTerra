using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GuildsOfArcanaTerra.Combat
{
    /// <summary>
    /// Interface that all combat units must implement
    /// </summary>
    public interface ICombatant
    {
        string Name { get; }
        string CharacterName { get; }
        int AGI { get; }
        int INT { get; }  // Intelligence stat for magic damage
        Dictionary<string, int> Cooldowns { get; }
        bool IsAlive { get; }
        
        /// <summary>
        /// Called when this unit's turn begins
        /// </summary>
        void OnTurnStart();
        
        /// <summary>
        /// Called when this unit's turn ends
        /// </summary>
        void OnTurnEnd();
        
        /// <summary>
        /// Take damage from an attack or effect
        /// </summary>
        void TakeDamage(int damage);
        
        /// <summary>
        /// Heal the combatant
        /// </summary>
        void Heal(int healAmount);
    }

    /// <summary>
    /// Manages turn order in combat based on AGI stat with random tie-breaking
    /// </summary>
    public class TurnOrderSystem : MonoBehaviour
    {
        [Header("Turn Order Settings")]
        [SerializeField] private bool debugMode = false;
        
        [Header("Events")]
        public UnityEvent<ICombatant> OnTurnChanged;
        public UnityEvent<ICombatant> OnCombatantTurnStart;
        public UnityEvent<ICombatant> OnCombatantTurnEnd;
        public UnityEvent OnCombatStart;
        public UnityEvent OnCombatEnd;
        
        // Turn order queue
        private Queue<ICombatant> turnQueue = new Queue<ICombatant>();
        private List<ICombatant> allCombatants = new List<ICombatant>();
        
        // Current state
        private ICombatant currentCombatant;
        private bool isCombatActive = false;
        
        // Properties
        public ICombatant CurrentCombatant => currentCombatant;
        public bool IsCombatActive => isCombatActive;
        public int TurnQueueCount => turnQueue.Count;
        public List<ICombatant> AllCombatants => new List<ICombatant>(allCombatants);
        
        /// <summary>
        /// Initialize combat with the given combatants and start turn order
        /// </summary>
        public void StartCombat(List<ICombatant> combatants)
        {
            if (combatants == null || combatants.Count == 0)
            {
                Debug.LogError("TurnOrderSystem: Cannot start combat with null or empty combatant list");
                return;
            }
            
            // Clear previous state
            turnQueue.Clear();
            allCombatants.Clear();
            
            // Add all combatants
            allCombatants.AddRange(combatants);
            
            // Sort by AGI (highest first) with random tie-breaking
            var sortedCombatants = SortCombatantsByAGI(allCombatants);
            
            // Build turn queue
            foreach (var combatant in sortedCombatants)
            {
                turnQueue.Enqueue(combatant);
            }
            
            isCombatActive = true;
            currentCombatant = null;
            
            if (debugMode)
            {
                Debug.Log($"TurnOrderSystem: Combat started with {combatants.Count} combatants");
                LogTurnOrder();
            }
            
            OnCombatStart?.Invoke();
            
            // Start first turn
            AdvanceToNextTurn();
        }
        
        /// <summary>
        /// End the current turn and advance to the next combatant
        /// </summary>
        public void EndCurrentTurn()
        {
            if (!isCombatActive)
            {
                Debug.LogWarning("TurnOrderSystem: Cannot end turn - combat not active");
                return;
            }
            
            if (currentCombatant != null)
            {
                // End current combatant's turn
                currentCombatant.OnTurnEnd();
                OnCombatantTurnEnd?.Invoke(currentCombatant);
                
                // Reduce cooldowns for current combatant
                ReduceCooldowns(currentCombatant);
            }
            
            AdvanceToNextTurn();
        }
        
        /// <summary>
        /// End combat and clean up
        /// </summary>
        public void EndCombat()
        {
            if (!isCombatActive) return;
            
            isCombatActive = false;
            currentCombatant = null;
            turnQueue.Clear();
            allCombatants.Clear();
            
            if (debugMode)
            {
                Debug.Log("TurnOrderSystem: Combat ended");
            }
            
            OnCombatEnd?.Invoke();
        }
        
        /// <summary>
        /// Remove a combatant from the turn order (e.g., when they die)
        /// </summary>
        public void RemoveCombatant(ICombatant combatant)
        {
            if (combatant == null) return;
            
            allCombatants.Remove(combatant);
            
            // Rebuild turn queue without the removed combatant
            if (isCombatActive)
            {
                RebuildTurnQueue();
            }
            
            if (debugMode)
            {
                Debug.Log($"TurnOrderSystem: Removed {combatant.Name} from combat");
            }
        }
        
        /// <summary>
        /// Get the next few combatants in the turn order (for UI display)
        /// </summary>
        public List<ICombatant> GetUpcomingTurns(int count = 3)
        {
            var upcoming = new List<ICombatant>();
            var tempQueue = new Queue<ICombatant>(turnQueue);
            
            for (int i = 0; i < count && tempQueue.Count > 0; i++)
            {
                upcoming.Add(tempQueue.Dequeue());
            }
            
            return upcoming;
        }
        
        /// <summary>
        /// Sort combatants by AGI with random tie-breaking
        /// </summary>
        private List<ICombatant> SortCombatantsByAGI(List<ICombatant> combatants)
        {
            // Group by AGI value to handle ties
            var groupedByAGI = combatants
                .Where(c => c.IsAlive)
                .GroupBy(c => c.AGI)
                .OrderByDescending(g => g.Key) // Highest AGI first
                .ToList();
            
            var sortedCombatants = new List<ICombatant>();
            
            foreach (var group in groupedByAGI)
            {
                if (group.Count() == 1)
                {
                    // No tie, just add the single combatant
                    sortedCombatants.Add(group.First());
                }
                else
                {
                    // Tie detected - randomize order within this AGI group
                    var tiedCombatants = group.ToList();
                    ShuffleList(tiedCombatants);
                    sortedCombatants.AddRange(tiedCombatants);
                    
                    if (debugMode)
                    {
                        var names = string.Join(", ", tiedCombatants.Select(c => c.Name));
                        Debug.Log($"TurnOrderSystem: AGI tie at {group.Key} between: {names}");
                    }
                }
            }
            
            return sortedCombatants;
        }
        
        /// <summary>
        /// Shuffle a list using Fisher-Yates algorithm
        /// </summary>
        private void ShuffleList<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
        
        /// <summary>
        /// Advance to the next turn in the queue
        /// </summary>
        private void AdvanceToNextTurn()
        {
            // Check if combat should end (no more alive combatants)
            var aliveCombatants = allCombatants.Where(c => c.IsAlive).ToList();
            if (aliveCombatants.Count <= 1)
            {
                EndCombat();
                return;
            }
            
            // Get next combatant from queue
            if (turnQueue.Count == 0)
            {
                // Rebuild queue for next round
                RebuildTurnQueue();
            }
            
            if (turnQueue.Count > 0)
            {
                currentCombatant = turnQueue.Dequeue();
                
                // Skip dead combatants
                while (currentCombatant != null && !currentCombatant.IsAlive && turnQueue.Count > 0)
                {
                    currentCombatant = turnQueue.Dequeue();
                }
                
                if (currentCombatant != null && currentCombatant.IsAlive)
                {
                    // Start this combatant's turn
                    currentCombatant.OnTurnStart();
                    OnCombatantTurnStart?.Invoke(currentCombatant);
                    OnTurnChanged?.Invoke(currentCombatant);
                    
                    if (debugMode)
                    {
                        Debug.Log($"TurnOrderSystem: {currentCombatant.Name}'s turn (AGI: {currentCombatant.AGI})");
                    }
                }
                else
                {
                    // No more alive combatants in queue, rebuild
                    AdvanceToNextTurn();
                }
            }
        }
        
        /// <summary>
        /// Rebuild the turn queue for the next round
        /// </summary>
        private void RebuildTurnQueue()
        {
            var aliveCombatants = allCombatants.Where(c => c.IsAlive).ToList();
            if (aliveCombatants.Count == 0)
            {
                EndCombat();
                return;
            }
            
            var sortedCombatants = SortCombatantsByAGI(aliveCombatants);
            
            turnQueue.Clear();
            foreach (var combatant in sortedCombatants)
            {
                turnQueue.Enqueue(combatant);
            }
            
            if (debugMode)
            {
                Debug.Log($"TurnOrderSystem: Rebuilt turn queue for round {aliveCombatants.Count} alive combatants");
            }
        }
        
        /// <summary>
        /// Reduce cooldowns for a combatant at the end of their turn
        /// </summary>
        private void ReduceCooldowns(ICombatant combatant)
        {
            if (combatant?.Cooldowns == null) return;
            
            var cooldownsToReduce = new List<string>();
            
            foreach (var kvp in combatant.Cooldowns)
            {
                if (kvp.Value > 0)
                {
                    cooldownsToReduce.Add(kvp.Key);
                }
            }
            
            foreach (var skillName in cooldownsToReduce)
            {
                combatant.Cooldowns[skillName]--;
                
                if (debugMode && combatant.Cooldowns[skillName] == 0)
                {
                    Debug.Log($"TurnOrderSystem: {combatant.Name}'s {skillName} is now off cooldown");
                }
            }
        }
        
        /// <summary>
        /// Log the current turn order for debugging
        /// </summary>
        private void LogTurnOrder()
        {
            var order = new List<ICombatant>(turnQueue);
            var orderString = string.Join(" -> ", order.Select(c => $"{c.Name}(AGI:{c.AGI})"));
            Debug.Log($"TurnOrderSystem: Turn order: {orderString}");
        }
        
        #region Unity Lifecycle
        
        private void OnDestroy()
        {
            // Clean up events
            OnTurnChanged?.RemoveAllListeners();
            OnCombatantTurnStart?.RemoveAllListeners();
            OnCombatantTurnEnd?.RemoveAllListeners();
            OnCombatStart?.RemoveAllListeners();
            OnCombatEnd?.RemoveAllListeners();
        }
        
        #endregion
    }
} 