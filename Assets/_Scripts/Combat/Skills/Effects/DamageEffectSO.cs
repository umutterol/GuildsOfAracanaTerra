using System.Collections.Generic;
using UnityEngine;
using GuildsOfArcanaTerra.Combat; // For Combatant, ICombatant
using GuildsOfArcanaTerra.Combat.Core;

namespace GuildsOfArcanaTerra.Combat.Skills.Effects
{
    [CreateAssetMenu(fileName = "DamageEffect", menuName = "GOAT/Skills/Effects/Damage")] 
    public class DamageEffectSO : SkillEffectSO
    {
        public enum DamageScalingStat { Strength, Agility, Intelligence }

        [Header("Damage Settings")]
        public float baseDamage = 10f;
        public float scaling = 1.0f;
        public DamageScalingStat scalingStat = DamageScalingStat.Strength;
        public float critChance = 0.1f;
        public float critMultiplier = 1.5f;

        public override void Apply(Combatant caster, List<ICombatant> targets, StatusEffectSystem statusEffectSystem)
        {
            if (targets == null || targets.Count == 0) return;

            foreach (var target in targets)
            {
                int attackerStat = scalingStat switch
                {
                    DamageScalingStat.Strength => caster.Strength,
                    DamageScalingStat.Agility => caster.Agility,
                    DamageScalingStat.Intelligence => caster.Intelligence,
                    _ => caster.Strength
                };

                int targetDefense = (target is Combatant ct) ? ct.Defense : 0;

                int baseCalc = scalingStat == DamageScalingStat.Intelligence
                    ? DamageCalculator.CalculateMagicalDamage(attackerStat, targetDefense, scaling)
                    : DamageCalculator.CalculatePhysicalDamage(attackerStat, targetDefense, scaling);

                int raw = baseCalc + Mathf.RoundToInt(baseDamage);
                int withCrit = DamageCalculator.CalculateDamageWithCrit(raw, critChance, critMultiplier);
                target.TakeDamage(withCrit);

                Debug.Log($"[Effects] {caster.Name} -> {target.Name}: {effectName} dealt {withCrit} damage (raw:{raw}, def:{targetDefense}, crit:{(withCrit>raw)})");
            }
        }
    }
}


