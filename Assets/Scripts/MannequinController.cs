using UnityEngine;
using UnityEngine.AI;

public class MannequinController : MonoBehaviour
{
    public AudioManager audioManager;
    public GameManager gameManager;

    // Basic enemy vars
    public Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    public bool isSeen; // Raycast controller will modify this
    public bool lightsOut = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 toEnemy = transform.position - player.position;
        // If distance between enemy and player is low, trigger player death
        if (toEnemy.magnitude < 4.0f && !gameManager.gameOver)
        {
            gameManager.gameOver = true;
            if (audioManager != null && audioManager.deathSFX != null) {
                audioManager.PlaySFX(audioManager.deathSFX);
            }
        }

        if (!isSeen || lightsOut) // Player NOT looking at enemy or lights are out
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.speed = 1f; // Play animation normally
        }
        else // Player IS looking at enemy 
        {
            agent.isStopped = true;
            agent.ResetPath();
            animator.speed = 0f; // Freeze animation exactly as it is
        }
    }
} // End of MannequinController