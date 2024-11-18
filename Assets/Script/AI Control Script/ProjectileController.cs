using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float damage;
    private float speed;
    private Vector3 direction;
    private bool hasBeenInitialized = false;

    public void Initialize(Vector3 dir, float projSpeed, float projDamage)
    {
        direction = dir.normalized;
        speed = projSpeed;
        damage = projDamage;
        hasBeenInitialized = true;
        
        transform.forward = direction;
    }

    void FixedUpdate()
    {
        if (!hasBeenInitialized) return;
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Hit "+ other);
        // Ignore collisions with the shooter and other projectiles
        if (other.collider.Equals("Projectile")) return;
        
        // If we hit something, try to damage it
        bool damageDealt = false;

        // Check for player
        if(other.collider.Equals("Player")){
            PlayerHealth playerHealth = other.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                damageDealt = true;
                Debug.Log($"Hit player for {damage} damage");
            }
        }

        // Check for AI
        AIHitBox aiHitBox = other.collider.GetComponent<AIHitBox>();
        if (aiHitBox != null && aiHitBox.health != null)
        {
            aiHitBox.health.TakeDamage(damage);
            damageDealt = true;
            Debug.Log($"Hit AI for {damage} damage");
        }

        // // Alternative way to find AI health
        // AIHealth aiHealth = other.GetComponent<AIHealth>();
        // if (!damageDealt && aiHealth != null)
        // {
        //     aiHealth.TakeDamage(damage);
        //     damageDealt = true;
        //     Debug.Log($"Hit AI directly for {damage} damage");
        // }

        // Destroy the projectile when it hits anything
        Destroy(gameObject);
    }
}