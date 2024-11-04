using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 10f;
    public float speed = 3f;

    void Update() {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < chaseRange){
            Chase();
        }
    }

    void Chase(){
        // Move towards the player
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
}

