using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFleeState : AIState
{
    public Transform player;
    public float fleeRange = 10f;
    public float speed = 3f;

    public AIStateId GetId()
    {
        return AIStateId.Flee;
    }

    public void Update(AIAgent agent)
    {
        Vector3 directionAwayFromPlayer = agent.transform.position - player.position;
        directionAwayFromPlayer.Normalize();
        agent.transform.position += directionAwayFromPlayer * speed * Time.deltaTime;
    }
}