using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health = 10f;
    public Slider healthBar;
    public Animator animator;

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.value = health;

        if (health <= 0)
        {
            Debug.Log("Player has died!");
            animator.SetBool("isPlayerAlive", false);
            Destroy(gameObject);
            // Load the losing scene
            FindFirstObjectByType<SceneController>().LoadLosingScene();
        }
    }
}

