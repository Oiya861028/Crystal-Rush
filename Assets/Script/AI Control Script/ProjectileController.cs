using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float damage;
    private float speed;
    private Vector3 direction;
    private bool hasBeenInitialized = false;
    private Vector3 lastPosition;

    public void Initialize(Vector3 dir, float projSpeed, float projDamage)
    {
        direction = dir.normalized;
        speed = projSpeed;
        damage = projDamage;
        hasBeenInitialized = true;
        lastPosition = transform.position;
        transform.forward = direction;
    }

    void FixedUpdate()
    {
        if (!hasBeenInitialized) return;

        Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;
        
        // Cast a ray from last position to new position
        float distance = Vector3.Distance(lastPosition, newPosition);
        Ray ray = new Ray(lastPosition, direction);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, distance))
        {
            HandleHit(hit);
            Destroy(gameObject);
            return;
        }
        
        // Update position if no hit
        transform.position = newPosition;
        lastPosition = newPosition;
    }

    void HandleHit(RaycastHit hit)
    {
        // Ignore other projectiles
        if (hit.collider.CompareTag("Projectile")) return;
        
        Debug.Log($"Projectile hit: {hit.collider.gameObject.name} with tag: {hit.collider.tag}");

        // Check for player
        if(hit.collider.CompareTag("Player"))
        {
            PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"Hit player for {damage} damage");
            }
            else
            {
                Debug.LogWarning("Player hit but no PlayerHealth component found!");
            }
        }
        // Check for AI
        else if(hit.collider.CompareTag("Enemy"))
        {
            AIHitBox aiHitBox = hit.collider.GetComponent<AIHitBox>();
            if (aiHitBox != null && aiHitBox.health != null)
            {
                aiHitBox.health.TakeDamage(damage);
                Debug.Log($"Hit AI for {damage} damage");
            }
            else
            {
                Debug.LogWarning("Enemy hit but no AIHitBox/health component found!");
            }
        }
    }
}