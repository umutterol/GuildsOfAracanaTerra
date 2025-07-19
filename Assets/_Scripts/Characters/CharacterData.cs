using System;

[Serializable]
public class CharacterData
{
    public string name;
    public int level;
    public int classId;  // Integer ID instead of string
    public int traitId;  // Integer ID instead of string
    // Add more fields as needed
}

[Serializable]
public class CharacterDataList
{
    public CharacterData[] characters;
} 