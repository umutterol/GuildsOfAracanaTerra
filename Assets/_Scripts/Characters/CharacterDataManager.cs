using System.Collections.Generic;
using UnityEngine;
using GuildsOfArcanaTerra.ScriptableObjects.Classes;
using GuildsOfArcanaTerra.Traits;

public class CharacterDataManager : MonoBehaviour
{
    [Header("Class Definitions")]
    public List<CharacterClassDefinition> availableClasses = new List<CharacterClassDefinition>();
    
    [Header("Trait Definitions")]
    public List<IRLTraitSO> availableTraits = new List<IRLTraitSO>();
    
    [Header("Character Data")]
    public CharacterDataList characterList;
    
    private static CharacterDataManager instance;
    public static CharacterDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CharacterDataManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("CharacterDataManager");
                    instance = go.AddComponent<CharacterDataManager>();
                }
            }
            return instance;
        }
    }
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCharacterData();
            LogAvailableAssets();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    void LoadCharacterData()
    {
        TextAsset json = Resources.Load<TextAsset>("characters");
        if (json != null)
        {
            characterList = JsonUtility.FromJson<CharacterDataList>(json.text);
            Debug.Log($"Loaded {characterList.characters.Length} characters from JSON.");
        }
        else
        {
            Debug.LogError("Could not find characters.json in Resources!");
        }
    }
    
    void LogAvailableAssets()
    {
        Debug.Log("=== AVAILABLE ASSETS ===");
        Debug.Log("Classes:");
        for (int i = 0; i < availableClasses.Count; i++)
        {
            if (availableClasses[i] != null)
                Debug.Log($"  ID {i}: {availableClasses[i].ClassName}");
        }
        
        Debug.Log("Traits:");
        for (int i = 0; i < availableTraits.Count; i++)
        {
            if (availableTraits[i] != null)
                Debug.Log($"  ID {i}: {availableTraits[i].GetType().Name}");
        }
    }
    
    public CharacterClassDefinition GetClassById(int classId)
    {
        if (classId >= 0 && classId < availableClasses.Count)
        {
            return availableClasses[classId];
        }
        
        Debug.LogWarning($"Class with ID {classId} not found! Available range: 0-{availableClasses.Count - 1}");
        return null;
    }
    
    public IRLTraitSO GetTraitById(int traitId)
    {
        if (traitId >= 0 && traitId < availableTraits.Count)
        {
            return availableTraits[traitId];
        }
        
        Debug.LogWarning($"Trait with ID {traitId} not found! Available range: 0-{availableTraits.Count - 1}");
        return null;
    }
    
    public CharacterData[] GetAllCharacters()
    {
        return characterList?.characters ?? new CharacterData[0];
    }
    
    public CharacterData GetCharacterByName(string characterName)
    {
        if (characterList?.characters == null) return null;
        
        foreach (var character in characterList.characters)
        {
            if (character.name == characterName)
            {
                return character;
            }
        }
        
        return null;
    }
} 