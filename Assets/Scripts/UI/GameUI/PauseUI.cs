using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Панель паузы")]
    [SerializeField] private GameObject pausePanel;

    [Tooltip("Панель настроек")]
    [SerializeField] private GameObject pauseSettingsPanel;

    [Tooltip("UI панели выхода")]
    [SerializeField] private GameObject exitPanel;
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
        audioManager.Play("ClickUI");
        Time.timeScale = 1f;
        settingsInfo.SaveSettings();
        GameButtons.IsGamePausedState = false;
        IsSettingsState = false;
        gameObject.SetActive(GameButtons.IsGamePausedState);
        pausePanel.GetComponent<MovementUI>().MoveToStart();
        pauseSettingsPanel.GetComponent<MovementUI>().SetStart();
        gameButtons.ShowHideControlUI(true);
    }

    public void SettingsPanelOpenClose()
    {
        audioManager.Play("ClickUI");
        IsSettingsState = !IsSettingsState;
        if (IsSettingsState)
        {
            pauseSettingsPanel.GetComponent<MovementUI>().MoveToEnd();
            settingsInfo.SaveSettings();
        }
        else
            pauseSettingsPanel.GetComponent<MovementUI>().MoveToStart();

    }

    public void GoToMenu()
    {
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
                currentGame.canExit = false;
                CurrentGameInfo.instance.SaveCurrentGame();
                SceneManager.LoadScene("Menu");
            }
            else
            {
                pausePanel.GetComponent<MovementUI>().MoveToStart();
                pauseSettingsPanel.GetComponent<MovementUI>().MoveToStart();
                exitPanel.GetComponent<MovementUI>().MoveToEnd();
                IsSettingsState = false;
            }
        }                 
    }

    public void GoToMenuExitPanel()
    {
        ClosePause();
        DeleteGame();
        SceneManager.LoadScene("Menu");
    }

    public void CloseExitPanel()
    {
        pausePanel.GetComponent<MovementUI>().MoveToEnd();
        exitPanel.GetComponent<MovementUI>().MoveToStart();
    }

    private void DeleteGame()
    {
        NewSaveSystem.Delete("character");
        NewSaveSystem.Delete("currentGame");
    }
}
