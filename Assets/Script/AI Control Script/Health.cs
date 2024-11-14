using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    AIAgent agent;
    public float MaxHealth;
    [HideInInspector]
    public float currentHealth;
    void Start() {
        agent = GtetComponent<AIagent>();
        currentHealth = MaxHealth;
        var rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach(var rigidbody in rigidbodies){
            HitBox hitbox = rigidbody.gameObject.AddComponent<HitBox>();
            hitbox.health = this;
        }
    }
    public void TakeDamage(float amount) {
        currentHealth -= amount;
        if(currentHealth <= 0.0f) {
            die();
        }
    } 
    private void die() {
        AIDeathState deathState = agent.StateMachine.GetState(AIStateId.Death) as AIDeathState;
        agent.stateMachine.ChangeState(AIStateId.Death);
    }
}