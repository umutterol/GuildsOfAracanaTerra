using System.Collections.Generic;
using UnityEngine;
using GuildsOfArcanaTerra.Combat;
using GuildsOfArcanaTerra.Combat.Effects;

namespace GuildsOfArcanaTerra.Combat.Skills.Effects
{
    [CreateAssetMenu(fileName = "ApplyStatusEffect", menuName = "GOAT/Skills/Effects/Apply Status Effect")]
    public class ApplyStatusEffectSO : SkillEffectSO
    {
        public enum StatusKind
        {
            Burn,
            Bleed,
            Poison,
            Stun,
            Slow,
            ShieldFlat,
            ShieldIntScaling
        }

        [Header("Status Settings")]
        public StatusKind statusKind = StatusKind.Burn;
        [Tooltip("Duration in turns")] public int duration = 2;
        [Tooltip("Flat shield value (only for ShieldFlat)")] public int shieldValue = 25;

        public override void Apply(Combatant caster, List<ICombatant> targets, StatusEffectSystem statusEffectSystem)
        {
            if (targets == null || targets.Count == 0 || statusEffectSystem == null) return;

            foreach (var target in targets)
            {
                IStatusEffect effect = null;
                switch (statusKind)
                {
                    case StatusKind.Burn:
                        effect = statusEffectSystem.CreateBurnEffect(target, caster, duration);
                        break;
                    case StatusKind.Bleed:
                        effect = statusEffectSystem.CreateBleedEffect(target, caster, duration);
                        break;
                    case StatusKind.Poison:
                        effect = statusEffectSystem.CreatePoisonEffect(target, caster, duration);
                        break;
                    case StatusKind.Stun:
                        effect = statusEffectSystem.CreateStunEffect(target, caster, duration);
                        break;
                    case StatusKind.Slow:
                        effect = statusEffectSystem.CreateSlowEffect(target, caster, duration);
                        break;
                    case StatusKind.ShieldFlat:
                        effect = statusEffectSystem.CreateShieldEffect(target, caster, shieldValue, duration);
                        break;
                    case StatusKind.ShieldIntScaling:
                        effect = statusEffectSystem.CreateShieldEffect(target, caster, 0, duration); // 0 → INT-scaling
                        break;
                }

                if (effect != null)
                {
                    statusEffectSystem.ApplyEffect(target, effect);
                    Debug.Log($"[Effects] {caster.Name} -> {target.Name}: Applied {effect.Name} ({duration}t)");
                }
            }
        }
    }
}


