using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purchase : MonoBehaviour
{
    public static bool is2Purchase = false;
    public static bool is3Purchase = false;
    public PlatformerMovement player; // Reference to PlatformerMovement

    void Awake()
    {
        Debug.Log($"Purchase script initialized on {gameObject.name}, Player assigned: {player != null}");
    }

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player reference not assigned in Purchase script!");
        }
    }

    void Update()
    {
    }

    public void Select(string car)
    {
        Debug.Log($"Purchase.Select called with car: '{car}', Player: {(player != null ? player.gameObject.name : "null")}");
        
        if (player == null)
        {
            Debug.LogError("Cannot purchase: Player reference is null!");
            return;
        }

        int currentCoins = player.GetCoins();
        Debug.Log($"Attempting to purchase {car}, Coins: {currentCoins}, Car2Purchased: {is2Purchase}, Car3Purchased: {is3Purchase}");

        if (string.Equals(car, "Car2", System.StringComparison.OrdinalIgnoreCase) && !is2Purchase && currentCoins >= 30)
        {
            is2Purchase = true;
            player.UpdateCoins(currentCoins - 30);
            Debug.Log("Purchased Car2 for 30 coins");
        }
        else if (string.Equals(car, "Car3", System.StringComparison.OrdinalIgnoreCase) && !is3Purchase && currentCoins >= 100)
        {
            is3Purchase = true;
            player.UpdateCoins(currentCoins - 100);
            Debug.Log("Purchased Car3 for 100 coins");
        }
        else
        {
            Debug.LogWarning($"Purchase failed: car={car}, Coins={currentCoins}, Car2Purchased={is2Purchase}, Car3Purchased={is3Purchase}");
        }
    }
}