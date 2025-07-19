using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GuildsOfArcanaTerra.Combat.Skills.Interfaces;

namespace GuildsOfArcanaTerra.Combat.Skills
{
    /// <summary>
    /// Manages a collection of skills for a single combatant
    /// </summary>
    public class SkillSet : MonoBehaviour
    {
        [Header("Skills")]
        [SerializeField] private List<IBaseSkill> skills = new List<IBaseSkill>();
        
        [Header("Debug")]
        [SerializeField] private bool debugMode = false;
        
        // Events
        public UnityEvent<IBaseSkill> OnSkillUsed;
        public UnityEvent<IBaseSkill> OnSkillCooldownReset;
        
        // Properties
        public List<IBaseSkill> Skills => new List<IBaseSkill>(skills);
        public int SkillCount => skills.Count;
        
        /// <summary>
        /// Initialize the skill set with the given skills
        /// </summary>
        public void InitializeSkills(List<IBaseSkill> skillList)
        {
            skills.Clear();
            skills.AddRange(skillList);
            
            if (debugMode)
            {
                Debug.Log($"SkillSystem: Initialized skill set with {skills.Count} skills");
                foreach (var skill in skills)
                {
                    Debug.Log($"SkillSystem: Added skill {skill.SkillName} (CD: {skill.Cooldown})");
                }
            }
        }
        
        /// <summary>
        /// Add a skill to the skill set
        /// </summary>
        public void AddSkill(IBaseSkill skill)
        {
            if (skill == null) return;
            
            skills.Add(skill);
            
            if (debugMode)
            {
                Debug.Log($"SkillSystem: Added skill {skill.SkillName} to skill set");
            }
        }
        
        /// <summary>
        /// Remove a skill from the skill set
        /// </summary>
        public void RemoveSkill(IBaseSkill skill)
        {
            if (skill == null) return;
            
            skills.Remove(skill);
            
            if (debugMode)
            {
                Debug.Log($"SkillSystem: Removed skill {skill.SkillName} from skill set");
            }
        }
        
        /// <summary>
        /// Get a skill by name
        /// </summary>
        public IBaseSkill GetSkill(string skillName)
        {
            return skills.Find(s => s.SkillName == skillName);
        }
        
        /// <summary>
        /// Get a skill by index
        /// </summary>
        public IBaseSkill GetSkill(int index)
        {
            if (index >= 0 && index < skills.Count)
            {
                return skills[index];
            }
            return null;
        }
        
        /// <summary>
        /// Get the basic attack skill (first skill in the list)
        /// </summary>
        public IBaseSkill GetBasicAttack()
        {
            return skills.Count > 0 ? skills[0] : null;
        }
        
        /// <summary>
        /// Get all active skills (all skills except the first one)
        /// </summary>
        public List<IBaseSkill> GetActiveSkills()
        {
            if (skills.Count <= 1) return new List<IBaseSkill>();
            return skills.GetRange(1, skills.Count - 1);
        }
        
        /// <summary>
        /// Use a skill by name
        /// </summary>
        public bool UseSkill(string skillName)
        {
            var skill = GetSkill(skillName);
            if (skill == null)
            {
                Debug.LogWarning($"SkillSystem: Skill {skillName} not found!");
                return false;
            }
            
            return UseSkill(skill);
        }
        
        /// <summary>
        /// Use a skill by index
        /// </summary>
        public bool UseSkill(int index)
        {
            var skill = GetSkill(index);
            if (skill == null)
            {
                Debug.LogWarning($"SkillSystem: No skill at index {index}!");
                return false;
            }
            
            return UseSkill(skill);
        }
        
        /// <summary>
        /// Use a specific skill
        /// </summary>
        public bool UseSkill(IBaseSkill skill, Combatant caster = null, List<ICombatant> targets = null)
        {
            if (skill == null)
            {
                Debug.LogWarning("SkillSystem: Cannot use null skill!");
                return false;
            }
            
            if (!skills.Contains(skill))
            {
                Debug.LogWarning($"SkillSystem: Skill {skill.SkillName} is not in this skill set!");
                return false;
            }
            
            if (!skill.CanUse(caster))
            {
                Debug.LogWarning($"SkillSystem: Skill {skill.SkillName} cannot be used!");
                return false;
            }
            
            skill.Execute(caster, targets, null);
            OnSkillUsed?.Invoke(skill);
            return true;
        }
        
        /// <summary>
        /// Reduce cooldowns for all skills by 1 turn
        /// </summary>
        public void ReduceAllCooldowns()
        {
            foreach (var skill in skills)
            {
                if (skill.CurrentCooldown > 0)
                {
                    skill.CurrentCooldown--;
                    if (skill.CurrentCooldown == 0)
                    {
                        skill.OnCooldownComplete();
                        OnSkillCooldownReset?.Invoke(skill);
                    }
                }
            }
            
            if (debugMode)
            {
                Debug.Log($"SkillSystem: Reduced cooldowns for {skills.Count} skills");
            }
        }
        
        /// <summary>
        /// Reset cooldowns for all skills
        /// </summary>
        public void ResetAllCooldowns()
        {
            foreach (var skill in skills)
            {
                skill.CurrentCooldown = 0;
                skill.OnCooldownComplete();
            }
            
            if (debugMode)
            {
                Debug.Log($"SkillSystem: Reset cooldowns for {skills.Count} skills");
            }
        }
        
        /// <summary>
        /// Get a dictionary of skill names and their current cooldowns
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
        /// Clear all skills from the skill set
        /// </summary>
        public void ClearSkills()
        {
            skills.Clear();
            
            if (debugMode)
            {
                Debug.Log("SkillSystem: Cleared all skills from skill set");
            }
        }
        
        /// <summary>
        /// Check if there are any skills available (not on cooldown)
        /// </summary>
        public bool HasAvailableSkills()
        {
            return skills.Exists(s => s.CanUse(null));
        }
        
        /// <summary>
        /// Get all skills that are currently available (not on cooldown)
        /// </summary>
        public List<IBaseSkill> GetAvailableSkills()
        {
            return skills.FindAll(s => s.CanUse(null));
        }
        
        /// <summary>
        /// Get all skills including those on cooldown
        /// </summary>
        public List<IBaseSkill> GetAllSkills()
        {
            return new List<IBaseSkill>(skills);
        }
        
        /// <summary>
        /// Clean up when destroyed
        /// </summary>
        private void OnDestroy()
        {
            ClearSkills();
        }
    }
} 