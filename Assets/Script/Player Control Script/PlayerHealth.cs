using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float Maxhealth;
    private float currentHealth;
    public Slider healthBar;
    public Animator animator;
    public void Start(){
        currentHealth = Maxhealth;
        if(healthBar==null){
            healthBar = GetComponent<Slider>();
            healthBar.maxValue = currentHealth;
        }
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Debug.Log("Player has died!");
            animator.SetBool("isPlayerAlive", false);
            Destroy(gameObject);
            // Load the losing scene
            FindFirstObjectByType<SceneController>().LoadLosingScene();
        }
    }
}

