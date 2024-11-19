using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPatrolState : AIState
{
    public AIStateId GetId(){
        return AIStateId.Patrol;
    }

    public float range; //radius of sphere
    public StormSystem storm;

    public Vector3 centrePoint; //centre of the area the agent wants to move around in
    //instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area

    public void Enter(AIAgent agent)
    {
        centrePoint = Vector3.zero;//the AI will patrol with the center of map as center
        range = agent.storm.currentStormRadius;
    }

    
    public void Update(AIAgent agent)
    {
        

        float distanceToPlayer = Vector3.Distance(agent.transform.position, agent.playerTransform.position);
        
        if(distanceToPlayer < agent.AIStat.DetectionDistance) {
            Debug.Log("Switching from Patrol to Chase");
            agent.stateMachine.ChangeState(AIStateId.Chase);
        }
        detectOtherAgent(agent);
        //Detect Storm
        float distanceFromCenter = Vector3.Distance(
                new Vector3(agent.transform.position.x, 0, agent.transform.position.z), 
                Vector3.zero
            );
            if (distanceFromCenter > agent.storm.currentStormRadius)
            {
                agent.stateMachine.ChangeState(AIStateId.MoveInward);
            }
        //continue patrol
        if(agent.navmeshAgent.remainingDistance <= agent.navmeshAgent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(centrePoint, range, out point))
            {
                agent.navmeshAgent.SetDestination(point);
            }
        }
    }
    public void Exit(AIAgent agent){

    }
    private void detectOtherAgent(AIAgent agent){
        float sphereRadius = 5.0f; // Radius of the sphere for detection
        float detectionDistance = agent.AIStat.DetectionDistance;
        RaycastHit[] hits;

        // Cast a sphere in all directions
        hits = Physics.SphereCastAll(agent.transform.position, sphereRadius, Vector3.forward, detectionDistance);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.CompareTag("Enemy")) // Assuming other agents have the tag "AIAgent"
            {
                Debug.Log("Detected another AI agent with sphere cast");
                // Implement behavior when another AI agent is detected
                Debug.Log("Switching from Patrol to Chase");
                agent.stateMachine.ChangeState(AIStateId.Chase);
            }
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        randomPoint.y = center.y+20;//limit the height to be 20 units above center of map
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        { 
            //the 10f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}