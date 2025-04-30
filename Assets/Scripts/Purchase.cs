using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purchase : MonoBehaviour
{
    public static bool is2Purchase = false;
    public static bool is3Purchase = false;
    public PlatformerMovement player; // Reference to PlatformerMovement

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player reference not assigned in Purchase script!");
        }
    }

    public void Select(string car)
    {
        if (player == null)
        {
            Debug.LogError("Cannot purchase: Player reference is null!");
            return;
        }

        if (car == "Car2" && !is2Purchase && player.GetCoins() >= 30)
        {
            is2Purchase = true;
            player.UpdateCoins(player.GetCoins() - 30);
            Debug.Log("Purchased Car2 for 30 coins");
        }
        if (car == "Car3" && !is3Purchase && player.GetCoins() >= 100)
        {
            is3Purchase = true;
            player.UpdateCoins(player.GetCoins() - 100);
            Debug.Log("Purchased Car3 for 100 coins");
        }
    }
}