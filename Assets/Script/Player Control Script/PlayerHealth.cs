using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
public class PlayerHealth : MonoBehaviour
{
    public string losingSceneName;
    public float Maxhealth;
    private float currentHealth;
    public Slider healthBar;
    public Animator animator;
    public void Start(){
        currentHealth = Maxhealth;
        healthBar.maxValue = currentHealth;
        healthBar.value = currentHealth;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Debug.Log("Player has died!");
            animator.SetBool("isPlayerAlive", false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(losingSceneName);
            // Load the losing scene
        }
    }
}

