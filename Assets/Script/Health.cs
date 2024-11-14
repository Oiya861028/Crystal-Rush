using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHealth;
    [HideInInspector]
    public float currentHealth;
    void Start() {
        currentHealth = MaxHealth;
        var rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach(var rigidbody in rigidbodies){
            HitBox hitbox = rigidbody.gameObject.AddComponent<HitBox>();
            hitbox.health = this;
        }
    }
    public void TakeDamage(float amount) {
        currentHealth -= amount;
        if(currentHealth <= 0.0f) {
            die();
        }
    } 
    private void die() {
        Debug.Log("DEAD");
        Destroy(this);
    }
}