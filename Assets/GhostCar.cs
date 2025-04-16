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
        if (player != null)
        {
            PlatformerMovement.HighScoreData highScore = player.GetHighScoreData();
            if (highScore != null && highScore.bestRunPositions != null && highScore.bestRunPositions.Count > 0)
            {
                positions = highScore.bestRunPositions;
                bestTime = highScore.bestTime;
                hasPreviousRun = true;
                Debug.Log($"Loaded high score: {highScore.bestTime:F2}s, {highScore.bestRunPositions.Count} positions");
            }
            else
            {
                positions = player.GetStoredPositions();
                Debug.Log($"No high score, using current positions: {positions.Count} positions");
            }
        }
        else
        {
            Debug.LogError("Player reference not set in GhostCar!");
        }

        Debug.Log($"hasPreviousRun: {hasPreviousRun}, DissapearObject.CanStart: {DissapearObject.CanStart}");
        if (hasPreviousRun && DissapearObject.CanStart)
        {
            Debug.Log("Starting ReplayPositions coroutine");
            StartCoroutine(ReplayPositions());
        }
        else
        {
            Debug.LogWarning("Replay not started: " + 
                (hasPreviousRun ? "" : "No previous run, ") + 
                (DissapearObject.CanStart ? "" : "CanStart is false"));
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