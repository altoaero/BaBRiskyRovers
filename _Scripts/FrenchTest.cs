using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FrenchTest : MonoBehaviour
{
    // Start is called before the first frame update
    public float radius = 27f;
    public Vector2 regionSize = new Vector2(27, 27);
    public int rejection = 30;
    public GameObject[] rockPrefabs; // Array to hold the different rock prefabs 
    List<Vector2> points;

    private void Start()
    {
        //points = Frenchdist.GeneratePoints(radius, regionSize, rejection);
    }
    void OnValidate()
    {
        //points = Frenchdist.GeneratePoints(radius, regionSize, rejection);
    }

    void OnSuccess()
    {
        if(points != null)
        {
            foreach (Vector2 point in points)
            {
                Instantiate(rockPrefabs[Random.Range(0, rockPrefabs.Length)],point, Quaternion.identity);
                Debug.Log("A new rock was made!");
            }
        }
    }


   
}
