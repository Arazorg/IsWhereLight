using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [Tooltip("Кнопка паузы")]
    [SerializeField] private Button pauseButton;

    [Tooltip("Панель настроек")]
    [SerializeField] private GameObject pauseSettingsPanel;

    private AudioManager audioManager;
    private SettingsInfo settingsInfo;
    private CharInfo charInfo;

    public static bool IsSettingsState;

    void Start()
    {
        charInfo = GameObject.Find("Character(Clone)").GetComponent<CharInfo>();
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        StartUIActive();
       
    }

    private void StartUIActive()
    {
        IsSettingsState = false;
        pauseSettingsPanel.SetActive(false);
    }
    public void ClosePause()
    {
        Time.timeScale = 1f;
        settingsInfo.SaveSettings();
        GameButtons.IsGamePausedState = false;
        GameButtons.IsGamePausedPanelState = false;
        gameObject.SetActive(GameButtons.IsGamePausedPanelState);
        IsSettingsState = false;
        pauseSettingsPanel.SetActive(PauseUI.IsSettingsState);
        pauseButton.gameObject.SetActive(true);
    }

    public void SettingsPanelOpenClose()
    {
        audioManager.Play("ClickUI");
        IsSettingsState = !IsSettingsState;
        pauseSettingsPanel.SetActive(IsSettingsState);
        if (IsSettingsState)
        {
            settingsInfo.SaveSettings();
        }
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        GameButtons.IsGamePausedState = false;
        GameButtons.IsGamePausedPanelState = false;
        settingsInfo.SaveSettings();
        if(SceneManager.GetActiveScene().name != "Game")
            SaveSystem.DeleteCurrentGame();

        SceneManager.LoadScene("Menu");
    }
}
