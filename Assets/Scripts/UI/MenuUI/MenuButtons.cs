using System.IO;
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

    [Tooltip("UI панели настроек интерфейса")]
    [SerializeField] private GameObject interfaceSettings;

    [Tooltip("Кнопка настроек")]
    [SerializeField] private Button settingsButton;

    [Tooltip("Кнопка выхода из игры")]
    [SerializeField] private Button exitButton;

    [Tooltip("Кнопка 'продолжить игру'")]
    [SerializeField] private Button continueButton;

    [Tooltip("Кнопка 'новая игра'")]
    [SerializeField] private Button newGameButton;

    [Tooltip("Скорость кнопок")]
    [SerializeField] private float buttonSpeed;
#pragma warning restore 0649

    //Переменные состояния игры
    public static bool firstPlay;
    public static bool firstRun;

    //Переменные состояния UI элементов
    public static bool IsSettingPanelState;

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
        localizationPanel.SetActive(false);
        secretCode.SetActive(false);
        settingsPanel.SetActive(false);
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
            newGameButton.GetComponent<ButtonActive>().startPos = new Vector3(-350, -250);
            continueButton.gameObject.SetActive(true);
            firstPlay = false;
        }
        else
        {
            newGameButton.GetComponent<ButtonActive>().startPos = new Vector3(0, -250);
            continueButton.gameObject.SetActive(false);
            firstPlay = true;
        }
    }
    public void NewGame()
    {
        firstPlay = true;
        SaveSystem.DeleteCurrentGame();
        audioManager.Play("ClickUI");
        SceneManager.LoadScene("Lobby");
    }

    public void ContinueGame()
    {
        firstPlay = false;
        audioManager.Play("ClickUI");
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
            IsSettingPanelState = true;
            settingsPanel.SetActive(IsSettingPanelState);
            settingsButton.gameObject.SetActive(!IsSettingPanelState);
        }
    }

    public void AllPanelClose()
    {
        audioManager.Play("ClickUI");
        localizationPanel.SetActive(false);
        secretCode.SetActive(false);
        settingsPanel.SetActive(false);
        exitButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        audioManager.Play("ClickUI");
        Application.Quit();
    }
}
