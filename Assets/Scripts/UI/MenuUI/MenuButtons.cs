﻿using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    public GameObject settings;
    public GameObject settingsButton;
    public InputField secretCodeField;
    public GameObject secretCodePanel;
    public GameObject interfaceSettings;
    public GameObject exitButton;
    public GameObject continueButton;
    public GameObject newGameButton;

    public static bool firstPlay;
    public static bool firstRun;

    private SettingsInfo settingsInfo;
    private ProgressInfo progressInfo;
    private AudioManager audioManager;

    void Awake()
    {
        exitButton.SetActive(false);
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        progressInfo = GameObject.Find("ProgressHandler").GetComponent<ProgressInfo>();
        settingsInfo.InitDictionary();

        if (File.Exists(SaveSystem.CurrentSettingsFile))
        {
            settingsInfo.LoadSettings();
        }
        else
        {
            settingsInfo.SetStartSettings();
        }

        if (File.Exists(SaveSystem.ProgressFile))
        {
            progressInfo.LoadProgress();
            firstRun = false;
        }
        else
        {
            progressInfo.SetStartProgress();
            firstRun = true;
        }

        if (File.Exists(SaveSystem.CurrentGameFile))
        {
            newGameButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(-400, 200);
            continueButton.SetActive(true);
            firstPlay = false;
        }
        else
        {
            newGameButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 200);
            continueButton.SetActive(false);
            firstPlay = true;
        }

        secretCodePanel.SetActive(false);
        settings.SetActive(false);
        settingsButton.SetActive(true);
        interfaceSettings.SetActive(false);
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("Theme");
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                exitButton.SetActive(true);
            }
        }
    }

    public void NewGame()
    {
        audioManager.Play("ClickUI");
        firstPlay = true;
        SceneManager.LoadScene("Lobby");
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

    public void SecretCodePanelOpen()
    {
        secretCodePanel.SetActive(true);
    }

    public void SecretCodePanelClose()
    {
        secretCodePanel.SetActive(false);
    }

    public void CheckSecretCode()
    {
        Debug.Log(progressInfo.CheckSecretCode(secretCodeField.text));
    }

    public void AllPanelClose()
    {
        secretCodePanel.SetActive(false);
        settings.SetActive(false);
        settingsButton.SetActive(true);
        exitButton.SetActive(false);
    }

    public void ExitGame()
    {
        audioManager.Play("ClickUI");
        Application.Quit();
    }
}
