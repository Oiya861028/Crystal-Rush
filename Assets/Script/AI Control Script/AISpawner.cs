using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement; // Required for scene management

public class AISpawner : MonoBehaviour
{
    [SerializeField] private AIStatScriptableObject AIStat;
    [SerializeField] private Weapon WeaponStat;
    [SerializeField, Range(1, 99)] private int NumberOfAI;
    [SerializeField, Range(1, 500)] private float offSet;
    [SerializeField, Range(0, 10)] private float heightAdjust;

    private GameObject[] AIModelInstances;

    // Start is called before the first frame update
    public void Start()
    {
        AIModelInstances = new GameObject[NumberOfAI];
        for (int i = 0; i < NumberOfAI; i++)
        {
            Vector3 SpawnOffset = new Vector3(Random.Range(-offSet, offSet), 100, Random.Range(-offSet, offSet));
            if (Physics.Raycast(SpawnOffset, Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                SpawnOffset.y = hit.point.y + heightAdjust;
            }
            AIModelInstances[i] = Instantiate(AIStat.Model, SpawnOffset, Quaternion.identity, transform);

            // Assign weapon to AI (left hand or other attach point)
            GameObject attachPoint = GetWeaponAttachPoint(i);
            if (attachPoint != null && WeaponStat != null)
            {
                Instantiate(WeaponStat.WeaponModel, attachPoint.transform.position, attachPoint.transform.rotation, attachPoint.transform);
            }
        }
    }

    private void Update()
    {
        // Check if all AI have been destroyed
        bool allDestroyed = true;
        foreach (GameObject ai in AIModelInstances)
        {
            if (ai != null)
            {
                allDestroyed = false;
                break;
            }
        }

        if (allDestroyed)
        {
            // Load the WinningScene
            SceneManager.LoadScene("WinningScene");
        }
    }

    private GameObject GetWeaponAttachPoint(int AINum)
    {
        return RecursiveFindChild(AIModelInstances[AINum], "LeftHand");
    }

    private GameObject RecursiveFindChild(GameObject parent, string childName)
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
}
