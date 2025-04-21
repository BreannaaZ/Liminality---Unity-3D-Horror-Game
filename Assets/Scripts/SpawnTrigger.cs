using UnityEngine;
using UnityEngine.UIElements;

public class SpawnTrigger : MonoBehaviour
{
    public GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameManager != null)
            {
                gameManager.startEnemy = true;
            }
        }
    }
}
