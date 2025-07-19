using System.Collections.Generic;
using UnityEngine;

namespace GuildsOfArcanaTerra.Combat
{
    /// <summary>
    /// Simple test combatant for testing the status effect system
    /// </summary>
    public class TestCombatant : MonoBehaviour, ICombatant
    {
        [Header("Test Combatant Stats")]
        [SerializeField] private string characterName = "Test Warrior";
        [SerializeField] private int agility = 15;
        [SerializeField] private int intelligence = 10;
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int currentHealth = 100;
        
        [Header("Debug")]
        [SerializeField] private bool debugMode = true;
        
        // ICombatant Implementation
        public string Name => characterName;
        public int AGI => agility;
        public int INT => intelligence;
        public bool IsAlive => currentHealth > 0;
        
        // Cooldowns (for ICombatant interface)
        private Dictionary<string, int> cooldowns = new Dictionary<string, int>();
        public Dictionary<string, int> Cooldowns => cooldowns;
        
        // Events
        public System.Action<int> OnDamageTaken;
        public System.Action<int> OnHealed;
        public System.Action OnTurnStarted;
        public System.Action OnTurnEnded;
        
        /// <summary>
        /// Take damage from attacks or effects
        /// </summary>
        public void TakeDamage(int damage)
        {
            if (!IsAlive) return;
            
            currentHealth = Mathf.Max(0, currentHealth - damage);
            
            if (debugMode)
            {
                Debug.Log($"{Name} takes {damage} damage! Health: {currentHealth}/{maxHealth}");
            }
            
            OnDamageTaken?.Invoke(damage);
            
            if (!IsAlive)
            {
                if (debugMode)
                {
                    Debug.Log($"{Name} has been defeated!");
                }
            }
        }
        
        /// <summary>
        /// Heal the combatant
        /// </summary>
        public void Heal(int amount)
        {
            if (!IsAlive) return;
            
            int oldHealth = currentHealth;
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            int actualHeal = currentHealth - oldHealth;
            
            if (actualHeal > 0)
            {
                if (debugMode)
                {
                    Debug.Log($"{Name} heals {actualHeal} health! Health: {currentHealth}/{maxHealth}");
                }
                
                OnHealed?.Invoke(actualHeal);
            }
        }
        
        /// <summary>
        /// Called when this unit's turn begins
        /// </summary>
        public void OnTurnStart()
        {
            if (debugMode)
            {
                Debug.Log($"{Name}'s turn starts!");
            }
            
            OnTurnStarted?.Invoke();
        }
        
        /// <summary>
        /// Called when this unit's turn ends
        /// </summary>
        public void OnTurnEnd()
        {
            if (debugMode)
            {
                Debug.Log($"{Name}'s turn ends!");
            }
            
            OnTurnEnded?.Invoke();
        }
        
        /// <summary>
        /// Set a skill cooldown
        /// </summary>
        public void SetCooldown(string skillName, int turns)
        {
            cooldowns[skillName] = turns;
        }
        
        /// <summary>
        /// Get current health percentage
        /// </summary>
        public float GetHealthPercentage()
        {
            return (float)currentHealth / maxHealth;
        }
        
        /// <summary>
        /// Reset health to full
        /// </summary>
        public void ResetHealth()
        {
            currentHealth = maxHealth;
            if (debugMode)
            {
                Debug.Log($"{Name} health reset to {currentHealth}/{maxHealth}");
            }
        }
        
        /// <summary>
        /// Get a string representation of the combatant's status
        /// </summary>
        public override string ToString()
        {
            return $"{Name} (HP: {currentHealth}/{maxHealth}, AGI: {AGI}, INT: {INT}, Alive: {IsAlive})";
        }
    }
} 