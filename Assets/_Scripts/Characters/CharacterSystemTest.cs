using UnityEngine;
using UnityEngine.UI;
using GuildsOfArcanaTerra.Combat;

public class CharacterSystemTest : MonoBehaviour
{
    [Header("UI Elements")]
    public Text outputText;
    public Button testLoadButton;
    public Button testLookupButton;
    public Button testCombatantButton;
    
    void Start()
    {
        // Set up button listeners
        if (testLoadButton != null)
            testLoadButton.onClick.AddListener(TestCharacterLoading);
        
        if (testLookupButton != null)
            testLookupButton.onClick.AddListener(TestLookupSystem);
        
        if (testCombatantButton != null)
            testCombatantButton.onClick.AddListener(TestCombatantCreation);
        
        // Auto-run basic test on start
        TestCharacterLoading();
    }
    
    void TestCharacterLoading()
    {
        string result = "=== CHARACTER LOADING TEST ===\n";
        
        if (CharacterDataManager.Instance == null)
        {
            result += "❌ CharacterDataManager not found!\n";
        }
        else
        {
            result += "✅ CharacterDataManager found\n";
            
            var characters = CharacterDataManager.Instance.GetAllCharacters();
            result += $"📊 Loaded {characters.Length} characters:\n";
            
            foreach (var character in characters)
            {
                result += $"  - {character.name} (Level {character.level})\n";
                result += $"    Class: {character.classId}\n";
                result += $"    Trait: {character.traitId}\n";
            }
        }
        
        DisplayResult(result);
    }
    
    void TestLookupSystem()
    {
        string result = "=== LOOKUP SYSTEM TEST ===\n";
        
        if (CharacterDataManager.Instance == null)
        {
            result += "❌ CharacterDataManager not found!\n";
            DisplayResult(result);
            return;
        }
        
        // Test class lookups
        result += "🔍 Testing Class Lookups:\n";
        int[] testClassIds = { 0, 1, 2, 3 };
        foreach (var classId in testClassIds)
        {
            var classDef = CharacterDataManager.Instance.GetClassById(classId);
            if (classDef != null)
            {
                result += $"  ✅ ID {classId} -> {classDef.ClassName}\n";
            }
            else
            {
                result += $"  ❌ ID {classId} -> NOT FOUND\n";
            }
        }
        
        // Test trait lookups
        result += "\n🔍 Testing Trait Lookups:\n";
        int[] testTraitIds = { 0, 1, 2 };
        foreach (var traitId in testTraitIds)
        {
            var trait = CharacterDataManager.Instance.GetTraitById(traitId);
            if (trait != null)
            {
                result += $"  ✅ ID {traitId} -> {trait.GetType().Name}\n";
            }
            else
            {
                result += $"  ❌ ID {traitId} -> NOT FOUND\n";
            }
        }
        
        DisplayResult(result);
    }
    
    void TestCombatantCreation()
    {
        string result = "=== COMBATANT CREATION TEST ===\n";
        
        if (CharacterDataManager.Instance == null)
        {
            result += "❌ CharacterDataManager not found!\n";
            DisplayResult(result);
            return;
        }
        
        var characters = CharacterDataManager.Instance.GetAllCharacters();
        if (characters.Length == 0)
        {
            result += "❌ No characters loaded!\n";
            DisplayResult(result);
            return;
        }
        
        // Test creating a combatant from the first character
        var testCharacter = characters[0];
        result += $"🎯 Testing with character: {testCharacter.name}\n";
        
        // Create a test GameObject
        GameObject testGO = new GameObject($"Test_{testCharacter.name}");
        var combatant = testGO.AddComponent<Combatant>();
        
        try
        {
            // Initialize the combatant
            combatant.InitializeFromCharacterData(testCharacter);
            result += "✅ Combatant created successfully!\n";
            
            // Check if class and trait were assigned
            var classDef = CharacterDataManager.Instance.GetClassById(testCharacter.classId);
            var trait = CharacterDataManager.Instance.GetTraitById(testCharacter.traitId);
            
            if (classDef != null)
                result += $"✅ Class assigned: {classDef.ClassName}\n";
            else
                result += $"❌ Class not found: {testCharacter.classId}\n";
                
            if (trait != null)
                result += $"✅ Trait assigned: {trait.GetType().Name}\n";
            else
                result += $"❌ Trait not found: {testCharacter.traitId}\n";
        }
        catch (System.Exception e)
        {
            result += $"❌ Error creating combatant: {e.Message}\n";
        }
        
        // Clean up
        DestroyImmediate(testGO);
        
        DisplayResult(result);
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