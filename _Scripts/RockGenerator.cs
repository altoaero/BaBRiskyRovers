using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGenerator : MonoBehaviour
{
    public GameObject[] rockPrefabs; // Array to hold the different rock prefabs
    public int numberOfRocks = 20; // Number of rocks to generate
    public float areaWidth = 50f; // Width of the area where rocks can be placed
    public float areaHeight = 50f; // Height of the area where rocks can be placed
    public float minDistanceBetweenRocks = 2f; // Minimum distance between rocks
    private Vector2 spaceBaseLocation = new Vector2(-0.172f, 0.379f);
    private List<Vector2> rockPositions = new List<Vector2>();

    void Start()
    {
        //GenerateRocks();
    }

    public void changeNumRocks(int n) // used in the Terrain difficulty scene. 
    {
        numberOfRocks = n;
        PlayerPrefs.SetInt("numberOfRocks", numberOfRocks); // sets the number of rocks that will generate in . 
    }

    /*public void AddLocation(Vector2 loc){
        rockPositions.Add(loc);
    }

    public List<Vector2> GetRockPositions(){
        return rockPositions;
    }*/

    void GenerateRocks()
    {

        //List<Vector2> rockPositions = new List<Vector2>();
        rockPositions.Add(spaceBaseLocation);

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
            Instantiate(rockPrefabs[Random.Range(0, rockPrefabs.Length)], newPosition, Quaternion.identity); // selects a random index from the rockprefabs and generates. 
        }
    }
}

