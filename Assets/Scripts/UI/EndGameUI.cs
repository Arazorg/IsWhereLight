using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Бэкграунд панели выигрыша")]
    [SerializeField] private Image endGamePanelBG;

    [Tooltip("Текст панели выигрыша")]
    [SerializeField] private TextMeshProUGUI endGamePanelText;

    [Tooltip("Текст количества убитых врагов")]
    [SerializeField] private TextMeshProUGUI countEnemiesText;

    [Tooltip("Текст времени проведенного на арене")]
    [SerializeField] private TextMeshProUGUI durationText;

    [Tooltip("Текст количества выстрелов")]
    [SerializeField] private TextMeshProUGUI countShootsText;

    [Tooltip("Текст биома и арены")]
    [SerializeField] private TextMeshProUGUI biomeLevelText;

    [Tooltip("Кнопка выхода в лобби")]
    [SerializeField] private GameObject lobbyButton;
#pragma warning restore 0649

    public static bool isEndGamePanelOpen;
    private bool isResultSet = false;
    private string biomeAndLevel = "";
    

    void Start()
    {
        AudioManager.instance.PlayAllSounds();
        SetUI();
        AudioManager.instance.Play("Theme");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            GoToScene("Lobby");
    }

    private void SetUI()
    {
        isEndGamePanelOpen = false;
        foreach (var image in GetComponentsInChildren<Image>())
        {
            var color = image.color;
            color.a = 0f;
            image.color = color;
            image.raycastTarget = false;
        }

        foreach (var text in GetComponentsInChildren<TextMeshProUGUI>())
        {
            var color = text.color;
            color.a = 0f;
            text.color = color;
            text.raycastTarget = false;
        }
    }

    public void SetResults(bool isWin, float gameDuration)
    {
        gameObject.SetActive(true);
        if (!isResultSet)
        {
            isResultSet = true;
            var countKilledEnemies = CharInfo.instance.currentCountKilledEnemies;
            var countShoots = CharInfo.instance.currentCountShoots;
            countEnemiesText.text = countKilledEnemies.ToString();
            countShootsText.text = countShoots.ToString();

            string timeString = "";
            var timeSpan = System.TimeSpan.FromSeconds(gameDuration);
            if (timeSpan.Hours != 0)
                timeString += $"{timeSpan.Hours}h, ";
            if (timeSpan.Minutes != 0)
                timeString += $"{timeSpan.Minutes}m, ";
            if (timeSpan.Seconds != 0)
                timeString += $"{timeSpan.Seconds}s";

            durationText.text = timeString;
            ProgressInfo.instance.countShoots += countShoots;
            ProgressInfo.instance.countKilledEnemies += countKilledEnemies;
            ProgressInfo.instance.SaveProgress();

            OpenPanel(isWin);
        }
    }

    private void OpenPanel(bool isWin)
    {
        isEndGamePanelOpen = true;
        if (isWin)
            endGamePanelText.GetComponent<LocalizedText>().key = "FinishOfGameWin";
        else
            endGamePanelText.GetComponent<LocalizedText>().key = "FinishOfGameLose";
        endGamePanelText.GetComponent<LocalizedText>().SetLocalization();
        biomeAndLevel = System.Text.RegularExpressions.Regex.Replace(CurrentGameInfo.instance.challengeName, @"[\d-]", string.Empty);
        biomeLevelText.text = biomeAndLevel;
        ShowEndGamePanel();
    }

    private void ShowEndGamePanel()
    {
        InvokeRepeating("UpdatingUIColor", 0.5f, 0.025f);
        Invoke("EnableEndGamePanels", 0.4f);
    }

    private void UpdatingUIColor()
    {
        foreach (var image in GetComponentsInChildren<Image>())
        {
            var color = image.color;
            color.a += 0.05f;
            image.color = color;
            image.raycastTarget = true;
        }

        foreach (var text in GetComponentsInChildren<TextMeshProUGUI>())
        {
            var color = text.color;
            color.a += 0.05f;
            text.color = color;
            text.raycastTarget = true;
        }
    }

    private void EnableEndGamePanels()
    {
        GetComponentInChildren<StarsImages>().FillStars(biomeAndLevel);
    }

    public void GoToScene(string scene)
    {
        Time.timeScale = 1f;
        AudioManager.instance.Play("ClickUI");
        GameObject.Find("Character(Clone)").GetComponent<CharController>().SetZeroSpeed(false);
        SceneManager.LoadScene(scene);
        DestroyGameObjects();
    }

    private void DestroyGameObjects()
    {
        Destroy(GameObject.Find("CurrentGameHandler"));
        Destroy(GameObject.Find("LevelGeneration"));
        Destroy(GameObject.Find("CharParametrsHandler"));
        Destroy(GameObject.Find("CharInfoHandler"));
        Destroy(GameObject.Find("CharAmplificationsHandler"));
        Destroy(GameObject.Find("EnemySpawner"));
    }
}
