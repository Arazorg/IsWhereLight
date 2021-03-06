﻿
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

    private SettingsInfo settingsInfo;
    private Sound theme;

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        theme = null;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = mixerGroup;
        }
        if (settingsInfo.musicOn)
            Play("Theme");
    }

    void Update()
    {
        if(theme != null)
            theme.source.volume = settingsInfo.musicVolume;
    }

    public void StopAllSounds()
    {
        AudioListener.pause = true;
        var allAudioSources = FindObjectsOfType<AudioSource>() as AudioSource[];
        foreach (var audio in allAudioSources)
        {
            if(!audio.ignoreListenerPause)
                audio.Stop();
        }
    }

    public void PlayAllSounds()
    {
        AudioListener.pause = false;
    }

    public Sound Play(string sound, bool loop = false)
    {
        if ((sound == "Theme" && settingsInfo.musicOn == true) 
                || (sound != "Theme" && settingsInfo.effectsOn == true))
        {
            Sound s = Array.Find(sounds, item => item.nameOfSound == sound);

            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return null;
            }
            else if (sound != "Theme" && settingsInfo.effectsOn)
            {
                s.source.volume = s.volume * 1f * settingsInfo.effectsVolume;
                s.source.pitch = s.pitch * 1f;
                s.source.ignoreListenerPause = s.ignorePause;
                if (!loop)
                {
                    s.source.PlayOneShot(s.clip);
                    return null;
                }
                else
                {
                    s.source.Play();
                    return s;
                }
            }
            else if (sound == "Theme")
            {
                theme = s;
                theme.source.volume = theme.volume * 1f * settingsInfo.musicVolume;
                theme.source.pitch = theme.pitch * 1f;
                theme.source.ignoreListenerPause = theme.ignorePause;
                theme.source.Play();
            }

        }
        return null;
    }

    public void Stop(Sound sound)
    {
        sound.source.Stop();
    }

    public void On(string sound)
    {
        switch (sound)
        {
            case "Theme":
                settingsInfo.musicOn = true;
                Play(sound);
                break;
            default:
                settingsInfo.effectsOn = true;
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
            s.source.Stop();

        switch (sound)
        {
            case "Theme":
                settingsInfo.musicOn = false;
                break;
            default:
                settingsInfo.effectsOn = false;
                break;
        }
    }
}