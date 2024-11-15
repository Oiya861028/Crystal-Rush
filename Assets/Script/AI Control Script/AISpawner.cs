using UnityEngine;
using UnityEngine.AI;

//The purpose of this script is to spawn multiple AI across the map and equip them with weapon
public class AISpawner : MonoBehaviour
{
    [SerializeField] private AIStatScriptableObject AIStat;
    [SerializeField] private Weapon WeaponStat;
    [SerializeField] private Transform player;
    [SerializeField, Range(1,99)] private int NumberOfAI;
    [SerializeField, Range(1,500)] private float offSet;
    // Start is called before the first frame update
    private GameObject[] AIModelInstances;
    
    
    void Start()
    {
        //Instantiate bots and 
        AIModelInstances = new GameObject[10];
        for(int i = 0; i < NumberOfAI; i++) {
            Vector3 SpawnOffset = new Vector3(Random.Range(0f, offSet), 20, Random.Range(0f, offSet));
            AIModelInstances[i] = Instantiate(AIStat.Model, (transform.position + SpawnOffset), Quaternion.identity);
            AIModelInstances[i].GetComponent<AIController>().setPlayer(player);
            //Get Weapon_Attach_Point
            GameObject Weapon = Instantiate(WeaponStat.WeaponModel, getWeaponAttachPoint(i).transform);
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
