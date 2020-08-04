using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishOfGameButton : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Текст количества убитых врагов")]
    [SerializeField] private TextMeshProUGUI countKilledEnemiesText;

    [Tooltip("Текст количества выстрелов")]
    [SerializeField] private TextMeshProUGUI countShootsText;
#pragma warning restore 0649

    public static int finishGameMoney;
    private ProgressInfo progressInfo;
    private CurrentGameInfo currentGameInfo;
    private float showStatsTime;
    private bool isShow;
    void Start()
    {
        progressInfo = GameObject.Find("ProgressHandler").GetComponent<ProgressInfo>();
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        progressInfo.playerMoney += finishGameMoney;
        progressInfo.SaveProgress();
        showStatsTime = Time.time + 0.5f;
        isShow = false;
    }

    void Update()
    {
        if (Time.time > showStatsTime)
        {
            if (!isShow)
            {
                countKilledEnemiesText.text += currentGameInfo.countKilledEnemy.ToString();
                countShootsText.text += currentGameInfo.countShoots.ToString();
                SaveSystem.DeleteCurrentGame();
            }
            isShow = true;
        }
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
