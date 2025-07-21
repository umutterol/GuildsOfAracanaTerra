using System;
using UnityEngine;
using GuildsOfArcanaTerra.ScriptableObjects.Classes;
using GuildsOfArcanaTerra.Traits;

[Serializable]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int level;
    public GameObject prefab; // Reference to the character's prefab
    public CharacterClassDefinition characterClass; // Reference to the class ScriptableObject
    public IRLTraitSO trait; // Reference to the trait ScriptableObject
    // Add more fields as needed (stats, etc.)
}

[Serializable]
public class CharacterDataList
{
    public CharacterData[] characters;
} 