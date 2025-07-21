using UnityEngine;
using GuildsOfArcanaTerra.ScriptableObjects.Classes;
using System.Reflection;

namespace GuildsOfArcanaTerra.Characters
{
    /// <summary>
    /// Creates basic class definitions for testing when ScriptableObjects aren't available
    /// </summary>
    public class BasicClassDefinitions : MonoBehaviour
    {
        [Header("Basic Class Definitions")]
        [SerializeField] private CharacterClassDefinition warriorClass;
        [SerializeField] private CharacterClassDefinition mageClass;
        [SerializeField] private CharacterClassDefinition rangerClass;
        [SerializeField] private CharacterClassDefinition rogueClass;
        [SerializeField] private CharacterClassDefinition clericClass;
        
        private void Start()
        {
            CreateBasicClassDefinitions();
        }
        
        [ContextMenu("Create Basic Class Definitions")]
        public void CreateBasicClassDefinitions()
        {
            Debug.Log("Creating basic class definitions for testing...");
            
            // Create Warrior class
            if (warriorClass == null)
            {
                warriorClass = ScriptableObject.CreateInstance<CharacterClassDefinition>();
                SetClassDefinition(warriorClass, "Warrior", 100, 15, 10, 5, 12, 15);
                Debug.Log("Created Warrior class definition");
            }
            
            // Create Mage class
            if (mageClass == null)
            {
                mageClass = ScriptableObject.CreateInstance<CharacterClassDefinition>();
                SetClassDefinition(mageClass, "Mage", 70, 5, 8, 18, 6, 10);
                Debug.Log("Created Mage class definition");
            }
            
            // Create Ranger class
            if (rangerClass == null)
            {
                rangerClass = ScriptableObject.CreateInstance<CharacterClassDefinition>();
                SetClassDefinition(rangerClass, "Ranger", 85, 10, 15, 8, 8, 12);
                Debug.Log("Created Ranger class definition");
            }
            
            // Create Rogue class
            if (rogueClass == null)
            {
                rogueClass = ScriptableObject.CreateInstance<CharacterClassDefinition>();
                SetClassDefinition(rogueClass, "Rogue", 75, 8, 18, 10, 6, 10);
                Debug.Log("Created Rogue class definition");
            }
            
            // Create Cleric class
            if (clericClass == null)
            {
                clericClass = ScriptableObject.CreateInstance<CharacterClassDefinition>();
                SetClassDefinition(clericClass, "Cleric", 90, 8, 8, 12, 10, 14);
                Debug.Log("Created Cleric class definition");
            }
            
            // Add to CharacterDataManager if it exists
            var charManager = FindObjectOfType<CharacterDataManager>();
            if (charManager != null)
            {
                charManager.availableClasses.Clear();
                charManager.availableClasses.Add(warriorClass);
                charManager.availableClasses.Add(mageClass);
                charManager.availableClasses.Add(rangerClass);
                charManager.availableClasses.Add(rogueClass);
                charManager.availableClasses.Add(clericClass);
                
                Debug.Log($"Added {charManager.availableClasses.Count} class definitions to CharacterDataManager");
            }
            else
            {
                Debug.LogWarning("CharacterDataManager not found, class definitions created but not assigned");
            }
        }
        
        /// <summary>
        /// Set class definition values using reflection
        /// </summary>
        private void SetClassDefinition(CharacterClassDefinition classDef, string className, int baseHealth, 
            int baseStrength, int baseAgility, int baseIntelligence, int baseDefense, int baseVitality)
        {
            try
            {
                var type = typeof(CharacterClassDefinition);
                
                // Set class name
                var classNameField = type.GetField("className", BindingFlags.NonPublic | BindingFlags.Instance);
                if (classNameField != null)
                    classNameField.SetValue(classDef, className);
                
                // Set base stats
                var healthField = type.GetField("baseHealth", BindingFlags.NonPublic | BindingFlags.Instance);
                if (healthField != null)
                    healthField.SetValue(classDef, baseHealth);
                
                var strengthField = type.GetField("baseStrength", BindingFlags.NonPublic | BindingFlags.Instance);
                if (strengthField != null)
                    strengthField.SetValue(classDef, baseStrength);
                
                var agilityField = type.GetField("baseAgility", BindingFlags.NonPublic | BindingFlags.Instance);
                if (agilityField != null)
                    agilityField.SetValue(classDef, baseAgility);
                
                var intelligenceField = type.GetField("baseIntelligence", BindingFlags.NonPublic | BindingFlags.Instance);
                if (intelligenceField != null)
                    intelligenceField.SetValue(classDef, baseIntelligence);
                
                var defenseField = type.GetField("baseDefense", BindingFlags.NonPublic | BindingFlags.Instance);
                if (defenseField != null)
                    defenseField.SetValue(classDef, baseDefense);
                
                var vitalityField = type.GetField("baseVitality", BindingFlags.NonPublic | BindingFlags.Instance);
                if (vitalityField != null)
                    vitalityField.SetValue(classDef, baseVitality);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Could not set class definition for {className}: {e.Message}");
            }
        }
        
        /// <summary>
        /// Get class definition by ID
        /// </summary>
        public CharacterClassDefinition GetClassById(int classId)
        {
            switch (classId)
            {
                case 0: return warriorClass;
                case 1: return mageClass;
                case 2: return rangerClass;
                case 3: return rogueClass;
                case 4: return clericClass;
                default: return warriorClass; // Default fallback
            }
        }
    }
} 