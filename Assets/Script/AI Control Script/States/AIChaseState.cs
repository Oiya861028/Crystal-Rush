using UnityEngine;
public class AIChaseState : AIState
{
    AIStatScriptableObject AIStat;
    float timer = 0.0f;
    private float optimalAttackDistance;
    
    public AIStateId GetId(){
        return AIStateId.Chase;
    }
    

    public void Enter(AIAgent agent) {
        if(AIStat == null) {
            AIStat = agent.AIStat;
        }
        optimalAttackDistance = agent.AIStat.AttackDistance * 0.8f;
    }
    
    public void Update(AIAgent agent){
        if(!agent.enabled || agent.currentTarget == null) {
            return;
        }

        float distanceToTarget = Vector3.Distance(agent.transform.position, agent.currentTarget.position);
        
        if(distanceToTarget > agent.AIStat.AttackDistance + 2f) {
            if(timer >= AIStat.minTime){
                Vector3 directionToTarget = (agent.currentTarget.position - agent.transform.position).normalized;
                Vector3 targetPosition = agent.currentTarget.position - (directionToTarget * optimalAttackDistance);
                agent.navmeshAgent.destination = targetPosition;
                timer = 0.0f;
            } else {
                timer += Time.deltaTime;
            }
        } else {
            agent.navmeshAgent.destination = agent.transform.position;
        }

        checkChangeStateCondition(agent);
    }

    public void Exit(AIAgent agent) {
        timer = 0.0f;
    }

    private void checkChangeStateCondition(AIAgent agent){
        float distanceToTarget = Vector3.Distance(agent.transform.position, agent.currentTarget.position);
        
        if(distanceToTarget <= agent.AIStat.AttackDistance && 
           agent.navmeshAgent.velocity.magnitude < 0.1f) {
            Debug.Log("Switching from chase to Attack");
            agent.stateMachine.ChangeState(AIStateId.Attack);
        }
        else if(distanceToTarget > agent.AIStat.DetectionDistance) {
            Debug.Log("Switching from chase to patrol");
            agent.stateMachine.ChangeState(AIStateId.Patrol);
        }
    }
}