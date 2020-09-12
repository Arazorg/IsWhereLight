﻿using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

#pragma warning disable 0649
    [Tooltip("Группа миксеров")]
    [SerializeField] private AudioMixerGroup mixerGroup;

    [Tooltip("ЗВуки")]
    [SerializeField] private Sound[] sounds;
#pragma warning restore 0649

    private bool musicOn;
    private bool effectsOn;
    private SettingsInfo settingsInfo;

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

    public void Play(string sound)
    {
        if ((sound == "Theme" && musicOn == true) || (sound != "Theme" && effectsOn == true))
        {
            Sound s = Array.Find(sounds, item => item.nameOfSound == sound);
            if (s == null && sound != "Effects")
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            else
            {
                s.source.volume = s.volume * 1f;
                s.source.pitch = s.pitch * 1f;
                s.source.Play();
            }
        }    
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
        else
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
