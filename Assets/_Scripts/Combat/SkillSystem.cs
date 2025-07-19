using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GuildsOfArcanaTerra.Combat
{
    /// <summary>
    /// Defines the type of skill
    /// </summary>
    public enum SkillType
    {
        Basic,      // 0-CD basic attack
        Active      // Skills with cooldowns
    }
    
    /// <summary>
    /// Represents a single skill with cooldown management
    /// </summary>
    [System.Serializable]
    public class Skill
    {
        [Header("Skill Info")]
        [SerializeField] private string skillName;
        [SerializeField] private SkillType skillType;
        [SerializeField] private int cooldownDuration;
        
        [Header("Current State")]
        [SerializeField] private int currentCooldown;
        
        // Events
        public UnityEvent<Skill> OnSkillUsed;
        public UnityEvent<Skill> OnCooldownReset;
        public UnityEvent<Skill> OnCooldownReduced;
        
        // Properties
        public string SkillName => skillName;
        public SkillType Type => skillType;
        public int CooldownDuration => cooldownDuration;
        public int CurrentCooldown => currentCooldown;
        public bool IsOnCooldown => currentCooldown > 0;
        public bool CanUse => !IsOnCooldown;
        
        /// <summary>
        /// Constructor for creating skills
        /// </summary>
        public Skill(string name, SkillType type, int cooldown = 0)
        {
            skillName = name;
            skillType = type;
            cooldownDuration = cooldown;
            currentCooldown = 0;
            
            // Initialize events
            OnSkillUsed = new UnityEvent<Skill>();
            OnCooldownReset = new UnityEvent<Skill>();
            OnCooldownReduced = new UnityEvent<Skill>();
        }
        
        /// <summary>
        /// Attempt to use the skill
        /// </summary>
        /// <returns>True if skill was used successfully</returns>
        public bool UseSkill()
        {
            if (!CanUse)
            {
                Debug.LogWarning($"SkillSystem: Cannot use {skillName} - still on cooldown ({currentCooldown} turns remaining)");
                return false;
            }
            
            // Set cooldown if this is an active skill
            if (skillType == SkillType.Active && cooldownDuration > 0)
            {
                currentCooldown = cooldownDuration;
            }
            
            // Trigger skill effect
            TriggerSkillEffect();
            
            // Notify listeners
            OnSkillUsed?.Invoke(this);
            
            Debug.Log($"SkillSystem: Used skill {skillName}");
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
            if (skillType == SkillType.Basic)
            {
                return "Ready";
            }
            
            return IsOnCooldown ? $"{currentCooldown}/{cooldownDuration}" : "Ready";
        }
    }
    
    /// <summary>
    /// Manages a collection of skills for a single combatant
    /// </summary>
    public class SkillSet : MonoBehaviour
    {
        [Header("Skills")]
        [SerializeField] private List<Skill> skills = new List<Skill>();
        
        [Header("Debug")]
        [SerializeField] private bool debugMode = false;
        
        // Events
        public UnityEvent<Skill> OnSkillUsed;
        public UnityEvent<Skill> OnSkillCooldownReset;
        
        // Properties
        public List<Skill> Skills => new List<Skill>(skills);
        public int SkillCount => skills.Count;
        
        /// <summary>
        /// Initialize the skill set with the given skills
        /// </summary>
        public void InitializeSkills(List<Skill> skillList)
        {
            skills.Clear();
            skills.AddRange(skillList);
            
            if (debugMode)
            {
                Debug.Log($"SkillSystem: Initialized skill set with {skills.Count} skills");
                foreach (var skill in skills)
                {
                    Debug.Log($"SkillSystem: Added skill {skill.SkillName} (Type: {skill.Type}, CD: {skill.CooldownDuration})");
                }
            }
        }
        
        /// <summary>
        /// Add a skill to the skill set
        /// </summary>
        public void AddSkill(Skill skill)
        {
            if (skill == null) return;
            
            skills.Add(skill);
            
            // Subscribe to skill events
            skill.OnSkillUsed.AddListener(OnSkillUsedHandler);
            skill.OnCooldownReset.AddListener(OnSkillCooldownResetHandler);
            
            if (debugMode)
            {
                Debug.Log($"SkillSystem: Added skill {skill.SkillName}");
            }
        }
        
        /// <summary>
        /// Remove a skill from the skill set
        /// </summary>
        public void RemoveSkill(Skill skill)
        {
            if (skill == null) return;
            
            // Unsubscribe from events
            skill.OnSkillUsed.RemoveListener(OnSkillUsedHandler);
            skill.OnCooldownReset.RemoveListener(OnSkillCooldownResetHandler);
            
            skills.Remove(skill);
            
            if (debugMode)
            {
                Debug.Log($"SkillSystem: Removed skill {skill.SkillName}");
            }
        }
        
        /// <summary>
        /// Get a skill by name
        /// </summary>
        public Skill GetSkill(string skillName)
        {
            return skills.Find(s => s.SkillName.Equals(skillName, StringComparison.OrdinalIgnoreCase));
        }
        
        /// <summary>
        /// Get a skill by index
        /// </summary>
        public Skill GetSkill(int index)
        {
            if (index >= 0 && index < skills.Count)
            {
                return skills[index];
            }
            return null;
        }
        
        /// <summary>
        /// Get the basic attack skill
        /// </summary>
        public Skill GetBasicAttack()
        {
            return skills.Find(s => s.Type == SkillType.Basic);
        }
        
        /// <summary>
        /// Get all active skills (non-basic)
        /// </summary>
        public List<Skill> GetActiveSkills()
        {
            return skills.FindAll(s => s.Type == SkillType.Active);
        }
        
        /// <summary>
        /// Attempt to use a skill by name
        /// </summary>
        public bool UseSkill(string skillName)
        {
            var skill = GetSkill(skillName);
            if (skill == null)
            {
                Debug.LogError($"SkillSystem: Skill '{skillName}' not found");
                return false;
            }
            
            return UseSkill(skill);
        }
        
        /// <summary>
        /// Attempt to use a skill by index
        /// </summary>
        public bool UseSkill(int index)
        {
            var skill = GetSkill(index);
            if (skill == null)
            {
                Debug.LogError($"SkillSystem: No skill at index {index}");
                return false;
            }
            
            return UseSkill(skill);
        }
        
        /// <summary>
        /// Attempt to use a skill
        /// </summary>
        public bool UseSkill(Skill skill)
        {
            if (skill == null) return false;
            
            bool success = skill.UseSkill();
            if (success)
            {
                OnSkillUsed?.Invoke(skill);
            }
            
            return success;
        }
        
        /// <summary>
        /// Reduce cooldowns for all skills (called at end of turn)
        /// </summary>
        public void ReduceAllCooldowns()
        {
            foreach (var skill in skills)
            {
                skill.ReduceCooldown();
            }
            
            if (debugMode)
            {
                Debug.Log($"SkillSystem: Reduced cooldowns for all skills");
            }
        }
        
        /// <summary>
        /// Reset all cooldowns (called at combat start)
        /// </summary>
        public void ResetAllCooldowns()
        {
            foreach (var skill in skills)
            {
                skill.ResetCooldown();
            }
            
            if (debugMode)
            {
                Debug.Log($"SkillSystem: Reset all cooldowns");
            }
        }
        
        /// <summary>
        /// Get a dictionary of skill names to current cooldowns (for ICombatant interface)
        /// </summary>
        public Dictionary<string, int> GetCooldownDictionary()
        {
            var cooldowns = new Dictionary<string, int>();
            foreach (var skill in skills)
            {
                cooldowns[skill.SkillName] = skill.CurrentCooldown;
            }
            return cooldowns;
        }
        
        /// <summary>
        /// Clear all skills from this skill set
        /// </summary>
        public void ClearSkills()
        {
            skills.Clear();
            
            if (debugMode)
            {
                Debug.Log($"SkillSet: Cleared all skills");
            }
        }
        
        /// <summary>
        /// Check if any skill is available to use
        /// </summary>
        public bool HasAvailableSkills()
        {
            return skills.Exists(s => s.CanUse);
        }
        
        /// <summary>
        /// Get all skills that are currently available
        /// </summary>
        public List<Skill> GetAvailableSkills()
        {
            return skills.FindAll(s => s.CanUse);
        }
        
        /// <summary>
        /// Get all skills including those on cooldown
        /// </summary>
        public List<Skill> GetAllSkills()
        {
            return new List<Skill>(skills);
        }
        
        #region Event Handlers
        
        private void OnSkillUsedHandler(Skill skill)
        {
            if (debugMode)
            {
                Debug.Log($"SkillSystem: Skill {skill.SkillName} was used");
            }
        }
        
        private void OnSkillCooldownResetHandler(Skill skill)
        {
            if (debugMode)
            {
                Debug.Log($"SkillSystem: Skill {skill.SkillName} cooldown reset");
            }
        }
        
        #endregion
        
        #region Unity Lifecycle
        
        private void OnDestroy()
        {
            // Clean up events
            OnSkillUsed?.RemoveAllListeners();
            OnSkillCooldownReset?.RemoveAllListeners();
            
            // Unsubscribe from skill events
            foreach (var skill in skills)
            {
                if (skill != null)
                {
                    skill.OnSkillUsed.RemoveListener(OnSkillUsedHandler);
                    skill.OnCooldownReset.RemoveListener(OnSkillCooldownResetHandler);
                }
            }
        }
        
        #endregion
    }
    
    /// <summary>
    /// Global skill system manager that integrates with TurnOrderSystem
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
            }
            else
            {
                Debug.LogWarning("SkillSystemManager: No TurnOrderSystem found - cooldown reduction will not work automatically");
            }
        }
        
        /// <summary>
        /// Called when a combatant's turn ends - reduce their cooldowns
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
        /// Called when combat starts - reset all cooldowns
        /// </summary>
        private void OnCombatStart()
        {
            // Find all skill sets in the scene and reset their cooldowns
            var allSkillSets = FindObjectsOfType<SkillSet>();
            foreach (var skillSet in allSkillSets)
            {
                skillSet.ResetAllCooldowns();
            }
            
            OnCombatSkillsReset?.Invoke();
            
            if (debugMode)
            {
                Debug.Log($"SkillSystemManager: Reset cooldowns for {allSkillSets.Length} skill sets");
            }
        }
        
        /// <summary>
        /// Manually reduce cooldowns for a specific combatant
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
        /// Manually reset all cooldowns in the scene
        /// </summary>
        public void ResetAllCooldowns()
        {
            var allSkillSets = FindObjectsOfType<SkillSet>();
            foreach (var skillSet in allSkillSets)
            {
                skillSet.ResetAllCooldowns();
            }
        }
        
        #region Unity Lifecycle
        
        private void OnDestroy()
        {
            // Unsubscribe from turn order events
            if (turnOrderSystem != null)
            {
                turnOrderSystem.OnCombatantTurnEnd.RemoveListener(OnCombatantTurnEnd);
                turnOrderSystem.OnCombatStart.RemoveListener(OnCombatStart);
            }
            
            // Clean up events
            OnGlobalSkillUsed?.RemoveAllListeners();
            OnCombatSkillsReset?.RemoveAllListeners();
        }
        
        #endregion
    }
} 