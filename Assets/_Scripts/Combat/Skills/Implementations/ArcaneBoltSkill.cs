using System.Collections.Generic;
using GuildsOfArcanaTerra.Combat.Core;
using UnityEngine;

namespace GuildsOfArcanaTerra.Combat.Skills.Implementations
{
    public class ArcaneBoltSkill : BaseSkill
    {
        public override string SkillName => "Arcane Bolt";
        public override string Description => "Launch a bolt of pure arcane energy at an enemy.";
        public override int Cooldown => 1;
        public override SkillTargetType TargetType => SkillTargetType.SingleEnemy;
        public override int MaxTargets => 1;
        
        public override void Execute(Combatant caster, List<ICombatant> targets, StatusEffectSystem statusEffectSystem)
        {
            if (targets == null || targets.Count == 0) return;
            
            ICombatant target = targets[0];
            int damage = CalculateDamage(caster, target, 18f, 1.0f);
            
            // Apply damage
            target.TakeDamage(damage);
            
            StartCooldown();
            
            Debug.Log($"{caster.CharacterName} casts Arcane Bolt on {target.CharacterName} for {damage} damage!");
        }
        
        protected override int CalculateDamage(Combatant caster, ICombatant target, float baseDamage, float scaling)
        {
            // Magic damage uses Intelligence
            float damage = baseDamage + (caster.Intelligence * scaling);
            return Mathf.RoundToInt(damage);
        }
    }
} 