using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.TextCore.Text;
public class AIController : MonoBehaviour
{
    //References
    
    [SerializeField] private BasicStatScriptableObject AIStat;//Stat Data Bank for Weak AI
    
    
    [SerializeField] private Weapon WeaponStat;
    private Transform player; //player location
    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 velocity;
    // Start is called before the first frame update

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    { 
        
        animator.SetFloat("Speed", agent.velocity.magnitude);
        playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance < AIStat.DetectionDistance)
        {
            AttackPlayer();
        }
        else if (playerDistance < AIStat.AttackDistance) {
            ChasePlayer();
        }


    }
    public void setPlayer(Transform p){
        player = p;
    }
    //Counter variables
    private float lastBulletTime;
    private float sprintCD;
    private float playerDistance;
    void AttackPlayer()
    {
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
        agent.destination = player.position;
    }

}
