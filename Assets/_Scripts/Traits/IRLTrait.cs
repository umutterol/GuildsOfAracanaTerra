using GuildsOfArcanaTerra.Combat;
using GuildsOfArcanaTerra.Combat.Skills;
using GuildsOfArcanaTerra.Combat.Skills.Interfaces;

namespace GuildsOfArcanaTerra.Traits
{
    /// <summary>
    /// Interface for IRL traits that can be assigned to combatants
    /// </summary>
    public interface IRLTrait
    {
        string TraitName { get; }
        string Description { get; }

        /// <summary>
        /// Called before combat starts (for stat modifications, etc.)
        /// </summary>
        void ApplyPreCombat(ICombatant target);

        /// <summary>
        /// Called at the start of the combatant's turn
        /// </summary>
        void OnTurnStart(ICombatant target);

        /// <summary>
        /// Called when the combatant uses a skill
        /// </summary>
        void OnSkillUsed(ICombatant target, IBaseSkill skill);

        /// <summary>
        /// Called when the combatant takes damage
        /// </summary>
        void OnDamaged(ICombatant target, int damage);
    }
} 