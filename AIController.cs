using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public float attackRange = 5f;
    public Transform player;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float damage = 1f;
    private float health = 10f;

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit, attackRange))
            {
                if (hit.transform == player)
                {
                    AttackPlayer();
                }
            }
        }
    }

    void AttackPlayer()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = (player.position - transform.position).normalized * projectileSpeed;

        Destroy(projectile, 2f);  // Destroy the projectile after 2 seconds
    }
}

