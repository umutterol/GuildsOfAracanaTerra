using System.Collections.Generic;
using UnityEngine;
using GuildsOfArcanaTerra.ScriptableObjects.Classes;
using GuildsOfArcanaTerra.Traits;

[CreateAssetMenu(menuName = "Characters/Character Definition")]
public class CharacterDefinition : ScriptableObject
{
    public string characterName;
    public int maxHealth;
    public int level;
    public CharacterClassDefinition characterClass; // Reference to class ScriptableObject
    public GameObject prefab; // Reference to the character's prefab
    public List<IRLTraitSO> traits; // List of trait ScriptableObject references
    public List<string> skills;
    // Add other fields as needed (e.g., icon, description, etc.)
} 