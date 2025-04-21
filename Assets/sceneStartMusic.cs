using UnityEngine;

public class sceneStartMusic : MonoBehaviour
{
    public AudioManager audioManager;

    public AudioClip newMusic; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (audioManager != null && newMusic != null)
        {
            audioManager.PlayMusic(newMusic);
        }
    }
}
