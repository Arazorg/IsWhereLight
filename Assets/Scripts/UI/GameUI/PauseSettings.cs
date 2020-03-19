using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSettings : MonoBehaviour
{
    [Tooltip("UI локализации")]
    [SerializeField] private GameObject localizationPanel;

    [Tooltip("UI паузы")]
    [SerializeField] private GameObject pausePanel;

    //Скрипты
    private SettingsInfo settingsInfo;
    private AudioManager audioManager;
    private LocalizationManager localizationManager;

    //Переменные состояния
    private bool musicOn;
    private bool effectsOn;
    public static bool IsLocalizationPanelState;

    void Start()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        localizationManager = GameObject.Find("LocalizationManager").GetComponent<LocalizationManager>();
        musicOn = settingsInfo.musicOn;
        effectsOn = settingsInfo.effectsOn;
        StartUIActive();
    }

    private void StartUIActive()
    {
        IsLocalizationPanelState = false;
        localizationPanel.SetActive(false);
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

    public void OpenCloseLocalizationPanel()
    {
        IsLocalizationPanelState = !IsLocalizationPanelState;
        localizationPanel.SetActive(IsLocalizationPanelState);

        GameButtons.IsGamePausedPanelState = !IsLocalizationPanelState;
        pausePanel.SetActive(GameButtons.IsGamePausedPanelState);

    }

    public void ChangeLanguage(string fileName)
    {
        localizationManager.LoadLocalizedText(fileName);
    }
}
