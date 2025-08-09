namespace GuildsOfArcanaTerra.Combat.Core
{
    /// <summary>
    /// Centralized combat constants and enums
    /// </summary>
    public static class CombatConstants
    {
        /// <summary>
        /// Default values for combat calculations
        /// </summary>
        public static class Defaults
        {
            public const float CRITICAL_DAMAGE_MULTIPLIER = 1.5f;
            public const float BASE_CRIT_CHANCE = 0.05f; // 5%
            public const float PHYSICAL_DEFENSE_REDUCTION = 0.10f; // 10%
            public const float MAGICAL_DEFENSE_REDUCTION = 0.05f; // 5%
            public const int MINIMUM_DAMAGE = 1;
            public const int MAX_SKILL_TARGETS = 5;
        }
        
        /// <summary>
        /// Skill-related constants
        /// </summary>
        public static class Skills
        {
            public const int BASIC_ATTACK_COOLDOWN = 0;
            public const int DEFAULT_ACTIVE_COOLDOWN = 3;
            public const int MAX_SKILL_COUNT = 10;
        }
        
        /// <summary>
        /// Status effect constants
        /// </summary>
        public static class StatusEffects
        {
            public const int DEFAULT_BURN_DURATION = 2;
            public const int DEFAULT_POISON_DURATION = 3;
            public const int DEFAULT_STUN_DURATION = 1;
            public const int DEFAULT_SHIELD_DURATION = 2;
        }
    }
    
    /// <summary>
    /// Defines the type of skill
    /// </summary>
    public enum SkillType
    {
        Basic,      // 0-CD basic attack
        Active      // Skills with cooldowns
    }
    
    /// <summary>
    /// Defines the target type for skills
    /// </summary>
    public enum SkillTargetType
    {
        SingleEnemy,        // Target one enemy
        SingleAlly,         // Target one ally
        AllEnemies,         // Target all enemies
        AllAllies,          // Target all allies
        Self,               // Target self only
        SingleAny,          // Target any single combatant
        AllAny              // Target all combatants
    }
    
    /// <summary>
    /// Defines the damage type for skills
    /// </summary>
    public enum DamageType
    {
        Physical,   // Reduced by physical defense
        Magical,    // Reduced by magical defense
        True        // Ignores defense
    }
    
    /// <summary>
    /// Defines the primary stat for different character classes
    /// </summary>
    public enum PrimaryStat
    {
        Strength,       // Warriors
        Agility,        // Rogues
        Intelligence    // Mages
    }
    
    /// <summary>
    /// Defines combat phases
    /// </summary>
    public enum CombatPhase
    {
        Preparation,    // Before combat starts
        TurnStart,      // Start of a turn
        Action,         // During action selection
        Execution,      // During skill execution
        TurnEnd,        // End of a turn
        Victory,        // Combat victory
        Defeat          // Combat defeat
    }

    /// <summary>
    /// Row-based positioning for combatants
    /// </summary>
    public enum RowPosition
    {
        Front,
        Back
    }

    /// <summary>
    /// Reach requirements for skills (used with row-based targeting)
    /// </summary>
    public enum SkillReach
    {
        AdjacentOnly,   // Only front-to-front or back-to-back within same side
        MeleeFrontOnly, // Can only hit front row enemies
        MeleeFrontThenBack, // Prefer front; can hit back if front empty
        RangedAny,      // Can hit any enemy row
        AllySelf,       // Self only
        AllyAny         // Any ally row
    }
} 