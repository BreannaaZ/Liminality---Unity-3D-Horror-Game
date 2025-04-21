using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool hasKey = false;
    public bool gameOver = false;

    public bool startEnemy = false;
    public GameObject mannequinToActivate;
    public GameObject mannequinToDestroy;
    public GameObject roomFront1;
    public GameObject roomFront2;
    public GameObject playerCapsule;
    public GameObject raycasts;
    public GameObject lights;

    private bool hasSwitched = false;
    private bool hasTriggeredGameOver = false;
    public bool hasTriggeredSpawns = false;
        
    public UIDocument gameOverUI;
    private VisualElement root;

    private void Start()
    {
        if (gameOverUI != null)
        {
            root = gameOverUI.rootVisualElement;
            root.style.display = DisplayStyle.None;

            root.Q<Button>("retry-button").clicked += RetryScene;
            root.Q<Button>("quit-button").clicked += QuitToMenu;
        }
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
    }

    void Update()
    {
        // Event to spawn enemy moving mannequin and key, and swap room to larger one
        if (startEnemy && !hasSwitched)
        {
            // Swap room
            if (roomFront2 != null) roomFront2.SetActive(true);
            if (roomFront1 != null) Destroy(roomFront1);

            // Swap mannequin
            if (mannequinToActivate != null)
            {
                mannequinToActivate.SetActive(true);
            }
            if (raycasts != null)
            {
                raycasts.SetActive(true); // Only activate raycasts once mannequin is spawned
            }
            if (lights != null)
            {
                lights.SetActive(true);
            }
            if (mannequinToDestroy != null)
                Destroy(mannequinToDestroy);

            hasSwitched = true; // Flag to only run event once
        }

        if (gameOver && !hasTriggeredGameOver)
        {
            TriggerGameOver();
        }
    }
    
    public void TriggerGameOver()
    {
        hasTriggeredGameOver = true;
        Time.timeScale = 0f; // Pause the game
        root.style.display = DisplayStyle.Flex;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        // Disable player capsule to prevent looking around
        playerCapsule.GetComponent<StarterAssets.StarterAssetsInputs>().enabled = false;
        playerCapsule.GetComponent<FirstPersonController>().enabled = false;
    }

    private void RetryScene()
    {
        Time.timeScale = 1f; // Unpause
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void QuitToMenu()
    {
        Time.timeScale = 1f; // Unpause
        SceneManager.LoadScene("MainScreen"); 
    }
}
