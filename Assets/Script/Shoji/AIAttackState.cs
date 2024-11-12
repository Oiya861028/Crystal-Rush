using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackState : AIState
{
    public float attackRange = 5f;
    public Transform player;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float damage = 1f;
    private float health = 10f;

    private float attackCooldown = 1f;
    private float lastAttackTime = 0f;

    public AIStateId GetId()
    {
        return AIStateId.Attack;
    }

    public void Update(AIAgent agent)
    {
        Vector3 direction = (player.position - agent.transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(agent.transform.position, direction, out hit, attackRange))
        {
            if (hit.transform == player)
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    AttackPlayer(agent);
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    void AttackPlayer(AIAgent agent)
    {
        Vector3 direction = (player.position - agent.transform.position).normalized;
        GameObject projectile = Object.Instantiate(projectilePrefab, agent.transform.position, Quaternion.identity);

        Collider aiCollider = agent.gameObject.GetComponent<Collider>();
        Collider projectileCollider = projectile.GetComponent<Collider>();

        if (aiCollider != null && projectileCollider != null)
        {
            Physics.IgnoreCollision(aiCollider, projectileCollider);
        }

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }
    Object.Destroy(projectile, 2f);
    }
}