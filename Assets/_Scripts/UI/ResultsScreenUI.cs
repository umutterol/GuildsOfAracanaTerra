using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultsScreenUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI statsText;
    public Button returnButton;
    public Button restartButton;

    public void ShowResults(bool victory, string stats)
    {
        resultText.text = victory ? "Victory" : "Defeat";
        statsText.text = stats;
        gameObject.SetActive(true);
    }

    public void ShowVictory()
    {
        ShowResults(true, ""); // You can customize stats as needed
    }

    public void ShowDefeat()
    {
        ShowResults(false, ""); // You can customize stats as needed
    }

    private void Awake()
    {
        // Optionally hide on start
        gameObject.SetActive(false);
        // Button listeners can be set up here or externally
        // returnButton.onClick.AddListener(() => ...);
        // restartButton.onClick.AddListener(() => ...);
    }
} 