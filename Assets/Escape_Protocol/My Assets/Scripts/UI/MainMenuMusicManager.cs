using UnityEngine;
using GD.Audio;
using GD.Types;

public class MainMenuMusicManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip mainMenuMusic;   // Background music for the main menu
    public AudioClip playGameMusic;  // Background music when the game starts
    public AudioClip menuClickSound; // Sound effect for button clicks

    [Header("Audio Settings")]
    public AudioMixerGroupName backgroundGroup = AudioMixerGroupName.Background;
    public AudioMixerGroupName uiGroup = AudioMixerGroupName.UI;

    private AudioSource currentMusicSource;

    private void Start()
    {
        PlayMainMenuMusic();
    }

    public void PlayMainMenuMusic()
    {
        PlayMusic(mainMenuMusic, true); // Loop main menu music
    }

    public void PlayGameMusic()
    {
        PlayMusic(playGameMusic, true); // Loop game music
    }

    public void PlayButtonClickSound()
    {
        AudioManager.Instance.PlaySound(menuClickSound, uiGroup);
    }

    private void PlayMusic(AudioClip clip, bool loop)
    {
        if (currentMusicSource != null)
        {
            currentMusicSource.Stop(); // Stop current music
            Destroy(currentMusicSource.gameObject); // Clean up
        }

        // Create a new AudioSource for the new music
        currentMusicSource = new GameObject("MusicSource").AddComponent<AudioSource>();
        currentMusicSource.clip = clip;
        currentMusicSource.loop = loop;
        currentMusicSource.outputAudioMixerGroup = AudioManager.Instance.GetAudioMixerGroup(backgroundGroup);
        currentMusicSource.Play();
    }
}
