using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterChooseUI : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Лобби UI")]
    [SerializeField] private GameObject lobbyUI;

    [Tooltip("Управление персонажем UI")]
    [SerializeField] private GameObject characterControlUI;

    [Tooltip("Панель доната")]
    [SerializeField] private GameObject donatePanel;

    [Tooltip("Кнопка перехода в игру")]
    [SerializeField] private Button goToGameButton;

    [Tooltip("Кнопка покупки персонажа")]
    [SerializeField] private Button buyButton;

    [Tooltip("Кнопка перехода в лобби")]
    [SerializeField] private Button backToLobbyButton;

    [Tooltip("Панеель описания персонажа")]
    [SerializeField] private GameObject infoBar;

    [Tooltip("Панель описания способности персонажа")]
    [SerializeField] private GameObject skillBar;

    [Tooltip("Кнопка предыдущего скина")]
    [SerializeField] private Button prevSkinButton;

    [Tooltip("Кнопка следующего скина")]
    [SerializeField] private Button nextSkinButton;

    [Tooltip("Изображение типа персонажа")]
    [SerializeField] private Image typeImage;

    [Tooltip("Текст типа персонажа")]
    [SerializeField] private TextMeshProUGUI typeText;

    [Tooltip("Текст описания персонажа")]
    [SerializeField] private TextMeshProUGUI typeDescriptionText;

    [Tooltip("Текст описания способности персонажа")]
    [SerializeField] private TextMeshProUGUI skillDescriptionText;

    [Tooltip("Изображение денег")]
    [SerializeField] private GameObject moneyImage;

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

    [Tooltip("Лист персонажей лобби")]
    [SerializeField] private List<Sprite> charactersTypes;

    [Tooltip("Размер камеры в игре")]
    [SerializeField] private float cameraSizeGame;

    [Tooltip("Размер камеры при выборе персонажа")]
    [SerializeField] private float cameraSizeChooseCharacter;

    [Tooltip("Позиция камеры при выборе персонажа")]
    [SerializeField] private Vector3 cameraLobbyPosition;
#pragma warning restore 0649
    public bool IsCharacterChooseUI
    {
        get { return isCharacterChooseUIState; }
        set { isCharacterChooseUIState = value; }
    }
    private bool isCharacterChooseUIState;

    private Animator animator;
    private CurrentGameInfo currentGameInfo;
    private CharParametrs charParametrs;
    private ProgressInfo progressInfo;
    private Character currentCharacter;
    private AudioManager audioManager;

    private int characterPrice;
    private int skinCounter;
    private bool isCharacter;
    
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public void ChooseCharacter(Character character, Animator animator)
    {
        skinCounter = 0;
        charParametrs = GameObject.Find("CharParametrsHandler").GetComponent<CharParametrs>();
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        progressInfo = GameObject.Find("ProgressHandler").GetComponent<ProgressInfo>();

        currentCharacter = character;

        this.animator = animator;

        SetSpecChar();
        SetInfoBar();
        RefreshUI();
    }

    void Update()
    {
        if (isCharacter)
            animator.runtimeAnimatorController = currentCharacter.Animations[skinCounter];
        if (Input.GetKeyDown(KeyCode.Escape))
            BackToLobby();
    }

    public void SetSpecChar()
    {
        currentGameInfo.character = currentCharacter.CharacterClass;
        currentGameInfo.skin = currentCharacter.Animations[skinCounter].name;
        charParametrs.CharHp = currentCharacter.MaxHealth;
        charParametrs.CharMane = currentCharacter.MaxMane;
        currentGameInfo.startWeapon = currentCharacter.StartWeapon;
        currentGameInfo.characterType = currentCharacter.CharacterType;
        currentGameInfo.skillTime = currentCharacter.SkillTime;
        characterPrice = currentCharacter.Price;
        currentGameInfo.isLobby = true;
        currentGameInfo.currentAmplifications = new string[4];
    }

    private void SetInfoBar()
    {
        skinText.gameObject.GetComponent<MovementUI>().MoveToEnd();
        goToGameButton.GetComponent<MovementUI>().MoveToEnd();
        prevSkinButton.GetComponent<MovementUI>().MoveToEnd();
        nextSkinButton.GetComponent<MovementUI>().MoveToEnd();
        infoBar.GetComponent<MovementUI>().MoveToEnd();
        skillBar.GetComponent<MovementUI>().MoveToEnd();
        backToLobbyButton.GetComponent<MovementUI>().MoveToEnd();
        moneyImage.GetComponent<MovementUI>().MoveToEnd();

        switch (currentGameInfo.characterType)
        {
            case "Defence":
                typeImage.sprite = charactersTypes[0];
                break;
            case "Attack":
                typeImage.sprite = charactersTypes[1];
                break;
            case "Healing":
                typeImage.sprite = charactersTypes[2];
                break;
        }

        typeText.GetComponent<LocalizedText>().key = currentGameInfo.characterType;
        typeText.GetComponent<LocalizedText>().SetLocalization();
        typeDescriptionText.GetComponent<LocalizedText>().key = $"{currentGameInfo.character}Description";
        typeDescriptionText.GetComponent<LocalizedText>().SetLocalization();
        skillDescriptionText.GetComponent<LocalizedText>().key = $"{currentGameInfo.character}SkillDescription";
        skillDescriptionText.GetComponent<LocalizedText>().SetLocalization();
        healthText.GetComponent<LocalizedText>().SetLocalization();
        maneText.GetComponent<LocalizedText>().SetLocalization();
        startWeaponText.GetComponent<LocalizedText>().SetLocalization();

        healthText.text += ": " + charParametrs.CharHp.ToString();
        maneText.text += ": " + charParametrs.CharMane.ToString();
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
        isCharacter = true;
    }


    public void NextSkin()
    {
        audioManager.Play("ClickUI");
        if (skinCounter + 1 < currentCharacter.Animations.Length)
        {
            skinCounter++;
            ChangeCharacterSkin();
        }
    }

    public void PrevSkin()
    {
        audioManager.Play("ClickUI");
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
        audioManager.Play("ClickUI");
        if (progressInfo.playerMoney >= characterPrice)
        {
            progressInfo.characters[currentCharacter.CharacterClass] = true;
            progressInfo.playerMoney -= characterPrice;
            moneyText.text = progressInfo.playerMoney.ToString();
            GameBuyButtonAccess();
            progressInfo.SaveProgress();
        }
        else
            donatePanel.GetComponent<MovementUI>().MoveToEnd();

    }

    public void ConfirmCharacter()
    {
        audioManager.Play("ClickUI");
        isCharacter = false;
        characterText.gameObject.SetActive(false);
        gameObject.SetActive(false);
        characterControlUI.SetActive(true);
        lobbyUI.SetActive(false);
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
        audioManager.Play("ClickUI");
        isCharacter = false;
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
        prevSkinButton.GetComponent<MovementUI>().MoveToStart();
        nextSkinButton.GetComponent<MovementUI>().MoveToStart();
        infoBar.GetComponent<MovementUI>().MoveToStart();
        skillBar.GetComponent<MovementUI>().MoveToStart();
        backToLobbyButton.GetComponent<MovementUI>().MoveToStart();
        moneyImage.GetComponent<MovementUI>().MoveToStart();
        buyButton.GetComponent<MovementUI>().MoveToStart();
    }
}
