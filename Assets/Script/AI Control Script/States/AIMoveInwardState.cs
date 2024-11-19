using UnityEngine;
using UnityEngine.AI;
public class MoveInward : AIState
{
    public void Enter(AIAgent Agent)
    {
        Agent.navmeshAgent.SetDestination(Vector3.zero);
    }

    public void Exit(AIAgent agent)
    {
        
    }

    public AIStateId GetId()
    {
        return AIStateId.MoveInward;
    }

    public void Update(AIAgent agent)
    {
        float distanceFromCenter = Vector3.Distance(
                new Vector3(agent.transform.position.x, 0, agent.transform.position.z), 
                Vector3.zero
            );
            if (distanceFromCenter < agent.storm.currentStormRadius)
            {
                agent.stateMachine.ChangeState(AIStateId.Patrol);
            }
    }
}
