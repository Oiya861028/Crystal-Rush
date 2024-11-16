using UnityEngine;
using UnityEngine.AI;
//This script will be attached to the AI prefab
public class AIAgent : MonoBehaviour
{
    public FSM stateMachine;
    public AIStateId initialState;
    public Transform playerTransform;
    public NavMeshAgent navmeshAgent;//component
    public AIStatScriptableObject AIStat;//scriptableObject
    public StormSystem storm;
    
    public void Start()
    {
        //Assigning states
        stateMachine = new FSM(this);
        registerMachineStates();
        stateMachine.ChangeState(initialState);
        //Assigning variables 
        navmeshAgent = GetComponent<NavMeshAgent>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        storm = GameObject.FindAnyObjectByType<StormSystem>();
    }
    public void Update()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        storm = GameObject.FindAnyObjectByType<StormSystem>();
        
        if(storm == null){
            Debug.LogError("Did not find any storm script");
        }
        if(playerTransform==null ){
            Debug.LogError("Did not find any playerTransform script");
        }
        if(playerTransform==null ){
            Debug.LogError("Did not find any navMeshAgent script");
        }
        if(stateMachine==null){
            Debug.Log("StateMachine is null");
        }
        stateMachine.Update();
    }
    private void registerMachineStates(){
        stateMachine.RegisterState(new AIChaseState());
        stateMachine.RegisterState(new AIPatrolState());
        stateMachine.RegisterState(new AIAttackState());
        stateMachine.RegisterState(new AIFleeState());
    }
    public void die(){
        playerAliveCount counter = FindFirstObjectByType<playerAliveCount>();
        counter.updateCounter();
        Destroy(gameObject);
    }
}
