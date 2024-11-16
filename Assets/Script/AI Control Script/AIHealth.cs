using UnityEngine;
using UnityEngine.UI;

public class AIHealth : MonoBehaviour
{
     AIAgent agent;
    public float MaxHealth;
    [HideInInspector]
    public float currentHealth;
    public Slider HealthBar;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public float BlinkIntensity;
    public float BlinkDuration;
    private float blinkTimer;
    void Start() {
        agent = GetComponent<AIAgent>();
        currentHealth = MaxHealth;
        var rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach(var rigidbody in rigidbodies) {
            AIHitBox hitbox = rigidbody.gameObject.AddComponent<AIHitBox>();
            hitbox.health = this;
        }
        HealthBar.maxValue = MaxHealth;
        HealthBar.value = currentHealth;
    }
    public void TakeDamage(float amount) {
        currentHealth -= amount;
        HealthBar.value = currentHealth;
        if(currentHealth <= 0.3*MaxHealth) {
            agent.stateMachine.ChangeState(AIStateId.Flee);
        }
        if(currentHealth <= 0.0f) {
            Debug.Log("Dead");
            die();
        }
        blinkTimer = BlinkDuration;
    } 
    public void Update(){
        blinkTimer-=Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer/BlinkDuration);
        float intensity = lerp * BlinkIntensity + 1.0f;
        skinnedMeshRenderer.material.color = Color.white * intensity;
    }
    private void die() {
        agent.stateMachine.ChangeState(AIStateId.Die);
    }
}
