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

    private AudioManager audioManager;
    private SettingsInfo settingsInfo;
    private GameButtons gameButtons;

    public static bool IsSettingsState;

    void Start()
    {
        IsSettingsState = false;
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        gameButtons = GameObject.Find("CharacterControlUI").GetComponent<GameButtons>();         
    }

    public void ClosePause()
    {
        audioManager.PlayAllSounds();
        audioManager.Play("ClickUI");
        settingsText.GetComponent<MovementUI>().SetStart();
        soundVolumePanel.GetComponent<MovementUI>().SetStart();
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
            DeleteGame();
            SceneManager.LoadScene("Menu");
        }
        else
        {     
            if(currentGame.canExit)
            {
                ClosePause();
                CharAction.isDeath = false;
                currentGame.canExit = false;
                CurrentGameInfo.instance.SaveCurrentGame();
                SceneManager.LoadScene("Menu");
            }
            else
            {
                audioManager.Play("ClickUI");
                pausePanel.GetComponent<MovementUI>().MoveToStart();
                pauseSettingsPanel.GetComponent<MovementUI>().MoveToStart();
                exitPanel.GetComponent<MovementUI>().MoveToEnd();
                IsSettingsState = false;
            }
        }                 
    }

    public void GoToMenuExitPanel()
    {
        CharAction.isDeath = false;
        ClosePause();
        DeleteGame();
        SceneManager.LoadScene("Menu");
    }

    public void CloseExitPanel()
    {
        audioManager.Play("ClickUI");
        pausePanel.GetComponent<MovementUI>().MoveToEnd();
        exitPanel.GetComponent<MovementUI>().MoveToStart();
    }

    private void DeleteGame()
    {
        NewSaveSystem.Delete("character");
        NewSaveSystem.Delete("currentGame");
    }
}
