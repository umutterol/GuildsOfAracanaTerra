using System.Collections.Generic;
using GuildsOfArcanaTerra.Combat.Core;
using GuildsOfArcanaTerra.Combat.Skills.Effects;

namespace GuildsOfArcanaTerra.Combat.Skills.Implementations
{
    /// <summary>
    /// A generic skill that executes one or more ScriptableObject effects
    /// </summary>
    public class GenericSkill : BaseSkill
    {
        private readonly string _name;
        private readonly string _description;
        private readonly int _cooldown;
        private readonly SkillTargetType _targetType;
        private readonly int _maxTargets;
        private readonly List<SkillEffectSO> _effects;

        public GenericSkill(string name, string description, int cooldown, SkillTargetType targetType, int maxTargets, List<SkillEffectSO> effects)
        {
            _name = name;
            _description = description;
            _cooldown = cooldown;
            _targetType = targetType;
            _maxTargets = maxTargets;
            _effects = effects ?? new List<SkillEffectSO>();
        }

        public override string SkillName => _name;
        public override string Description => _description;
        public override int Cooldown => _cooldown;
        public override SkillTargetType TargetType => _targetType;
        public override int MaxTargets => _maxTargets;

        public override void Execute(Combatant caster, List<ICombatant> targets, StatusEffectSystem statusEffectSystem)
        {
            if (_effects.Count == 0) return;

            foreach (var effect in _effects)
            {
                effect.Apply(caster, targets, statusEffectSystem);
            }
            StartCooldown();
        }

        public IReadOnlyList<SkillEffectSO> Effects => _effects;
    }
}


