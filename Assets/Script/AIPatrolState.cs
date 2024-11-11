using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AIPatrolState : AIState
{
    AIStateId GetId(){
        return AIStateId.Patrol;
    }
    public List<Transform> waypoints;
    public float speed = 2f;
    public float detectionDistance = 2f;
    public float rotationSpeed = 2f;

    private int currentWaypoint = 0;
    private Vector3 targetPosition;

    void Start(){
        if (waypoints.Count > 0){
            targetPosition =  waypoints[currentWaypoint].position;
        }
    }
    void Update(){
        MoveTowardsTarget();
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, forward, out hit, detectionDistance)){
            if (hit.collider.CompareTag("Wall")){
                SelectNewWaypoint();
            }
        }

        if (Vector3.Distance(transform.position, targetPosition) < 0.5f){
            SelectNextWaypoint();
        }
    }
    void MoveTowardsTarget(){
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    void SelectNextWaypoint(){
        if (waypoints.Count == 0)
            return;

        currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
        targetPosition = waypoints[currentWaypoint].position;
    }
    void SelectNewWaypoint(){
        if (waypoints.Count == 0)
            return;
        int newWaypoint = currentWaypoint;
        while (newWaypoint == currentWaypoint){
            newWaypoint = Random.Range(0, waypoints.Count);
        }
        currentWaypoint = newWaypoint;
        targetPosition = waypoints[currentWaypoint].position;
    }
}