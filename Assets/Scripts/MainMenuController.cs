using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Cursor = UnityEngine.Cursor;

public class MainMenuController : MonoBehaviour
{
    public UIDocument uiDocument;
    public string nextScene;

    void OnEnable()
    {
        var root = uiDocument.rootVisualElement;

        root.Q<Button>("start-button").clicked += () => StartGame();
        root.Q<Button>("quit-button").clicked += () => QuitGame();

        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void StartGame()
    {
        SceneManager.LoadScene(nextScene); 
        Debug.Log("Start");
    }

    void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
