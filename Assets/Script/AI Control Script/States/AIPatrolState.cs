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
    public Vector3 centrePoint;

    public void Enter(AIAgent agent)
    {
        centrePoint = Vector3.zero;
        range = agent.storm.currentStormRadius;
    }

    public void Update(AIAgent agent)
    {
        // Check for nearby targets (both player and other AIs)
        CheckForTargets(agent);

        if(agent.navmeshAgent.remainingDistance <= agent.navmeshAgent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(centrePoint, range, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                agent.navmeshAgent.SetDestination(point);
            }
        }
    }

    private void CheckForTargets(AIAgent agent)
    {
        // First check for any nearby AIs or player
        Collider[] hitColliders = Physics.OverlapSphere(agent.transform.position, agent.AIStat.DetectionDistance);
        float closestDistance = float.MaxValue;
        Transform closestTarget = null;

        foreach (var hitCollider in hitColliders)
        {
            // Skip self
            if (hitCollider.gameObject == agent.gameObject) continue;

            // Check if it's a valid target (player or enemy)
            if (hitCollider.CompareTag("Player") || hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(agent.transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    // Do a raycast to check if we have line of sight
                    RaycastHit hit;
                    Vector3 directionToTarget = (hitCollider.transform.position - agent.transform.position).normalized;
                    if (Physics.Raycast(agent.transform.position + Vector3.up, directionToTarget, out hit, agent.AIStat.DetectionDistance))
                    {
                        if (hit.collider == hitCollider)
                        {
                            closestDistance = distance;
                            closestTarget = hitCollider.transform;
                        }
                    }
                }
            }
        }

        // If we found a target, update the agent's current target and switch state
        if (closestTarget != null)
        {
            agent.currentTarget = closestTarget;
            if (closestDistance <= agent.AIStat.AttackDistance)
            {
                Debug.Log($"Found target {closestTarget.name} within attack range, switching to Attack");
                agent.stateMachine.ChangeState(AIStateId.Attack);
            }
            else
            {
                Debug.Log($"Found target {closestTarget.name}, switching to Chase");
                agent.stateMachine.ChangeState(AIStateId.Chase);
            }
        }
    }

    public void Exit(AIAgent agent)
    {
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
        randomPoint.y = center.y + 20;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas))
        { 
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}