using UnityEngine;
using GuildsOfArcanaTerra.Combat;
using GuildsOfArcanaTerra.ScriptableObjects.Classes;
using GuildsOfArcanaTerra.Combat.Skills;
using GuildsOfArcanaTerra.Combat.Skills.Interfaces;
using GuildsOfArcanaTerra.Combat.Core;

namespace GuildsOfArcanaTerra.Traits
{
    [CreateAssetMenu(fileName = "GlassCannonMainTrait", menuName = "IRL Trait/Glass Cannon Main")]
    public class GlassCannonMainTrait : IRLTraitSO
    {
        public override void ApplyPreCombat(ICombatant target)
        {
            if (target is Combatant c)
            {
                c.SetDefenseModifier(0.75f); // -25% DEF
            }
        }

        public override void OnTurnStart(ICombatant target) { }

        public override void OnSkillUsed(ICombatant target, IBaseSkill skill)
        {
            if (target is Combatant c && skill != null)
            {
                float bonus = 1.25f;
                if (skill.TargetType == GuildsOfArcanaTerra.Combat.Core.SkillTargetType.AllEnemies)
                    bonus += 0.10f;
                c.SetDamageModifier(bonus);
            }
        }

        public override void OnDamaged(ICombatant target, int damage) { }
    }
} 