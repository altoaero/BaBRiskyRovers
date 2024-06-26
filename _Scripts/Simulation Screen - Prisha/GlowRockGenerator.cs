using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public GameObject[] rockPrefabs; // Array of different rock prefabs
    public int numberOfRocks = 20; // Number of rocks to spawn
    public Vector2 spawnAreaMin; // Minimum spawn area coordinates
    public Vector2 spawnAreaMax; // Maximum spawn area coordinates

    void Start()
    {
        SpawnRocks();
    }

    void SpawnRocks()
    {
        for (int i = 0; i < numberOfRocks; i++)
        {
            Vector2 spawnPosition = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            GameObject selectedRockPrefab = rockPrefabs[Random.Range(0, rockPrefabs.Length)];
            Instantiate(selectedRockPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
