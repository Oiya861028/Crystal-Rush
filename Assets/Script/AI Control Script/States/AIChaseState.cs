using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AIChaseState : AIState
{
    public Transform playerTransform;
    BasicStatScriptableObject AIStat;
    float timer = 0.0f;
    public AIStateId GetId(){
        return AIStateId.Chase;
    }
    public void Enter(AIAgent agent) {
        if(playerTransform == null) {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            Debug.Log("Assigning playerTransform in AIChaseState");
        }
        if(AIStat == null) {
            AIStat = agent.AIStat;
        }
    }
    public void Update(AIAgent agent){
        if(!agent.enabled) {
            return;
        }
        if(agent.navmeshAgent.destination == null){
            agent.navmeshAgent.destination = playerTransform.position;
        }
        if(timer>=AIStat.minTime){
            if((playerTransform.position - agent.navmeshAgent.destination).magnitude > AIStat.minDistance)
                agent.navmeshAgent.destination = playerTransform.position;
            timer = 0.0f;
        } else{
            timer+=Time.deltaTime;
        }
    }
    public void Exit(AIAgent agent) {

    }
        
    
}
