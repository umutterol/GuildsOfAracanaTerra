using System.Collections.Generic;
using GuildsOfArcanaTerra.Combat.Core;

namespace GuildsOfArcanaTerra.Combat.Skills.Interfaces
{
    public interface IBaseSkill
    {
        string SkillName { get; }
        string Description { get; }
        int Cooldown { get; }
        int CurrentCooldown { get; set; }
        SkillTargetType TargetType { get; }
        int MaxTargets { get; }
        
        void Execute(Combatant caster, List<ICombatant> targets, StatusEffectSystem statusEffectSystem);
        bool CanUse(Combatant caster);
        void OnCooldownComplete();
        string GetDescription();
    }
} 