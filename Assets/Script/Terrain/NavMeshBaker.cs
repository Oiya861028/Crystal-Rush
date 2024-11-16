using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshBaker : MonoBehaviour
{
    NavMeshSurface navMeshSurface;
    void Start()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
        // Set the layer mask to exclude the "IgnoreNavMesh" layer
        int ignoreLayer = LayerMask.NameToLayer("IgnoreNavMesh");
        navMeshSurface.layerMask = ~(1 << ignoreLayer);
        navMeshSurface.BuildNavMesh();
    }

}
