using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;

//The purpose of this script is to spawn multiple AI across the map and equip them with weapon
public class AISpawner : MonoBehaviour
{
    [SerializeField] private AIStatScriptableObject AIStat;
    [SerializeField] private Weapon WeaponStat;
    [SerializeField, Range(1,99)] private int NumberOfAI;
    [SerializeField, Range(1,500)] private float offSet;
    [SerializeField, Range(0,10)] private float heightAdjust;

    // Start is called before the first frame update
    private GameObject[] AIModelInstances;
    
    
    public void Start()
    {
        //Instantiate bots and 
        AIModelInstances = new GameObject[NumberOfAI];
        for(int i = 0; i < NumberOfAI; i++) {
            Vector3 SpawnOffset = new Vector3(Random.Range(-offSet, offSet), 100, Random.Range(-offSet, offSet));
            if(Physics.Raycast(SpawnOffset, Vector3.down, out RaycastHit hit, Mathf.Infinity)){
                SpawnOffset.y = hit.point.y + heightAdjust;
            }        
            AIModelInstances[i] = Instantiate(AIStat.Model, SpawnOffset, Quaternion.identity, transform);
            //Get Weapon_Attach_Point

        }
        
    }

    GameObject getWeaponAttachPoint(int AINum){
        return RecursiveFindChild(AIModelInstances[AINum], "LeftHand"); 
    }
    GameObject RecursiveFindChild(GameObject parent, string childName) {
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
