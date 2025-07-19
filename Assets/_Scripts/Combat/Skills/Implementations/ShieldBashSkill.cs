using System.Collections.Generic;
using GuildsOfArcanaTerra.Combat.Core;
using UnityEngine;

namespace GuildsOfArcanaTerra.Combat.Skills.Implementations
{
    public class ShieldBashSkill : BaseSkill
    {
        public override string SkillName => "Shield Bash";
        public override string Description => "Bash an enemy with your shield, dealing damage and stunning them.";
        public override int Cooldown => 3;
        public override SkillTargetType TargetType => SkillTargetType.SingleEnemy;
        public override int MaxTargets => 1;
        
        public override void Execute(Combatant caster, List<ICombatant> targets, StatusEffectSystem statusEffectSystem)
        {
            if (targets == null || targets.Count == 0) return;
            
            ICombatant target = targets[0];
            int damage = CalculateDamage(caster, target, 15f, 0.8f);
            
            // Apply damage
            target.TakeDamage(damage);
            
            // Apply stun effect
            if (statusEffectSystem != null)
            {
                var stunEffect = statusEffectSystem.CreateStunEffect(target, caster, 1);
                if (stunEffect != null)
                {
                    statusEffectSystem.ApplyEffect(target, stunEffect);
                }
            }
            
            StartCooldown();
            
            Debug.Log($"{caster.CharacterName} uses Shield Bash on {target.CharacterName} for {damage} damage!");
        }
    }
} 