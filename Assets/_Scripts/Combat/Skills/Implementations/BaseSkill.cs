using System.Collections.Generic;
using GuildsOfArcanaTerra.Combat.Core;
using GuildsOfArcanaTerra.Combat.Skills.Interfaces;
using UnityEngine;

namespace GuildsOfArcanaTerra.Combat.Skills.Implementations
{
    public abstract class BaseSkill : IBaseSkill
    {
        public abstract string SkillName { get; }
        public abstract string Description { get; }
        public abstract int Cooldown { get; }
        public int CurrentCooldown { get; set; }
        public abstract SkillTargetType TargetType { get; }
        public abstract int MaxTargets { get; }
        
        public abstract void Execute(Combatant caster, List<ICombatant> targets, StatusEffectSystem statusEffectSystem);
        
        public virtual bool CanUse(Combatant caster)
        {
            return CurrentCooldown <= 0;
        }
        
        public virtual void OnCooldownComplete()
        {
            CurrentCooldown = 0;
        }
        
        public virtual string GetDescription()
        {
            return Description;
        }
        
        protected virtual void StartCooldown()
        {
            CurrentCooldown = Cooldown;
        }
        
        protected virtual int CalculateDamage(Combatant caster, ICombatant target, float baseDamage, float scaling)
        {
            // Base damage calculation logic
            float damage = baseDamage + (caster.Strength * scaling);
            return Mathf.RoundToInt(damage);
        }
        
        protected virtual int CalculateHealing(Combatant caster, ICombatant target, float baseHealing, float scaling)
        {
            // Base healing calculation logic
            float healing = baseHealing + (caster.Intelligence * scaling);
            return Mathf.RoundToInt(healing);
        }
    }
} 