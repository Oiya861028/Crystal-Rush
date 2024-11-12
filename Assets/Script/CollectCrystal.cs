using UnityEngine;

public class CollectCrystal : MonoBehaviour
{
    public int points = 1; // Points to add when collected

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // GameManager gameManager = FindObjectOfType<GameManager>(); // Find the GameManager
            // if (gameManager != null)
            // {
            //     gameManager.AddScore(points); // Add points to the score
            // }
            DestroyImmediate(gameObject); // Remove the treasure
        }
    }
}
