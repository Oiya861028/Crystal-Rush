using System;
using UnityEngine;

public class AIAttackState : AIState
{
    private float attackCooldown = 1f;
    private float lastAttackTime = 0f;
    private float switchToChaseBuffer = 3f;
    
    public void Update(AIAgent agent)
    {
        if (agent == null || agent.currentTarget == null) return;

        float distanceToTarget = Vector3.Distance(agent.transform.position, agent.currentTarget.position);
        
        // Only attack if within range
        if (distanceToTarget <= agent.AIStat.AttackDistance)
        {
            Vector3 directionToTarget = (agent.currentTarget.position - agent.transform.position).normalized;
            
            // Check if we have line of sight
            RaycastHit hit;
            if (Physics.Raycast(agent.transform.position + Vector3.up, directionToTarget, out hit, agent.AIStat.AttackDistance))
            {
                // If we hit the target and attack is not on cooldown
                if (hit.transform == agent.currentTarget && Time.time >= lastAttackTime + attackCooldown)
                {
                    ShootProjectile(agent, directionToTarget);
                    lastAttackTime = Time.time;
                }
            }
        }
        
        // Switch to chase if target gets too far
        if (distanceToTarget > agent.AIStat.AttackDistance + switchToChaseBuffer)
        {
            agent.stateMachine.ChangeState(AIStateId.Chase);
        }
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
            Debug.Log("shooting a bullet");
            // Calculate spawn position (slightly in front and above the AI)
            Vector3 spawnOffset = (direction * 1f) + (Vector3.up * 1.5f);
            Vector3 spawnPosition = agent.transform.position + spawnOffset;

            // Instantiate the projectile
            GameObject projectile = UnityEngine.Object.Instantiate(
                agent.AIStat.projectileData.model, 
                spawnPosition, 
                Quaternion.identity
            );

            // Add necessary components if they don't exist
            if (!projectile.GetComponent<Collider>())
            {
                SphereCollider collider = projectile.AddComponent<SphereCollider>();
                collider.isTrigger = true;
                collider.radius = 0.25f;
            }

            // Ensure it has the ProjectileController
            ProjectileController controller = projectile.GetComponent<ProjectileController>();
            if (controller == null)
            {
                controller = projectile.AddComponent<ProjectileController>();
            }

            // Tag it as a projectile
            projectile.tag = "Projectile";

            // Initialize the projectile
            controller.Initialize(
                direction,
                agent.AIStat.projectileData.projectileSpeed,
                agent.AIStat.projectileData.baseDamage
            );

            // Destroy after a delay
            UnityEngine.Object.Destroy(projectile, 5f);
        }
        catch (Exception e)
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
        lastAttackTime = 0f; // Reset cooldown when entering state
    }

    public void Exit(AIAgent agent)
    {
    }
}