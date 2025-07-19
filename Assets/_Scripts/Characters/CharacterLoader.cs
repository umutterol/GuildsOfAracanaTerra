using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    public CharacterDataList characterList;

    void Awake()
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
} 