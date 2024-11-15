using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.TextCore.Text;
//Attach Script to AI prefab, this script will 
public class AIController : MonoBehaviour
{
    [SerializeField] private AIStatScriptableObject AIStat;//Stat Data Bank for Weak AI
    [SerializeField] private Weapon WeaponStat;
    private Transform PlayerTransform;
    private Animator animator;
    private Vector3 velocity;
    private AIAgent agent;
    void Start() {
        
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
}
