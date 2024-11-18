using UnityEngine;
using UnityEngine.AI;
//This script will be attached to the AI prefab
public class AIAgent : MonoBehaviour
{
    public FSM stateMachine;
    public AIStateId initialState;
    public Transform playerTransform;
    public Transform currentTarget; // Add this to track current target
    public NavMeshAgent navmeshAgent;
    public AIStatScriptableObject AIStat;
    public StormSystem storm;
    public string[] targetTags = { "Player", "Enemy" }; // Add tags to check for targets
    public float targetUpdateInterval = 0.5f; // How often to check for new targets
    private float targetUpdateTimer;
    

    public void Start()
    {
        storm = GameObject.FindAnyObjectByType<StormSystem>();
        stateMachine = new FSM(this);
        registerMachineStates();
        stateMachine.ChangeState(initialState);
        navmeshAgent = GetComponent<NavMeshAgent>();
        
        // Set initial target as player
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        currentTarget = playerTransform;
        
        // Add "Enemy" tag to this AI
        gameObject.tag = "Enemy";
    }

    public void Update()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
        storm = GameObject.FindAnyObjectByType<StormSystem>();
        
        // Update target periodically
        targetUpdateTimer += Time.deltaTime;
        if (targetUpdateTimer >= targetUpdateInterval)
        {
            UpdateTarget();
            targetUpdateTimer = 0f;
        }

        // Error checking
        if(storm == null) Debug.LogError("Did not find any storm script");
        if(currentTarget == null) Debug.LogError("No target found");
        if(navmeshAgent == null) Debug.LogError("Did not find any navMeshAgent script");
        if(stateMachine == null) Debug.Log("StateMachine is null");
        
        stateMachine.Update();
    }

    private void UpdateTarget()
    {
        Transform nearestTarget = null;
        float nearestDistance = float.MaxValue;

        // Check all potential targets
        foreach (string tag in targetTags)
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject target in targets)
            {
                // Skip if it's this AI
                if (target == gameObject) continue;

                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTarget = target.transform;
                }
            }
        }

        // Update current target if we found a closer one
        if (nearestTarget != null)
        {
            currentTarget = nearestTarget;
        }
        else
        {
            // Fallback to player if no other targets found
            currentTarget = playerTransform;
        }
    }

    private void registerMachineStates()
    {
        stateMachine.RegisterState(new AIChaseState());
        stateMachine.RegisterState(new AIPatrolState());
        stateMachine.RegisterState(new AIAttackState());
        stateMachine.RegisterState(new AIFleeState());
    }

    public void die()
    {
        playerAliveCount counter = FindFirstObjectByType<playerAliveCount>();
        counter.updateCounter();
        Destroy(gameObject);
    }
}
