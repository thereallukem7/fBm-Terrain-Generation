using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private int octaves = 4;
    [SerializeField] private float frequency = 3f;
    [SerializeField] private float amplitude = 0.1f;
    [SerializeField] private float lacunarity = 2f;
    [SerializeField] private float persistence = 0.5f;
    [SerializeField] private int seed = 12345;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateTerrain();
    }
    
    [ContextMenu("Generate Terrain")]
    void GenerateTerrain()
    {
        Terrain terrain = GetComponent<Terrain>();
        TerrainData data = terrain.terrainData;

        //Seed creation
        System.Random prng = new System.Random(seed);
        Vector2 noiseOffset = new Vector2(prng.Next(-10000, 10000), prng.Next(-10000, 10000));

        int resolution = data.heightmapResolution;
        float[,] heights = new float[resolution, resolution];
        //heights[0,0] = 0.5f;
        for(int z = 0; z<resolution; z++){
            for(int x = 0; x<resolution; x++){
                float u = x / (float)(resolution - 1);
                float v = z / (float)(resolution - 1);
                float currentFrequency = frequency;
                float currentAmplitude = amplitude;
                float height = 0f;
                float totalWeight = 0f;

                for(int octave = 0; octave < octaves; octave++){
                    float noiseValue = Mathf.PerlinNoise((u + noiseOffset.x) * currentFrequency, (v + noiseOffset.y) * currentFrequency);
                    height += Mathf.Clamp01(noiseValue) * currentAmplitude;
                    totalWeight += currentAmplitude;

                    currentFrequency *= lacunarity;
                    currentAmplitude *= persistence;
                }
                if (totalWeight > 0)
                {
                    height /= totalWeight;
                }

                heights[x, z] = height; 
            }
        }
        data.SetHeights(0, 0, heights);
    }
    [ContextMenu("Reset Terrain")]
    void resetTerrain()
    {
        Terrain terrain = GetComponent<Terrain>();
        TerrainData data = terrain.terrainData;
        int resolution = data.heightmapResolution;
        float[,] heights = new float[resolution, resolution];
        for(int z = 0; z<resolution; z++){
            for(int x = 0; x<resolution; x++){
                heights[x, z] = 0f; 
            }
        }
        data.SetHeights(0, 0, heights);
    }
    // Update is called once per frame

    void Update()
    {
        
    }
}
