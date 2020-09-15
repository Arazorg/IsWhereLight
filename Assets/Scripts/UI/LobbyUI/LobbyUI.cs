﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Текст выбора персонажа")]
    [SerializeField] private LocalizedText characterText;

    [Tooltip("UI текста")]
    [SerializeField] private GameObject characterTextUI;

    [Tooltip("Кнопка назад в меню")]
    [SerializeField] private Button backToLobbyButton;

    [Tooltip("Кнопка магазина внутриигровых покупок")]
    [SerializeField] private Button shopButton;

    [Tooltip("UI управления персонажем")]
    [SerializeField] private GameObject characterControlUI;
#pragma warning restore 0649

    private string characterKey;
    private AudioManager audioManager;
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        NewSaveSystem.Delete("character");
        NewSaveSystem.Delete("currentGame");
        Camera.main.backgroundColor = Color.black;
        characterKey = "chooseCharacter";
        characterText.key = characterKey;
        characterText.SetLocalization();
        characterControlUI.SetActive(false);
        ShowLobby();
    }

    public void HideLobby()
    {
        backToLobbyButton.GetComponent<MovementUI>().MoveToStart();
        shopButton.GetComponent<MovementUI>().MoveToStart();
    }

    public void ShowLobby()
    {
        characterTextUI.GetComponent<MovementUI>().MoveToEnd();
        backToLobbyButton.GetComponent<MovementUI>().MoveToEnd();
        shopButton.GetComponent<MovementUI>().MoveToEnd();
    }

    public void BackToMenu()
    {
        audioManager.StopAllSounds();
        audioManager.Play("ClickUI");
        SceneManager.LoadScene("Menu");
    }
}
