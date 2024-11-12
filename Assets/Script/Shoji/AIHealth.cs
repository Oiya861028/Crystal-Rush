using UnityEngine;
using UnityEngine.UI;

public class AIHealth : MonoBehaviour
{
    public float health = 10f;
    public Slider healthBar;
    private UnityEngine.AI.NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        healthBar.maxValue = health;
        healthBar.value = health;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.value = health;
        if (health <= 3) {
            agent.stateMachine.changeState(AIStateId.Flee);
        }
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
