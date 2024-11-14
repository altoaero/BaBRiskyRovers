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
    private Vector2 spaceBaseLocation = new Vector2(-0.172f, 0.379f);
    //private RockGenerator generator;

    void Start()
    {
        GenerateRocks();
    }
    

    void GenerateRocks()
    {
        //numberOfRocks = PlayerPrefs.GetInt("numberOfRocks");
        List<Vector2> rockPositions = new List<Vector2>();
        rockPositions.Add(spaceBaseLocation);

        for (int i = 0; i < numberOfRocks; i++)
        {
            Vector2 newPosition;
            bool validPosition; // checks if you can place rocks in this spot (doesn't collide with any other special rocks. )

            do
            {
                validPosition = true;
                newPosition = new Vector2(
                    Random.Range(-areaWidth / 2, areaWidth / 2),
                    Random.Range(-areaHeight / 2, areaHeight / 2)
                );
                
                foreach (Vector2 position in rockPositions) // loops through each rock position that is already used.
                {
                    if (Vector2.Distance(position, newPosition) < minDistanceBetweenRocks) // checks if the minDistance is more or less. 
                    {
                        validPosition = false;
                        break;
                    }
                }
            } while (!validPosition);

            rockPositions.Add(newPosition); // makes sure that no other rocks can generate in the same spot. 
            Instantiate(rockPrefabs[i], newPosition, Quaternion.identity); // same as the rock generator script, however, it only instantiates each one once (rockPrefabs[i]).
        }
    }
}


