using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.TextCore.Text;
public class AIController : MonoBehaviour
{
    [SerializeField] private BasicStatScriptableObject AIStat;//Stat Data Bank for Weak AI
    [SerializeField] private Weapon WeaponStat;
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 velocity;
    public float chaseRange = 10f;
    public float attackRange = 5f;
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    void Update()
    { 
        
        animator.SetFloat("Speed", agent.velocity.magnitude);
        playerDistance = Vector3.Distance(transform.position, player.position);
        if (!agent.stateMachine.currentState == AIStateId.Flee) {
            if (Vector3.Distance(transform.position, player.position) < chaseRange){ 
                agent.stateMachine.changeState(AIStateId.Chase);
            }
            else if (Vector3.Distance(transform.position, player.position) < attackRange) {
                agent.stateMachine.changeState(AIStateId.Attack);
            }
            else {
                agent.stateMachine.changeState(AIStateId.Patrol);
            }
        }
    }
    public void setPlayer(Transform p){
        player = p;
    }
    private float lastBulletTime;
    private float sprintCD;
    private float playerDistance;
}
