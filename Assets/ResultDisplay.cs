using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class ResultDisplay : MonoBehaviour
{
    public PlatformerMovement player;
    public TextMeshProUGUI currentTimeText;
    public TextMeshProUGUI bestTimeText;
    public TextMeshProUGUI carNameText;
    public TextMeshProUGUI coinText; // Field for coin display
    float currentTime;

    void Start()
    {
        DisplayResults();
    }

    void Update()
    {
        DisplayResults();
    }

    void DisplayResults()
    {
        PlatformerMovement.HighScoreData highScore = null;
        if (player != null)
        {
            currentTime = player.GetRaceTime();
            highScore = player.GetHighScoreData();
        }
        else
        {
            string savePath = Application.persistentDataPath + "/highscore.json";
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                highScore = JsonUtility.FromJson<PlatformerMovement.HighScoreData>(json);
                currentTime = highScore.bestTime; // Fallback to best time
            }
        }

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

        if (coinText != null && highScore != null)
        {
            coinText.text = $"Coins: {highScore.coins}";
        }
    }
}