using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class NextRoomTrigger : MonoBehaviour
{
    public GameManager gameManager;
    public AudioManager audioManager;

    public string nextScene = "Maze"; 
    private bool isPlayerInRange = false;
    private bool enteredRange = false;
    public Transform player;
    public Transform door;
    public bool hasTurnedAway = false;
    public GameObject key;

    public UIDocument PromptDoc;
    private VisualElement root;
    private Label enterPrompt;

    private void Start()
    {
        root = PromptDoc.rootVisualElement;
        enterPrompt = root.Q<Label>("enterPrompt");
        enterPrompt.style.display = DisplayStyle.None;
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (gameManager != null && gameManager.hasKey)
            {
                Debug.Log("Next scene");
                if (nextScene == "ConnectorRoom") {
                    gameManager.hasKey = false; // Don't start with key in connector room level
                }
                SceneManager.LoadScene(nextScene);
            }
        }
        // Check for turned back in scene 2
        if (nextScene == "ChaseMaze")
        {
            if ((enteredRange || isPlayerInRange) && !hasTurnedAway)
            {
                Vector3 toDoor = door.position - player.position;
                float angle = Vector3.Angle(player.forward, toDoor);

                // Player turned away from door
                if (angle > 100f)
                {
                    hasTurnedAway = true;
                    enterPrompt.text = "What the...?!";
                    gameManager.startEnemy = true;
                    // Trigger audio SFX
                    if (audioManager != null)
                    {
                        audioManager.PlaySFX(audioManager.roomChange);
                        audioManager.PlayMusic(audioManager.chase2);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            enteredRange = true;
            // Show UI prompt
            if (gameManager != null)
            {
                if (gameManager.hasKey)
                {
                    enterPrompt.text = "Press E to Enter";
                }
                else if (nextScene == "ChaseMaze")
                {
                    enterPrompt.text = "I think I dropped the key by the door...";
                }
                else
                {
                    enterPrompt.text = "It's Locked...";
                }
            }
            enterPrompt.style.display = DisplayStyle.Flex;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            // Hide UI prompt
            enterPrompt.style.display = DisplayStyle.None;

            if (gameManager != null && gameManager.startEnemy && nextScene == "ChaseMaze")
            {
                key.SetActive(true);
            }
        }
    }
}
