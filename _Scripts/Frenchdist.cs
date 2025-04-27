using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Frenchdist 
{
   
    public static List<Vector2> GeneratePoints(float radius, Vector2 sampleRegionSize, int rejection)
    {
        float cellSize = radius / Mathf.Sqrt(2);
        Debug.Log("Got called");
        int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x/cellSize),Mathf.CeilToInt(sampleRegionSize.y/cellSize)];
        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawnpoints = new List<Vector2>();

        spawnpoints.Add(sampleRegionSize / 2);
        while (spawnpoints.Count > 0 && points.Count < 27)
        {
            bool Accpeted = false;
            int spawnIndex = Random.Range(0, spawnpoints.Count);
            Vector2 spawnCenter = spawnpoints[spawnIndex];

            for (int i = 0; i < rejection; i++)
            {
                float angle = Random.value * Mathf.PI * 2;
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 canidate = spawnCenter + dir * Random.Range(radius, radius * 2);

                if (IsValid(canidate,sampleRegionSize,cellSize,points, grid, radius))
                {
                    points.Add(canidate);
                    spawnpoints.Add(canidate);
                    grid[(int)(canidate.x/cellSize),(int)(canidate.y/cellSize)] = points.Count;
                    Accpeted = true;
                    break;

                }
            }
            if (!Accpeted)
            {
                spawnpoints.RemoveAt(spawnIndex);
            }

        }
        return points;
    }

    
    static bool IsValid(Vector2 canidate, Vector2 sampleRegionSize,float cellSize, List<Vector2> points, int[,] grid,float radius)
    {
        if (canidate.x >= 0 && canidate.x < sampleRegionSize.x && canidate.y >= 0 && canidate.y < sampleRegionSize.y) 
        {
            int cellx = (int)(canidate.x / cellSize);
            int celly = (int)(canidate.y / cellSize);

            int searchstartx = Mathf.Max(0, cellx - 2);
            int searchendx = Mathf.Min(cellx + 2, grid.GetLength(0) - 1);

            int searchstarty = Mathf.Max(0, celly - 2);
            int searchendy = Mathf.Min(celly + 2, grid.GetLength(1) - 1);

            for (int x = searchstartx; x < searchendx; x++) {
                for (int y = searchstarty; y < searchendy; y++) { 
                    
                    int pointIndex = grid[x, y]-1;

                    if(pointIndex != -1)
                    {
                        float sqrdst = (canidate - points[pointIndex]).sqrMagnitude;
                        if(sqrdst > radius)
                        {
                            return false; 
                        }
                    }
                }
            }
            return true;
        }
        return false;
    }


}
