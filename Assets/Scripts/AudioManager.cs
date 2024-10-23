using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundEffect
{
    public string soundName;
    public AudioClip clip;
}

[System.Serializable]
public class MusicTrack
{
    public string trackName;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public List<SoundEffect> soundEffects = new List<SoundEffect>();
    public List<MusicTrack> musicTracks = new List<MusicTrack>();

    private AudioSource sfxAudioSource;
    private AudioSource musicAudioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        sfxAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource.loop = true;
    }

    public void  PlaySound(string soundName)
    {
        foreach (SoundEffect soundEffect in soundEffects)
        {
            if (soundEffect.soundName == soundName)
            {
                sfxAudioSource.PlayOneShot(soundEffect.clip);
                return;
            }
        }
        Debug.LogWarning("Sound not found: " + soundName);
    }

    public void PlayMusic(string trackName)
    {
        foreach (MusicTrack musicTrack in musicTracks)
        {
            if (musicTrack.trackName == trackName)
            {
                musicAudioSource.clip = musicTrack.clip;
                musicAudioSource.loop = true;
                musicAudioSource.volume = 0.1f;
                musicAudioSource.Play();
                return;
            }
        }
        Debug.LogWarning("Music track not found: " + trackName);
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }
}