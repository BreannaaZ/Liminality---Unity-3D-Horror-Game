using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip menuMusic;
    public AudioClip ambientLoop;
    public AudioClip roomChange;
    public AudioClip chase1;
    public AudioClip chase2;
    public AudioClip chase3;
    public AudioClip deathSFX;
    public AudioClip endGameMusic;

    // Starts on main menu with menu music
    void Start()
    {
        if (menuMusic != null)
        {
            PlayMusic(menuMusic, true);
        }
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
