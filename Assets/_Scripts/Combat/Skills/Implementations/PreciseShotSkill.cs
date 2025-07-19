using System.Collections.Generic;
using GuildsOfArcanaTerra.Combat.Core;
using UnityEngine;

namespace GuildsOfArcanaTerra.Combat.Skills.Implementations
{
    public class PreciseShotSkill : BaseSkill
    {
        public override string SkillName => "Precise Shot";
        public override string Description => "A carefully aimed shot that deals increased damage.";
        public override int Cooldown => 2;
        public override SkillTargetType TargetType => SkillTargetType.SingleEnemy;
        public override int MaxTargets => 1;
        
        public override void Execute(Combatant caster, List<ICombatant> targets, StatusEffectSystem statusEffectSystem)
        {
            if (targets == null || targets.Count == 0) return;
            
            ICombatant target = targets[0];
            int damage = CalculateDamage(caster, target, 22f, 1.0f);
            
            // Apply damage
            target.TakeDamage(damage);
            
            StartCooldown();
            
            Debug.Log($"{caster.CharacterName} takes a precise shot at {target.CharacterName} for {damage} damage!");
        }
        
        protected override int CalculateDamage(Combatant caster, ICombatant target, float baseDamage, float scaling)
        {
            // Ranger damage uses Agility
            float damage = baseDamage + (caster.Agility * scaling);
            return Mathf.RoundToInt(damage);
        }
    }
} 