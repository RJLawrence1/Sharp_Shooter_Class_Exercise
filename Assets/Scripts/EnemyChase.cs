using UnityEngine;
using UnityEngine.AI;

// Put this on the Enemy prefab.
public class EnemyChase : MonoBehaviour
{
    [SerializeField] Transform target; // can leave empty: it finds the Player automatically

    NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        // Spawned prefabs can't hold scene references,
        // so find the object tagged "Player" if no target was assigned
        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }
    }

    void Update()
    {
        if (target == null) return;

        // Keep chasing the target's CURRENT position every frame
        agent.SetDestination(target.position);
    }
}
