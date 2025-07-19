using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GuildsOfArcanaTerra.ScriptableObjects.Classes;
using GuildsOfArcanaTerra.Combat.Core;
using GuildsOfArcanaTerra.Combat.Skills;

namespace GuildsOfArcanaTerra.Combat
{
    /// <summary>
    /// Global skill system manager that coordinates skill usage across all combatants
    /// Integrates with TurnOrderSystem to manage cooldowns and skill availability
    /// </summary>
    public class SkillSystemManager : MonoBehaviour
    {
        [Header("Integration")]
        [SerializeField] private TurnOrderSystem turnOrderSystem;
        
        [Header("Debug")]
        [SerializeField] private bool debugMode = false;
        
        // Events
        public UnityEvent<SkillSet, Skill> OnGlobalSkillUsed;
        public UnityEvent OnCombatSkillsReset;
        
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
                turnOrderSystem.OnCombatantTurnEnd.AddListener(OnCombatantTurnEnd);
                turnOrderSystem.OnCombatStart.AddListener(OnCombatStart);
                turnOrderSystem.OnCombatEnd.AddListener(OnCombatEnd);
            }
            else
            {
                Debug.LogWarning("SkillSystemManager: No TurnOrderSystem found - cooldowns will not be managed automatically");
            }
            
            if (debugMode)
            {
                Debug.Log("SkillSystemManager: Initialized global skill system");
            }
        }
        
        /// <summary>
        /// Called when a combatant's turn ends - reduce their skill cooldowns
        /// </summary>
        private void OnCombatantTurnEnd(ICombatant combatant)
        {
            if (combatant is MonoBehaviour mb)
            {
                var skillSet = mb.GetComponent<SkillSet>();
                if (skillSet != null)
                {
                    skillSet.ReduceAllCooldowns();
                    
                    if (debugMode)
                    {
                        Debug.Log($"SkillSystemManager: Reduced cooldowns for {combatant.Name}");
                    }
                }
            }
        }
        
        /// <summary>
        /// Called when combat starts - reset all skill cooldowns
        /// </summary>
        private void OnCombatStart()
        {
            // Find all skill sets in the scene and reset their cooldowns
            var skillSets = FindObjectsOfType<SkillSet>();
            foreach (var skillSet in skillSets)
            {
                skillSet.ResetAllCooldowns();
            }
            
            OnCombatSkillsReset?.Invoke();
            
            if (debugMode)
            {
                Debug.Log($"SkillSystemManager: Reset cooldowns for {skillSets.Length} skill sets");
            }
        }
        
        /// <summary>
        /// Called when combat ends
        /// </summary>
        private void OnCombatEnd()
        {
            if (debugMode)
            {
                Debug.Log("SkillSystemManager: Combat ended");
            }
        }
        
        /// <summary>
        /// Reduce cooldowns for a specific combatant
        /// </summary>
        public void ReduceCooldownsForCombatant(ICombatant combatant)
        {
            if (combatant is MonoBehaviour mb)
            {
                var skillSet = mb.GetComponent<SkillSet>();
                if (skillSet != null)
                {
                    skillSet.ReduceAllCooldowns();
                }
            }
        }
        
        /// <summary>
        /// Reset all skill cooldowns across all combatants
        /// </summary>
        public void ResetAllCooldowns()
        {
            var skillSets = FindObjectsOfType<SkillSet>();
            foreach (var skillSet in skillSets)
            {
                skillSet.ResetAllCooldowns();
            }
            
            OnCombatSkillsReset?.Invoke();
        }
        
        /// <summary>
        /// Clean up event subscriptions when destroyed
        /// </summary>
        private void OnDestroy()
        {
            if (turnOrderSystem != null)
            {
                turnOrderSystem.OnCombatantTurnEnd.RemoveListener(OnCombatantTurnEnd);
                turnOrderSystem.OnCombatStart.RemoveListener(OnCombatStart);
                turnOrderSystem.OnCombatEnd.RemoveListener(OnCombatEnd);
            }
        }
    }
} 