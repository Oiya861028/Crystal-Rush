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
    void Start(){
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update() {
        if(agent.hasPath){
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
        else{
            animator.SetFloat("Speed", 0);
        }

    }
}
