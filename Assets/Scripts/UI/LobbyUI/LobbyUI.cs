using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Текст выбора персонажа")]
    [SerializeField] private LocalizedText characterText;

    [Tooltip("UI текста")]
    [SerializeField] private GameObject characterTextUI;

    [Tooltip("Панель доната")]
    [SerializeField] private GameObject donatePanel;

    [Tooltip("Кнопка назад в меню")]
    [SerializeField] private Button backToLobbyButton;

    [Tooltip("Кнопка магазина внутриигровых покупок")]
    [SerializeField] private Button shopButton;

    [Tooltip("UI управления персонажем")]
    [SerializeField] private GameObject characterControlUI;

    [Tooltip("Объект прибавления денег")]
    [SerializeField] private GameObject moneyText;
#pragma warning restore 0649

    public bool IsLobbyState
    {
        get { return isLobbyState; }
        set { isLobbyState = value; }
    }
    private bool isLobbyState;

    private string characterKey;
    
    void Start()
    {
        isLobbyState = true;
        NewSaveSystem.Delete("character");
        NewSaveSystem.Delete("currentGame");
        Camera.main.backgroundColor = Color.black;
        characterKey = "chooseCharacter";
        characterText.key = characterKey;
        characterText.SetLocalization();
        characterControlUI.SetActive(false);
        ShowLobby();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isLobbyState)
            BackToMenu();
    }

    public void HideLobby()
    {
        backToLobbyButton.GetComponent<MovementUI>().MoveToStart();
        shopButton.GetComponent<MovementUI>().MoveToStart();
        isLobbyState = false;
    }

    public void ShowLobby()
    {
        characterTextUI.GetComponent<MovementUI>().MoveToEnd();
        backToLobbyButton.GetComponent<MovementUI>().MoveToEnd();
        shopButton.GetComponent<MovementUI>().MoveToEnd();
        isLobbyState = true;
    }

    public void ShowDonatePanel()
    {
        donatePanel.GetComponent<MovementUI>().MoveToEnd();
        isLobbyState = false;
    }

    public void BackToMenu()
    {
        AudioManager.instance.StopAllSounds();
        AudioManager.instance.Play("ClickUI");
        SceneManager.LoadScene("Menu");
    }

    public void HideMoneyPanel()
    {
        moneyText.GetComponentInChildren<MovementUI>().SetStart();
        moneyText.SetActive(false);
    }
}
