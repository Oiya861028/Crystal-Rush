using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Terrain))]
public class TerrainDecorator : MonoBehaviour
{
    [Header("Terrain Features")]
    [Range(0.1f, 1f)]
    public float heightScale = 0.112f;
    public float noiseScale = 0.01f;

    [Header("Trees")]
    public bool addTrees = true;
    public int numberOfTrees = 1000;
    public GameObject treePrefab;

    [System.Serializable]
    public class PrefabSpawnSettings
    {
        public GameObject prefab;
        public int count = 100;
        [Range(0.5f, 2f)]
        public float minScale = 0.8f;
        [Range(0.5f, 2f)]
        public float maxScale = 1.2f;
        [Range(0f, 90f)]
        public float maxSlopeAngle = 45f; // Maximum terrain slope angle for spawning
        [Range(0f, 1f)]
        public float minHeight = 0f; // Minimum normalized height for spawning (0-1)
        [Range(0f, 1f)]
        public float maxHeight = 1f; // Maximum normalized height for spawning (0-1)
    }

    [Header("Additional Objects")]
    public bool spawnObjects = true;
    public List<PrefabSpawnSettings> prefabSettings = new List<PrefabSpawnSettings>();
    
    private Terrain terrain;
    private TerrainData terrainData;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    void OnEnable()
    {
        
        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
        
        GenerateTerrain();
        if(addTrees) AddTrees();
        if(spawnObjects) SpawnPrefabs();
    }

    void GenerateTerrain()
    {
        Debug.Log("Generating Terrain>>>");
        // Random offsets for Perlin noise to create different terrain each time
        float randomXOffset = Random.Range(0f, 9999f);
        float randomZOffset = Random.Range(0f, 9999f);

        float[,] heights = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];
        for (int x = 0; x < terrainData.heightmapResolution; x++)
        {
            for (int y = 0; y < terrainData.heightmapResolution; y++)
            {
                heights[x, y] = Mathf.PerlinNoise(
                    (x * noiseScale) + randomXOffset, 
                    (y * noiseScale) + randomZOffset
                ) * heightScale;
            }
        }
        terrainData.SetHeights(0, 0, heights);
    }

    void AddTrees()
    {
        if (treePrefab == null) return;

        TreePrototype[] treePrototypes = new TreePrototype[1];
        treePrototypes[0] = new TreePrototype
        {
            prefab = treePrefab
        };
        terrainData.treePrototypes = treePrototypes;

        List<TreeInstance> trees = new List<TreeInstance>();
        Vector3 size = terrainData.size;

        for (int i = 0; i < numberOfTrees; i++)
        {
            float x = Random.value;
            float z = Random.value;
            float y = terrainData.GetHeight(
                Mathf.RoundToInt(x * terrainData.heightmapResolution),
                Mathf.RoundToInt(z * terrainData.heightmapResolution)
            ) / size.y;

            TreeInstance tree = new TreeInstance
            {
                position = new Vector3(x, y, z),
                rotation = Random.Range(0, 360),
                prototypeIndex = 0,
                heightScale = Random.Range(0.8f, 1.2f),
                widthScale = Random.Range(0.8f, 1.2f)
            };
            trees.Add(tree);
        }
        terrainData.treeInstances = trees.ToArray();
    }

    void SpawnPrefabs()
    {
        if (prefabSettings == null || prefabSettings.Count == 0) return;

        // Clear any previously spawned objects
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                #if UNITY_EDITOR
                    DestroyImmediate(obj);
                #else
                    Destroy(obj);
                #endif
            }
        }
        spawnedObjects.Clear();

        Vector3 terrainSize = terrainData.size;
        float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        foreach (PrefabSpawnSettings settings in prefabSettings)
        {
            if (settings.prefab == null) continue;

            for (int i = 0; i < settings.count; i++)
            {
                // Try several times to find a valid position
                for (int attempt = 0; attempt < 10; attempt++)
                {
                    float x = Random.value;
                    float z = Random.value;
                    
                    // Get height and normalized height
                    int heightmapX = Mathf.RoundToInt(x * (terrainData.heightmapResolution - 1));
                    int heightmapZ = Mathf.RoundToInt(z * (terrainData.heightmapResolution - 1));
                    float height = terrainData.GetHeight(heightmapX, heightmapZ);
                    float normalizedHeight = height / terrainSize.y;

                    // Check height constraints
                    if (normalizedHeight < settings.minHeight || normalizedHeight > settings.maxHeight)
                        continue;

                    // Calculate slope
                    Vector3 normal = terrainData.GetInterpolatedNormal(x, z);
                    float slope = Vector3.Angle(normal, Vector3.up);

                    // Check slope constraint
                    if (slope > settings.maxSlopeAngle)
                        continue;

                    // Position is valid, spawn object
                    Vector3 position = new Vector3(
                        x * terrainSize.x-500,
                        height,
                        z * terrainSize.z-500
                    );

                    GameObject obj = Instantiate(
                        settings.prefab,
                        position,
                        Quaternion.Euler(0, Random.Range(0f, 360f), 0),
                        transform
                    );

                    float scale = Random.Range(settings.minScale, settings.maxScale);
                    obj.transform.localScale = Vector3.one * scale;

                    // Align to terrain normal (optional, comment out if not wanted)
                    obj.transform.up = normal;

                    spawnedObjects.Add(obj);
                    break; // Successfully placed object, break attempt loop
                }
            }
        }
    }

    // Optional: Add button to regenerate in editor
    [ContextMenu("Regenerate Terrain")]
    public void RegenerateTerrain()
    {
        // Clear existing decorations and regenerate
        OnEnable();
    }

    // Clean up when script is disabled or destroyed
    void OnDisable()
    {
        // Clean up spawned objects
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                #if UNITY_EDITOR
                    DestroyImmediate(obj);
                #else
                    Destroy(obj);
                #endif
            }
        }
        spawnedObjects.Clear();
    }
}