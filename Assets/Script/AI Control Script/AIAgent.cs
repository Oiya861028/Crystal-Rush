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
    public Transform playerTransform;
    public NavMeshAgent navmeshAgent;//component
    public BasicStatScriptableObject AIStat;//scriptableObject
    public Transform Mapcenter; //The center of MAP in which the AI will patrol
    public float mapRange; //The range of MAP
    
    void Start()
    {
        //Assigning variables 
        if(navmeshAgent == null){
            navmeshAgent = GetComponent<NavMeshAgent>();
        }   
        if(playerTransform == null) {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        //Assigning states
        stateMachine = new FSM(this);
        registerMachineStates();
        stateMachine.ChangeState(initialState);
    }
    void Update()
    {
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
