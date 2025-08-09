using System.Collections.Generic;
using UnityEngine;

namespace GuildsOfArcanaTerra.Combat.Skills.Effects
{
    [CreateAssetMenu(fileName = "CompositeEffect", menuName = "GOAT/Skills/Effects/Composite")]
    public class CompositeEffectSO : SkillEffectSO
    {
        [Tooltip("Effects executed in order")] public List<SkillEffectSO> effects = new List<SkillEffectSO>();

        public override void Apply(Combatant caster, List<ICombatant> targets, StatusEffectSystem statusEffectSystem)
        {
            if (effects == null || effects.Count == 0) return;
            foreach (var effect in effects)
            {
                if (effect != null)
                {
                    effect.Apply(caster, targets, statusEffectSystem);
                }
            }
        }
    }
}


