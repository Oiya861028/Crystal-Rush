using UnityEngine.AI;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
public class AIDeathState: AIState {
    public AIStateId GetId(){
        return AIStateId.Die;
    }
    public void Enter(AIAgent agent){
        agent.Die();
    }
    public void Update(AIAgent agent){
    }
    public void Exit(AIAgent agent){
        
    }

    
}
