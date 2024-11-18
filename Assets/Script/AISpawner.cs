using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using UnityEngine.AI;

public class AISpawner : MonoBehaviour
{
    [SerializeField] private BasicStatScriptableObject AIStat;
    [SerializeField] private Weapon WeaponStat;
    [SerializeField] private Transform player;
    [SerializeField, Range(1, 10)] private int NumberOfAI;
    [SerializeField, Range(1, 100)] private float offSet;

    private GameObject[] AIModelInstances;
    private int currentAI; // Tracks the number of active AI

    void Awake()
    {
        // Instantiate bots
        AIModelInstances = new GameObject[NumberOfAI];
        currentAI = NumberOfAI; // Initialize the active AI count

        for (int i = 0; i < NumberOfAI; i++)
        {
            Vector3 SpawnOffset = new Vector3(Random.Range(0f, offSet), 0, Random.Range(0f, offSet));
            AIModelInstances[i] = Instantiate(AIStat.Model, (transform.position + SpawnOffset), Quaternion.identity);
            AIModelInstances[i].GetComponent<AIController>().setPlayer(player);

            // Attach weapon
            GameObject weapon = Instantiate(WeaponStat.WeaponModel, getWeaponAttachPoint(i).transform);

            // Subscribe to AI destruction event
            AIModelInstances[i].GetComponent<AIController>().OnAIDestroyed += HandleAIDestroyed;
        }
    }

    GameObject getWeaponAttachPoint(int AINum)
    {
        return RecursiveFindChild(AIModelInstances[AINum], "LeftHand");
    }

    GameObject RecursiveFindChild(GameObject parent, string childName)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.name == childName)
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

    private void HandleAIDestroyed()
    {
        currentAI--;

        if (currentAI <= 0)
        {
            DisplayWinningScene();
        }
    }

    private void DisplayWinningScene()
    {
        SceneManager.LoadScene("WinningScene");
    }
}
