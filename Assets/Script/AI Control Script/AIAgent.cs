using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//This script will be attached to the AI prefab
//Although this ithe controller of AI states, it's more of a middle man that provides all needed components
public class AIAgent : MonoBehaviour
{
    public FSM stateMachine;
    public AIStateId initialState;
    [HideInInspector]
    public Transform playerTransform;
    [HideInInspector]
    public NavMeshAgent navmeshAgent;//component
    public AIStatScriptableObject AIStat;//scriptableObject
    [HideInInspector]
    public Transform MapCenter; //The center of MAP in which the AI will patrol
    public float mapRange = 500; //The range of MAP
    
    void Start()
    {
        //Assigning states
        stateMachine = new FSM(this);
        registerMachineStates();
        stateMachine.ChangeState(initialState);
        //Assigning variables 
        if(navmeshAgent == null){
            navmeshAgent = GetComponent<NavMeshAgent>();
        }   
        if(playerTransform == null) {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if(MapCenter == null) {
            MapCenter = GameObject.FindGameObjectWithTag("MapCenter").transform;
        }
        
    }
    void Update()
    {
        if(stateMachine==null){
            Debug.Log("StateMachine is null");
        }
        stateMachine.Update();
    }
    private void registerMachineStates(){
        stateMachine.RegisterState(new AIChaseState());
        // stateMachine.RegisterState(new AIPatrolState());
        // stateMachine.RegisterState(new AIAttackState());
        // stateMachine.RegisterState(new AIFleeState());
        // stateMachine.RegisterState(new AIDieState());
    }
    public void Die(){
        Debug.Log("Dead");
        Destroy(gameObject);
    }
}
