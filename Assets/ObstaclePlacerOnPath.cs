using UnityEngine;
using Cinemachine;

public class ObstaclePlacerOnPath : MonoBehaviour
{
    public GameObject obstaclePrefab; // Assign your obstacle prefab in the Inspector
    public CinemachinePathBase path; // Assign your CinemachinePath in the Inspector
    public int numberOfObstacles = 1000; // Total obstacles to place
    public float minSpacing = 1f; // Minimum distance between obstacles
    public float maxSpacing = 5f; // Maximum distance between obstacles

    void Start()
    {
        if (obstaclePrefab == null || path == null)
        {
            Debug.LogError("Obstacle Prefab or Path not assigned!");
            return;
        }

        float pathLength = path.PathLength; // Total length of the path
        float currentDistance = 0f;

        for (int i = 0; i < numberOfObstacles; i++)
        {
            // Increment position by random spacing
            currentDistance += Random.Range(minSpacing, maxSpacing);
            if (currentDistance > pathLength) break; // Stop if we exceed path length

            // Get position and rotation along the path
            Vector3 position = path.EvaluatePositionAtUnit(currentDistance, CinemachinePathBase.PositionUnits.Distance);
            Quaternion rotation = path.EvaluateOrientationAtUnit(currentDistance, CinemachinePathBase.PositionUnits.Distance);


            // Instantiate the obstacle
            Instantiate(obstaclePrefab, position, rotation, transform);
        }
    }
}