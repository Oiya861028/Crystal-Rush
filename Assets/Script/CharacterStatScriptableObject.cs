using System.Security.Principal;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "CharacterStatScriptableObject", menuName = "ScriptableObject/CharacterData")]
public class CharacterStatScriptableObject:ScriptableObject{
    //Basic Stats
    public float Health;
    public float Speed;
    public float SprintSpeed;
    public float JumpForce;
    //Special Features
    public GameObject CharacterModel; 

    public Transform getWeaponAttachPoint(){
        return CharacterModel.transform.Find("Weapon_Attachment_Point");
    }
}