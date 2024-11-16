using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFleeState : AIState
{
    public Transform player;
    public float fleeRange = 10f;

    public AIStateId GetId()
    {
        return AIStateId.Flee;
    }
    public void Enter(AIAgent agent) {
        if(player==null){
            player = agent.playerTransform;
        }
    }
    public void Update(AIAgent agent) {
        Vector3 directionAwayFromPlayer = agent.transform.position - player.position;
        if(directionAwayFromPlayer.magnitude > 100f){
            Debug.Log("Exiting flee");
            agent.stateMachine.ChangeState(AIStateId.Patrol);
        }
        agent.navmeshAgent.nextPosition += directionAwayFromPlayer.normalized;
    }
    public void Exit(AIAgent agent) {
        
    }
}