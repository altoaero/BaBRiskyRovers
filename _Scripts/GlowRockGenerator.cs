using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public GameObject[] rockPrefabs; // Array of different rock prefabs
    public int numberOfRocks = 20; // Number of rocks to spawn
    public Vector2 spawnAreaMin; // Minimum spawn area coordinates
    public Vector2 spawnAreaMax; // Maximum spawn area coordinates

    void Start()
    {
        SpawnRocks(); // Call the function to spawn rocks when the game starts
    }

    void SpawnRocks()
    {
    // Loop to spawn the specified number of rocks
        for (int i = 0; i < numberOfRocks; i++)
        {
        // Calculate a random spawn position within the specified area
            Vector2 spawnPosition = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );
// Choose a random rock prefab from the array
            GameObject selectedRockPrefab = rockPrefabs[Random.Range(0, rockPrefabs.Length)];
            // Instantiate the selected rock prefab at the calculated spawn position with no rotation
            Instantiate(selectedRockPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
