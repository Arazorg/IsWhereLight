﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishOfGameButton : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Текст о смерти")]
    [SerializeField] private TextMeshProUGUI finishOfGameText;

    [Tooltip("Задний фон стастики")]
    [SerializeField] private GameObject statsPanel;

    [Tooltip("Текст количества убитых врагов")]
    [SerializeField] private TextMeshProUGUI countKilledEnemiesText;

    [Tooltip("Текст количества выстрелов")]
    [SerializeField] private TextMeshProUGUI countShootsText;

    [Tooltip("Кнопка выхода в лобби")]
    [SerializeField] private Button goToLobby;

    [Tooltip("Кнопка выхода в игру")]
    [SerializeField] private Button goToMenu;
#pragma warning restore 0649

    public static int finishGameMoney;
    private ProgressInfo progressInfo;
    private CurrentGameInfo currentGameInfo;
    private bool isShow;
    private float timeToShow;
    void Start()
    {
        progressInfo = GameObject.Find("ProgressHandler").GetComponent<ProgressInfo>();
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        progressInfo.playerMoney += finishGameMoney;
        progressInfo.SaveProgress();

        statsPanel.GetComponent<MovementUI>().MoveToEnd();
        finishOfGameText.GetComponent<MovementUI>().MoveToEnd();
        goToLobby.GetComponent<MovementUI>().MoveToEnd();
        goToMenu.GetComponent<MovementUI>().MoveToEnd();
        countShootsText.GetComponent<MovementUI>().MoveToEnd();
        countKilledEnemiesText.GetComponent<MovementUI>().MoveToEnd();
        timeToShow = Time.time + 0.1f;
        isShow = false;
        SaveSystem.DeleteCurrentGame();
    }

    void Update()
    {
        if(Time.time > timeToShow && !isShow)
        {
            countKilledEnemiesText.text += currentGameInfo.countKilledEnemy.ToString();
            countShootsText.text += currentGameInfo.countShoots.ToString();
            isShow = true;
        }
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GoToLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
