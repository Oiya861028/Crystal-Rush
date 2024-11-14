using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AIStateId{
    Patrol, 
    Chase, 
    Attack, 
    Flee
}
public interface AIState {
    AIStateId GetId();
    void Enter(AIagent Agent);
    void Update(AIAgent agent);
    void Exit(AIagent agent);
}
