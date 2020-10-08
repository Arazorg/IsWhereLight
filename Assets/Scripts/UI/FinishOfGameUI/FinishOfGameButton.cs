using System.Collections;
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

    [Tooltip("Задний фон статистики")]
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
    private bool isShow;
    private float timeToShow;
    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.PlayAllSounds();
        progressInfo = GameObject.Find("ProgressHandler").GetComponent<ProgressInfo>();
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
        NewSaveSystem.Delete("character");
        NewSaveSystem.Delete("currentGame");
        audioManager.Play("Theme");
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
            GoToLobby();

        if (Time.time > timeToShow && !isShow)
        {
            countKilledEnemiesText.text += progressInfo.currentCountKilledEnemies;
            countShootsText.text += progressInfo.currentCountShoots;
            progressInfo.countShoots += progressInfo.currentCountShoots;
            progressInfo.countKilledEnemies += progressInfo.currentCountKilledEnemies;
            progressInfo.SaveProgress();
            isShow = true;
        }
    }
    public void GoToMenu()
    {
        audioManager.Play("ClickUI");
        DestroyGameObjects();
        SceneManager.LoadScene("Menu");
    }

    public void GoToLobby()
    {
        audioManager.Play("ClickUI");
        DestroyGameObjects();
        SceneManager.LoadScene("Lobby");
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
