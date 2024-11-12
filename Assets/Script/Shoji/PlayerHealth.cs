using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health = 10f;
    public Slider healthBar;

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.value = health;

        if (health <= 0)
        {
            Debug.Log("Player has died!");
            // Implement player death logic, like restarting the game or showing a game over screen.
        }
    }
}

