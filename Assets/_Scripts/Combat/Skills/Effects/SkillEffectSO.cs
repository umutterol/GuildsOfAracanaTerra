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

        public abstract void Apply(Combatant caster, List<ICombatant> targets, StatusEffectSystem statusEffectSystem);
    }
}


