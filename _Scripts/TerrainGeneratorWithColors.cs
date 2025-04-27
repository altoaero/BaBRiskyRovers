using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86;

public class DiamondSquareTerrainGenerator : MonoBehaviour
{
    // Configuration parameters
    public int mapSize = 2049;
    public static int lowResMapSize = 16;
    public int lowResScale = 2048 / lowResMapSize;
    public float roughness = 0.5f;
    public float heightScale = 1f;
    public int terrainCount = 1;
    public int gridWidth = 3;
    public float spacing = -1.0f;
    public Vector3 terrainScale = new Vector3(1.0f, 1.0f, 1.0f);
    public Vector3 mapPositionOffset = new Vector3(500f, 500f, 0f);
    public RawImage rawImage;
    public Transform rover;
    public float rawX;
    public float rawY;
    public int convertedX;
    public int convertedY;
    public bool[,] boolArray = new bool[16, 16];
    public float[,] heightMap;
    public float[,] lowResMap;
    public float[,] weightMap;
    public bool showWeightedMap = false;
    public RockGenerator rockGenerator;
    public AStarPathfinder astarPathfinder;
    public bool autoGenerateOnStart = false;

    void Start()
    {
        // Initialize bool array for mini-map tracking
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                boolArray[i, j] = false;
            }
        }

        if (autoGenerateOnStart)
        {
            for (int i = 0; i < terrainCount; i++)
            {
                GenerateTerrain(i);
            }
        }
    }

    void Update()
    {
        // Update rover position tracking and mini-map visibility
        if (rover && heightMap != null)
        {
            UpdateRoverPositionTracking();

            if (lowResMap != null)
            {
                DisplayLowResMap(lowResMap, 1, boolArray);
            }
        }
    }

    // Main terrain generation method
    public void GenerateTerrain(int index)
    {
        heightMap = new float[mapSize, mapSize];
        InitializeCornerHeights();
        DiamondSquare(heightMap, 0, 0, mapSize - 1, mapSize - 1, heightScale);
        CalculateWeightsForHeightmap();
        lowResMapCreate();
        DisplayHeightMap(heightMap, index);
    }

    // Creates weight map based on height and slope
    void CalculateWeightsForHeightmap()
    {
        weightMap = new float[mapSize, mapSize];

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                float height = heightMap[x, y];
                float gradient = CalculateGradient(x, y);
                weightMap[x, y] = Mathf.Clamp01(height * 0.7f + gradient * 0.3f);
            }
        }

        SmoothWeightMap();
    }

    // Applies simple box blur to smooth weight map
    void SmoothWeightMap()
    {
        float[,] tempWeightMap = (float[,])weightMap.Clone();

        for (int y = 1; y < mapSize - 1; y++)
        {
            for (int x = 1; x < mapSize - 1; x++)
            {
                weightMap[x, y] = CalculateNeighborhoodAverage(tempWeightMap, x, y);
            }
        }
    }

    // Diamond-square algorithm implementation
    void DiamondSquare(float[,] heightMap, int x1, int y1, int x2, int y2, float scale)
    {
        if (x2 - x1 <= 1 || y2 - y1 <= 1) return;

        PerformDiamondStep(heightMap, x1, y1, x2, y2, scale);
        PerformSquareStep(heightMap, x1, y1, x2, y2, scale);

        float newScale = scale * roughness;
        int halfX = (x2 - x1) / 2;
        int halfY = (y2 - y1) / 2;

        DiamondSquare(heightMap, x1, y1, x1 + halfX, y1 + halfY, newScale);
        DiamondSquare(heightMap, x1 + halfX, y1, x2, y1 + halfY, newScale);
        DiamondSquare(heightMap, x1, y1 + halfY, x1 + halfX, y2, newScale);
        DiamondSquare(heightMap, x1 + halfX, y1 + halfY, x2, y2, newScale);
    }

    // Creates low-resolution version of heightmap
    void lowResMapCreate()
    {
        lowResMap = new float[lowResMapSize, lowResMapSize];

        for (int x = 0; x < lowResMapSize; x++)
        {
            for (int y = 0; y < lowResMapSize; y++)
            {
                lowResMap[x, y] = CalculateLowResCellAverage(x, y);
            }
        }
    }

    // Visualizes the full-resolution heightmap
    void DisplayHeightMap(float[,] heightMap, int index)
    {
        Texture2D texture = CreateHeightmapTexture(heightMap);
        CreateTerrainGameObject(texture, index, mapSize, terrainScale);
    }

    // Visualizes the low-resolution map with rover tracking
    void DisplayLowResMap(float[,] lowResMap, int index, bool[,] boolArray)
    {
        Texture2D texture = CreateLowResTexture(lowResMap);
        ApplyRoverVisibility(texture, boolArray);

        if (rawImage != null)
        {
            rawImage.texture = texture;
        }
    }

    // Helper methods
    private void InitializeCornerHeights()
    {
        heightMap[0, 0] = Random.Range(0f, 1f);
        heightMap[0, mapSize - 1] = Random.Range(0f, 1f);
        heightMap[mapSize - 1, 0] = Random.Range(0f, 1f);
        heightMap[mapSize - 1, mapSize - 1] = Random.Range(0f, 1f);
    }

    private void UpdateRoverPositionTracking()
    {
        Vector3 roverWorldPosition = rover.transform.position;
        rawX = roverWorldPosition.x;
        rawY = roverWorldPosition.y;
        convertedX = (int)((rawX + 10.0f) * 0.8f);
        convertedY = (int)((rawY + 10.0f) * 0.8f);

        if (convertedX < 16 && convertedY < 16)
        {
            UpdateBoolArrayAt(convertedX, convertedY);
        }
    }

    private void UpdateBoolArrayAt(int x, int y)
    {
        boolArray[x, y] = true;
        // Update adjacent cells
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int nx = x + dx;
                int ny = y + dy;
                if (nx >= 0 && nx < 16 && ny >= 0 && ny < 16)
                {
                    boolArray[nx, ny] = true;
                }
            }
        }
    }

    private float CalculateGradient(int x, int y)
    {
        if (x <= 0 || x >= mapSize - 1 || y <= 0 || y >= mapSize - 1)
            return 0f;

        float dx = Mathf.Abs(heightMap[x + 1, y] - heightMap[x - 1, y]) / 2f;
        float dy = Mathf.Abs(heightMap[x, y + 1] - heightMap[x, y - 1]) / 2f;
        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    private float CalculateNeighborhoodAverage(float[,] map, int x, int y)
    {
        float sum = 0f;
        for (int ny = -1; ny <= 1; ny++)
        {
            for (int nx = -1; nx <= 1; nx++)
            {
                sum += map[x + nx, y + ny];
            }
        }
        return sum / 9f;
    }

    private void PerformDiamondStep(float[,] map, int x1, int y1, int x2, int y2, float scale)
    {
        int centerX = x1 + (x2 - x1) / 2;
        int centerY = y1 + (y2 - y1) / 2;
        float avg = (map[x1, y1] + map[x2, y1] + map[x1, y2] + map[x2, y2]) / 4.0f;
        map[centerX, centerY] = avg + Random.Range(-1f, 1f) * scale;
    }

    private void PerformSquareStep(float[,] map, int x1, int y1, int x2, int y2, float scale)
    {
        int centerX = x1 + (x2 - x1) / 2;
        int centerY = y1 + (y2 - y1) / 2;

        for (int y = y1 + (y2 - y1) / 2; y <= y2; y += (y2 - y1) / 2)
        {
            for (int x = x1 + (x2 - x1) / 2; x <= x2; x += (x2 - x1) / 2)
            {
                if (x == centerX && y == centerY) continue;

                float sum = 0;
                int count = 0;

                if (x > x1) { sum += map[x - (x2 - x1) / 2, y]; count++; }
                if (x < x2) { sum += map[x + (x2 - x1) / 2, y]; count++; }
                if (y > y1) { sum += map[x, y - (y2 - y1) / 2]; count++; }
                if (y < y2) { sum += map[x, y + (y2 - y1) / 2]; count++; }

                map[x, y] = sum / count + Random.Range(-1f, 1f) * scale;
            }
        }
    }

    private float CalculateLowResCellAverage(int x, int y)
    {
        float avg = 0.0f;
        int startX = lowResScale * x;
        int startY = lowResScale * y;

        for (int a = 0; a < lowResScale; a++)
        {
            for (int b = 0; b < lowResScale; b++)
            {
                avg += heightMap[startX + a, startY + b];
            }
        }

        return avg / (lowResScale * lowResScale);
    }

    private Texture2D CreateHeightmapTexture(float[,] map)
    {
        Texture2D texture = new Texture2D(mapSize, mapSize, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Bilinear;
        Color[] colors = new Color[mapSize * mapSize];

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                colors[y * mapSize + x] = GetHeightmapColor(x, y);
            }
        }

        texture.SetPixels(colors);
        texture.Apply();
        return texture;
    }

    private Color GetHeightmapColor(int x, int y)
    {
        float heightValue = heightMap[x, y];

        if (showWeightedMap && weightMap != null)
        {
            float weight = weightMap[x, y];
            Color baseColor = Color.Lerp(new Color(0.3f, 0.15f, 0.05f), new Color(0.8f, 0.4f, 0.2f), heightValue);
            return Color.Lerp(baseColor, Color.green, weight * 0.7f);
        }

        return Color.Lerp(new Color(0.3f, 0.15f, 0.05f), new Color(0.8f, 0.4f, 0.2f), heightValue);
    }

    private Texture2D CreateLowResTexture(float[,] map)
    {
        Texture2D texture = new Texture2D(lowResMapSize, lowResMapSize, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Bilinear;
        Color[] colors = new Color[lowResMapSize * lowResMapSize];

        for (int y = 0; y < lowResMapSize; y++)
        {
            for (int x = 0; x < lowResMapSize; x++)
            {
                colors[y * lowResMapSize + x] = GetLowResColor(x, y);
            }
        }

        texture.SetPixels(colors);
        texture.Apply();
        return texture;
    }

    private Color GetLowResColor(int x, int y)
    {
        float heightValue = lowResMap[x, y];
        float avgWeight = CalculateLowResWeightAverage(x, y);

        if (showWeightedMap && weightMap != null)
        {
            Color baseColor = Color.Lerp(new Color(0.3f, 0.15f, 0.05f), new Color(0.8f, 0.4f, 0.2f), heightValue);
            return Color.Lerp(baseColor, Color.green, avgWeight * 0.7f);
        }

        return Color.Lerp(new Color(0.3f, 0.15f, 0.05f), new Color(0.8f, 0.4f, 0.2f), heightValue);
    }

    private float CalculateLowResWeightAverage(int x, int y)
    {
        if (weightMap == null) return 0f;

        float avgWeight = 0f;
        int startX = x * lowResScale;
        int startY = y * lowResScale;

        for (int a = 0; a < lowResScale; a++)
        {
            for (int b = 0; b < lowResScale; b++)
            {
                if (startX + a < mapSize && startY + b < mapSize)
                {
                    avgWeight += weightMap[startX + a, startY + b];
                }
            }
        }

        return avgWeight / (lowResScale * lowResScale);
    }

    private void ApplyRoverVisibility(Texture2D texture, bool[,] visibility)
    {
        Color[] colors = texture.GetPixels();

        for (int m = 0; m < 16; m++)
        {
            for (int n = 0; n < 16; n++)
            {
                if (visibility[m, n])
                {
                    colors[n * lowResMapSize + m] = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                }
            }
        }

        texture.SetPixels(colors);
        texture.Apply();
    }

    private void CreateTerrainGameObject(Texture2D texture, int index, int size, Vector3 scale)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, size, size), Vector2.one * 0.5f);
        GameObject terrain = new GameObject("GeneratedTerrain_" + index);
        SpriteRenderer renderer = terrain.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;

        int row = index / gridWidth;
        int col = index % gridWidth;
        float xOffset = col * (size - 1 + spacing);
        float yOffset = row * (size - 1 + spacing);

        terrain.transform.position = new Vector3(xOffset, yOffset, 0);
        terrain.transform.localScale = scale;
    }

    // Public utility methods
    public float GetWeightAt(int x, int y)
    {
        if (weightMap != null && x >= 0 && x < mapSize && y >= 0 && y < mapSize)
        {
            return weightMap[x, y];
        }
        return 0f;
    }

    public void DisplayWeightMap()
    {
        if (weightMap == null) return;

        Texture2D texture = new Texture2D(mapSize, mapSize, TextureFormat.RGBA32, false);
        Color[] colors = new Color[mapSize * mapSize];

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                float weight = weightMap[x, y];
                colors[y * mapSize + x] = new Color(weight, weight, weight);
            }
        }

        texture.SetPixels(colors);
        texture.Apply();

        GameObject weightVisualizer = CreateVisualizerObject(texture, "WeightMapVisualizer");
        weightVisualizer.transform.position = new Vector3(mapSize * 1.5f, 0f, 0f);
        weightVisualizer.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        Destroy(weightVisualizer, 10f);
    }

    private GameObject CreateVisualizerObject(Texture2D texture, string name)
    {
        GameObject obj = new GameObject(name);
        SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        renderer.sprite = sprite;
        return obj;
    }

    public float[,] ExportWeightMap()
    {
        return weightMap;
    }
}