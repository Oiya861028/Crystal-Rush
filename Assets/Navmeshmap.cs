using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshBaker : MonoBehaviour
{
    private NavMeshSurface surface;

    void Awake()
    {
        // Get the NavMeshSurface component
        surface = GetComponent<NavMeshSurface>();
    }

    void Start()
    {
        // Bake the NavMesh when the scene starts
        BakeNavMesh();
    }

    public void BakeNavMesh()
    {
        // Clear any existing NavMesh data
        surface.RemoveData();
        
        // Bake new NavMesh
        surface.BuildNavMesh();
        Debug.Log("NavMesh has been baked for the procedural terrain.");
    }

    // Optional: Call this method whenever you regenerate your terrain
    public void RegenerateNavMesh()
    {
        // Wait for a frame to ensure terrain generation is complete
        StartCoroutine(BakeAfterFrame());
    }

    private System.Collections.IEnumerator BakeAfterFrame()
    {
        // Wait for the end of the frame
        yield return new WaitForEndOfFrame();
        
        // Rebake the NavMesh
        BakeNavMesh();
    }
}