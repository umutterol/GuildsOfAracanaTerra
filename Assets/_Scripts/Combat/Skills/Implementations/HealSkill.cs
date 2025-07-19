using System.Collections.Generic;
using GuildsOfArcanaTerra.Combat.Core;
using UnityEngine;

namespace GuildsOfArcanaTerra.Combat.Skills.Implementations
{
    public class HealSkill : BaseSkill
    {
        public override string SkillName => "Heal";
        public override string Description => "Restore health to an ally.";
        public override int Cooldown => 3;
        public override SkillTargetType TargetType => SkillTargetType.SingleAlly;
        public override int MaxTargets => 1;
        
        public override void Execute(Combatant caster, List<ICombatant> targets, StatusEffectSystem statusEffectSystem)
        {
            if (targets == null || targets.Count == 0) return;
            
            ICombatant target = targets[0];
            int healing = CalculateHealing(caster, target, 30f, 1.5f);
            
            // Apply healing
            target.Heal(healing);
            
            StartCooldown();
            
            Debug.Log($"{caster.CharacterName} heals {target.CharacterName} for {healing} health!");
        }
    }
} 