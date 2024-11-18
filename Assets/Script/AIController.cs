using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    // References
    [SerializeField] private AIStatScriptableObject AIStat; // Stat Data Bank for Weak AI
    [SerializeField] private Weapon WeaponStat;
    
    private Transform player; // Player location
    private NavMeshAgent agent;
    private Animator animator;
    private float lastBulletTime;
    private float playerDistance;

    // Event to notify when the AI is destroyed
    public delegate void AIDestroyedHandler();
    public event AIDestroyedHandler OnAIDestroyed;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
        playerDistance = Vector3.Distance(transform.position, player.position);

        if (playerDistance < AIStat.AttackDistance)
        {
            AttackPlayer();
        }
        else if (playerDistance < AIStat.DetectionDistance)
        {
            ChasePlayer();
        }
    }

    public void setPlayer(Transform p)
    {
        player = p;
    }

    void AttackPlayer()
    {
        if (Time.time - lastBulletTime > WeaponStat.reloadTime)
        {
            lastBulletTime = Time.time;
            GameObject projectile = Instantiate(WeaponStat.projectileModel.model, transform.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.linearVelocity = (player.position - transform.position).normalized * WeaponStat.bulletSpeed;

            Destroy(projectile, 2f); // Destroy the projectile after 2 seconds
        }
    }

    void ChasePlayer()
    {
        // Move toward player
        agent.destination = player.position;
    }

    void OnDestroy()
    {
        // Notify listeners (e.g., AISpawner) when the AI is destroyed
        OnAIDestroyed?.Invoke();
    }
}
