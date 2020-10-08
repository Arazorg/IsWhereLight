using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Панель паузы")]
    [SerializeField] private GameObject pausePanel;

    [Tooltip("Панель громкости звуков")]
    [SerializeField] private GameObject soundVolumePanel;

    [Tooltip("Панель настроек")]
    [SerializeField] private GameObject pauseSettingsPanel;

    [Tooltip("UI панели выхода")]
    [SerializeField] private GameObject exitPanel;

    [Tooltip("Текст настроек паузы")]
    [SerializeField] private TextMeshProUGUI settingsText;
#pragma warning restore 0649
    public static bool IsSettingsState;
    public float timeToClose;

    private AudioManager audioManager;
    private SettingsInfo settingsInfo;
    private GameButtons gameButtons;

    void Start()
    {
        IsSettingsState = false;
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        gameButtons = GameObject.Find("CharacterControlUI").GetComponent<GameButtons>();
    }

    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Escape) 
                && Time.realtimeSinceStartup > timeToClose)
            ClosePause();
    }

    public void ClosePause()
    {
        audioManager.PlayAllSounds();
        audioManager.Play("ClickUI");
        settingsText.GetComponent<MovementUI>().SetStart();
        soundVolumePanel.GetComponent<MovementUI>().SetStart();
        if(PauseSettings.IsLocalizationPanelState)
            pauseSettingsPanel.GetComponent<PauseSettings>().OpenCloseLocalizationPanel(true);
        if (SceneManager.GetActiveScene().name == "Game")
            exitPanel.GetComponent<MovementUI>().SetStart();
        Time.timeScale = 1f;
        settingsInfo.SaveSettings();
        GameButtons.IsGamePausedState = false;
        IsSettingsState = false;
        pausePanel.GetComponent<MovementUI>().SetStart();
        pauseSettingsPanel.GetComponent<MovementUI>().SetStart();
        gameObject.SetActive(GameButtons.IsGamePausedState);
        gameButtons.ShowHideControlUI(true);
    }

    public void SettingsPanelOpenClose()
    {
        audioManager.Play("ClickUI");
        IsSettingsState = !IsSettingsState;
        if (IsSettingsState)
        {
            if (settingsInfo.musicOn || settingsInfo.effectsOn)
                soundVolumePanel.GetComponent<MovementUI>().MoveToEnd();
            pauseSettingsPanel.GetComponent<MovementUI>().MoveToEnd();
            settingsInfo.SaveSettings();
        }
        else
        {
            soundVolumePanel.GetComponent<MovementUI>().MoveToStart();
            settingsText.GetComponent<MovementUI>().SetStart();
            pauseSettingsPanel.GetComponent<MovementUI>().MoveToStart();
        }
    }

    public void GoToMenu()
    {
        audioManager.StopAllSounds();
        settingsText.GetComponent<MovementUI>().SetStart();
        var currentGame = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        if (SceneManager.GetActiveScene().name != "Game")
        {
            ClosePause();
            DestroyGameObjects();
            SceneManager.LoadScene("Menu");
        }
        else
        {
            audioManager.Play("ClickUI");
            pausePanel.GetComponent<MovementUI>().MoveToStart();
            pauseSettingsPanel.GetComponent<MovementUI>().MoveToStart();
            soundVolumePanel.GetComponent<MovementUI>().MoveToStart();
            exitPanel.GetComponent<MovementUI>().MoveToEnd();
            IsSettingsState = false;
        }
    }

    public void GoToFinishGame()
    {
        CharAction.isDeath = false;
        ClosePause();
        SceneManager.LoadScene("FinishGame");
    }

    public void CloseExitPanel()
    {
        audioManager.Play("ClickUI");
        pausePanel.GetComponent<MovementUI>().MoveToEnd();
        exitPanel.GetComponent<MovementUI>().MoveToStart();
    }

    private void DestroyGameObjects()
    {
        Destroy(GameObject.Find("CurrentGameHandler"));
        Destroy(GameObject.Find("LevelGeneration"));
        Destroy(GameObject.Find("CharParametrsHandler"));
        Destroy(GameObject.Find("CharInfoHandler"));
        Destroy(GameObject.Find("CharAmplificationsHandler"));
    }
}
