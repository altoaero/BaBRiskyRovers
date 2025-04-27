using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder : MonoBehaviour
{
    public DiamondSquareTerrainGenerator terrainGenerator;
    public RockGenerator rockGenerator;
    // Flag to track completion
    private bool terrainGenerated = false;
    private bool rocksGenerated = false;
    
    // A* specific variables
    private Node[,] grid;
    private int gridSizeX, gridSizeZ = 100;
    public float nodeRadius = 0.5f;
    public LayerMask unwalkableMask;
    public Vector3 gridWorldSize = new Vector3(100, 0, 100);
    public float maxSlopeAngle = 45f;
    public float weightThreshold = 0.8f; // Threshold for determining if terrain is too difficult to traverse
    public float terrainCostFactor = 5f; // Factor to multiply terrain cost by

    // Path variables
    public Transform startPoint; // Reference to the starting point (e.g., player)
    public List<Node> path;
    private GameObject targetMarker; // Visual indicator for the selected rock
    private GameObject pathContainer; // Container for path visualization
    public Transform roverTransform;

    [Header("Visualization Settings")]
    public bool showGrid = true;
    public bool showUnwalkable = true;
    public bool showWalkable = true;
    public bool showRockPositions = true;
    public Color walkableColor = Color.green;
    public Color unwalkableColor = Color.red;
    public Color rockColor = Color.magenta;
    public float nodeGizmoSize = 0.3f;
    void Start()
    {
        // Start the generation sequence
        StartCoroutine(GenerationSequence());

    }

    IEnumerator GenerationSequence()
    {
        Debug.Log("AStarPathfinder: Starting generation sequence...");
        // Step 1: Generate terrain first
        if (terrainGenerator == null)
        {
            terrainGenerator = FindObjectOfType<DiamondSquareTerrainGenerator>();
            if (terrainGenerator == null)
            {
                Debug.LogError("AStarPathfinder: No DiamondSquareTerrainGenerator found!");
                yield break;
            }
        }

        // Disable auto-start in other scripts
        DisableAutoStartInOtherScripts();

        // Generate terrain
        Debug.Log("AStarPathfinder: Generating terrain...");
        terrainGenerator.GenerateTerrain(0);
        terrainGenerated = true;
        Debug.Log("AStarPathfinder: Terrain generation complete.");

        // Wait one frame to ensure terrain is fully generated
        yield return null;

        // Step 2: Generate rocks
        if (rockGenerator == null)
        {
            rockGenerator = FindObjectOfType<RockGenerator>();
            if (rockGenerator == null)
            {
                Debug.LogError("AStarPathfinder: No RockGenerator found!");
                yield break;
            }
        }

        // Set weight map reference from terrain generator to rock generator
        rockGenerator.map = terrainGenerator.weightMap;
        Debug.Log("AStarPathfinder: Generating rocks...");
        rockGenerator.GenerateRocks();
        rockGenerator.SaveRockData();
        rocksGenerated = true;
        Debug.Log("AStarPathfinder: Rock generation complete.");

        // Wait one frame to ensure rocks are fully generated
        yield return null;

        // Initialize rover reference if not set
        if (roverTransform == null)
        {
            // Try to find the rover by name or tag
            GameObject rover = GameObject.Find("Rover"); // or use FindWithTag if you prefer
            if (rover != null)
            {
                roverTransform = rover.transform;
                Debug.Log($"Found rover at position: {roverTransform.position}");
            }
            else
            {
                Debug.LogError("AStarPathfinder: No rover transform assigned and no 'Rover' GameObject found!");
            }
        }
        if (roverTransform != null)
        {
            startPoint = roverTransform;
            Debug.Log($"Pathfinder start point set to rover position: {startPoint.position}");
        }
        else
        {
            Debug.LogError("Pathfinder cannot initialize - no rover reference available!");
           
        }
        // Step 3: Initialize pathfinder with the newly generated data
        InitializePathfinder();

        // Step 4: Find path to a random rock
        if (rockGenerator.SpawnedRocks.Count > 0)
        {
            FindPathToRandomRock();
        }
        else
        {
            Debug.LogWarning("AStarPathfinder: No rocks available to find path to!");
        }
    }

    private void DisableAutoStartInOtherScripts()
    {
        // If you have any auto-start flags in other scripts, disable them here
        if (terrainGenerator != null)
        {
            terrainGenerator.autoGenerateOnStart = false;
        }
    }

    private void InitializePathfinder()
    {
        Debug.Log("AStarPathfinder: Initializing pathfinding system...");

        // Access terrain data
        if (terrainGenerator.weightMap != null)
        {
            Debug.Log($"AStarPathfinder: Terrain weight map size: {terrainGenerator.weightMap.GetLength(0)}x{terrainGenerator.weightMap.GetLength(1)}");

            // Initialize grid based on terrain size
            gridSizeX = terrainGenerator.weightMap.GetLength(0);
            gridSizeZ = terrainGenerator.weightMap.GetLength(1);
            CreateGrid();
        }

        Debug.Log($"AStarPathfinder: Number of rocks generated: {rockGenerator.SpawnedRocks.Count}");

        // Add rocks as obstacles in the grid
        AddRocksToGrid();

        // Create a starting point if none exists
        if (startPoint == null)
        {
            GameObject startObj = new GameObject("PathStart");
            startPoint = startObj.transform;

            // Set the start point at a reasonable position on the terrain
            int midX = gridSizeX / 2;
            int midZ = gridSizeZ / 2;
            float y = GetTerrainHeight(midX, midZ);
            startPoint.position = new Vector3(midX * nodeRadius * 2, y, midZ * nodeRadius * 2);
        }
    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeZ];

        // World position of bottom left of the grid
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.z / 2;

        // Create nodes for each cell in the grid
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                // Calculate world position for this node
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeRadius * 2 + nodeRadius)
                                   + Vector3.forward * (z * nodeRadius * 2 + nodeRadius);

                // Adjust Y position based on terrain height
                worldPoint.y = GetTerrainHeight(x, z);

                // Check if the node is walkable based on physics and terrain weight
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);

                // Check terrain walkability using weightMap
                if (x < terrainGenerator.weightMap.GetLength(0) && z < terrainGenerator.weightMap.GetLength(1))
                {
                    // If weight is too high, mark as unwalkable (too steep or high)
                    if (terrainGenerator.weightMap[x, z] > weightThreshold)
                    {
                        walkable = false;
                    }
                }

                // Get terrain cost directly from weight map
                float terrainCost = 0f;
                if (x < terrainGenerator.weightMap.GetLength(0) && z < terrainGenerator.weightMap.GetLength(1))
                {
                    // Use weight value as terrain cost, multiplied by the factor to make it more significant
                    terrainCost = terrainGenerator.weightMap[x, z] * terrainCostFactor;
                }

                grid[x, z] = new Node(walkable, worldPoint, x, z, terrainCost);
            }
        }
#if UNITY_EDITOR
        UnityEditor.SceneView.RepaintAll();
#endif
    }

    private float GetTerrainHeight(int x, int z)
    {
        // Make sure we don't go out of bounds
        if (x < 0 || x >= terrainGenerator.heightMap.GetLength(0) ||
            z < 0 || z >= terrainGenerator.heightMap.GetLength(1))
        {
            return 0f;
        }

        // Get the height value from the terrain generator
        return terrainGenerator.heightMap[x, z];
    }

    private void AddRocksToGrid()
    {
        foreach (var rock in rockGenerator.SpawnedRocks)
        {
            Vector3 rockPos = rock.transform.position;
            Node node = NodeFromWorldPoint(rockPos);
            if (node != null)
                node.walkable = false;
        }
    }

    public void FindPathToRandomRock()
    {
        if (roverTransform == null)
        {
            Debug.LogError("Rover transform is not assigned!");
            return;
        }

        if (rockGenerator.SpawnedRocks.Count == 0)
        {
            Debug.LogError("No rocks available for pathfinding!");
            return;
        }

        // Update start point to current rover position
        startPoint.position = roverTransform.position;
        Debug.Log($"Rover position: {startPoint.position}");

        // Select a random rock
        int randomIndex = Random.Range(0, rockGenerator.SpawnedRocks.Count);
        GameObject randomRock = rockGenerator.SpawnedRocks[randomIndex];
        Vector3 targetPos = randomRock.transform.position;

        Debug.Log($"Selected rock at position: {targetPos}");

        // Verify the positions are within the grid
        Node startNode = NodeFromWorldPoint(startPoint.position);
        Node targetNode = NodeFromWorldPoint(targetPos);

        if (startNode == null || targetNode == null)
        {
            Debug.LogError("Start or target position is outside the grid!");
            Debug.Log($"Grid size: {gridSizeX}x{gridSizeZ}");
            Debug.Log($"Start node: {(startNode == null ? "null" : "valid")}");
            Debug.Log($"Target node: {(targetNode == null ? "null" : "valid")}");
            return;
        }

        // Find nearest walkable position
        Vector3 walkableTargetPos = FindNearestWalkablePosition(targetPos);
        Debug.Log($"Adjusted target position: {walkableTargetPos}");

        // Request the path
        path = FindPath(startPoint.position, walkableTargetPos);

        if (path != null)
        {
            Debug.Log($"Path found with {path.Count} nodes");
            VisualizePath();
        }
        else
        {
            Debug.LogError("Failed to find path");
            // Additional debugging:
            Debug.Log($"Start walkable: {startNode.walkable}");
            Debug.Log($"Target walkable: {targetNode.walkable}");
            Debug.Log($"Start terrain cost: {startNode.terrainCost}");
            Debug.Log($"Target terrain cost: {targetNode.terrainCost}");
        }
    }

    // Helper method to format the path for debug output
    private string GetPathDebugString(List<Node> path)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("Complete Path:");
        sb.AppendLine("Node #\tPosition\t\t\tGrid Coords\tWalkable\tTerrain Cost");

        for (int i = 0; i < path.Count; i++)
        {
            Node node = path[i];
            sb.AppendLine($"{i}\t{node.worldPosition}\t({node.gridX},{node.gridZ})\t{node.walkable}\t{node.terrainCost:F2}");
        }

        return sb.ToString();
    }
    private Vector3 FindNearestWalkablePosition(Vector3 position)
    {
        // Check if the position itself is walkable
        Node node = NodeFromWorldPoint(position);
        if (node != null && node.walkable)
            return position;

        // Search in expanding rings for a walkable node
        for (int radius = 1; radius < 10; radius++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int z = -radius; z <= radius; z++)
                {
                    // Only check the perimeter
                    if (Mathf.Abs(x) == radius || Mathf.Abs(z) == radius)
                    {
                        int checkX = node.gridX + x;
                        int checkZ = node.gridZ + z;

                        if (checkX >= 0 && checkX < gridSizeX && checkZ >= 0 && checkZ < gridSizeZ)
                        {
                            if (grid[checkX, checkZ].walkable)
                                return grid[checkX, checkZ].worldPosition;
                        }
                    }
                }
            }
        }

        // Fallback to the original position if no walkable position is found
        return position;
    }

    private void VisualizePath()
    {
        // Clear previous path visualization
        if (pathContainer != null)
            Destroy(pathContainer);

        pathContainer = new GameObject("PathVisualization");

        // Create spheres along the path
        foreach (Node node in path)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = node.worldPosition;
            sphere.transform.localScale = Vector3.one * 0.3f;
            sphere.GetComponent<Renderer>().material.color = Color.green;
            sphere.transform.parent = pathContainer.transform;
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        // Convert world position to grid coordinates
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentZ = (worldPosition.z + gridWorldSize.z / 2) / gridWorldSize.z;
        percentX = Mathf.Clamp01(percentX);
        percentZ = Mathf.Clamp01(percentZ);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int z = Mathf.RoundToInt((gridSizeZ - 1) * percentZ);

        if (x >= 0 && x < gridSizeX && z >= 0 && z < gridSizeZ)
            return grid[x, z];

        return null;
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        // Check 8 surrounding nodes
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkZ = node.gridZ + z;

                if (checkX >= 0 && checkX < gridSizeX && checkZ >= 0 && checkZ < gridSizeZ)
                {
                    neighbors.Add(grid[checkX, checkZ]);
                }
            }
        }

        return neighbors;
    }

    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Debug.Log($"Starting pathfinding from {startPos} to {targetPos}");

        Node startNode = NodeFromWorldPoint(startPos);
        Node targetNode = NodeFromWorldPoint(targetPos);

        if (startNode == null)
        {
            Debug.LogError("Start node is null - position might be outside grid");
            return null;
        }
        if (targetNode == null)
        {
            Debug.LogError("Target node is null - position might be outside grid");
            return null;
        }

        Debug.Log($"Start node: Grid({startNode.gridX},{startNode.gridZ}) Walkable:{startNode.walkable}");
        Debug.Log($"Target node: Grid({targetNode.gridX},{targetNode.gridZ}) Walkable:{targetNode.walkable}");

        if (!startNode.walkable)
        {
            Debug.LogWarning("Start position is not walkable!");
        }
        if (!targetNode.walkable)
        {
            Debug.LogWarning("Target position is not walkable!");
        }

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        int iterationCount = 0;
        const int maxIterations = 1000; // Safety limit

        while (openSet.Count > 0 && iterationCount < maxIterations)
        {
            iterationCount++;

            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            if (iterationCount % 100 == 0) // Log every 100 iterations
            {
                Debug.Log($"Iteration {iterationCount}: Current node at ({currentNode.gridX},{currentNode.gridZ}) " +
                         $"G={currentNode.gCost:F1}, H={currentNode.hCost:F1}, F={currentNode.fCost:F1}");
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                Debug.Log($"Path found! Iterations: {iterationCount}");
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                    continue;

                float newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor) + neighbor.terrainCost;

                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        Debug.LogWarning($"Pathfinding failed after {iterationCount} iterations");
        Debug.Log($"Open set count: {openSet.Count}, Closed set count: {closedSet.Count}");

        // Try to find why it failed
        if (iterationCount >= maxIterations)
        {
            Debug.LogError("Pathfinding hit max iterations - possible infinite loop");
        }

        return null;
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    float GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);

        if (dstX > dstZ)
            return 14 * dstZ + 10 * (dstX - dstZ);
        return 14 * dstX + 10 * (dstZ - dstX);
    }

    // Optional: Add UI button to find path to another random rock
    void OnGUI()
    {
        if (rocksGenerated && terrainGenerated)
        {
            if (GUI.Button(new Rect(10, 10, 200, 30), "Find Path to Random Rock"))
            {
                FindPathToRandomRock();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (grid == null) return;

        // Draw walkable areas
        foreach (Node node in grid)
        {
            Gizmos.color = node.walkable ? Color.green : Color.red;
            Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeRadius * 0.9f));
        }

        // Draw start and end positions if available
        if (startPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(startPoint.position, nodeRadius * 1.5f);
        }

        if (targetMarker != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(targetMarker.transform.position - Vector3.up * 2, nodeRadius * 1.5f);
        }
    }
}

// Node class for A* algorithm
public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridZ;
    public float terrainCost;

    public Node parent;

    public float gCost; // Cost from start node
    public float hCost; // Heuristic cost to target

    public float fCost { get { return gCost + hCost; } }

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridZ, float _terrainCost)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridZ = _gridZ;
        terrainCost = _terrainCost;
    }
}