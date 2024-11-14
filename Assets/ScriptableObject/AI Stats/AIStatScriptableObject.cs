using System.Security.Principal;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "AIStatScriptableObject", menuName = "ScriptableObject/AIStats")]
public class AIStatScriptableObject:BasicStatScriptableObject{
    public float minTime = 3f;//time to refresh NavMesh pathfinder
}