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
    private NavMeshAgent navAgent;
    private Animator animator;
    private Vector3 velocity;
    private AIAgent agent;
    public float chaseRange = 10f;
    public float attackRange = 5f;
    void Start() {
        agent = GetComponent<AIAgent>();
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    { 
        
        animator.SetFloat("Speed", navAgent.velocity.magnitude);
        playerDistance = Vector3.Distance(transform.position, player.position);
        if (agent.stateMachine.currentState != AIStateId.Flee) {
            if (Vector3.Distance(transform.position, player.position) < chaseRange){ 
                Debug.Log("Chasing Mode");
                agent.stateMachine.ChangeState(AIStateId.Chase);
            }
            else if (Vector3.Distance(transform.position, player.position) < attackRange) {
                Debug.Log("Attack Mode");
                agent.stateMachine.ChangeState(AIStateId.Attack);
            }
            else {
                Debug.Log("Patrol Mode");
                agent.stateMachine.ChangeState(AIStateId.Patrol);
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
