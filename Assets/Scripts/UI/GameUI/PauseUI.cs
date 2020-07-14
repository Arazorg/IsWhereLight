using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Кнопка паузы")]
    [SerializeField] private Button pauseButton;

    [Tooltip("Панель паузы")]
    [SerializeField] private GameObject pausePanel;

    [Tooltip("Панель настроек")]
    [SerializeField] private GameObject pauseSettingsPanel;
#pragma warning restore 0649

    private AudioManager audioManager;
    private SettingsInfo settingsInfo;
    private CharInfo charInfo;

    public static bool IsSettingsState;

    void Start()
    {
        IsSettingsState = false;
        charInfo = GameObject.Find("Character(Clone)").GetComponent<CharInfo>();
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public void ClosePause()
    {
        audioManager.Play("ClickUI");
        Time.timeScale = 1f;
        settingsInfo.SaveSettings();
        GameButtons.IsGamePausedState = false;

        gameObject.SetActive(GameButtons.IsGamePausedState);
        pausePanel.GetComponent<MovementUI>().MoveToStart();
        pauseSettingsPanel.GetComponent<MovementUI>().MoveToStart();
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
        ClosePause();
        if(SceneManager.GetActiveScene().name != "Game")
            SaveSystem.DeleteCurrentGame();
        SceneManager.LoadScene("Menu");
    }
}
