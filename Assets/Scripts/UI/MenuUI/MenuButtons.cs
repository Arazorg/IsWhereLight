using System.IO;
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
    [SerializeField] private GameObject secretCode;

    [Tooltip("Кнопка 'новая игра'")]
    [SerializeField] private Button newGameButton;

    [Tooltip("Кнопка 'продолжить игру'")]
    [SerializeField] private Button continueButton;

    [Tooltip("Кнопка настроек")]
    [SerializeField] private Button settingsButton;

    [Tooltip("Кнопка 'VK'")]
    [SerializeField] private Button VkButton;

    [Tooltip("Кнопка 'Twitter'")]
    [SerializeField] private Button TwitterButton;

    [Tooltip("Кнопка выхода из игры")]
    [SerializeField] private Button exitButton;
#pragma warning restore 0649

    //Переменные состояния игры
    public static bool firstPlay;
    public static bool firstRun;

    //Скрипты
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
        Camera.main.backgroundColor = Color.black;
    }
    private void FilesCheck()
    {
        if (File.Exists(SaveSystem.CurrentSettingsFile))
            settingsInfo.LoadSettings();
        else
            settingsInfo.SetStartSettings();

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
            newGameButton.GetComponent<MovementUI>().startPos = new Vector3(-350, -250);
            firstPlay = false;
        }
        else
        {
            newGameButton.GetComponent<MovementUI>().startPos = new Vector3(0, -250);
            firstPlay = true;
        }
    }

    private void SetStartObjectsActive()
    {
        newGameButton.GetComponent<MovementUI>().MoveToEnd();
        if (!firstPlay)
            continueButton.GetComponent<MovementUI>().MoveToEnd();
           
        settingsButton.GetComponent<MovementUI>().MoveToEnd();
        VkButton.GetComponent<MovementUI>().MoveToEnd();
        TwitterButton.GetComponent<MovementUI>().MoveToEnd();
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android
            || Application.platform == RuntimePlatform.IPhonePlayer
                || Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                exitButton.GetComponent<MovementUI>().MoveToEnd();
            }
        }
    }

    public void NewGame()
    {
        audioManager.Play("ClickUI");
        firstPlay = true;
        SaveSystem.DeleteCurrentGame();
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
        settingsButton.GetComponent<MovementUI>().MoveToStart();
        settingsPanel.GetComponent<MovementUI>().MoveToEnd();
    }

    public void AllPanelHide()
    {
        audioManager.Play("ClickUI");
        exitButton.GetComponent<MovementUI>().MoveToStart();
        settingsPanel.GetComponent<MovementUI>().MoveToStart();
        settingsButton.GetComponent<MovementUI>().MoveToEnd();
        secretCode.GetComponent<MovementUI>().MoveToStart();
        localizationPanel.GetComponent<MovementUI>().MoveToStart();
    }

    public void ExitGame()
    {
        audioManager.Play("ClickUI");
        Application.Quit();
    }
}
