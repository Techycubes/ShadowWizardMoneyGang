using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
    public PlatformerMovement player; // Reference to the PlatformerMovement script
    public TextMeshProUGUI coinText; // Reference to the UI Text component

    void Start()
    {
        if (coinText == null)
        {
            Debug.LogError("Coin Text is not assigned in the Inspector!");
        }
        UpdateCoinDisplay();
    }

    void Update()
    {
        UpdateCoinDisplay();
    }

    void UpdateCoinDisplay()
    {
        if (player != null && coinText != null)
        {
            coinText.text = "Coins: " + player.GetCoins().ToString();
        }
    }
}