using System.Collections.Generic;
using UnityEngine;
using GuildsOfArcanaTerra.Combat.Core;

namespace GuildsOfArcanaTerra.Combat.Skills.Effects
{
    /// <summary>
    /// Base ScriptableObject for skill effects
    /// </summary>
    public abstract class SkillEffectSO : ScriptableObject, ISkillEffect
    {
        [Header("Common Settings")]
        [Tooltip("Optional label for debugging")] public string effectName;
        [Tooltip("Row/slot reach constraint for this effect (optional)")] public GuildsOfArcanaTerra.Combat.Core.SkillReach reach = GuildsOfArcanaTerra.Combat.Core.SkillReach.RangedAny;

        public abstract void Apply(Combatant caster, List<ICombatant> targets, StatusEffectSystem statusEffectSystem);
    }
}


