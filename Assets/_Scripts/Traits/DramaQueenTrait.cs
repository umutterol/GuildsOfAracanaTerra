using UnityEngine;
using GuildsOfArcanaTerra.Combat;
using GuildsOfArcanaTerra.Combat.Skills;
using GuildsOfArcanaTerra.Combat.Skills.Interfaces;
using System.Collections.Generic;

namespace GuildsOfArcanaTerra.Traits
{
    [CreateAssetMenu(fileName = "DramaQueenTrait", menuName = "IRL Trait/Drama Queen")]
    public class DramaQueenTrait : IRLTraitSO
    {
        private int hitsThisRound = 0;
        private bool overdriveNextTurn = false;

        public override void ApplyPreCombat(ICombatant target) { }

        public override void OnTurnStart(ICombatant target)
        {
            if (overdriveNextTurn && target is Combatant c)
            {
                c.SetOverdrive(true);
                overdriveNextTurn = false;
            }
            hitsThisRound = 0;
        }

        public override void OnSkillUsed(ICombatant target, IBaseSkill skill) { }

        public override void OnDamaged(ICombatant target, int damage)
        {
            hitsThisRound++;
            if (hitsThisRound >= 3)
            {
                overdriveNextTurn = true;
            }
        }

        // Party bonus logic would be handled in a party manager or similar system
        public static void ApplyPartyBonus(List<ICombatant> party)
        {
            bool anyBelowHalf = false;
            foreach (var member in party)
            {
                if (member is Combatant c && c.HealthPercentage < 0.5f)
                {
                    anyBelowHalf = true;
                    break;
                }
            }
            if (anyBelowHalf)
            {
                foreach (var member in party)
                {
                    if (member is Combatant c)
                    {
                        c.SetCritModifier(1.05f); // +5% crit
                    }
                }
            }
            else
            {
                foreach (var member in party)
                {
                    if (member is Combatant c)
                    {
                        c.SetCritModifier(1.0f);
                    }
                }
            }
        }
    }
} 