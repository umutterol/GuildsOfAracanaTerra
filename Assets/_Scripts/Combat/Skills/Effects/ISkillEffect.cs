using System.Collections.Generic;
using GuildsOfArcanaTerra.Combat;

namespace GuildsOfArcanaTerra.Combat.Skills.Effects
{
    /// <summary>
    /// Interface for executable skill effects
    /// </summary>
    public interface ISkillEffect
    {
        void Apply(Combatant caster, System.Collections.Generic.List<ICombatant> targets, StatusEffectSystem statusEffectSystem);
    }
}


