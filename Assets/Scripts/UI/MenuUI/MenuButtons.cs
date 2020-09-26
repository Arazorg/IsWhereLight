﻿using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("UI панели настроек")]
    [SerializeField] private GameObject settingsPanel;

    [Tooltip("UI панели локализации")]
    [SerializeField] private GameObject localizationPanel;

    [Tooltip("UI панели секретного кода")]
    [SerializeField] private GameObject secretCodePanel;

    [Tooltip("UI панели секретного кода")]
    [SerializeField] private GameObject achivmentPanel;

    [Tooltip("Кнопка 'новая игра'")]
    [SerializeField] private Button newGameButton;

    [Tooltip("Кнопка настроек")]
    [SerializeField] private Button settingsButton;

    [Tooltip("Кнопка 'VK'")]
    [SerializeField] private Button vkButton;

    [Tooltip("Кнопка 'Twitter'")]
    [SerializeField] private Button twitterButton;

    [Tooltip("Кнопка выхода из игры")]
    [SerializeField] private Button exitButton;

    [Tooltip("Текст подсказок")]
    [SerializeField] private TextMeshProUGUI hintsText;

    [Tooltip("Скрипт настроек в меню")]
    [SerializeField] private SettingsButtons settingsButtons;
#pragma warning restore 0649

    //Переменные состояния игры
    public static bool firstRun;
    public static bool firstPlay;
    private bool showHint;

    //Скрипты
    private SettingsInfo settingsInfo;
    private ProgressInfo progressInfo;
    private AudioManager audioManager;
    private LocalizationManager localizationManager;

    private float timeToHint;
    private readonly float hintTime = 7.5f;
    private int currentHint = 0;
    private bool isAllPanelHide;
    private float isAllPanelHideTime;
    private readonly float achivmentTime = 2f;
    private float timeToAchivment;

    void Awake()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        progressInfo = GameObject.Find("ProgressHandler").GetComponent<ProgressInfo>();
        localizationManager = GameObject.Find("LocalizationManager").GetComponent<LocalizationManager>();

        try
        {
            Destroy(GameObject.Find("CurrentGameHandler"));
            Destroy(GameObject.Find("LevelGeneration"));
        }
        catch { }

        FilesCheck();
        localizationManager.LoadLocalizedText(settingsInfo.currentLocalization);
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.PlayAllSounds();
        audioManager.Play("Theme");
        SetStartObjectsActive();
        Camera.main.backgroundColor = Color.black;
        NewHint();
        currentHint++;
        isAllPanelHideTime = float.MinValue;
    }

    void Update()
    {
        if (Time.time > isAllPanelHideTime)
        {
            isAllPanelHide = true;
            isAllPanelHideTime = Time.time + 0.5f;
        }
        if (Application.platform == RuntimePlatform.Android
            || Application.platform == RuntimePlatform.IPhonePlayer
                || Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                exitButton.GetComponent<MovementUI>().MoveToEnd();
            }
        }

        if (showHint)
        {
            Color color = hintsText.color;
            color.a += 2.5f * Time.deltaTime;
            hintsText.color = color;
            if (color.a >= 1)
                showHint = false;
        }
        else if (Time.time > timeToHint && !showHint)
        {
            Color color = hintsText.color;
            color.a -= 2.5f * Time.deltaTime;
            hintsText.color = color;
            if (color.a <= 0)
            {
                NewHint();
                currentHint++;
            }
        }

        if (Time.time > timeToAchivment)
        {
            achivmentPanel.GetComponent<MovementUI>().MoveToStart();
            timeToAchivment = float.MaxValue;
        }
    }

    private void FilesCheck()
    {
        settingsInfo.LoadSettings();
        progressInfo.LoadProgress();

        if (!PlayerPrefs.HasKey("settings"))
        {
            if (progressInfo.NewAchivment("FirstStartAchivment"))
            {
                localizationManager.LoadLocalizedText(settingsInfo.currentLocalization, false);
                achivmentPanel.GetComponent<MovementUI>().MoveToEnd();
                achivmentPanel.GetComponentInChildren<LocalizedText>().key = "FirstStartAchivment";
                achivmentPanel.GetComponentInChildren<LocalizedText>().SetLocalization();
                timeToAchivment = Time.time + achivmentTime;
            }
        }
        settingsInfo.SaveSettings();
        progressInfo.SaveProgress();
    }

    private void SetStartObjectsActive()
    {
        newGameButton.GetComponent<MovementUI>().MoveToEnd();
        settingsButton.GetComponent<MovementUI>().MoveToEnd();
        vkButton.GetComponent<MovementUI>().MoveToEnd();
        twitterButton.GetComponent<MovementUI>().MoveToEnd();
        hintsText.GetComponent<MovementUI>().MoveToEnd();
    }

    private void NewHint()
    {
        Color color = hintsText.color;
        color.a = 0;
        hintsText.color = color;
        hintsText.GetComponent<LocalizedText>().key = $"HintMenu{currentHint % 3}";
        hintsText.GetComponent<LocalizedText>().SetLocalization();
        showHint = true;
        timeToHint = Time.time + hintTime;
    }

    public void NewGame()
    {
        audioManager.Play("ClickUI");
        firstPlay = true;
        SceneManager.LoadScene("Lobby");
    }

    public void LinkToVk()
    {
        //audioManager.Play("ClickUI");
        Application.OpenURL("https://vk.com/arazorg");
    }

    public void LinkToTwitter()
    {
        // audioManager.Play("ClickUI");
        Application.OpenURL("https://twitter.com/arazorg");
    }

    public void SettingsPanelOpen()
    {
        audioManager.Play("ClickUI");
        audioManager.Play("ClickUI");
        settingsButton.GetComponent<MovementUI>().MoveToStart();
        settingsPanel.GetComponent<MovementUI>().MoveToEnd();
    }

    public void AllPanelHide()
    {
        if (isAllPanelHide)
        {
            exitButton.GetComponent<MovementUI>().MoveToStart();
            settingsPanel.GetComponent<MovementUI>().MoveToStart();
            settingsButton.GetComponent<MovementUI>().MoveToEnd();
            secretCodePanel.GetComponent<MovementUI>().MoveToStart();
            localizationPanel.GetComponent<MovementUI>().MoveToStart();
            settingsButtons.IsLocalizationPanelState = false;
            settingsButtons.IsSecretPanelState = false;
            isAllPanelHide = false;
        }
    }

    public void ExitGame()
    {
        audioManager.Play("ClickUI");
        Application.Quit();
    }

    public void DeleteAllKeys()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}
