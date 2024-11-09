using UnityEngine;
using UnityEngine.AI;

//The purpose of this script is to spawn multiple AI across the map and equip them with weapon
public class AISpawner : MonoBehaviour
{
    [SerializeField] private BasicStatScriptableObject AIStat;
    [SerializeField] private Weapon WeaponStat;
    [SerializeField] private Transform player;
    [SerializeField, Range(1,10)] private int NumberOfAI;
    // Start is called before the first frame update
    private GameObject[] AIModelInstances;
    
    
    void Awake()
    {
        //Instantiate bots and 
        AIModelInstances = new GameObject[10];
        for(int i = 1; i <= NumberOfAI; i++) {
            Vector3 SpawnOffset = new Vector3(Random.Range(0f, 10f), 0, Random.Range(0f, 10f));
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
