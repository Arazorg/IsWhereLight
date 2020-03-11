using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    public GameObject settings;
    public GameObject localization;
    public GameObject secretCode;
    public GameObject interfaceSettings;

    public Button settingsButton;
    public Button exitButton;
    public Button continueButton;
    public Button newGameButton;
    public InputField secretCodeField;
    
    public static bool firstPlay;
    public static bool firstRun;

    private SettingsInfo settingsInfo;
    private ProgressInfo progressInfo;
    private AudioManager audioManager;
    private LocalizationManager localizationManager;

    void Awake()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        progressInfo = GameObject.Find("ProgressHandler").GetComponent<ProgressInfo>();
        settingsInfo.InitDictionary();
        FilesCheck();
        localizationManager = GameObject.Find("LocalizationManager").GetComponent<LocalizationManager>();
        localizationManager.LoadLocalizedText(settingsInfo.currentLocalization);
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("Theme");
        SetStartObjectsActive();
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                exitButton.gameObject.SetActive(true);
            }
        }
    }

    private void SetStartObjectsActive()
    {
        exitButton.gameObject.SetActive(false);
        localization.SetActive(false);
        secretCode.SetActive(false);
        settings.SetActive(false);
        settingsButton.gameObject.SetActive(true);
        interfaceSettings.SetActive(false);
    }

    private void FilesCheck()
    {
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
            continueButton.gameObject.SetActive(true);
            firstPlay = false;
        }
        else
        {
            newGameButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 200);
            continueButton.gameObject.SetActive(false);
            firstPlay = true;
        }
    }

    public void CheckSecretCode()
    {
        Debug.Log(progressInfo.CheckSecretCode(secretCodeField.text));
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
        if (settingsButton.gameObject.activeSelf == true)
        {
            settings.SetActive(true);
            settingsButton.gameObject.SetActive(false);
        }
    }

    public void AllPanelClose()
    {
        localization.SetActive(false);
        secretCode.SetActive(false);
        settings.SetActive(false);
        exitButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        audioManager.Play("ClickUI");
        Application.Quit();
    }
}
