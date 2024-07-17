using UnityEngine;
using UnityEngine.UI;

public class DiamondSquareTerrainGenerator : MonoBehaviour
{
    public int mapSize = 2049; // Size of the heightmap grid (must be 2^n + 1, e.g., 65, 129, 257, ...)
    public static int lowResMapSize = 16; // Size of the low res map ((2049-1)/64)
    public int lowResScale = 2048/lowResMapSize ;
    public float roughness = 0.7f; // Roughness factor for terrain generation
    public float heightScale = 10f; // Scale factor for terrain height
    public int terrainCount = 1; // Number of terrains to generate
    public int gridWidth = 3; // Number of terrains per row
    public float spacing = -1.0f; // Spacing between terrains (use negative or zero for minimal spacing)
    public Vector3 terrainScale = new Vector3(1.0f, 1.0f, 1.0f); // Scale for each terrain tile
    public Vector3 mapPositionOffset = new Vector3(500f, 500f, 0f);
    public RawImage rawImage; // Reference to the RawImage component to display the map
    public Transform rover; // Rover to track
    public float rawX; // To reveal mini map
    public float rawY; // To reveal mini map
    public int convertedX; // To reveal mini map
    public int convertedY; // To reveal mini map
    public bool[,] boolArray = new bool[16, 16]; // Bool array for mini map
    public float[,] heightMap;
    public float [,] lowResMap;

    void Start()
    {
        for (int i = 0; i < 16; i++) 
        {
            for (int j = 0; j < 16; j++)
            {
                boolArray[i, j] = false;
            }
        }

        for (int i = 0; i < terrainCount; i++)
        {
            GenerateTerrain(i);
        }
    }

    void Update() {

        // Get the world coordinates of the rover
        Vector3 roverWorldPosition = rover.transform.position;

        // Get just the raw X and Y from the vector
        rawX = (float) roverWorldPosition.x;
        rawY = (float) roverWorldPosition.y;

        // Convert it to 16 x 16
        convertedX = (int) ((rawX + 10.0f) * 0.8f);
        convertedY = (int) ((rawY + 10.0f) * 0.8f);

        // Change the appropriate value in the bool array
        if (convertedX < 16 && convertedY < 16) {
            boolArray[convertedX, convertedY] = true;
            
            if (convertedX > 0) {
                boolArray[convertedX - 1, convertedY] = true;
            }
            if (convertedY > 0) {
                boolArray[convertedX, convertedY - 1] = true;
            }
            if (convertedX < 15) {
                boolArray[convertedX + 1, convertedY] = true;
            }
            if (convertedY < 15) {
                boolArray[convertedX, convertedY + 1] = true;
            }
            if (convertedX > 0 && convertedY > 0) {
                boolArray[convertedX - 1, convertedY - 1] = true;
            }
            if (convertedX > 0 && convertedY < 15) {
                boolArray[convertedX - 1, convertedY + 1] = true;
            }
            if (convertedX < 15 && convertedY < 15) {
                boolArray[convertedX + 1, convertedY + 1] = true;
            }
            if (convertedX < 15 && convertedY > 0) {
                boolArray[convertedX + 1, convertedY - 1] = true;
            }
        }
        
        // Display the lowResMap visually
        DisplayLowResMap(lowResMap, 1, boolArray);
    }

    void GenerateTerrain(int index)
    {
        // Initialize the heightmap
        heightMap = new float[mapSize, mapSize];

        // Initialize the lowResMap
        lowResMap = new float [lowResMapSize, lowResMapSize];

        // Perform diamond-square algorithm to generate terrain
        DiamondSquare(heightMap, 0, 0, mapSize - 1, mapSize - 1, heightScale);

        // Generate the lowResMap
        lowResMapCreate();

        // Display the heightmap visually
        DisplayHeightMap(heightMap, index);
    }

    void DiamondSquare(float[,] heightMap, int x1, int y1, int x2, int y2, float scale)
    {
        if (x2 - x1 <= 1 || y2 - y1 <= 1)
            return;

        for (int y = y1 + (y2 - y1) / 2; y <= y2; y += (y2 - y1) / 2)
        {
            for (int x = x1 + (x2 - x1) / 2; x <= x2; x += (x2 - x1) / 2)
            {
                float avg = (heightMap[x1, y1] + heightMap[x2, y1] + heightMap[x1, y2] + heightMap[x2, y2]) / 4.0f;
                heightMap[x, y] = avg + Random.Range(-1f, 1f) * scale;
            }
        }

        for (int y = y1 + (y2 - y1) / 2; y <= y2; y += (y2 - y1) / 2)
        {
            for (int x = x1 + (x2 - x1) / 2; x <= x2; x += (x2 - x1) / 2)
            {
                if (x == x1 + (x2 - x1) / 2 || y == y1 + (y2 - y1) / 2)
                    continue;

                float sum = 0;
                int count = 0;

                if (x > x1) { sum += heightMap[x - (x2 - x1) / 2, y]; count++; }
                if (x < x2) { sum += heightMap[x + (x2 - x1) / 2, y]; count++; }
                if (y > y1) { sum += heightMap[x, y - (y2 - y1) / 2]; count++; }
                if (y < y2) { sum += heightMap[x, y + (y2 - y1) / 2]; count++; }

                heightMap[x, y] = sum / count + Random.Range(-1f, 1f) * scale;
            }
        }

        DiamondSquare(heightMap, x1, y1, x1 + (x2 - x1) / 2, y1 + (y2 - y1) / 2, scale * roughness);
        DiamondSquare(heightMap, x1 + (x2 - x1) / 2, y1, x2, y1 + (y2 - y1) / 2, scale * roughness);
        DiamondSquare(heightMap, x1, y1 + (y2 - y1) / 2, x1 + (x2 - x1) / 2, y2, scale * roughness);
        DiamondSquare(heightMap, x1 + (x2 - x1) / 2, y1 + (y2 - y1) / 2, x2, y2, scale * roughness);
    }

    void lowResMapCreate() {
        for (int x = 0; x < lowResMapSize; x++) {
            for (int y = 0; y < lowResMapSize; y++) {
                float avg = 0.0f;

                // Multiply both coords by lowResMapSize
                int tempXCoord = lowResScale * x;
                int tempYCoord = lowResScale * y;
               
                // Add up the values
                for (int a = 0; a < lowResScale; a++) {
                    for (int b = 0; b < lowResScale; b++) {
                        avg += heightMap[tempXCoord + a, tempYCoord + b];
                    }
                }
               
                // Divide by lowResScale squared
                avg = avg / (lowResScale*lowResScale);

                // Assign value to lowResMap
                lowResMap[x,y] = avg;
            }
        }
    }

    void DisplayHeightMap(float[,] heightMap, int index)
    {
        Texture2D texture = new Texture2D(mapSize, mapSize, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Bilinear; // Smoothing the texture
        Color[] colors = new Color[mapSize * mapSize];

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                float heightValue = heightMap[x, y];
                // Mars-like color gradient from dark brown to light brown
                colors[y * mapSize + x] = Color.Lerp(new Color(0.3f, 0.15f, 0.05f), new Color(0.8f, 0.4f, 0.2f), heightValue);
            }
        }

        texture.SetPixels(colors);
        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, mapSize, mapSize), Vector2.one * 0.5f);
        GameObject terrain = new GameObject("GeneratedTerrain_" + index);
        SpriteRenderer renderer = terrain.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;

        // Calculate position for the terrain with minimal spacing
        int row = index / gridWidth;
        int col = index % gridWidth;
        float xOffset = col * (mapSize - 1 + spacing);
        float yOffset = row * (mapSize - 1 + spacing);
        terrain.transform.position = new Vector3(xOffset, yOffset, 0);

        // Apply the scaling transform
        terrain.transform.localScale = terrainScale;
    }

    void DisplayLowResMap(float[,] lowResMap, int index, bool[,] boolArray)
    {
        Texture2D texture = new Texture2D(lowResMapSize, lowResMapSize, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Bilinear; // Smoothing the texture
        Color[] colors = new Color[lowResMapSize * lowResMapSize];

        for (int y = 0; y < lowResMapSize; y++)
        {
            for (int x = 0; x < lowResMapSize; x++)
            {
                float heightValue = lowResMap[x, y];
                // Mars-like color gradient from dark brown to light brown
                colors[y * lowResMapSize + x] = Color.Lerp(new Color(0.3f, 0.15f, 0.05f), new Color(0.8f, 0.4f, 0.2f), heightValue);
            }
        }

        

        // Make the color change
        for (int m = 0; m < 16; m++) {
            for (int n = 0; n < 16; n++) {
                if (boolArray[m,n] == true) {
                    colors[n * lowResMapSize + m ] = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                }
            }
        }

        texture.SetPixels(colors);
        texture.Apply();

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, lowResMapSize, lowResMapSize), Vector2.one * 0.5f);
        GameObject terrain = new GameObject("GeneratedTerrain_" + index);
        SpriteRenderer renderer = terrain.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;

        // Calculate position for the terrain with minimal spacing
        int row = index / gridWidth;
        int col = index % gridWidth;
        float xOffset = col * (lowResMapSize - 1 + spacing);
        float yOffset = row * (lowResMapSize - 1 + spacing);
        terrain.transform.position = new Vector3(xOffset, yOffset, 0);

        // Apply the scaling transform
        terrain.transform.localScale = new Vector3(10f, 10f, 10f);

        //Apply the position transform
        terrain.transform.position = mapPositionOffset;

        if (rawImage != null) {
            rawImage.texture = texture;
        }
        Destroy(terrain,0.1f);
    }
}