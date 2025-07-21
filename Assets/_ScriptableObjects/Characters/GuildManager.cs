using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Guilds/Guild Manager")]
public class GuildManager : ScriptableObject
{
    // List of all available characters in the guild
    // Each CharacterDefinition should reference its prefab, class, and traits
    public List<CharacterDefinition> guildMembers;
} 