using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultDisplay : MonoBehaviour
{
    public PlatformerMovement player;
    public TextMeshProUGUI currentTimeText;
    public TextMeshProUGUI bestTimeText;
    public TextMeshProUGUI carNameText;
    float currentTime;
    void Start()
    {
        DisplayResults();
    }
    void Update(){
        DisplayResults();
    }

    void DisplayResults()
    {
        if (player == null)
        {
            Debug.LogError("Player reference not set in ResultsDisplay!");
            return;
        }

        currentTime = player.GetRaceTime();
        PlatformerMovement.HighScoreData highScore = player.GetHighScoreData();

        if (currentTimeText != null)
        {
            currentTimeText.text = $"Current Time: {currentTime:F2}s";
        }

        if (bestTimeText != null && highScore != null)
        {
            bestTimeText.text = $"Best Time: {highScore.bestTime:F2}s";
        }

        if (carNameText != null && highScore != null)
        {
            carNameText.text = $"Car: {highScore.carName}";
        }
    }
}