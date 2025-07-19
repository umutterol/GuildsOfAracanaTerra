using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GuildsOfArcanaTerra.Combat.Skills;
using GuildsOfArcanaTerra.Combat.Skills.Interfaces;

namespace GuildsOfArcanaTerra.Combat
{
    public enum AIType
    {
        Aggressor,
        Disruptor,
        Protector,
        Healer,
        WildCard
    }

    /// <summary>
    /// Enemy combatant with AI-driven turn logic
    /// </summary>
    public class EnemyCombatant : Combatant
    {
        [Header("AI Settings")]
        public AIType aiType = AIType.Aggressor;
        [SerializeField] public List<Combatant> enemyParty; // Player party
        [SerializeField] public List<Combatant> allyParty;  // Enemy party (other enemies)

        public override void OnTurnStart()
        {
            base.OnTurnStart();
            StartCoroutine(EnemyTurnRoutine());
        }

        private System.Collections.IEnumerator EnemyTurnRoutine()
        {
            yield return new WaitForSeconds(0.5f); // Small delay for readability

            // 1. Select a usable skill
            var usableSkills = GetAvailableSkills();
            IBaseSkill chosenSkill = null;
            Combatant chosenTarget = null;

            switch (aiType)
            {
                case AIType.Aggressor:
                    chosenSkill = SelectHighestDamageSkill(usableSkills);
                    chosenTarget = SelectLowestHP(enemyParty);
                    break;
                case AIType.Disruptor:
                    chosenSkill = SelectDisruptSkill(usableSkills);
                    chosenTarget = SelectRandom(enemyParty);
                    break;
                case AIType.Protector:
                    chosenSkill = SelectProtectSkill(usableSkills);
                    chosenTarget = SelectLowestHP(allyParty);
                    break;
                case AIType.Healer:
                    chosenSkill = SelectHealSkill(usableSkills);
                    chosenTarget = SelectLowestHP(allyParty);
                    break;
                case AIType.WildCard:
                    chosenSkill = SelectRandom(usableSkills);
                    chosenTarget = SelectRandom(enemyParty.Concat(allyParty).ToList());
                    break;
            }

            // Fallbacks
            if (chosenSkill == null)
                chosenSkill = usableSkills.FirstOrDefault();
            if (chosenTarget == null)
                chosenTarget = SelectRandom(enemyParty);

            // 2. Execute skill
            if (chosenSkill != null && chosenTarget != null)
            {
                ExecuteSkill(chosenSkill, chosenTarget);
            }
            else
            {
                // If no valid skill/target, just end turn
                EndTurn();
                yield break;
            }

            yield return new WaitForSeconds(0.5f);
            // 3. End turn
            EndTurn();
        }

        // --- AI Skill/Target Selection Helpers ---
        private IBaseSkill SelectHighestDamageSkill(List<IBaseSkill> skills)
        {
            // Placeholder: just pick the first active skill
            return skills.OrderByDescending(s => s.Cooldown).FirstOrDefault();
        }
        private IBaseSkill SelectDisruptSkill(List<IBaseSkill> skills)
        {
            // Placeholder: pick a skill with debuff/effect type if available
            return skills.FirstOrDefault(s => s.SkillName.ToLower().Contains("poison") || s.SkillName.ToLower().Contains("bleed") || s.SkillName.ToLower().Contains("stun") || s.SkillName.ToLower().Contains("slow"))
                ?? skills.FirstOrDefault();
        }
        private IBaseSkill SelectProtectSkill(List<IBaseSkill> skills)
        {
            // Placeholder: pick a skill with "shield" or "guard"
            return skills.FirstOrDefault(s => s.SkillName.ToLower().Contains("shield") || s.SkillName.ToLower().Contains("guard") || s.SkillName.ToLower().Contains("taunt"))
                ?? skills.FirstOrDefault();
        }
        private IBaseSkill SelectHealSkill(List<IBaseSkill> skills)
        {
            // Placeholder: pick a skill with "heal"
            return skills.FirstOrDefault(s => s.SkillName.ToLower().Contains("heal") || s.SkillName.ToLower().Contains("purify"))
                ?? skills.FirstOrDefault();
        }
        private IBaseSkill SelectRandom(List<IBaseSkill> skills)
        {
            if (skills == null || skills.Count == 0) return null;
            return skills[Random.Range(0, skills.Count)];
        }
        private Combatant SelectLowestHP(List<Combatant> party)
        {
            if (party == null || party.Count == 0) return null;
            return party.Where(c => c.IsAlive).OrderBy(c => c.CurrentHealth).FirstOrDefault();
        }
        private Combatant SelectRandom(List<Combatant> party)
        {
            var alive = party?.Where(c => c.IsAlive).ToList();
            if (alive == null || alive.Count == 0) return null;
            return alive[Random.Range(0, alive.Count)];
        }

        // --- Skill Execution ---
        private void ExecuteSkill(IBaseSkill skill, Combatant target)
        {
            // Use the skill and apply effects to the target
            UseSkill(skill.SkillName);
            // You may want to call a method on the target or trigger animation here
        }

        private void EndTurn()
        {
            OnTurnEnd();
        }
    }
} 