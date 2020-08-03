using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterChooseUI : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Лобби UI")]
    [SerializeField] private GameObject lobbyUI;

    [Tooltip("Управление персонажем UI")]
    [SerializeField] private GameObject characterControlUI;

    [Tooltip("Кнопка перехода в игру")]
    [SerializeField] private Button goToGameButton;

    [Tooltip("Кнопка покупки персонажа")]
    [SerializeField] private Button buyButton;

    [Tooltip("Кнопка перехода в лобби")]
    [SerializeField] private Button backToLobbyButton;

    [Tooltip("Инфо бар")]
    [SerializeField] private GameObject InfoBar;

    [Tooltip("Кнопка предыдущего скина")]
    [SerializeField] private Button PrevSkinButton;

    [Tooltip("Кнопка следующего скина")]
    [SerializeField] private Button NextSkinButton;

    [Tooltip("Изображение денег")]
    [SerializeField] private GameObject moneyImage;

    [Tooltip("Аниматор персонажа")]
    [SerializeField] public Animator animator;

    [Tooltip("Текст имени персонажа")]
    [SerializeField] private TextMeshProUGUI characterText;

    [Tooltip("Текст имени скина")]
    [SerializeField] private TextMeshProUGUI skinText;

    [Tooltip("Текст здоровья персонажа")]
    [SerializeField] private TextMeshProUGUI healthText;

    [Tooltip("Текст маны персонажа")]
    [SerializeField] private TextMeshProUGUI maneText;

    [Tooltip("Текст стартового оружия персонажа")]
    [SerializeField] private TextMeshProUGUI startWeaponText;

    [Tooltip("Текст количества монет игрока")]
    [SerializeField] private TextMeshProUGUI moneyText;

    [Tooltip("Текст цены персонажа")]
    [SerializeField] private TextMeshProUGUI priceText;

    [Tooltip("Лист персонажей лобби")]
    [SerializeField] private List<Character> characters;

    [Tooltip("Размер камеры в игре")]
    [SerializeField] private float cameraSizeGame;

    [Tooltip("Размер камеры при выборе персонажа")]
    [SerializeField] private float cameraSizeChooseCharacter;

    [Tooltip("Позиция камеры при выборе персонажа")]
    [SerializeField] private Vector3 cameraLobbyPosition;
#pragma warning restore 0649

    private CurrentGameInfo currentGameInfo;
    private ProgressInfo progressInfo;
    private Character currentCharacter;

    private int characterPrice;
    private int skinCounter;

    public void ChooseCharacter(Character character, Animator animator)
    {
        skinCounter = 0;

        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        progressInfo = GameObject.Find("ProgressHandler").GetComponent<ProgressInfo>();

        currentCharacter = character;

        this.animator = animator;

        SetSpecChar();
        SetInfoBar();
        RefreshUI();
    }

    public void SetSpecChar()
    {
        currentGameInfo.character = currentCharacter.CharacterClass;
        currentGameInfo.skin = currentCharacter.Animations[skinCounter].name;

        currentGameInfo.maxHealth = currentCharacter.MaxHealth;
        currentGameInfo.maxMane = currentCharacter.MaxMane;

        currentGameInfo.startWeapon = currentCharacter.StartWeapon;
        currentGameInfo.isLobby = true;
        characterPrice = currentCharacter.Price;
    }

    private void SetInfoBar()
    {
        skinText.gameObject.GetComponent<MovementUI>().MoveToEnd();
        goToGameButton.GetComponent<MovementUI>().MoveToEnd();
        PrevSkinButton.GetComponent<MovementUI>().MoveToEnd();
        NextSkinButton.GetComponent<MovementUI>().MoveToEnd();
        InfoBar.GetComponent<MovementUI>().MoveToEnd();
        backToLobbyButton.GetComponent<MovementUI>().MoveToEnd();
        moneyImage.GetComponent<MovementUI>().MoveToEnd();


        healthText.GetComponent<LocalizedText>().SetLocalization();
        maneText.GetComponent<LocalizedText>().SetLocalization();
        startWeaponText.GetComponent<LocalizedText>().SetLocalization();

        healthText.text += ": " + currentGameInfo.maxHealth.ToString();
        maneText.text += ": " + currentGameInfo.maxMane.ToString();
        startWeaponText.text += ": " + LocalizedText.SetLocalization(currentGameInfo.startWeapon.ToString());
    }

    private void RefreshUI()
    {
        moneyText.text = progressInfo.playerMoney.ToString();

        characterText.GetComponent<LocalizedText>().key = currentCharacter.CharacterClass;
        characterText.GetComponent<LocalizedText>().SetLocalization();

        skinText.GetComponent<LocalizedText>().key = currentCharacter.Animations[skinCounter].name;
        skinText.GetComponent<LocalizedText>().SetLocalization();

        priceText.text = characterPrice.ToString();
        if (progressInfo.CharacterAccess(currentCharacter.CharacterClass))
            buyButton.GetComponent<MovementUI>().MoveToEnd();
        GameBuyButtonAccess();
    }


    public void NextSkin()
    {
        if (skinCounter + 1 < currentCharacter.Animations.Length)
        {
            skinCounter++;
            ChangeCharacterSkin();
        }
    }

    public void PrevSkin()
    {
        if (skinCounter - 1 >= 0)
        {
            skinCounter--;
            ChangeCharacterSkin();
        }
    }

    private void ChangeCharacterSkin()
    {
        skinText.GetComponent<LocalizedText>().key = currentCharacter.Animations[skinCounter].name;
        skinText.GetComponent<LocalizedText>().SetLocalization();
        animator.runtimeAnimatorController = currentCharacter.Animations[skinCounter];
        currentGameInfo.skin = currentCharacter.Animations[skinCounter].name;
    }

    private void GameBuyButtonAccess()
    {
        if (progressInfo.CharacterAccess(currentCharacter.CharacterClass))
        {
            goToGameButton.gameObject.SetActive(true);
            buyButton.GetComponent<MovementUI>().MoveToStart();
        }
        else
        {
            goToGameButton.gameObject.SetActive(false);
            buyButton.GetComponent<MovementUI>().MoveToEnd();
        }
    }

    public void BuyCharacter()
    {
        if (progressInfo.playerMoney >= characterPrice)
        {
            progressInfo.characters[currentCharacter.CharacterClass] = true;
            progressInfo.playerMoney -= characterPrice;
            moneyText.text = progressInfo.playerMoney.ToString();
            GameBuyButtonAccess();
            progressInfo.SaveProgress();
        }

    }

    public void ConfirmCharacter()
    {
        currentGameInfo.SaveCurrentGame();
        characterText.gameObject.SetActive(false);
        gameObject.SetActive(false);
        characterControlUI.SetActive(true);

        GameButtons.SpawnPosition = currentCharacter.transform.position;

        characters.Remove(currentCharacter);
        foreach (var character in characters)
        {
            character.playerCharacter = GameObject.Find("Character(Clone)");
        }

        Destroy(currentCharacter.gameObject);
        Camera.main.orthographicSize = cameraSizeGame;
    }

    public void BackToLobby()
    {
        HideCharacterChooseUI();
        skinCounter = 0;
        ChangeCharacterSkin();

        characterText.GetComponent<LocalizedText>().key = "chooseCharacter";
        characterText.GetComponent<LocalizedText>().SetLocalization();

        Camera.main.orthographicSize = cameraSizeChooseCharacter;
        Camera.main.transform.position = cameraLobbyPosition;

        lobbyUI.GetComponent<LobbyUI>().ShowLobby();
    }

    private void HideCharacterChooseUI()
    {
        skinText.gameObject.GetComponent<MovementUI>().MoveToStart();
        goToGameButton.GetComponent<MovementUI>().MoveToStart();
        PrevSkinButton.GetComponent<MovementUI>().MoveToStart();
        NextSkinButton.GetComponent<MovementUI>().MoveToStart();
        InfoBar.GetComponent<MovementUI>().MoveToStart();
        backToLobbyButton.GetComponent<MovementUI>().MoveToStart();
        moneyImage.GetComponent<MovementUI>().MoveToStart();
        buyButton.GetComponent<MovementUI>().MoveToStart();
    }
}
