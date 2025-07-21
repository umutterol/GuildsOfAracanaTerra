using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlowManager : MonoBehaviour
{
    public static SceneFlowManager Instance;

    [Header("Scene Names")]
    public string dungeonSceneName = "DungeonCraghold";
    public string guildHallSceneName = "GuildHall";

    [Header("Party Data")]
    public List<CharacterDefinition> selectedParty;
    public DungeonDefinition currentDungeon;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Call this to start a dungeon run
    public void StartDungeon(DungeonDefinition dungeonDef, List<CharacterDefinition> party = null)
    {
        currentDungeon = dungeonDef;
        if (party != null)
            selectedParty = new List<CharacterDefinition>(party);
        SceneManager.LoadScene(dungeonSceneName);
    }

    // Call this to return to the Guild Hall
    public void ReturnToGuildHall()
    {
        SceneManager.LoadScene(guildHallSceneName);
    }

    // Call this to restart the current dungeon
    public void RestartDungeon()
    {
        SceneManager.LoadScene(dungeonSceneName);
    }

    // This should be called by DungeonRunManager after scene load
    public void InjectPartyToDungeon(DungeonRunManager dungeonRunManager)
    {
        if (dungeonRunManager != null)
        {
            dungeonRunManager.dungeonDefinition = currentDungeon;
            // If DungeonRunManager supports party assignment, do it here
            // dungeonRunManager.SetParty(selectedParty);
        }
    }
} 