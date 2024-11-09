using System.Security.Principal;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "CharacterStatScriptableObject", menuName = "ScriptableObject/Stats")]
public class BasicStatScriptableObject:ScriptableObject{
    //Basic Stats
    public float Health;
    public float Speed;
    public float SprintSpeed;
    public float JumpForce;
    //Special Features
    public GameObject Model; 
}