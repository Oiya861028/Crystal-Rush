using UnityEngine;
using System.Collections;
using UnityEngine.VFX;

public class StormSystem : MonoBehaviour
{
    [Header("Storm Settings")]
    [SerializeField] private float initialStormRadius = 500f;
    [SerializeField] private float finalStormRadius = 50f;
    [SerializeField] private float stormShrinkTime = 300f;
    [SerializeField] private float damagePerSecond = 10f;
    [SerializeField] private float damageTickRate = 1f;
    
    [Header("Visual Settings")]
    [SerializeField] private Color stormColor = new Color(0.5f, 0f, 1f, 0.5f); // More opaque purple
    [SerializeField] private Color stormEmissionColor = new Color(0.7f, 0f, 1f, 1f); // Brighter purple for glow
    [SerializeField] private float stormWallHeight = 50f;
    [SerializeField] private float emissionIntensity = 2f;
    [SerializeField] private float scrollSpeed = 2f;
    
    private float currentStormRadius;
    private float shrinkStartTime;
    private bool isStormActive = false;
    private GameObject stormWall;
    private Material stormMaterial;
    private ParticleSystem stormParticles;
    private ParticleSystem topParticles;
    private float textureOffset = 0f;

    private void Start()
    {
        CreateStormWall();
        CreateParticleSystems();
        currentStormRadius = initialStormRadius;
        UpdateStormWall();
        StartStorm();
    }

    private void CreateStormWall()
    {
        stormWall = new GameObject("StormWall");
        stormWall.transform.position = new Vector3(0, stormWallHeight/2, 0);

        // Create a cylinder mesh without caps
        Mesh mesh = new Mesh();
        int segments = 32;
        Vector3[] vertices = new Vector3[(segments + 1) * 2];
        int[] triangles = new int[segments * 6];
        Vector2[] uv = new Vector2[(segments + 1) * 2];

        float deltaAngle = 2 * Mathf.PI / segments;
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * deltaAngle;
            float x = Mathf.Cos(angle);
            float z = Mathf.Sin(angle);

            // Bottom vertex
            vertices[i] = new Vector3(x, -0.5f, z);
            // Top vertex
            vertices[i + segments + 1] = new Vector3(x, 0.5f, z);

            // UVs
            uv[i] = new Vector2((float)i / segments, 0);
            uv[i + segments + 1] = new Vector2((float)i / segments, 1);

            // Create triangles
            if (i < segments)
            {
                int current = i;
                int next = i + 1;
                int currentTop = current + segments + 1;
                int nextTop = next + segments + 1;

                // First triangle
                triangles[i * 6] = current;
                triangles[i * 6 + 1] = nextTop;
                triangles[i * 6 + 2] = currentTop;

                // Second triangle
                triangles[i * 6 + 3] = current;
                triangles[i * 6 + 4] = next;
                triangles[i * 6 + 5] = nextTop;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();

        // Add MeshFilter and MeshRenderer
        MeshFilter meshFilter = stormWall.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer renderer = stormWall.AddComponent<MeshRenderer>();
        
        // Create and setup material for URP with more visual effects
        stormMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        stormMaterial.SetFloat("_Surface", 1); // Transparent
        stormMaterial.SetFloat("_Blend", 0);
        stormMaterial.SetFloat("_Metallic", 1.0f); // More metallic for better effect
        stormMaterial.SetFloat("_Smoothness", 0.8f);
        stormMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        stormMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        stormMaterial.SetInt("_ZWrite", 0);
        stormMaterial.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        stormMaterial.EnableKeyword("_EMISSION");
        stormMaterial.SetColor("_EmissionColor", stormEmissionColor * emissionIntensity);
        stormMaterial.renderQueue = 3000;
        stormMaterial.color = stormColor;
        
        // Set material and disable shadows
        var renderer1 = stormWall.GetComponent<MeshRenderer>();
        renderer1.material = stormMaterial;
        renderer1.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer1.receiveShadows = false;
        stormMaterial.SetFloat("_Cull", 0);
        
        Destroy(stormWall.GetComponent<Collider>());
    }

    private void CreateParticleSystems()
    {
        // Create particle system for the storm edge
        GameObject particleObj = new GameObject("StormParticles");
        particleObj.transform.parent = transform;
        stormParticles = particleObj.AddComponent<ParticleSystem>();
        
        var main = stormParticles.main;
        main.loop = true;
        //main.duration = 1;
        main.startLifetime = 3;
        main.startSpeed = 2;
        main.startSize = 5;
        main.maxParticles = 1000;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        var emission = stormParticles.emission;
        emission.rateOverTime = 200; // Increased particle count

        var shape = stormParticles.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = currentStormRadius;
        shape.arc = 360;
        shape.rotation = new Vector3(90, 0, 0);

        var colorOverLifetime = stormParticles.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { 
                new GradientColorKey(stormEmissionColor, 0.0f),
                new GradientColorKey(stormEmissionColor, 1.0f) 
            },
            new GradientAlphaKey[] { 
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(0.0f, 1.0f) 
            }
        );
        colorOverLifetime.color = gradient;
    }

    private void Update()
    {
        if (!isStormActive) return;

        float progress = (Time.time - shrinkStartTime) / stormShrinkTime;
        progress = Mathf.Clamp01(progress);
        
        currentStormRadius = Mathf.Lerp(initialStormRadius, finalStormRadius, progress);
        UpdateStormWall();
        UpdateParticles();
        
        // Scroll the emission texture
        textureOffset += Time.deltaTime * scrollSpeed;
        if (stormMaterial != null)
        {
            stormMaterial.SetFloat("_EmissionScrollSpeed", textureOffset);
        }
    }

    private void UpdateStormWall()
    {
        if (stormWall != null)
        {
            float diameter = currentStormRadius;
            stormWall.transform.localScale = new Vector3(diameter, stormWallHeight, diameter);
        }
    }

    private void UpdateParticles()
    {
        if (stormParticles != null)
        {
            var shape = stormParticles.shape;
            shape.radius = currentStormRadius;
            stormParticles.transform.position = new Vector3(0, 1, 0);
        }
    }

    private void StartStorm()
    {
        isStormActive = true;
        shrinkStartTime = Time.time;
        StartCoroutine(DamageCheckRoutine());
    }

    private IEnumerator DamageCheckRoutine()
    {
        while (isStormActive)
        {
            CheckPlayersInStorm();
            yield return new WaitForSeconds(damageTickRate);
        }
    }

    private void CheckPlayersInStorm()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (GameObject player in players)
        {
            float distanceFromCenter = Vector3.Distance(
                new Vector3(player.transform.position.x, 0, player.transform.position.z), 
                Vector3.zero
            );
            if (distanceFromCenter > currentStormRadius)
            {
                PlayerHealth health = player.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.TakeDamage(damagePerSecond * damageTickRate);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.5f, 0, 1f, 0.3f);
        Gizmos.DrawWireSphere(Vector3.zero, currentStormRadius);
    }
}