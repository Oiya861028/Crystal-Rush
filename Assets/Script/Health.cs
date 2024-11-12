using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHealth;
    [HideInInspector]
    public float currentHealth;
    void Start() {
        currentHealth = MaxHealth;
    }
    public void TakeDamage(float amount) {
        currentHealth -= amount;
        if(currentHealth <= 0.0f) {
            die();
        }
    } 
    private void die() {
        
    }
}