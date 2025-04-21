using UnityEngine;

// Simple script to move object towards given point
// for the hallway chase level
public class DarknessController : MonoBehaviour
{
    public AudioManager audioManager;
    public GameManager gameManager;

    public Transform targetPoint; 
    public float speed = 2f;

    private void Start()
    {
        // Start hallway music
        if (audioManager != null && audioManager.chase1 != null)
        {
            audioManager.PlayMusic(audioManager.chase1);
        }
    }
    void Update()
    {
        if (targetPoint != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.gameOver = true;
            // Death SFX
            if (audioManager != null && audioManager.deathSFX != null)
            {
                audioManager.PlaySFX(audioManager.deathSFX);
            }
        }
    }
}
