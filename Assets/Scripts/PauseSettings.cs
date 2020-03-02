using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSettings : MonoBehaviour
{
    SettingsInfo settingsInfo;
    AudioManager audioManager;
    private bool musicOn;
    private bool effectsOn;

    void Start()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        musicOn = settingsInfo.musicOn;
        effectsOn = settingsInfo.effectsOn;
    }

    public void MusicOnOff()
    {
        audioManager.Play("ClickUI");
        musicOn = !musicOn;
        if (musicOn)
            audioManager.On("Theme");
        else
            audioManager.Off("Theme");
        settingsInfo.musicOn = musicOn;
    }

    public void EffectsOnOff()
    {
        audioManager.Play("ClickUI");
        effectsOn = !effectsOn;
        if (effectsOn)
            audioManager.On("Effects");
        else
            audioManager.Off("Effects");
        settingsInfo.effectsOn = effectsOn;
    }

    public void SettingsPanelClose()
    {
        settingsInfo.SaveSettings();
        audioManager.Play("ClickUI");
        GameButtons.SettingsState = false;
        gameObject.SetActive(GameButtons.SettingsState);
    }
}
