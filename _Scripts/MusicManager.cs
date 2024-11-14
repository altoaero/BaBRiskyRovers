using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance; // Static reference to the singleton instance
    private AudioSource audioSource;      // Reference to the AudioSource component

    public AudioClip backgroundMusic;     // Background music AudioClip to play
    public string[] seamlessScenes;       // Array of scene names where music should continue playing

    private void Awake()
    {
        // Singleton pattern: Ensure only one instance of MusicManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scene loads
            audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
            PlayMusic(backgroundMusic); // Start playing background music
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    private void Start()
    {
        // Subscribe to sceneLoaded event when the scene starts
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe from sceneLoaded event when the object is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene is in seamlessScenes array
        if (System.Array.Exists(seamlessScenes, element => element == scene.name))
        {
            // If the background music is not playing, start playing it
            if (!audioSource.isPlaying)
            {
                PlayMusic(backgroundMusic);
            }
        }
        else
        {
            // Stop the music if the scene is not in seamlessScenes array
            StopMusic();
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        // Play the given AudioClip as background music
        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.loop = true; // Loop the music
            audioSource.Play();      // Start playing
        }
    }

    public void StopMusic()
    {
        // Stop playing the background music
        audioSource.Stop();
    }
}
