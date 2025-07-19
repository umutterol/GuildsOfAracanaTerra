using System.Collections.Generic;
using GuildsOfArcanaTerra.Combat.Core;
using UnityEngine;

namespace GuildsOfArcanaTerra.Combat.Skills.Implementations
{
    public class CleaveSkill : BaseSkill
    {
        public override string SkillName => "Cleave";
        public override string Description => "Swing your weapon in a wide arc, hitting all enemies.";
        public override int Cooldown => 4;
        public override SkillTargetType TargetType => SkillTargetType.AllEnemies;
        public override int MaxTargets => 5;
        
        public override void Execute(Combatant caster, List<ICombatant> targets, StatusEffectSystem statusEffectSystem)
        {
            if (targets == null || targets.Count == 0) return;
            
            foreach (ICombatant target in targets)
            {
                int damage = CalculateDamage(caster, target, 20f, 0.6f);
                target.TakeDamage(damage);
                
                Debug.Log($"{caster.CharacterName} cleaves {target.CharacterName} for {damage} damage!");
            }
            
            StartCooldown();
        }
    }
} 