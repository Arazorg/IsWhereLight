using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSettings : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("UI локализации")]
    [SerializeField] private GameObject localizationPanel;

    [Tooltip("UI паузы")]
    [SerializeField] private GameObject pausePanel;
#pragma warning restore 0649

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
        IsLocalizationPanelState = false;
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
        if (IsLocalizationPanelState)
        {
            localizationPanel.GetComponent<MovementUI>().MoveToEnd();
            pausePanel.GetComponent<MovementUI>().MoveToStart();
        } 
        else
        {
            localizationPanel.GetComponent<MovementUI>().MoveToStart();
            pausePanel.GetComponent<MovementUI>().MoveToEnd();
        }
    }

    public void ChangeLanguage(string fileName)
    {
        localizationManager.LoadLocalizedText(fileName);
    }
}
