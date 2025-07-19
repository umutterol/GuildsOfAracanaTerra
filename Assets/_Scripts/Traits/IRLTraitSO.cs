using UnityEngine;
using GuildsOfArcanaTerra.Combat;
using GuildsOfArcanaTerra.Combat.Skills;
using GuildsOfArcanaTerra.Combat.Skills.Interfaces;

namespace GuildsOfArcanaTerra.Traits
{
    public abstract class IRLTraitSO : ScriptableObject, IRLTrait
    {
        [Header("Trait Info")]
        [SerializeField] private string traitName;
        [SerializeField] [TextArea] private string description;

        public virtual string TraitName => traitName;
        public virtual string Description => description;

        public abstract void ApplyPreCombat(ICombatant target);
        public abstract void OnTurnStart(ICombatant target);
        public abstract void OnSkillUsed(ICombatant target, IBaseSkill skill);
        public abstract void OnDamaged(ICombatant target, int damage);
    }
} 