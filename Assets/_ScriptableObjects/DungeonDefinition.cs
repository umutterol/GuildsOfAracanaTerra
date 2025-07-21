using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon/Dungeon Definition")]
public class DungeonDefinition : ScriptableObject
{
    public string DungeonName;
    public List<EnemyPartyDefinition> Encounters;
    public Sprite DefaultBackground;
    public AudioClip DefaultMusic;
} 