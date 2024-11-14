using System.Security.Principal;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "CharacterStatScriptableObject", menuName = "ScriptableObject/AIStats")]
public class AIStatScriptableObject:ScriptableObject,BasicStatScriptableObject{
    public float minTime = 3f;//time to refresh NavMesh pathfinder
}