using System.Collections.Generic;
using UnityEngine;
using GuildsOfArcanaTerra.Combat.Core;

namespace GuildsOfArcanaTerra.Combat.Skills.Effects
{
    [CreateAssetMenu(fileName = "HealEffect", menuName = "GOAT/Skills/Effects/Heal")]
    public class HealEffectSO : SkillEffectSO
    {
        [Header("Heal Settings")]
        public float baseHealing = 10f;
        public float scaling = 1.0f;

        public override void Apply(Combatant caster, List<ICombatant> targets, StatusEffectSystem statusEffectSystem)
        {
            if (targets == null || targets.Count == 0) return;
            foreach (var target in targets)
            {
                int heal = DamageCalculator.CalculateHealing(caster.Intelligence, scaling) + Mathf.RoundToInt(baseHealing);
                target.Heal(heal);
            }
        }
    }
}


