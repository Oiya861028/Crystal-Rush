using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    AIAgent agent;
    public float MaxHealth;
    [HideInInspector]
    public float currentHealth;
    void Start() {
        agent = GetComponent<AIAgent>();
        currentHealth = MaxHealth;
        var rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach(var rigidbody in rigidbodies) {
            HitBox hitbox = rigidbody.gameObject.AddComponent<HitBox>();
            hitbox.health = this;
        }
    }
    public void TakeDamage(float amount) {
        currentHealth -= amount;
        if(currentHealth <= 0.3*MaxHealth) {
            agent.stateMachine.ChangeState(AIStateId.Flee);
        }
        if(currentHealth <= 0.0f) {
            die();
        }
    } 
    private void die() {
        AIDeathState deathState = agent.stateMachine.GetState(AIStateId.Die) as AIDeathState;
        agent.stateMachine.ChangeState(AIStateId.Die);
    }
}