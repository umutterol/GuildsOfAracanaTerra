using System.Collections.Generic;
using GuildsOfArcanaTerra.Combat.Core;
using UnityEngine;

namespace GuildsOfArcanaTerra.Combat.Skills.Implementations
{
    public class ShadowstepSkill : BaseSkill
    {
        public override string SkillName => "Shadowstep";
        public override string Description => "Teleport behind an enemy and strike from the shadows.";
        public override int Cooldown => 4;
        public override SkillTargetType TargetType => SkillTargetType.SingleEnemy;
        public override int MaxTargets => 1;
        
        public override void Execute(Combatant caster, List<ICombatant> targets, StatusEffectSystem statusEffectSystem)
        {
            if (targets == null || targets.Count == 0) return;
            
            ICombatant target = targets[0];
            int damage = CalculateDamage(caster, target, 25f, 1.1f);
            
            // Apply damage
            target.TakeDamage(damage);
            
            // Apply stealth effect to self (placeholder - Stealth effect not implemented yet)
            if (statusEffectSystem != null)
            {
                // TODO: Implement StealthEffect class
                Debug.Log($"{caster.CharacterName} gains stealth!");
            }
            
            StartCooldown();
            
            Debug.Log($"{caster.CharacterName} shadowsteps behind {target.CharacterName} for {damage} damage!");
        }
        
        protected override int CalculateDamage(Combatant caster, ICombatant target, float baseDamage, float scaling)
        {
            // Rogue damage uses Agility
            float damage = baseDamage + (caster.Agility * scaling);
            return Mathf.RoundToInt(damage);
        }
    }
} 