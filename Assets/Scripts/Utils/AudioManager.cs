using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to scene change event
        Play("MenuMusic");
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from the event
    }

        // Called when a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name) // Adjust based on scene names
        {
            case "MainMenu":
                Play("MenuMusic");
                break;
            case "StartingRoom":
                Play("AmbientSound");
                break;
            // Add more cases for additional scenes
            default:
                Debug.LogWarning("No music defined for this scene.");
                break;
        }
    }

    public void Play(string name)
    {
        // Stop all currently playing sounds
        StopAllSounds();

        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void StopAllSounds()
{
    foreach (Sound s in sounds)
    {
        if (s.source.isPlaying)
        {
            s.source.Stop();
        }
    }
}
}
