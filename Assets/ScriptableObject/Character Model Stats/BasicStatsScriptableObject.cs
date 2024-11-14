using System.Security.Principal;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "CharacterStatScriptableObject", menuName = "ScriptableObject/Stats")]
public class BasicStatScriptableObject:ScriptableObject{
    //Basic Stats
    public float Health;
    public float WalkingSpeed;
    public float RunningSpeed;
    public float JumpForce;
    //Special Features
    public GameObject Model; 
    public float DetectionDistance;
    public float AttackDistance;
    //NavMeshAgent
    public float minTime = 3f;//time to refresh NavMesh pathfinder
    public float minDistance = 3f; //distance to enable pathfinder refresh
}