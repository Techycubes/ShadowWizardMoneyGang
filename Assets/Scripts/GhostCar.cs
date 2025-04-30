using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCar : MonoBehaviour
{
    public PlatformerMovement player;
    public Animator animator; // Reference to GhostCar's Animator
    private List<Vector2> positions;
    private bool isReplaying = false;
    private bool hasPreviousRun = false;
    private float bestTime;
    private float previousAngle; // Track previous rotation for turn detection

    void Start()
    {
        Debug.Log("GhostCar Start: Initializing...");

        if (player == null)
        {
            Debug.LogError("Player reference not set in GhostCar!");
            return;
        }

        if (animator == null)
        {
            Debug.LogError("Animator not set in GhostCar!");
            animator = GetComponent<Animator>(); // Try to get Animator if not assigned
        }

        Debug.Log("Player reference found, loading high score...");
        PlatformerMovement.HighScoreData highScore = player.GetHighScoreData();
        PlatformerMovement.LevelData levelData = null;
        if (highScore != null)
        {
            string currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            levelData = highScore.levels.Find(ld => ld.level == currentLevel);
        }
        if (levelData != null && levelData.bestRunPositions != null && levelData.bestRunPositions.Count > 0)
        {
            positions = levelData.bestRunPositions;
            bestTime = levelData.bestTime;
            hasPreviousRun = true;
            Debug.Log($"Loaded high score: Time={levelData.bestTime:F2}s, Positions={levelData.bestRunPositions.Count}");
        }
        else
        {
            Debug.LogWarning("No valid high score data found for current level");
            positions = player.GetStoredPositions();
            Debug.Log($"Current positions count: {positions?.Count ?? 0}");
            if (positions == null || positions.Count == 0)
            {
                Debug.LogWarning("No positions available from GetStoredPositions");
            }
        }

        StartCoroutine(WaitForRaceStart());
    }

    IEnumerator WaitForRaceStart()
    {
        Debug.Log("Waiting for DissapearObject.CanStart to be true...");
        while (!DissapearObject.CanStart)
        {
            yield return null;
        }

        Debug.Log($"Race started, hasPreviousRun: {hasPreviousRun}, Positions: {(positions != null ? positions.Count : 0)}");
        if (hasPreviousRun || (positions != null && positions.Count > 0))
        {
            Debug.Log("Starting ReplayPositions coroutine");
            StartCoroutine(ReplayPositions());
        }
        else
        {
            Debug.LogWarning("Replay not started: No high score or positions available");
        }
    }

    void Update()
    {
        if (isReplaying)
        {
            Debug.Log($"GhostCar position: {transform.position}, rotation: {transform.eulerAngles.z}");
        }
    }

    IEnumerator ReplayPositions()
    {
        isReplaying = true;
        gameObject.SetActive(true);
        Debug.Log("Replay started");
        previousAngle = transform.eulerAngles.z; // Initialize previous angle

        for (int i = 0; i < positions.Count; i++)
        {
            transform.position = positions[i];
            Debug.Log($"Moving to position {i}: {positions[i]}");

            if (i < positions.Count - 1)
            {
                Vector2 direction = positions[i + 1] - positions[i];
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle - 90);
                Debug.Log($"Rotating to face position {i + 1}: {positions[i + 1]}, angle: {angle}");

                // Detect turn direction
                float angleDelta = Mathf.DeltaAngle(previousAngle, angle);
                const float turnThreshold = 5f; // Minimum angle change to trigger animation
                if (angleDelta > turnThreshold)
                {
                    animator.SetTrigger("LeftTurn");
                    Debug.Log($"Triggering LeftTurn: angleDelta={angleDelta:F2}");
                }
                else if (angleDelta < -turnThreshold)
                {
                    animator.SetTrigger("RightTurn");
                    Debug.Log($"Triggering RightTurn: angleDelta={angleDelta:F2}");
                }
                previousAngle = angle;
            }

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