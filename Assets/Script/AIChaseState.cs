using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AIChaseState : AIState
{
    public Transform player;
    public float chaseRange = 10f;
    public float speed = 3f;
    AIStateId GetId(){
        return AIStateId.Chase;
    }
    void Update(AIAgent agent){
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
}
