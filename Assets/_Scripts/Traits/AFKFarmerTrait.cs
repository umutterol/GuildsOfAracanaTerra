using UnityEngine;
using GuildsOfArcanaTerra.Combat;
using GuildsOfArcanaTerra.Combat.Skills;
using GuildsOfArcanaTerra.Combat.Skills.Interfaces;

namespace GuildsOfArcanaTerra.Traits
{
    [CreateAssetMenu(fileName = "AFKFarmerTrait", menuName = "IRL Trait/AFK Farmer")]
    public class AFKFarmerTrait : IRLTraitSO
    {
        private bool skippedLastTurn = false;

        public override void ApplyPreCombat(ICombatant target)
        {
            if (target is Combatant c)
            {
                c.SetAlwaysActsLast(true);
            }
        }

        public override void OnTurnStart(ICombatant target)
        {
            if (skippedLastTurn && target is Combatant c)
            {
                c.SetDamageModifier(1.3f);
                skippedLastTurn = false;
            }
            else if (target is Combatant c2)
            {
                c2.SetDamageModifier(1.0f);
            }
        }

        public override void OnSkillUsed(ICombatant target, IBaseSkill skill)
        {
            if (skill == null)
            {
                skippedLastTurn = true;
            }
        }

        public override void OnDamaged(ICombatant target, int damage) { }
    }
} 