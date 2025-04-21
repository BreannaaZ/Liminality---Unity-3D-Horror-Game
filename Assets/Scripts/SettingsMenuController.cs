using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.IO;
using Cursor = UnityEngine.Cursor;
using StarterAssets;

public class SettingsMenuController : MonoBehaviour
{
    private VisualElement root;

    private Button menuButton;
    private Button quitButton;
    private Button applyButton;
    private Button cancelButton;
    private DropdownField sceneDropdown;
    public GameObject playerCapsule;

    private int originalSceneIndex;
    private bool isMenuOpen = false;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        menuButton = root.Q<Button>("menu-button");
        quitButton = root.Q<Button>("quit-button");
        applyButton = root.Q<Button>("apply-button");
        cancelButton = root.Q<Button>("cancel-button");
        sceneDropdown = root.Q<DropdownField>("scene-menu");

        originalSceneIndex = SceneManager.GetActiveScene().buildIndex;

        PopulateSceneDropdown();

        sceneDropdown.index = originalSceneIndex;

        menuButton.clicked += OnMenuClicked;
        quitButton.clicked += OnQuitClicked;
        applyButton.clicked += OnApplyClicked;
        cancelButton.clicked += OnCancelClicked;

        // Start hidden
        root.style.display = DisplayStyle.None;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        root.style.display = isMenuOpen ? DisplayStyle.Flex : DisplayStyle.None;

        if (isMenuOpen)
        {
            Time.timeScale = 0f; // Pause game
            sceneDropdown.index = originalSceneIndex;
            // Unlock cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // Disable player capsule to prevent looking around
            playerCapsule.GetComponent<StarterAssets.StarterAssetsInputs>().enabled = false;
            playerCapsule.GetComponent<FirstPersonController>().enabled = false;
        }
        else
        {
            Time.timeScale = 1f; // Resume game
            // Relock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            // Re-enable player capsule to prevent looking around
            playerCapsule.GetComponent<StarterAssets.StarterAssetsInputs>().enabled = true;
            playerCapsule.GetComponent<FirstPersonController>().enabled = true;
        }
    }

    private void PopulateSceneDropdown()
    {
        var sceneNames = new List<string>();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = Path.GetFileNameWithoutExtension(path);
            sceneNames.Add(name);
        }

        sceneDropdown.choices = sceneNames;
    }

    private void OnMenuClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScreen");
    }

    private void OnQuitClicked()
    {
        Application.Quit();
        Debug.Log("Quitting game...");
    }

    private void OnApplyClicked()
    {
        if (sceneDropdown.index != originalSceneIndex)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(sceneDropdown.index);
        }

        ToggleMenu(); // Close after applying
    }

    private void OnCancelClicked()
    {
        sceneDropdown.index = originalSceneIndex;
        ToggleMenu(); // Close after canceling
    }
}
