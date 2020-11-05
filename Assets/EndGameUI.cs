using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Бэкграунд панели выигрыша")]
    [SerializeField] private Image endGamePanelBG;

    [Tooltip("Свечение(бэкграунд) панели выигрыша")]
    [SerializeField] private Image endGamePanelGlow;

    [Tooltip("Текст панели выигрыша")]
    [SerializeField] private TextMeshProUGUI endGamePanelText;

    [Tooltip("Кнопка выхода в лобби")]
    [SerializeField] private GameObject lobbyButton;

    [Tooltip("Префаб звезды")]
    [SerializeField] private GameObject starPrefab;

    [Tooltip("Место спауна звезды")]
    [SerializeField] private Transform starSpawnPosition;

    [Tooltip("Места окончания пути звезды")]
    [SerializeField] private Transform[] starsEndPositions;
#pragma warning restore 0649

    void Start()
    {
        SetUI();
    }

    private void SetUI()
    {
        foreach (var image in GetComponentsInChildren<Image>())
        {
            var color = image.color;
            color.a = 0f;
            image.color = color;
        }
        gameObject.SetActive(false);
    }

    public void OpenPanel(bool isWin)
    {
        gameObject.SetActive(true);
        if (isWin)
            endGamePanelText.GetComponent<LocalizedText>().key = "FinishOfGameWin";
        else
            endGamePanelText.GetComponent<LocalizedText>().key = "FinishOfGameLose";
        endGamePanelText.GetComponent<LocalizedText>().SetLocalization();
        ShowEndGamePanel();
    }

    private void ShowEndGamePanel()
    {
        InvokeRepeating("UpdatingUIColor", 1f, 0.05f);
        Invoke("EnableEndGamePanels", 0.25f);
    }

    private void EnableEndGamePanels()
    {
        gameObject.SetActive(true);
        lobbyButton.GetComponentInChildren<MovementUI>().MoveToEnd();
    }

    private void UpdatingUIColor()
    {
        var colorGlow = endGamePanelGlow.GetComponent<Image>().color;
        var colorBg = endGamePanelBG.GetComponent<Image>().color;
        var colorText = endGamePanelText.GetComponent<TextMeshProUGUI>().color;

        colorGlow.a += 0.05f;
        colorBg.a += 0.02f;
        colorText.a += 0.05f;

        endGamePanelGlow.GetComponent<Image>().color = colorGlow;
        endGamePanelBG.GetComponent<Image>().color = colorBg;
        endGamePanelText.GetComponent<TextMeshProUGUI>().color = colorText;
    }

    private void StarSpawn()
    {
        var currentStar = Instantiate(starPrefab, starSpawnPosition.position, Quaternion.identity);
        currentStar.GetComponent<MovementUI>().MoveToEnd();
    }

    public void GoToLobby()
    {
        GameObject.Find("Character(Clone)").GetComponent<CharController>().SetZeroSpeed(false);
        SceneManager.LoadScene("Lobby");
        Destroy(EnemySpawner.instance);
    }
}
