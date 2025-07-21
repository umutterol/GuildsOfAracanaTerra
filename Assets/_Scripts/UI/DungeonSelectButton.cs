using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonSelectButton : MonoBehaviour
{
    public DungeonDefinition testDungeon; // Assign your Test Dungeon asset in Inspector
    public string partySelectionSceneName = "PartySelection"; // Set to your party selection scene name

    public void OnSelectDungeon()
    {
        Debug.Log($"[DungeonSelectButton] Button pressed. SceneFlowManager.Instance: {(SceneFlowManager.Instance != null ? "FOUND" : "NOT FOUND")}, testDungeon: {(testDungeon != null ? testDungeon.name : "NULL")}");
        if (SceneFlowManager.Instance != null && testDungeon != null)
        {
            Debug.Log($"[DungeonSelectButton] Assigning dungeon '{testDungeon.name}' and loading scene '{partySelectionSceneName}'");
            SceneFlowManager.Instance.currentDungeon = testDungeon;
            SceneManager.LoadScene(partySelectionSceneName);
        }
        else
        {
            Debug.LogError($"[DungeonSelectButton] SceneFlowManager.Instance: {(SceneFlowManager.Instance != null ? "FOUND" : "NOT FOUND")}, testDungeon: {(testDungeon != null ? testDungeon.name : "NULL")}");
        }
    }
} 