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

    [Tooltip("UI панели закрытия текущей игры")]
    [SerializeField] private GameObject closeCurrentGamePanel;

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

    [Tooltip("Изображение типа игры, при наличии текущей игры")]
    [SerializeField] private Image currentGameTypeImage;

    [Tooltip("Текст волны текущей игры, при наличии текущей игры")]
    [SerializeField] private TextMeshProUGUI currentGameTypeText;

    [Tooltip("Текст подсказок")]
    [SerializeField] private TextMeshProUGUI hintsText;

    [Tooltip("Спрайты типов игры")]
    [SerializeField] private Sprite[] gameTypeList;
#pragma warning restore 0649

    //Переменные состояния игры
    public static bool firstPlay;
    public static bool firstRun;
    private bool showHint;

    //Скрипты
    private SettingsInfo settingsInfo;
    private ProgressInfo progressInfo;
    private AudioManager audioManager;
    private LocalizationManager localizationManager;

    private float timeToHint;
    private readonly float hintTime = 7.5f;
    private int currentHint = 0;

    void Awake()
    {
        //PlayerPrefs.DeleteAll();
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        progressInfo = GameObject.Find("ProgressHandler").GetComponent<ProgressInfo>();
        settingsInfo.InitDictionary();
        try
        {
            Destroy(GameObject.Find("CurrentGameHandler"));
            Destroy(GameObject.Find("LevelGeneration"));
        }
        catch { }
        FilesCheck();

        localizationManager = GameObject.Find("LocalizationManager").GetComponent<LocalizationManager>();
        localizationManager.LoadLocalizedText(settingsInfo.currentLocalization);
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("Theme");
        SetStartObjectsActive();
        Camera.main.backgroundColor = Color.black;
        NewHint();
        currentHint++;
    }

    private void FilesCheck()
    {
        settingsInfo.LoadSettings();
        progressInfo.LoadProgress();
        progressInfo.SaveProgress();
        if (PlayerPrefs.HasKey($"currentGame")
                && PlayerPrefs.HasKey($"character"))
        {
            firstPlay = false;
            newGameButton.GetComponent<MovementUI>().SetStartPos(new Vector3(-350, -250));
            newGameButton.GetComponent<MovementUI>().SetEndPos(new Vector3(-350, 250));
        }
        else
        {
            DeleteCurrentGame();
            newGameButton.GetComponent<MovementUI>().SetEndPos(new Vector3(0, 250));
            firstPlay = true;
        }
    }

    private void SetStartObjectsActive()
    {
        if (!firstPlay)
        {
            CurrentGameInfo currentGameInfo = new CurrentGameInfo();
            currentGameInfo.LoadCurrentGame();
            continueButton.GetComponent<MovementUI>().MoveToEnd();
            currentGameTypeImage.sprite = gameTypeList[currentGameInfo.challengeNumber];
            currentGameTypeText.GetComponent<LocalizedText>().addable = currentGameInfo.currentWave.ToString();
            Destroy(currentGameInfo);
        }

        newGameButton.GetComponent<MovementUI>().MoveToEnd();
        settingsButton.GetComponent<MovementUI>().MoveToEnd();
        VkButton.GetComponent<MovementUI>().MoveToEnd();
        TwitterButton.GetComponent<MovementUI>().MoveToEnd();
        hintsText.GetComponent<MovementUI>().MoveToEnd();
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
        if (!firstPlay)
            closeCurrentGamePanel.GetComponent<MovementUI>().MoveToEnd();
        else
        {
            firstPlay = true;
            SceneManager.LoadScene("Lobby");
        }
    }

    public void ContinueGame()
    {
        audioManager.Play("ClickUI");
        firstPlay = false;
        SceneManager.LoadScene("Game");
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

    public void DeleteCurrentGameStartNewGame()
    {
        DeleteCurrentGame();
        audioManager.Play("ClickUI");
        firstPlay = true;
        SceneManager.LoadScene("Lobby");
    }

    public void CloseCurrentGamePanel()
    {
        audioManager.Play("ClickUI");
        closeCurrentGamePanel.GetComponent<MovementUI>().MoveToStart();
    }

    public void AllPanelHide()
    {
        audioManager.Play("ClickUI");
        exitButton.GetComponent<MovementUI>().MoveToStart();
        settingsPanel.GetComponent<MovementUI>().MoveToStart();
        settingsButton.GetComponent<MovementUI>().MoveToEnd();
        secretCode.GetComponent<MovementUI>().MoveToStart();
        localizationPanel.GetComponent<MovementUI>().MoveToStart();
        closeCurrentGamePanel.GetComponent<MovementUI>().MoveToStart();
    }

    public void ExitGame()
    {
        audioManager.Play("ClickUI");
        Application.Quit();
    }

    private void DeleteCurrentGame()
    {
        PlayerPrefs.DeleteKey("character");
        PlayerPrefs.DeleteKey("currentGame");
    }
}
