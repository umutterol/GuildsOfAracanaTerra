using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GuildsOfArcanaTerra.ScriptableObjects.Classes;
using GuildsOfArcanaTerra.Combat.Core;
using GuildsOfArcanaTerra.Combat.Skills;
using GuildsOfArcanaTerra.Combat.Skills.Interfaces;
using GuildsOfArcanaTerra.Combat.Skills.Implementations;

namespace GuildsOfArcanaTerra.Combat
{
    /// <summary>
    /// Test script for the Skill Effects system
    /// </summary>
    public class SkillEffectsTest : MonoBehaviour
    {
        [Header("Test Combatants")]
        [SerializeField] private Combatant warrior;
        [SerializeField] private Combatant mage;
        [SerializeField] private Combatant enemy;
        
        [Header("Systems")]
        [SerializeField] private TurnOrderSystem turnOrderSystem;
        [SerializeField] private StatusEffectSystem statusEffectSystem;
        
        [Header("UI Elements")]
        [SerializeField] private Text outputText;
        [SerializeField] private Button testShieldBashButton;
        [SerializeField] private Button testFireballButton;
        [SerializeField] private Button testHealButton;
        [SerializeField] private Button resetHealthButton;
        
        private List<ICombatant> allCombatants = new List<ICombatant>();
        
        void Start()
        {
            // Set up button listeners
            if (testShieldBashButton != null)
                testShieldBashButton.onClick.AddListener(TestShieldBash);
            
            if (testFireballButton != null)
                testFireballButton.onClick.AddListener(TestFireball);
            
            if (testHealButton != null)
                testHealButton.onClick.AddListener(TestHeal);
            
            if (resetHealthButton != null)
                resetHealthButton.onClick.AddListener(ResetHealth);
            
            // Collect combatants
            CollectCombatants();
            
            // Auto-run basic test
            TestBasicSetup();
        }
        
        void CollectCombatants()
        {
            allCombatants.Clear();
            if (warrior != null) allCombatants.Add(warrior);
            if (mage != null) allCombatants.Add(mage);
            if (enemy != null) allCombatants.Add(enemy);
        }
        
        void TestBasicSetup()
        {
            string result = "=== SKILL EFFECTS TEST SETUP ===\n";
            
            if (warrior != null)
                result += $"✅ Warrior: {warrior.Name} (STR:{warrior.Strength}, HP:{warrior.CurrentHealth}/{warrior.MaxHealth})\n";
            else
                result += "❌ Warrior not assigned\n";
                
            if (mage != null)
                result += $"✅ Mage: {mage.Name} (INT:{mage.INT}, HP:{mage.CurrentHealth}/{mage.MaxHealth})\n";
            else
                result += "❌ Mage not assigned\n";
                
            if (enemy != null)
                result += $"✅ Enemy: {enemy.Name} (DEF:{enemy.Defense}, HP:{enemy.CurrentHealth}/{enemy.MaxHealth})\n";
            else
                result += "❌ Enemy not assigned\n";
            
            if (statusEffectSystem != null)
                result += "✅ StatusEffectSystem found\n";
            else
            {
                result += "❌ StatusEffectSystem not found - creating one\n";
                // Try to find or create StatusEffectSystem
                statusEffectSystem = FindObjectOfType<StatusEffectSystem>();
                if (statusEffectSystem == null)
                {
                    GameObject systemGO = new GameObject("StatusEffectSystem");
                    statusEffectSystem = systemGO.AddComponent<StatusEffectSystem>();
                    result += "✅ StatusEffectSystem created\n";
                }
                else
                {
                    result += "✅ StatusEffectSystem found in scene\n";
                }
            }
            
            DisplayResult(result);
        }
        
        void TestShieldBash()
        {
            if (warrior == null || enemy == null) return;
            
            string result = "=== SHIELD BASH TEST ===\n";
            
            int enemyHealthBefore = enemy.CurrentHealth;
            
            // Create a Shield Bash skill
            var shieldBash = new ShieldBashSkill();
            
            // Execute the skill
            var targets = new List<ICombatant> { enemy };
            SkillEffects.ExecuteSkill(shieldBash, warrior, targets, statusEffectSystem);
            
            int enemyHealthAfter = enemy.CurrentHealth;
            int damageDealt = enemyHealthBefore - enemyHealthAfter;
            
            result += $"Shield Bash dealt {damageDealt} damage\n";
            result += $"Enemy HP: {enemyHealthBefore} → {enemyHealthAfter}\n";
            
            DisplayResult(result);
        }
        
        void TestFireball()
        {
            if (mage == null || enemy == null) return;
            
            string result = "=== FIREBALL TEST ===\n";
            
            int enemyHealthBefore = enemy.CurrentHealth;
            
            // Create a Fireball skill
            var fireball = new FireballSkill();
            
            // Execute the skill
            var targets = new List<ICombatant> { enemy };
            SkillEffects.ExecuteSkill(fireball, mage, targets, statusEffectSystem);
            
            int enemyHealthAfter = enemy.CurrentHealth;
            int damageDealt = enemyHealthBefore - enemyHealthAfter;
            
            result += $"Fireball dealt {damageDealt} damage\n";
            result += $"Enemy HP: {enemyHealthBefore} → {enemyHealthAfter}\n";
            
            // Check if burn effect was applied
            if (enemy is Combatant enemyCombatant && enemyCombatant.StatusEffectManager != null && enemyCombatant.StatusEffectManager.EffectCount > 0)
            {
                result += "✅ Burn effect applied\n";
            }
            else
            {
                result += "❌ No burn effect applied\n";
            }
            
            DisplayResult(result);
        }
        
        void TestHeal()
        {
            if (mage == null || warrior == null) return;
            
            string result = "=== HEAL TEST ===\n";
            
            // First damage the warrior
            warrior.TakeDamage(50);
            int warriorHealthBefore = warrior.CurrentHealth;
            
            // Create a Heal skill
            var heal = new HealSkill();
            
            // Execute the skill
            var targets = new List<ICombatant> { warrior };
            SkillEffects.ExecuteSkill(heal, mage, targets, statusEffectSystem);
            
            int warriorHealthAfter = warrior.CurrentHealth;
            int healingDone = warriorHealthAfter - warriorHealthBefore;
            
            result += $"Heal restored {healingDone} HP\n";
            result += $"Warrior HP: {warriorHealthBefore} → {warriorHealthAfter}\n";
            
            DisplayResult(result);
        }
        
        void ResetHealth()
        {
            if (warrior != null)
            {
                warrior.Heal(warrior.MaxHealth);
            }
            if (mage != null)
            {
                mage.Heal(mage.MaxHealth);
            }
            if (enemy != null)
            {
                enemy.Heal(enemy.MaxHealth);
            }
            
            DisplayResult("=== HEALTH RESET ===\nAll combatants healed to full health!");
        }
        
        void DisplayResult(string result)
        {
            Debug.Log(result);
            if (outputText != null)
            {
                outputText.text = result;
            }
        }
    }
} 