using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    public FSM stateMachine;
    public AIStateId initialState;
    public Transform PlayerTransform;
    public UnityEngine.AI.NavMeshAgent navmeshAgent;
    
    public AIAgent(Transform player, UnityEngine.AI.NavMeshAgent agent){
        PlayerTransform = player;
        navmeshAgent = agent;
    }
    void Start()
    {
        stateMachine = new FSM(this);
        stateMachine.RegisterState(new AIPatrolState());
        stateMachine.RegisterState(new AIChaseState());
        stateMachine.RegisterState(new AIAttackState());
        stateMachine.RegisterState(new AIFleeState());
        stateMachine.ChangeState(initialState);
    }
    void Update()
    {
        stateMachine.Update();
    }
}
