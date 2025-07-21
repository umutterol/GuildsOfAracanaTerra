using System.Collections.Generic;
using GuildsOfArcanaTerra.Combat.Core;
using UnityEngine;

namespace GuildsOfArcanaTerra.Combat.Skills.Implementations
{
    public class BasicAttackSkill : BaseSkill
    {
        public BasicAttackSkill() { }
        private string skillName;
        
        public BasicAttackSkill(string name = "Basic Attack")
        {
            skillName = name;
        }
        
        public override string SkillName => skillName;
        public override string Description => "A basic attack using your primary weapon.";
        public override int Cooldown => 0;
        public override SkillTargetType TargetType => SkillTargetType.SingleEnemy;
        public override int MaxTargets => 1;
        
        public override void Execute(Combatant caster, List<ICombatant> targets, StatusEffectSystem statusEffectSystem)
        {
            if (targets == null || targets.Count == 0) return;
            
            ICombatant target = targets[0];
            int damage = CalculateDamage(caster, target, 10f, 1.0f);
            
            // Apply damage
            target.TakeDamage(damage);
            
            Debug.Log($"{caster.CharacterName} performs a basic attack on {target.CharacterName} for {damage} damage!");
        }
    }
} 