using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpecialGenerator : MonoBehaviour
{
    public GameObject[] rockPrefabs; // Array to hold the different rock prefabs
    public int numberOfRocks = 3; // Number of rocks to generate
    public float areaWidth = 20f; // Width of the area where rocks can be placed
    public float areaHeight = 20f; // Height of the area where rocks can be placed
    public float minDistanceBetweenRocks = 2f; // Minimum distance between rocks

    void Start()
    {
        GenerateRocks();
    }
    public void changeNumRocks(int n)
    {
        numberOfRocks = n;
        PlayerPrefs.SetInt("numberOfRocks", numberOfRocks);
    }

    void GenerateRocks()
    {
        //numberOfRocks = PlayerPrefs.GetInt("numberOfRocks");
        List<Vector2> rockPositions = new List<Vector2>();

        for (int i = 0; i < numberOfRocks; i++)
        {
            Vector2 newPosition;
            bool validPosition;

            do
            {
                validPosition = true;
                newPosition = new Vector2(
                    Random.Range(-areaWidth / 2, areaWidth / 2),
                    Random.Range(-areaHeight / 2, areaHeight / 2)
                );

                foreach (Vector2 position in rockPositions)
                {
                    if (Vector2.Distance(position, newPosition) < minDistanceBetweenRocks)
                    {
                        validPosition = false;
                        break;
                    }
                }
            } while (!validPosition);

            rockPositions.Add(newPosition);
            Instantiate(rockPrefabs[i], newPosition, Quaternion.identity);
        }
    }
}

