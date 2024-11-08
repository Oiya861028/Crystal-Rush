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

    public GameObject getWeaponAttachPoint(bool isLeftHand){
        if(isLeftHand){
            return RecursiveFindChild(CharacterModel, "LeftHand");
        }
        else {
            return RecursiveFindChild(CharacterModel, "RightHand");
        }
    }
    private GameObject RecursiveFindChild(GameObject parent, string childName) {
        foreach (Transform child in parent.transform)
        {
            if(child.name == childName)
            {
                return child.gameObject;
            }
            else
            {
                GameObject found = RecursiveFindChild(child.gameObject, childName);
                if (found != null)
                {
                        return found;
                }
            }
        }
        return null;
    }
}