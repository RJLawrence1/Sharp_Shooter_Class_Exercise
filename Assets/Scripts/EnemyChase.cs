using UnityEngine;
using UnityEngine.AI;

// Put this on the Enemy (Capsule) that has a NavMeshAgent.
public class EnemyChase : MonoBehaviour
{
    [SerializeField] Transform target; // drag the Player/Target here

    NavMeshAgent agent; // reference to the NavMeshAgent component

    void Awake()
    {
        // Find the NavMeshAgent on this same GameObject
        agent = GetComponent<NavMeshAgent>();
    }

    // PART 3 version (stationary target): set the destination once.
    // void Start()
    // {
    //     agent.SetDestination(target.position);
    // }

    // PART 4 version (moving target): keep updating the destination every frame,
    // so the enemy chases where the target is NOW, not where it was at the start.
    void Update()
    {
        agent.SetDestination(target.position);
    }
}
