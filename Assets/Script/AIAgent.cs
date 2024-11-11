using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    public FSM stateMachine;
    public AIStateId initialState;
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
