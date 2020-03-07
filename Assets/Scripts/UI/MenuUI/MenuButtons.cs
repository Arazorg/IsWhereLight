﻿using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject settings;
    public GameObject settingsButton;
    public GameObject interfaceSettings;
    public static bool firstPlay;
    public static bool firstRun;
    private int level;

    private SettingsInfo settingsInfo;

    private AudioManager audioManager;
    void Awake()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        settingsInfo.InitDictionary();
        if (File.Exists(SaveSystem.CurrentSettingsFile))
        {
            firstRun = false;
            settingsInfo.LoadSettings();
        }
        else
        {    
            settingsInfo.SetStartSettings();
            settingsInfo.SaveSettings();
            Debug.Log(SaveSystem.CurrentSettingsFile);
            firstRun = true;
        }

        level = SaveSystem.LoadSettings().level;
        settings.SetActive(false);
        settingsButton.SetActive(true);
        interfaceSettings.SetActive(false);
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("Theme");
    }

    public void NewGame()
    {
        audioManager.Play("ClickUI");
        firstPlay = true;
        SceneManager.LoadScene("Game");
    }

    public void ContinueGame()
    {
        audioManager.Play("ClickUI");
        firstPlay = false;
        SceneManager.LoadScene("Game");
    }

    public void LinkToVk()
    {
        audioManager.Play("ClickUI");
        Application.OpenURL("https://vk.com/arazorg");
    }

    public void LinkToTwitter()
    {
        audioManager.Play("ClickUI");
        Application.OpenURL("https://twitter.com/arazorg");
    }

    public void SettingsPanelOpen()
    {
        audioManager.Play("ClickUI");
        if (settingsButton.activeSelf == true)
        {
            settings.SetActive(true);
            settingsButton.SetActive(false);
        }
    }

    public void ExitGame()
    {
        audioManager.Play("ClickUI");
        Application.Quit();
    }

}
