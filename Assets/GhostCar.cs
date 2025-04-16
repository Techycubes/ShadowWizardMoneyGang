using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCar : MonoBehaviour
{
    public PlatformerMovement player;
    private List<Vector2> positions;
    private bool isReplaying = false;
    private bool hasPreviousRun = false;
    private float bestTime;

    void Start()
    {
        Debug.Log("GhostCar Start: Initializing...");

        if (player == null)
        {
            Debug.LogError("Player reference not set in GhostCar!");
            return;
        }

        Debug.Log("Player reference found, loading high score...");
        PlatformerMovement.HighScoreData highScore = player.GetHighScoreData();
        if (highScore != null && highScore.bestRunPositions != null && highScore.bestRunPositions.Count > 0)
        {
            positions = highScore.bestRunPositions;
            bestTime = highScore.bestTime;
            hasPreviousRun = true;
            Debug.Log($"Loaded high score: Time={highScore.bestTime:F2}s, Positions={highScore.bestRunPositions.Count}");
        }
        else
        {
            Debug.LogWarning("No valid high score data found");
            positions = player.GetStoredPositions();
            Debug.Log($"Current positions count: {positions?.Count ?? 0}");
            if (positions == null || positions.Count == 0)
            {
                Debug.LogWarning("No positions available from GetStoredPositions");
            }
        }

        bool canStart1 = false;
        try
        {
            canStart1 = DissapearObject.CanStart;
            Debug.Log($"DissapearObject.CanStart: {canStart1}");
        }
        catch
        {
            Debug.LogError("DissapearObject is not defined or inaccessible! Assuming CanStart=true");
            canStart1 = true; // Fallback to allow replay
        }

        Debug.Log($"hasPreviousRun: {hasPreviousRun}, CanStart: {canStart1}");
        if (hasPreviousRun && canStart1)
        {
            Debug.Log("Starting ReplayPositions coroutine (high score)");
            StartCoroutine(ReplayPositions());
        }
        else if (positions != null && positions.Count > 0 && canStart1)
        {
            Debug.Log("Starting ReplayPositions coroutine (current positions)");
            StartCoroutine(ReplayPositions());
        }
        else
        {
            Debug.LogWarning($"Replay not started: {(hasPreviousRun ? "" : "No high score, ")}{(positions != null && positions.Count > 0 ? "" : "No positions, ")}{(canStart1 ? "" : "CanStart is false")}");
        }
    }

    void Update()
    {
        if (isReplaying)
        {
            Debug.Log($"GhostCar position: {transform.position}");
        }
    }

    IEnumerator ReplayPositions()
    {
        isReplaying = true;
        gameObject.SetActive(true);
        Debug.Log("Replay started");

        for (int i = 0; i < positions.Count; i++)
        {
            transform.position = positions[i];
            Debug.Log($"Moving to position {i}: {positions[i]}");
            yield return new WaitForSecondsRealtime(0.1f);
        }

        isReplaying = false;
        gameObject.SetActive(false);
        Debug.Log($"Ghost car replay finished. Best time: {bestTime}s");
    }

    public bool HasPreviousRun()
    {
        return hasPreviousRun;
    }

    public float GetBestTime()
    {
        return bestTime;
    }
}