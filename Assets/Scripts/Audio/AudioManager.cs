using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

#pragma warning disable 0649
    [Tooltip("Группа миксеров")]
    [SerializeField] private AudioMixerGroup mixerGroup;

    [Tooltip("Звуки")]
    [SerializeField] private Sound[] sounds;
#pragma warning restore 0649

    private bool musicOn;
    private bool effectsOn;
    private SettingsInfo settingsInfo;
    private Dictionary<string, Sound> currentSounds = new Dictionary<string, Sound>();
    void Awake()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        musicOn = settingsInfo.musicOn;
        effectsOn = settingsInfo.effectsOn;

        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = mixerGroup;
        }
        if (musicOn)
            Play("Theme");
    }

    public void StopAllSounds()
    {
        var allAudioSources = FindObjectsOfType<AudioSource>() as AudioSource[];
        foreach (var audio in allAudioSources)
        {
            audio.Stop();
        }
    }

    public string Play(string sound, bool loop = false)
    {
        string soundKey = "";
        if ((sound == "Theme" && musicOn == true) || (sound != "Theme" && effectsOn == true))
        {
            Sound s = Array.Find(sounds, item => item.nameOfSound == sound);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return null;
            }
            else if(sound != "Theme" && effectsOn)
            {
                s.source.volume = s.volume * 1f;
                s.source.pitch = s.pitch * 1f;
                if (!loop)
                    s.source.PlayOneShot(s.clip);
                else
                {
                    var rnd = new System.Random();
                    soundKey = sound + Time.time + rnd.NextDouble();
                    if (!currentSounds.ContainsKey(sound))
                        currentSounds.Add(soundKey, s);
                    s.source.Play();
                }
                return soundKey;
            }
            else if(sound == "Theme")
                s.source.Play();
        }
        return null;
    }

    public string Stop(string soundKey)
    {
        currentSounds[soundKey].source.Stop();
        currentSounds.Remove(soundKey);
        return "";
    }

    public void On(string sound)
    {
        switch (sound)
        {
            case "Theme":
                musicOn = true;
                Play(sound);
                break;
            default:
                effectsOn = true;
                break;
        }
    }

    public void Off(string sound)
    {
        Sound s = Array.Find(sounds, item => item.nameOfSound == sound);
        if (s == null && sound != "Effects")
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return;
        }
        else if (sound != "Effects")
            s.source.Pause();

        switch (sound)
        {
            case "Theme":
                musicOn = false;
                break;
            default:
                effectsOn = false;
                break;
        }
    }
}
