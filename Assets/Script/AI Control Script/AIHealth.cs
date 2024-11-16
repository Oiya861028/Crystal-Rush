using UnityEngine;
using UnityEngine.UI;

public class AIHealth : MonoBehaviour
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
            AIHitBox hitbox = rigidbody.gameObject.AddComponent<AIHitBox>();
            hitbox.health = this;
        }
    }
    public void TakeDamage(float amount) {
        currentHealth -= amount;
        if(currentHealth <= 0.3*MaxHealth) {
            agent.stateMachine.ChangeState(AIStateId.Flee);
        }
        if(currentHealth <= 0.0f) {
            Debug.Log("Dead");
            die();
        }
    } 
    private void die() {
        agent.stateMachine.ChangeState(AIStateId.Die);
    }
}
