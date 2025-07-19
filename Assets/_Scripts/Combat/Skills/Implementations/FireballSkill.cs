using System.Collections.Generic;
using GuildsOfArcanaTerra.Combat.Core;
using UnityEngine;

namespace GuildsOfArcanaTerra.Combat.Skills.Implementations
{
    public class FireballSkill : BaseSkill
    {
        public override string SkillName => "Fireball";
        public override string Description => "Launch a ball of fire at an enemy, dealing magic damage.";
        public override int Cooldown => 2;
        public override SkillTargetType TargetType => SkillTargetType.SingleEnemy;
        public override int MaxTargets => 1;
        
        public override void Execute(Combatant caster, List<ICombatant> targets, StatusEffectSystem statusEffectSystem)
        {
            if (targets == null || targets.Count == 0) return;
            
            ICombatant target = targets[0];
            int damage = CalculateDamage(caster, target, 25f, 1.2f);
            
            // Apply damage
            target.TakeDamage(damage);
            
            // Apply burn effect
            if (statusEffectSystem != null)
            {
                var burnEffect = statusEffectSystem.CreateBurnEffect(target, caster, 2);
                if (burnEffect != null)
                {
                    statusEffectSystem.ApplyEffect(target, burnEffect);
                }
            }
            
            StartCooldown();
            
            Debug.Log($"{caster.CharacterName} casts Fireball on {target.CharacterName} for {damage} damage!");
        }
        
        protected override int CalculateDamage(Combatant caster, ICombatant target, float baseDamage, float scaling)
        {
            // Magic damage uses Intelligence instead of Strength
            float damage = baseDamage + (caster.Intelligence * scaling);
            return Mathf.RoundToInt(damage);
        }
    }
} 