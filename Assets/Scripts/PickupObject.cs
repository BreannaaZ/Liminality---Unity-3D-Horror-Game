using UnityEngine.UIElements;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public GameManager gameManager;

    private bool isPlayerInRange = false;

    public UIDocument PromptDoc;
    private VisualElement root;
    private Label actionPrompt;

    public GameObject keyObject; // Key object itself

    private void Start()
    {
        root = PromptDoc.rootVisualElement;
        actionPrompt = root.Q<Label>("enterPrompt");
        actionPrompt.style.display = DisplayStyle.None;
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            keyObject.SetActive(false); // disable the key
            gameManager.hasKey = true; // Set the key flag to true
            // Hide UI prompt
            actionPrompt.style.display = DisplayStyle.None;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            // Show UI prompt
            actionPrompt.text = "Press E to Pickup Key";
            actionPrompt.style.display = DisplayStyle.Flex;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            // Hide UI prompt
            actionPrompt.style.display = DisplayStyle.None;
        }
    }
}
