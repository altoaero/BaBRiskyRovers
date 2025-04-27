using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RockGenerator : MonoBehaviour
{
    // Configuration parameters for rock generation
    public GameObject[] rockPrefabs;          // Array of rock prefabs to spawn
    public int numberOfRocks = 20;            // Target number of rocks to generate
    public float areaWidth = 50f;             // Width of the generation area
    public float areaHeight = 50f;            // Height of the generation area
    public float minDistanceBetweenRocks = 2f;// Minimum spacing between rocks
    private Vector2 spaceBaseLocation = new Vector2(-0.172f, 0.379f); // Fixed base location
    private List<Vector2> rockPositions = new List<Vector2>(); // Stores all rock positions
    private bool terrainGenerationComplete = false; // Flag for terrain completion

    // Public access to spawned rock data
    public List<Vector2> SpawnedRockPositions { get; private set; } = new List<Vector2>();
    public List<GameObject> SpawnedRocks { get; private set; } = new List<GameObject>();

    // References to other components
    public AStarPathfinder astarPathfinder;
    public float[,] map;

    // Generation control flag
    public bool autoGenerateOnStart = false;

    // Data structure for serializing rock information
    [System.Serializable]
    public class RockData
    {
        public Vector2 position;     // World position of the rock
        public int prefabIndex;      // Index in the rockPrefabs array

        public RockData(Vector2 pos, int index)
        {
            position = pos;
            prefabIndex = index;
        }
    }

    // Collection of all rock data for serialization
    public List<RockData> RockDataCollection { get; private set; } = new List<RockData>();

    void Start()
    {
        // Auto-generate rocks if enabled
        if (autoGenerateOnStart)
        {
            GenerateRocks();
            SaveRockData();
        }
    }

    /// <summary>
    /// Main method to generate rocks using Poisson Disk Sampling
    /// </summary>
    public void GenerateRocks()
    {
        // Clear existing rocks and data
        ClearExistingRocks();

        // Always include the space base location
        rockPositions.Add(spaceBaseLocation);

        // Generate rock positions using Poisson Disk Sampling
        GenerateRockPositions();

        // Instantiate rock GameObjects
        InstantiateRocks();
    }

    /// <summary>
    /// Clears all existing rocks and reset collections
    /// </summary>
    private void ClearExistingRocks()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        rockPositions.Clear();
        SpawnedRockPositions.Clear();
        SpawnedRocks.Clear();
        RockDataCollection.Clear();
    }

    /// <summary>
    /// Generates rock positions using Poisson Disk Sampling algorithm
    /// </summary>
    private void GenerateRockPositions()
    {
        List<Vector2> newPoints = PoissonDiskSampling.GeneratePoints(
            minDistanceBetweenRocks,
            new Vector2(areaWidth, areaHeight),
            30, // Number of attempts per point
            numberOfRocks - 1 // Subtract 1 for space base
        );

        // Convert points to world space and validate distance from base
        foreach (Vector2 point in newPoints)
        {
            Vector2 worldPoint = new Vector2(
                point.x - areaWidth / 2,
                point.y - areaHeight / 2
            );

            if (Vector2.Distance(worldPoint, spaceBaseLocation) >= minDistanceBetweenRocks)
            {
                rockPositions.Add(worldPoint);
            }
        }
    }

    /// <summary>
    /// Instantiates rock GameObjects at calculated positions
    /// </summary>
    private void InstantiateRocks()
    {
        // Start from 1 to skip space base (index 0)
        for (int i = 1; i < Mathf.Min(rockPositions.Count, numberOfRocks + 1); i++)
        {
            Vector2 position = rockPositions[i];
            int prefabIndex = Random.Range(0, rockPrefabs.Length);

            GameObject rock = Instantiate(
                rockPrefabs[prefabIndex],
                position,
                Quaternion.identity,
                transform
            );

            // Track spawned rocks
            SpawnedRockPositions.Add(position);
            SpawnedRocks.Add(rock);
            RockDataCollection.Add(new RockData(position, prefabIndex));
        }
    }

    /// <summary>
    /// Saves rock data to JSON file
    /// </summary>
    /// <param name="fileName">Name of the save file</param>
    public void SaveRockData(string fileName = "rock_data.json")
    {
        string json = JsonUtility.ToJson(new SerializableRockDataList(RockDataCollection));
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, json);
    }

    /// <summary>
    /// Loads rock data from JSON file
    /// </summary>
    /// <param name="fileName">Name of the file to load</param>
    public void LoadRockData(string fileName = "rock_data.json")
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            // Deserialization logic would go here
        }
    }

    /// <summary>
    /// Wrapper class for JSON serialization of RockData list
    /// </summary>
    [System.Serializable]
    private class SerializableRockDataList
    {
        public List<RockData> rocks;

        public SerializableRockDataList(List<RockData> rockList)
        {
            rocks = rockList;
        }
    }
}

/// <summary>
/// Static class implementing Poisson Disk Sampling algorithm
/// for evenly distributed point generation
/// </summary>
public static class PoissonDiskSampling
{
    /// <summary>
    /// Generates evenly distributed points within a region
    /// </summary>
    /// <param name="radius">Minimum distance between points</param>
    /// <param name="sampleRegionSize">Size of the generation area</param>
    /// <param name="rejectionSamples">Attempts per point before rejection</param>
    /// <param name="maxPoints">Maximum points to generate</param>
    /// <returns>List of generated points</returns>
    public static List<Vector2> GeneratePoints(float radius, Vector2 sampleRegionSize, int rejectionSamples, int maxPoints)
    {
        // Setup grid for spatial partitioning
        float cellSize = radius / Mathf.Sqrt(2);
        int gridWidth = Mathf.CeilToInt(sampleRegionSize.x / cellSize);
        int gridHeight = Mathf.CeilToInt(sampleRegionSize.y / cellSize);
        int[,] grid = new int[gridWidth, gridHeight];
        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();

        // Start with center point
        spawnPoints.Add(sampleRegionSize / 2);

        while (spawnPoints.Count > 0 && points.Count < maxPoints)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2 spawnCenter = spawnPoints[spawnIndex];
            bool candidateAccepted = false;

            // Try to find valid candidate position
            for (int i = 0; i < rejectionSamples; i++)
            {
                Vector2 candidate = GenerateCandidate(spawnCenter, radius);

                if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;
                    candidateAccepted = true;
                    break;
                }
            }

            // Remove spawn point if no candidates found
            if (!candidateAccepted)
            {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }

        return points;
    }

    /// <summary>
    /// Generates a random candidate point around a center
    /// </summary>
    private static Vector2 GenerateCandidate(Vector2 center, float radius)
    {
        float angle = Random.value * Mathf.PI * 2;
        Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
        return center + dir * Random.Range(radius, 2 * radius);
    }

    /// <summary>
    /// Checks if a candidate point is valid (within bounds and properly spaced)
    /// </summary>
    private static bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector2> points, int[,] grid)
    {
        // Check bounds
        if (candidate.x < 0 || candidate.x >= sampleRegionSize.x ||
            candidate.y < 0 || candidate.y >= sampleRegionSize.y)
            return false;

        // Check neighboring cells
        int cellX = (int)(candidate.x / cellSize);
        int cellY = (int)(candidate.y / cellSize);

        int searchStartX = Mathf.Max(0, cellX - 2);
        int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
        int searchStartY = Mathf.Max(0, cellY - 2);
        int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

        for (int x = searchStartX; x <= searchEndX; x++)
        {
            for (int y = searchStartY; y <= searchEndY; y++)
            {
                int pointIndex = grid[x, y] - 1;
                if (pointIndex != -1)
                {
                    if ((candidate - points[pointIndex]).sqrMagnitude < radius * radius)
                        return false;
                }
            }
        }

        return true;
    }
}