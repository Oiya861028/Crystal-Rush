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

    public Transform getWeaponAttachPoint(bool isLeftHand){
        if(isLeftHand){
            return RecursiveFindChild(CharacterModel.transform, "LeftHand");
        }
        else {
            return RecursiveFindChild(CharacterModel.transform, "RightHand");
        }
    }
    private Transform RecursiveFindChild(Transform parent, string childName) {
         foreach (Transform child in parent)
    {
        if(child.name == childName)
        {
            return child;
        }
        else
        {
            Transform found = RecursiveFindChild(child, childName);
            if (found != null)
            {
                    return found;
            }
        }
    }
    return null;
    }
}