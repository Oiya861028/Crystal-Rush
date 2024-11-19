using UnityEngine;
using UnityEngine.AI;
using System;

public class AIChaseState : AIState
{
    private float targetCheckInterval = 0.5f;
    private float targetCheckTimer = 0f;
    
    public AIStateId GetId()
    {
        return AIStateId.Chase;
    }

    public void Enter(AIAgent agent) 
    {
        agent.navmeshAgent.stoppingDistance = agent.AIStat.AttackDistance * 0.8f;
    }
    
    public void Update(AIAgent agent)
    {
        if (agent.currentTarget == null)
        {
            FindNewTarget(agent);
            if (agent.currentTarget == null)
            {
                agent.stateMachine.ChangeState(AIStateId.Patrol);
            }
            return;
        }

        // Regular target checking
        targetCheckTimer += Time.deltaTime;
        if (targetCheckTimer >= targetCheckInterval)
        {
            FindNewTarget(agent);
            targetCheckTimer = 0f;
        }

        float distanceToTarget = Vector3.Distance(agent.transform.position, agent.currentTarget.position);

        // Simple state changes based only on distance
        if (distanceToTarget <= agent.AIStat.AttackDistance)
        {
            agent.stateMachine.ChangeState(AIStateId.Attack);
        }
        else if (distanceToTarget > agent.AIStat.DetectionDistance)
        {
            agent.stateMachine.ChangeState(AIStateId.Patrol);
        }
        else
        {
            // Just chase directly to the target
            agent.navmeshAgent.SetDestination(agent.currentTarget.position);
        }
    }

    private void FindNewTarget(AIAgent agent)
    {
        Collider[] hitColliders = Physics.OverlapSphere(agent.transform.position, agent.AIStat.DetectionDistance);
        float closestDistance = float.MaxValue;
        Transform closestTarget = null;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider == null || hitCollider.gameObject == agent.gameObject) 
                continue;

            if (hitCollider.CompareTag("Player") || hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(agent.transform.position, hitCollider.transform.position);
                if (distance < closestDistance && distance > 0.1f)
                {
                    closestDistance = distance;
                    closestTarget = hitCollider.transform;
                }
            }
        }

        if (closestTarget != null)
        {
            agent.currentTarget = closestTarget;
        }
    }

    public void Exit(AIAgent agent) 
    {
        agent.navmeshAgent.stoppingDistance = 0f;
    }
}