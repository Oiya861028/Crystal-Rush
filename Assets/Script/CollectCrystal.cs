using UnityEngine;

public class CollectCrystal : MonoBehaviour
{
    public int points = 1; // Points to add when collected
    private Transform playerTransform;
    public float detectRange = 1.0f;
    void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update(){
        float distance = Vector3.Distance(playerTransform.position, transform.position);
        if(distance<detectRange){
            Debug.Log("Destroyed");
            DestroyImmediate(gameObject);
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         // GameManager gameManager = FindObjectOfType<GameManager>(); // Find the GameManager
    //         // if (gameManager != null)
    //         // {
    //         //     gameManager.AddScore(points); // Add points to the score
    //         // }
    //          // Remove the treasure
    //     }
    // }
}
