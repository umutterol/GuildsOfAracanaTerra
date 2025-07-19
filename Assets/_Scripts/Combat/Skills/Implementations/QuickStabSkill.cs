using System.Collections.Generic;
using GuildsOfArcanaTerra.Combat.Core;
using UnityEngine;

namespace GuildsOfArcanaTerra.Combat.Skills.Implementations
{
    public class QuickStabSkill : BaseSkill
    {
        public override string SkillName => "Quick Stab";
        public override string Description => "A quick and precise strike with your dagger.";
        public override int Cooldown => 1;
        public override SkillTargetType TargetType => SkillTargetType.SingleEnemy;
        public override int MaxTargets => 1;
        
        public override void Execute(Combatant caster, List<ICombatant> targets, StatusEffectSystem statusEffectSystem)
        {
            if (targets == null || targets.Count == 0) return;
            
            ICombatant target = targets[0];
            int damage = CalculateDamage(caster, target, 12f, 0.9f);
            
            // Apply damage
            target.TakeDamage(damage);
            
            StartCooldown();
            
            Debug.Log($"{caster.CharacterName} quick stabs {target.CharacterName} for {damage} damage!");
        }
        
        protected override int CalculateDamage(Combatant caster, ICombatant target, float baseDamage, float scaling)
        {
            // Rogue damage uses Agility
            float damage = baseDamage + (caster.Agility * scaling);
            return Mathf.RoundToInt(damage);
        }
    }
} 