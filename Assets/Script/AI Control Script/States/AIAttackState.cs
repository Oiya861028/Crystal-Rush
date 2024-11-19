using System;
using UnityEngine;


public class AIAttackState : AIState
{
    private float attackCooldown = 1f;
    private float lastAttackTime = 0f;
    private float targetCheckInterval = 0.5f;
    private float targetCheckTimer = 0f;
    private float maxAttackDuration = 3f;
    private float attackStateTimer = 0f;
    private float minAttackDistance = 3f; // Minimum distance to maintain

    public void Update(AIAgent agent)
    {
        // Always check timer first
        attackStateTimer += Time.deltaTime;
        if (attackStateTimer >= maxAttackDuration)
        {
            agent.stateMachine.ChangeState(AIStateId.Chase);
            return;
        }

        // Regular target checking
        targetCheckTimer += Time.deltaTime;
        if (targetCheckTimer >= targetCheckInterval)
        {
            FindNewTarget(agent);
            targetCheckTimer = 0f;
        }

        if (agent.currentTarget == null)
        {
            FindNewTarget(agent);
            if (agent.currentTarget == null)
            {
                agent.stateMachine.ChangeState(AIStateId.Patrol);
            }
            return;
        }

        float distanceToTarget = Vector3.Distance(agent.transform.position, agent.currentTarget.position);

        // Back up if too close
        if (distanceToTarget < minAttackDistance)
        {
            Vector3 directionAway = (agent.transform.position - agent.currentTarget.position).normalized;
            Vector3 backupPosition = agent.transform.position + directionAway * (minAttackDistance - distanceToTarget);
            agent.navmeshAgent.SetDestination(backupPosition);
            return;
        }

        // Chase if too far
        if (distanceToTarget > agent.AIStat.AttackDistance)
        {
            agent.stateMachine.ChangeState(AIStateId.Chase);
            return;
        }

        // Always face target
        Vector3 targetDirection = (agent.currentTarget.position - agent.transform.position).normalized;
        targetDirection.y = 0f;
        agent.transform.rotation = Quaternion.LookRotation(targetDirection);

        // Attack when cooldown ready
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            AttackTarget(agent);
            lastAttackTime = Time.time;

            // Smaller random movement after shooting to maintain spacing
            Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * 0.5f;
            randomOffset.y = 0;
            Vector3 targetPos = agent.transform.position + randomOffset;
            agent.navmeshAgent.SetDestination(targetPos);
        }
    }

    private void FindNewTarget(AIAgent agent)
    {
        Collider[] hitColliders = Physics.OverlapSphere(agent.transform.position, agent.AIStat.DetectionDistance);
        float closestDistance = float.MaxValue;
        Transform closestTarget = null;
        Transform closestPlayer = null;
        float closestPlayerDistance = float.MaxValue;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider == null || hitCollider.gameObject == agent.gameObject) 
                continue;

            float distance = Vector3.Distance(agent.transform.position, hitCollider.transform.position);
            
            // Check for player first
            if (hitCollider.CompareTag("Player"))
            {
                if (distance < closestPlayerDistance && distance > 0.1f)
                {
                    closestPlayerDistance = distance;
                    closestPlayer = hitCollider.transform;
                }
            }
            // If not a player, check for other enemies
            else if (hitCollider.CompareTag("Enemy"))
            {
                if (distance < closestDistance && distance > 0.1f)
                {
                    closestDistance = distance;
                    closestTarget = hitCollider.transform;
                }
            }
        }

        // Prioritize player if found within range
        if (closestPlayer != null)
        {
            if (agent.currentTarget == null || !agent.currentTarget.CompareTag("Player"))
            {
                agent.currentTarget = closestPlayer;
            }
        }
        // Otherwise use closest enemy if we found one
        else if (closestTarget != null && closestTarget != agent.currentTarget)
        {
            agent.currentTarget = closestTarget;
        }
    }

    private void AttackTarget(AIAgent agent)
    {
        if (agent.currentTarget == null) return;
        
        Vector3 direction = (agent.currentTarget.position - agent.transform.position).normalized;
        ShootProjectile(agent, direction);
    }

    private void ShootProjectile(AIAgent agent, Vector3 direction)
    {
        try
        {
            if (agent.AIStat.projectileData == null || agent.AIStat.projectileData.model == null)
            {
                Debug.LogError("Projectile data or model is missing!");
                return;
            }

            Vector3 spawnOffset = (direction * 1f) + (Vector3.up * 1.5f);
            Vector3 spawnPosition = agent.transform.position + spawnOffset;

            GameObject projectile = UnityEngine.Object.Instantiate(
                agent.AIStat.projectileData.model, 
                spawnPosition, 
                Quaternion.LookRotation(direction)
            );

            if (!projectile.GetComponent<Collider>())
            {
                SphereCollider collider = projectile.AddComponent<SphereCollider>();
                collider.isTrigger = true;
                collider.radius = 0.25f;
            }

            ProjectileController controller = projectile.GetComponent<ProjectileController>();
            if (controller == null)
            {
                controller = projectile.AddComponent<ProjectileController>();
            }

            projectile.tag = "Projectile";

            controller.Initialize(
                direction,
                agent.AIStat.projectileData.projectileSpeed,
                agent.AIStat.projectileData.baseDamage
            );

            UnityEngine.Object.Destroy(projectile, 5f);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error shooting projectile: {e.Message}");
        }
    }

    public AIStateId GetId()
    {
        return AIStateId.Attack;
    }

    public void Enter(AIAgent agent)
    {
        lastAttackTime = 0f;
        attackStateTimer = 0f;
        targetCheckTimer = 0f;
    }

    public void Exit(AIAgent agent)
    {
    }
}