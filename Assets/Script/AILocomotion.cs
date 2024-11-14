using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;
public class AILocomotion : MonoBehaviour
{
    NavMeshAgent agent; //agent in component
    private Transform playerTransform; //player in Scene
    public Animator animator; //animator in component
    public float maxTime = 1.0f; //Time to refresh agent generating new path
    float timer = 0.0f;
    public float minDistance = 1.0f; //min distance between the player and agent to generate path
    void Start(){
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update() {
        if(timer>=maxTime){
            if((playerTransform.position - agent.destination).magnitude > minDistance)
                agent.destination = playerTransform.position;
            timer = 0.0f;
        } else{
            timer+=Time.deltaTime;
        }
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
