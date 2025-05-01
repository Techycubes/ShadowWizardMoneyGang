using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Purchase : MonoBehaviour
{
    public static bool is2Purchase = false;
    public static bool is3Purchase = false;
    public PlatformerMovement player; // Reference to PlatformerMovement
    private string savePath;

    void Awake()
    {
        savePath = Path.Combine("C:/Formula2Game", "highscore.json");
        player = FindObjectOfType<PlatformerMovement>();
        Debug.Log($"Purchase Awake: Initialized on {gameObject.name}, Player found: {player != null}, Player GameObject: {(player != null ? player.gameObject.name : "null")}, Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}, savePath: {savePath}");
    }

    void OnEnable()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlatformerMovement>();
            Debug.Log($"Purchase OnEnable: Refreshed player reference, Player found: {player != null}, Player GameObject: {(player != null ? player.gameObject.name : "null")}, Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        }
    }

    void Start()
    {
        if (player == null)
        {
            Debug.LogError($"Purchase Start: Player reference not found in {gameObject.name}, Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}!");
        }
    }

    void Update()
    {
        Debug.Log($"Purchase Update: Active on {gameObject.name}, Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}, Player: {(player != null ? player.gameObject.name : "null")}");
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Manual purchase test triggered with KeyCode.P");
            Select("Car2");
        }
    }

    public void Select(string car)
    {
        Debug.Log($"Purchase.Select called with car: '{car}', Player: {(player != null ? player.gameObject.name : "null")}, Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");

        if (player == null)
        {
            player = FindObjectOfType<PlatformerMovement>();
            Debug.Log($"Purchase.Select: Player was null, attempted to re-find, Player found: {player != null}, Player GameObject: {(player != null ? player.gameObject.name : "null")}");
        }

        if (player == null)
        {
            Debug.LogError($"Cannot purchase: Player reference is null in {gameObject.name}, Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}!");
            return;
        }

        // Force reload to ensure latest coins
        PlatformerMovement.HighScoreData highScoreData = player.GetHighScoreData();
        int currentCoins = player.GetCoins();
        if (currentCoins == 0 && highScoreData != null && highScoreData.coins > 0)
        {
            Debug.LogWarning($"Player coins are 0 but highScoreData has {highScoreData.coins}, syncing");
            player.UpdateCoins(highScoreData.coins);
            currentCoins = highScoreData.coins;
        }

        Debug.Log($"Attempting to purchase {car}, Coins: {currentCoins}, Car2Purchased: {is2Purchase}, Car3Purchased: {is3Purchase}");

        if (string.Equals(car, "Car2", System.StringComparison.OrdinalIgnoreCase) && !is2Purchase && currentCoins >= 30)
        {
            is2Purchase = true;
            Debug.Log($"Before UpdateCoins: Coins={currentCoins}, Car2Purchased={is2Purchase}, Car3Purchased={is3Purchase}");
            player.UpdateCoins(currentCoins - 30);
            Debug.Log($"After UpdateCoins: Coins={player.GetCoins()}, Car2Purchased={is2Purchase}, Car3Purchased={is3Purchase}, highscore.json exists: {File.Exists(savePath)}");
            // Verify file content
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                Debug.Log($"highscore.json content after purchase: {json}");
            }
        }
        else if (string.Equals(car, "Car3", System.StringComparison.OrdinalIgnoreCase) && !is3Purchase && currentCoins >= 100)
        {
            is3Purchase = true;
            Debug.Log($"Before UpdateCoins: Coins={currentCoins}, Car2Purchased={is2Purchase}, Car3Purchased={is3Purchase}");
            player.UpdateCoins(currentCoins - 100);
            Debug.Log($"After UpdateCoins: Coins={player.GetCoins()}, Car2Purchased={is2Purchase}, Car3Purchased={is3Purchase}, highscore.json exists: {File.Exists(savePath)}");
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                Debug.Log($"highscore.json content after purchase: {json}");
            }
        }
        else
        {
            Debug.LogWarning($"Purchase failed: car={car}, Coins={currentCoins}, Car2Purchased={is2Purchase}, Car3Purchased={is3Purchase}");
        }
    }
}