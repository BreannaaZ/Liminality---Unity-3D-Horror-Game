using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum EnemyState
{
    Wandering,
    Chasing,
    Frozen
}

public class MannequinMazeController : MonoBehaviour
{
    public AudioManager audioManager;
    public GameManager gameManager;

    public EnemyState currentState = EnemyState.Wandering;

    public float detectionRange = 10.0f;
    public float frozenChance = 0.1f;
    public float frozenDuration = 2.5f;
    public float stopDistance = 3.0f;

    public Transform[] patrolCorners; 
    private int currentPatrolIndex = 0;

    private NavMeshAgent agent;
    private Transform player;
    private float timer;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timer = 0;
        // Different chase music
        if (audioManager != null && audioManager.chase1 != null)
        {
            audioManager.PlayMusic(audioManager.chase3);
        }
    }

    void Update()
    {

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < 2.5f && !gameManager.gameOver)
        {
            gameManager.gameOver = true;
            if (audioManager != null && audioManager.deathSFX != null)
            {
                audioManager.PlaySFX(audioManager.deathSFX);
            }
        }

        switch (currentState)
        {
            case EnemyState.Wandering:
                Wander();
                if (distanceToPlayer <= detectionRange)
                {
                    currentState = EnemyState.Chasing;
                }
                break;

            case EnemyState.Chasing:
                agent.SetDestination(player.position);
                if (distanceToPlayer > detectionRange + 3f)
                {
                    currentState = EnemyState.Wandering;
                    timer = 0;
                }
                break;

            case EnemyState.Frozen:
                agent.SetDestination(transform.position);
                break;
        }
    }

    void Wander()
    {
        timer += 1;
        // Chance to enter Frozen state (check every few seconds)
        if (timer > 1000)
        {
            if (Random.value < frozenChance)
            {
                Debug.Log("Freeze");
                StartCoroutine(FreezeRoutine());
                return;
            }
        }

        // Move to next corner if not frozen/already going to one
        if (!agent.pathPending && agent.remainingDistance <= stopDistance)
        {
            // Move to next corner
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolCorners.Length;
            agent.SetDestination(patrolCorners[currentPatrolIndex].position);
        }
    }

    IEnumerator FreezeRoutine()
    {
        currentState = EnemyState.Frozen;
        agent.isStopped = true;
        animator.speed = 0f; // Freeze

        yield return new WaitForSeconds(frozenDuration);
        
        agent.isStopped = false;
        animator.speed = 1f; // Play animation 
        currentState = EnemyState.Wandering;
        timer = 0;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        if (NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, dist, layermask))
        {
            return navHit.position;
        }

        return origin;
    }
}
