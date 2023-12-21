using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    static public AudioManager instance { get; private set; }

    public AudioSource mainAudioSource { get; private set; }
    public float mainVolume
    {
        get { return mainAudioSource.volume; }
        set
        {
            mainAudioSource.volume = value;
            PlayerPrefs.SetFloat("MainVolume", value);
        }
    }

    private Dictionary<AudioClip, AudioSource> backgroundAudioSources = new Dictionary<AudioClip, AudioSource>();
    private float _backgroundVolume;
    public float backgroundVolume
    {
        get { return _backgroundVolume; }
        set
        {
            _backgroundVolume = value;
            foreach (AudioSource source in backgroundAudioSources.Values)
            {
                source.volume = value;
            }

            PlayerPrefs.SetFloat("BackgroundVolume", value);
        }
    }

    public AudioClip testClip;

    private void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(this); }
        else { Destroy(this.gameObject); }
    }
    private void Start()
    {
        mainAudioSource = GetComponent<AudioSource>();
        mainVolume = PlayerPrefs.HasKey("MainVolume") ? PlayerPrefs.GetFloat("MainVolume") : 1.0f;
        backgroundVolume = PlayerPrefs.HasKey("BackgroundVolume") ? PlayerPrefs.GetFloat("BackgroundVolume") : 1.0f;


        PlayBackgroundMusic(testClip);
    }

    public void PlayBackgroundMusic(AudioClip backgroundClip)
    {
        if (backgroundAudioSources.ContainsKey(backgroundClip))
            return;

        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = this.backgroundVolume;
        audioSource.loop = true;
        audioSource.clip = backgroundClip;
        audioSource.Play();

        backgroundAudioSources.Add(backgroundClip, audioSource);
    }
    public void StopBackgroundMusic(AudioClip backgroundClip)
    {
        Destroy(backgroundAudioSources[backgroundClip]);
        backgroundAudioSources.Remove(backgroundClip);
    }
    public void StopAllBackgroundMusic()
    {
        foreach (AudioSource audioSource in backgroundAudioSources.Values)
            Destroy(audioSource);

        if (backgroundAudioSources.Count > 0)
            backgroundAudioSources.Clear();
        else
            Debug.LogWarning("do not play background music");
    }

    public void ChangeBackgroundMusic(AudioClip backgroundClip)
    {
        StopAllBackgroundMusic();
        PlayBackgroundMusic(backgroundClip);
    }
}
