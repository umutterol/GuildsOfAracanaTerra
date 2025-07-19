using UnityEngine;
using UnityEditor;
using GuildsOfArcanaTerra.ScriptableObjects.Classes;

namespace GuildsOfArcanaTerra.ScriptableObjects.Classes.Editor
{
    /// <summary>
    /// Editor utility for creating class definitions
    /// </summary>
    public class ClassDefinitionCreator : EditorWindow
    {
        [MenuItem("Guilds of Arcana Terra/Create Class Definitions")]
        public static void CreateAllClassDefinitions()
        {
            CreateWarriorClass();
            CreateMageClass();
            CreateClericClass();
            CreateRogueClass();
            CreateRangerClass();
            
            Debug.Log("All class definitions created successfully!");
            AssetDatabase.Refresh();
        }
        
        [MenuItem("Guilds of Arcana Terra/Create Warrior Class")]
        public static void CreateWarriorClass()
        {
            var warriorClass = CreateInstance<CharacterClassDefinition>();
            
            // Class Identity
            warriorClass.name = "Warrior";
            SetPrivateField(warriorClass, "className", "Warrior");
            SetPrivateField(warriorClass, "classDescription", "STR-based Tank/Bruiser. High defense and melee damage.");
            SetPrivateField(warriorClass, "classColor", new Color(0.8f, 0.2f, 0.2f)); // Red
            
            // Base Stats
            SetPrivateField(warriorClass, "baseStrength", 15);
            SetPrivateField(warriorClass, "baseAgility", 8);
            SetPrivateField(warriorClass, "baseIntelligence", 5);
            SetPrivateField(warriorClass, "baseDefense", 8);
            SetPrivateField(warriorClass, "baseVitality", 12);
            SetPrivateField(warriorClass, "baseHealth", 120);
            
            // Class Specialization
            SetPrivateField(warriorClass, "primaryStat", "STR");
            SetPrivateField(warriorClass, "secondaryStat", "DEF");
            SetPrivateField(warriorClass, "classRole", "Tank");
            
            // Skills
            SetPrivateField(warriorClass, "basicAttack", new SkillDefinition(
                "Shield Bash",
                0,
                "Melee, Debuff: 100% STR damage, -10% DEF to target",
                SkillType.Basic,
                SkillTargetType.SingleEnemy,
                SkillEffectType.Hybrid
            ));
            
            var activeSkills = new SkillDefinition[4];
            activeSkills[0] = new SkillDefinition(
                "Cleave",
                2,
                "AoE, Melee: 130% STR to up to 3 front enemies",
                SkillType.Active,
                SkillTargetType.MultipleEnemies,
                SkillEffectType.Damage
            );
            
            activeSkills[1] = new SkillDefinition(
                "Taunt",
                3,
                "Utility, Self-Buff: Force targeting for 1 turn, gain +15 DEF",
                SkillType.Active,
                SkillTargetType.Self,
                SkillEffectType.Buff
            );
            
            activeSkills[2] = new SkillDefinition(
                "Warcry",
                4,
                "Buff: +20 STR & +200 HP for 2 turns",
                SkillType.Active,
                SkillTargetType.Self,
                SkillEffectType.Buff
            );
            
            activeSkills[3] = new SkillDefinition(
                "Unyielding Guard",
                3,
                "Defensive: Block 100% of next hit",
                SkillType.Active,
                SkillTargetType.Self,
                SkillEffectType.Buff
            );
            
            SetPrivateField(warriorClass, "activeSkills", activeSkills);
            
            SetPrivateField(warriorClass, "passiveSkill", new SkillDefinition(
                "Juggernaut",
                0,
                "Scaling: Gain +5% DEF for every 20% HP missing",
                SkillType.Passive,
                SkillTargetType.Self,
                SkillEffectType.Buff
            ));
            
            // Save asset
            AssetDatabase.CreateAsset(warriorClass, "Assets/_ScriptableObjects/Classes/Warrior.asset");
        }
        
        [MenuItem("Guilds of Arcana Terra/Create Mage Class")]
        public static void CreateMageClass()
        {
            var mageClass = CreateInstance<CharacterClassDefinition>();
            
            // Class Identity
            mageClass.name = "Mage";
            SetPrivateField(mageClass, "className", "Mage");
            SetPrivateField(mageClass, "classDescription", "INT-based AoE Caster. High magic damage and crowd control.");
            SetPrivateField(mageClass, "classColor", new Color(0.2f, 0.2f, 0.8f)); // Blue
            
            // Base Stats
            SetPrivateField(mageClass, "baseStrength", 5);
            SetPrivateField(mageClass, "baseAgility", 8);
            SetPrivateField(mageClass, "baseIntelligence", 15);
            SetPrivateField(mageClass, "baseDefense", 3);
            SetPrivateField(mageClass, "baseVitality", 8);
            SetPrivateField(mageClass, "baseHealth", 80);
            
            // Class Specialization
            SetPrivateField(mageClass, "primaryStat", "INT");
            SetPrivateField(mageClass, "secondaryStat", "AGI");
            SetPrivateField(mageClass, "classRole", "Caster");
            
            // Skills
            SetPrivateField(mageClass, "basicAttack", new SkillDefinition(
                "Arcane Bolt",
                0,
                "Ranged, Magic: 100% INT damage",
                SkillType.Basic,
                SkillTargetType.SingleEnemy,
                SkillEffectType.Damage
            ));
            
            var activeSkills = new SkillDefinition[4];
            activeSkills[0] = new SkillDefinition(
                "Fireball",
                2,
                "AoE, Burn: 150% INT + Burn (25% INT/turn, 2 turns)",
                SkillType.Active,
                SkillTargetType.MultipleEnemies,
                SkillEffectType.Hybrid
            );
            
            activeSkills[1] = new SkillDefinition(
                "Frost Nova",
                3,
                "AoE, Control: 100% INT AoE + Slow (AGI -10%, 2 turns)",
                SkillType.Active,
                SkillTargetType.MultipleEnemies,
                SkillEffectType.Hybrid
            );
            
            activeSkills[2] = new SkillDefinition(
                "Mana Surge",
                4,
                "Cooldown Utility: Reduce all own cooldowns by 1",
                SkillType.Active,
                SkillTargetType.Self,
                SkillEffectType.Utility
            );
            
            activeSkills[3] = new SkillDefinition(
                "Arcane Singularity",
                5,
                "AoE, Magic: 300% INT AoE",
                SkillType.Active,
                SkillTargetType.AllEnemies,
                SkillEffectType.Damage
            );
            
            SetPrivateField(mageClass, "activeSkills", activeSkills);
            
            SetPrivateField(mageClass, "passiveSkill", new SkillDefinition(
                "Arcane Mastery",
                0,
                "Support, Aura: +5% INT to party; every 5 turns, reduce all ally cooldowns by 1",
                SkillType.Passive,
                SkillTargetType.AllAllies,
                SkillEffectType.Buff
            ));
            
            // Save asset
            AssetDatabase.CreateAsset(mageClass, "Assets/_ScriptableObjects/Classes/Mage.asset");
        }
        
        [MenuItem("Guilds of Arcana Terra/Create Cleric Class")]
        public static void CreateClericClass()
        {
            var clericClass = CreateInstance<CharacterClassDefinition>();
            
            // Class Identity
            clericClass.name = "Cleric";
            SetPrivateField(clericClass, "className", "Cleric");
            SetPrivateField(clericClass, "classDescription", "INT-based Healer/Damage Hybrid. Balanced healing and damage.");
            SetPrivateField(clericClass, "classColor", new Color(0.8f, 0.8f, 0.2f)); // Yellow
            
            // Base Stats
            SetPrivateField(clericClass, "baseStrength", 8);
            SetPrivateField(clericClass, "baseAgility", 6);
            SetPrivateField(clericClass, "baseIntelligence", 12);
            SetPrivateField(clericClass, "baseDefense", 6);
            SetPrivateField(clericClass, "baseVitality", 10);
            SetPrivateField(clericClass, "baseHealth", 100);
            
            // Class Specialization
            SetPrivateField(clericClass, "primaryStat", "INT");
            SetPrivateField(clericClass, "secondaryStat", "VIT");
            SetPrivateField(clericClass, "classRole", "Support");
            
            // Skills
            SetPrivateField(clericClass, "basicAttack", new SkillDefinition(
                "Smite",
                0,
                "Hybrid, Sustain: 100% INT damage, heal lowest HP ally for 25% of damage dealt",
                SkillType.Basic,
                SkillTargetType.SingleEnemy,
                SkillEffectType.Hybrid
            ));
            
            var activeSkills = new SkillDefinition[4];
            activeSkills[0] = new SkillDefinition(
                "Radiant Heal",
                2,
                "Single-Target Heal: Heal one ally for 150% INT",
                SkillType.Active,
                SkillTargetType.SingleAlly,
                SkillEffectType.Heal
            );
            
            activeSkills[1] = new SkillDefinition(
                "Shield of Faith",
                3,
                "Shielding: Apply shield = 120% INT for 2 turns",
                SkillType.Active,
                SkillTargetType.SingleAlly,
                SkillEffectType.Buff
            );
            
            activeSkills[2] = new SkillDefinition(
                "Purify",
                3,
                "Utility: Remove all debuffs from target ally",
                SkillType.Active,
                SkillTargetType.SingleAlly,
                SkillEffectType.Utility
            );
            
            activeSkills[3] = new SkillDefinition(
                "Sacred Chain",
                4,
                "Hybrid AoE: 100% INT to 3 enemies; heal 3 allies for 100% INT",
                SkillType.Active,
                SkillTargetType.Hybrid,
                SkillEffectType.Hybrid
            );
            
            SetPrivateField(clericClass, "activeSkills", activeSkills);
            
            SetPrivateField(clericClass, "passiveSkill", new SkillDefinition(
                "Atonement",
                0,
                "Hybrid Sustain: Whenever you deal damage, heal lowest HP ally for 10% INT",
                SkillType.Passive,
                SkillTargetType.SingleAlly,
                SkillEffectType.Heal
            ));
            
            // Save asset
            AssetDatabase.CreateAsset(clericClass, "Assets/_ScriptableObjects/Classes/Cleric.asset");
        }
        
        [MenuItem("Guilds of Arcana Terra/Create Rogue Class")]
        public static void CreateRogueClass()
        {
            var rogueClass = CreateInstance<CharacterClassDefinition>();
            
            // Class Identity
            rogueClass.name = "Rogue";
            SetPrivateField(rogueClass, "className", "Rogue");
            SetPrivateField(rogueClass, "classDescription", "AGI-based Assassin/Burst DPS. High single-target damage and mobility.");
            SetPrivateField(rogueClass, "classColor", new Color(0.2f, 0.8f, 0.2f)); // Green
            
            // Base Stats
            SetPrivateField(rogueClass, "baseStrength", 6);
            SetPrivateField(rogueClass, "baseAgility", 15);
            SetPrivateField(rogueClass, "baseIntelligence", 8);
            SetPrivateField(rogueClass, "baseDefense", 4);
            SetPrivateField(rogueClass, "baseVitality", 8);
            SetPrivateField(rogueClass, "baseHealth", 80);
            
            // Class Specialization
            SetPrivateField(rogueClass, "primaryStat", "AGI");
            SetPrivateField(rogueClass, "secondaryStat", "STR");
            SetPrivateField(rogueClass, "classRole", "Assassin");
            
            // Skills
            SetPrivateField(rogueClass, "basicAttack", new SkillDefinition(
                "Quick Stab",
                0,
                "Melee, Buff: 100% AGI, gain +5 AGI for 1 turn",
                SkillType.Basic,
                SkillTargetType.SingleEnemy,
                SkillEffectType.Hybrid
            ));
            
            var activeSkills = new SkillDefinition[4];
            activeSkills[0] = new SkillDefinition(
                "Shadowstep",
                2,
                "Positioning, Burst: Move anywhere; deal 120% AGI to backline",
                SkillType.Active,
                SkillTargetType.SingleEnemy,
                SkillEffectType.Damage
            );
            
            activeSkills[1] = new SkillDefinition(
                "Poisoned Blade",
                3,
                "DOT, Single-Target: 150% AGI + Poison (20% AGI/turn for 3 turns)",
                SkillType.Active,
                SkillTargetType.SingleEnemy,
                SkillEffectType.Hybrid
            );
            
            activeSkills[2] = new SkillDefinition(
                "Evasion",
                3,
                "Survivability: Gain +30% Dodge for 2 turns",
                SkillType.Active,
                SkillTargetType.Self,
                SkillEffectType.Buff
            );
            
            activeSkills[3] = new SkillDefinition(
                "Fan of Knives",
                4,
                "AoE, Bleed: 110% AGI to all enemies + Bleed (15% AGI/turn, 2 turns)",
                SkillType.Active,
                SkillTargetType.AllEnemies,
                SkillEffectType.Hybrid
            );
            
            SetPrivateField(rogueClass, "activeSkills", activeSkills);
            
            SetPrivateField(rogueClass, "passiveSkill", new SkillDefinition(
                "Assassin's Flow",
                0,
                "Crit Scaling: +10% crit chance if not hit last round",
                SkillType.Passive,
                SkillTargetType.Self,
                SkillEffectType.Buff
            ));
            
            // Save asset
            AssetDatabase.CreateAsset(rogueClass, "Assets/_ScriptableObjects/Classes/Rogue.asset");
        }
        
        [MenuItem("Guilds of Arcana Terra/Create Ranger Class")]
        public static void CreateRangerClass()
        {
            var rangerClass = CreateInstance<CharacterClassDefinition>();
            
            // Class Identity
            rangerClass.name = "Ranger";
            SetPrivateField(rangerClass, "className", "Ranger");
            SetPrivateField(rangerClass, "classDescription", "AGI-based Ranged DPS / Control. High accuracy and utility.");
            SetPrivateField(rangerClass, "classColor", new Color(0.6f, 0.4f, 0.2f)); // Brown
            
            // Base Stats
            SetPrivateField(rangerClass, "baseStrength", 6);
            SetPrivateField(rangerClass, "baseAgility", 12);
            SetPrivateField(rangerClass, "baseIntelligence", 8);
            SetPrivateField(rangerClass, "baseDefense", 5);
            SetPrivateField(rangerClass, "baseVitality", 9);
            SetPrivateField(rangerClass, "baseHealth", 90);
            
            // Class Specialization
            SetPrivateField(rangerClass, "primaryStat", "AGI");
            SetPrivateField(rangerClass, "secondaryStat", "INT");
            SetPrivateField(rangerClass, "classRole", "Ranged");
            
            // Skills
            SetPrivateField(rangerClass, "basicAttack", new SkillDefinition(
                "Piercing Shot",
                0,
                "Ranged, Armor Pen: 100% AGI, ignore 10 DEF",
                SkillType.Basic,
                SkillTargetType.SingleEnemy,
                SkillEffectType.Damage
            ));
            
            var activeSkills = new SkillDefinition[4];
            activeSkills[0] = new SkillDefinition(
                "Multi-Shot",
                2,
                "Random AoE: Hit 3 random enemies for 120% AGI",
                SkillType.Active,
                SkillTargetType.MultipleEnemies,
                SkillEffectType.Damage
            );
            
            activeSkills[1] = new SkillDefinition(
                "Camouflage",
                3,
                "Stealth, Buff: Untargetable 1 turn, gain +10 AGI",
                SkillType.Active,
                SkillTargetType.Self,
                SkillEffectType.Buff
            );
            
            activeSkills[2] = new SkillDefinition(
                "Trap Arrow",
                3,
                "Control: 90% AGI damage",
                SkillType.Active,
                SkillTargetType.SingleEnemy,
                SkillEffectType.Damage
            );
            
            activeSkills[3] = new SkillDefinition(
                "Hawk's Mark",
                4,
                "Debuff, Focus Fire: Mark target; +20% damage taken for 2 turns",
                SkillType.Active,
                SkillTargetType.SingleEnemy,
                SkillEffectType.Debuff
            );
            
            SetPrivateField(rangerClass, "activeSkills", activeSkills);
            
            SetPrivateField(rangerClass, "passiveSkill", new SkillDefinition(
                "Predator's Focus",
                0,
                "Buff Extension: Crits extend buff durations on self by 1 turn",
                SkillType.Passive,
                SkillTargetType.Self,
                SkillEffectType.Buff
            ));
            
            // Save asset
            AssetDatabase.CreateAsset(rangerClass, "Assets/_ScriptableObjects/Classes/Ranger.asset");
        }
        
        /// <summary>
        /// Helper method to set private fields using reflection
        /// </summary>
        private static void SetPrivateField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName, 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(obj, value);
            }
        }
    }
} 