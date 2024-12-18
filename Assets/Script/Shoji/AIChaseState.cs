using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AIChaseState : AIState
{
    public Transform player;
    public float chaseRange = 10f;
    public float speed = 3f;
    public AIStateId GetId(){
        return AIStateId.Chase;
    }
    public void Update(AIAgent agent){
        agent.transform.position = Vector3.MoveTowards(agent.transform.position, player.position, speed * Time.deltaTime);
    }
}
