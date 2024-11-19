using UnityEngine;
using UnityEngine.AI;

public class AIFleeState : AIState
{
    private Transform player;
    public float fleeRange = 100f;
    private float minFleeTime = 3f; // Minimum time to flee before considering state change
    private float fleeTimer = 0f;
    private float updatePathInterval = 0.5f;
    private float pathTimer = 0f;
    private float safetyCheckInterval = 1f;
    private float safetyTimer = 0f;
    private Vector3 lastFleePosition;

    public AIStateId GetId()
    {
        return AIStateId.Flee;
    }

    public void Enter(AIAgent agent) 
    {
        if(player == null) {
            player = agent.playerTransform;
        }
        fleeTimer = 0f;
        pathTimer = 0f;
        safetyTimer = 0f;
        FindFleePosition(agent);
    }

    private void FindFleePosition(AIAgent agent)
    {
        // Try to find a position away from both the player and the storm edge
        Vector3 awayFromPlayer = (agent.transform.position - player.position).normalized;
        Vector3 awayFromStormCenter = (agent.transform.position - Vector3.zero).normalized;
        
        // Combine the directions with more weight to avoiding player
        Vector3 fleeDirection = (awayFromPlayer * 0.7f + awayFromStormCenter * 0.3f).normalized;
        
        // Try to find a valid position on the NavMesh
        Vector3 targetPosition = agent.transform.position + fleeDirection * fleeRange;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, fleeRange, NavMesh.AllAreas))
        {
            lastFleePosition = hit.position;
            agent.navmeshAgent.SetDestination(lastFleePosition);
        }
    }

    public void Update(AIAgent agent) 
    {
        fleeTimer += Time.deltaTime;
        pathTimer += Time.deltaTime;
        safetyTimer += Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(agent.transform.position, player.position);
        
        // Update flee path periodically
        if(pathTimer >= updatePathInterval)
        {
            pathTimer = 0f;
            FindFleePosition(agent);
        }

        // Check if we're in a safe position
        if(safetyTimer >= safetyCheckInterval)
        {
            safetyTimer = 0f;
            
            // Only consider changing state if we've fled for minimum time
            if(fleeTimer >= minFleeTime)
            {
                // Check if we're far enough and no line of sight
                RaycastHit hit;
                Vector3 directionToPlayer = (player.position - agent.transform.position).normalized;
                bool hasLineOfSight = Physics.Raycast(agent.transform.position, directionToPlayer, out hit, distanceToPlayer);
                
                if(distanceToPlayer > fleeRange && (!hasLineOfSight || hit.collider.CompareTag("Player") == false))
                {
                    agent.stateMachine.ChangeState(AIStateId.Patrol);
                    return;
                }
            }
        }

        // If we're stuck, try to find a new flee position
        if(agent.navmeshAgent.velocity.magnitude < 0.1f && agent.navmeshAgent.remainingDistance < 1f)
        {
            FindFleePosition(agent);
        }
    }

    public void Exit(AIAgent agent) 
    {
        fleeTimer = 0f;
    }
}