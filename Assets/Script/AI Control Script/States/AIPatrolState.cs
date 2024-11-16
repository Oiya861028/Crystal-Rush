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

    public Transform centrePoint; //centre of the area the agent wants to move around in
    //instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area

    public void Enter(AIAgent agent)
    {
        centrePoint = agent.MapCenter;//the AI will patrol with the center of map as center
    }

    
    public void Update(AIAgent agent)
    {
        if(agent.navmeshAgent.remainingDistance <= agent.navmeshAgent.stoppingDistance) //done with path
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                agent.navmeshAgent.SetDestination(point);
            }
        }
        float distance = (agent.playerTransform.position - agent.navmeshAgent.destination).magnitude;
        if(distance <agent.AIStat.AttackDistance){
             agent.stateMachine.ChangeState(AIStateId.Attack);
        }
        else if(distance < agent.AIStat.DetectionDistance){
            agent.stateMachine.ChangeState(AIStateId.Chase);
        }

    }
    public void Exit(AIAgent agent){

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