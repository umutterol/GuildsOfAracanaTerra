using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Enemy Party Definition")]
public class EnemyPartyDefinition : ScriptableObject
{
    public string EncounterName;
    public List<EnemyDefinition> Enemies;
    public bool isBossEncounter;
    public Sprite EncounterBackground;
    public AudioClip EncounterMusic;
}

[System.Serializable]
public class EnemyDefinition
{
    public GameObject Prefab;
    public CharacterDefinition characterData;
    public int spawnSlotIndex; // 0-4, determines which EnemySpawn_X to use
} 