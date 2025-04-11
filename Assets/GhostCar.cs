using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCar : MonoBehaviour
{
    public PlatformerMovement player; // Reference to the player’s script
    private List<Vector2> positions; // Will store reference to StoredPositions
    private bool isReplaying = false;

    void Start()
    {
        // Get the stored positions from PlatformerMovement
        if (player != null)
        {
            positions = player.GetStoredPositions();
        }
        else
        {
            Debug.LogError("Player reference not set in GhostCar!");
        }

        // Start replaying if positions are available
        if (positions != null && positions.Count > 0 && CanStart)
        {
            StartCoroutine(ReplayPositions());
        }
    }

    void Update()
    {
        // Optional: Add logic if you need manual control later
    }

    IEnumerator ReplayPositions()
    {
        isReplaying = true;
        for (int i = 0; i < positions.Count; i++)
        {
            // Move ghost car to the recorded position
            transform.position = positions[i];
            yield return new WaitForSecondsRealtime(0.1f); // Match recording interval
        }
        isReplaying = false;
        Debug.Log("Ghost car replay finished.");
    }
}