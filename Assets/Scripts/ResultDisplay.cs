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
        PlatformerMovement.LevelData levelData = null;
        if (player != null)
        {
            currentTime = player.GetRaceTime();
            highScore = player.GetHighScoreData();
            if (highScore != null)
            {
                string currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                levelData = highScore.levels.Find(ld => ld.level == currentLevel);
            }
        }
        else
        {
            string savePath = Application.persistentDataPath + "/highscore.json";
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                highScore = JsonUtility.FromJson<PlatformerMovement.HighScoreData>(json);
                if (highScore != null)
                {
                    string currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                    levelData = highScore.levels.Find(ld => ld.level == currentLevel);
                    currentTime = levelData != null ? levelData.bestTime : 0f; // Fallback to best time
                }
            }
        }

        if (currentTimeText != null)
        {
            currentTimeText.text = $"Current Time: {currentTime:F2}s";
        }

        if (bestTimeText != null && levelData != null)
        {
            bestTimeText.text = $"Best Time: {levelData.bestTime:F2}s";
        }

        if (carNameText != null && levelData != null)
        {
            carNameText.text = $"Car: {levelData.carName}";
        }

        if (coinText != null && highScore != null)
        {
            coinText.text = $"Coins: {highScore.coins}";
        }
    }
}