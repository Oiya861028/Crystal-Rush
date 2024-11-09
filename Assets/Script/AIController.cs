using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.TextCore.Text;
public class AIController : MonoBehaviour
{
    //References
    
    [SerializeField] private BasicStatScriptableObject AIStat;//Stat Data Bank for Weak AI
    
    [SerializeField] private Transform player; //player location
    
    [SerializeField] private Weapon WeaponStat;
    private CharacterController Controller;
    private NavMeshAgent agent;
    private static float GRAVITY = -9.81f;
    private Vector3 velocity;
    // Start is called before the first frame update
    void OnEnable() {
        Controller = GetComponent<CharacterController>();
    }

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    { 
        // Check if the AI is grounded
        if (!Controller.isGrounded)
        {
            // Apply gravity to the velocity
            velocity.y += GRAVITY * Time.deltaTime;
        }
        else
        {
            // Reset the velocity when grounded
            velocity.y = 0f;
        }
        // Move the object based on the calculated velocity
        Controller.Move(velocity * Time.deltaTime);

        playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance < AIStat.ChaseDistance)
        {
            AttackPlayer();
        }
        else if (playerDistance < AIStat.AttackDistance) {
            ChasePlayer();
        }

    }
    
    //Counter variables
    private float lastBulletTime;
    private float sprintCD;
    private float playerDistance;
    void AttackPlayer()
    {
        Debug.Log(Time.time);
        transform.LookAt(player);
        if(Time.deltaTime-lastBulletTime > WeaponStat.reloadTime){
            GameObject projectile = Instantiate(WeaponStat.projectileModel.model, transform.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = (player.position - transform.position).normalized * WeaponStat.bulletSpeed;

            Destroy(projectile, 2f);  // Destroy the projectile after 2 seconds
        }
        else{
            lastBulletTime+= Time.deltaTime;
        }
    }
    void ChasePlayer(){
        //move toward player
        transform.position = Vector3.MoveTowards(transform.position, player.position, AIStat.WalkingSpeed * Time.deltaTime);
        transform.LookAt(player);
    }
}
