﻿using System.Collections;
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
        ClosePause();
        var charInfo = GameObject.Find("Character(Clone)").GetComponent<CharInfo>();
        if (SceneManager.GetActiveScene().name != "Game")
        {
            DeleteCurrentGame();
        }
        else
        {     
            if(charInfo.canExit)
            {
                charInfo.canExit = false;
                charInfo.SaveChar();
            }
            else
            {
                
            }
        }          
        SceneManager.LoadScene("Menu");
    }

    private void DeleteCurrentGame()
    {
        NewSaveSystem.Delete("character");
        NewSaveSystem.Delete("currentGame");
    }
}
