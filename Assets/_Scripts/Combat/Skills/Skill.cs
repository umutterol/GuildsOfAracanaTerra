using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GuildsOfArcanaTerra.Combat.Core;
using GuildsOfArcanaTerra.Combat.Skills.Interfaces;

namespace GuildsOfArcanaTerra.Combat.Skills
{
    /// <summary>
    /// Represents a single skill with cooldown management
    /// Legacy class - use IBaseSkill implementations instead
    /// </summary>
    [System.Serializable]
    public class Skill : IBaseSkill
    {
        [Header("Skill Info")]
        [SerializeField] private string skillName;
        [SerializeField] private Core.SkillType skillType;
        [SerializeField] private int cooldownDuration;
        [SerializeField] private Core.SkillTargetType targetType;
        
        [Header("Current State")]
        [SerializeField] private int currentCooldown;
        
        // Events
        public UnityEvent<Skill> OnSkillUsed;
        public UnityEvent<Skill> OnCooldownReset;
        public UnityEvent<Skill> OnCooldownReduced;
        
        // Properties
        public string SkillName => skillName;
        public string Description => $"Legacy skill: {skillName}";
        public int Cooldown => cooldownDuration;
        public int CurrentCooldown { get; set; }
        public bool IsOnCooldown => currentCooldown > 0;
        public Core.SkillTargetType TargetType => targetType;
        public int MaxTargets => 1;
        
        // Legacy properties
        public Core.SkillType Type => skillType;
        public int CooldownDuration => cooldownDuration;
        
        /// <summary>
        /// Constructor for creating skills
        /// </summary>
        public Skill(string name, Core.SkillType type, int cooldown = 0, Core.SkillTargetType targetType = Core.SkillTargetType.SingleEnemy)
        {
            skillName = name;
            skillType = type;
            cooldownDuration = cooldown;
            currentCooldown = 0;
            this.targetType = targetType;
            
            // Initialize events
            OnSkillUsed = new UnityEvent<Skill>();
            OnCooldownReset = new UnityEvent<Skill>();
            OnCooldownReduced = new UnityEvent<Skill>();
        }
        
        /// <summary>
        /// Execute the skill (IBaseSkill implementation)
        /// </summary>
        public void Execute(Combatant caster, List<ICombatant> targets, StatusEffectSystem statusEffectSystem)
        {
            if (!CanUse(caster))
            {
                Debug.LogWarning($"SkillSystem: Cannot use {skillName} - still on cooldown ({currentCooldown} turns remaining)");
                return;
            }
            
            // Set cooldown if this is an active skill
            if (skillType == Core.SkillType.Active && cooldownDuration > 0)
            {
                currentCooldown = cooldownDuration;
            }
            
            // Execute the skill effect
            if (caster != null && targets != null && targets.Count > 0)
            {
                SkillEffects.ExecuteSkill(this, caster, targets, statusEffectSystem);
            }
            else
            {
                // Fallback for testing without proper parameters
                TriggerSkillEffect();
            }
            
            // Notify listeners
            OnSkillUsed?.Invoke(this);
            
            Debug.Log($"SkillSystem: Used skill {skillName}");
        }
        
        /// <summary>
        /// Check if skill can be used (IBaseSkill implementation)
        /// </summary>
        public bool CanUse(Combatant caster)
        {
            return !IsOnCooldown;
        }
        
        /// <summary>
        /// Called when cooldown completes (IBaseSkill implementation)
        /// </summary>
        public void OnCooldownComplete()
        {
            currentCooldown = 0;
        }
        
        /// <summary>
        /// Get description (IBaseSkill implementation)
        /// </summary>
        public string GetDescription()
        {
            return Description;
        }
        
        /// <summary>
        /// Legacy method - use Execute instead
        /// </summary>
        /// <returns>True if skill was used successfully</returns>
        public bool UseSkill(Combatant caster = null, System.Collections.Generic.List<ICombatant> targets = null, StatusEffectSystem statusEffectSystem = null)
        {
            Execute(caster, targets, statusEffectSystem);
            return true;
        }
        
        /// <summary>
        /// Reduce cooldown by 1 turn
        /// </summary>
        public void ReduceCooldown()
        {
            if (currentCooldown > 0)
            {
                currentCooldown--;
                OnCooldownReduced?.Invoke(this);
                
                if (currentCooldown == 0)
                {
                    OnCooldownReset?.Invoke(this);
                    Debug.Log($"SkillSystem: {skillName} is now off cooldown");
                }
            }
        }
        
        /// <summary>
        /// Reset cooldown to 0
        /// </summary>
        public void ResetCooldown()
        {
            if (currentCooldown > 0)
            {
                currentCooldown = 0;
                OnCooldownReset?.Invoke(this);
            }
        }
        
        /// <summary>
        /// Set cooldown to a specific value
        /// </summary>
        public void SetCooldown(int turns)
        {
            currentCooldown = Mathf.Max(0, turns);
        }
        
        /// <summary>
        /// Trigger the skill's effect (placeholder implementation)
        /// </summary>
        private void TriggerSkillEffect()
        {
            // Placeholder effect - this will be expanded with actual skill logic
            Debug.Log($"SkillSystem: {skillName} effect triggered!");
            
            // TODO: Implement actual skill effects based on class and skill type
            // This will be replaced with damage calculation, status effects, etc.
        }
        
        /// <summary>
        /// Get a string representation of the skill's cooldown status
        /// </summary>
        public string GetCooldownStatus()
        {
            if (skillType == Core.SkillType.Basic)
            {
                return "Ready";
            }
            
            return IsOnCooldown ? $"{currentCooldown}/{cooldownDuration}" : "Ready";
        }
    }
} 