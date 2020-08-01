using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameButtons : MonoBehaviour
{
    public static GameButtons instance;
#pragma warning disable 0649
    [Tooltip("UI магазина оружия")]
    [SerializeField] private GameObject weaponStoreUI;

    [Tooltip("UI паузы")]
    [SerializeField] private GameObject pause;

    [Tooltip("UI панель паузы")]
    [SerializeField] private GameObject pausePanel;

    [Tooltip("Джойстик")]
    [SerializeField] private GameObject joystick;

    [Tooltip("Кнопка паузы")]
    [SerializeField] private Button pauseButton;

    [Tooltip("Кнопка стрельбы и действий")]
    [SerializeField] private Button fireActButton;

    [Tooltip("Кнопка смены оружия")]
    [SerializeField] private Button swapWeaponButton;

    [Tooltip("Изображение текущего оружия")]
    [SerializeField] public GameObject currentWeaponImage;

    [Tooltip("UI картинки денег")]
    [SerializeField] private GameObject moneyImage;

    [Tooltip("Текст количества денег")]
    [SerializeField] private TextMeshProUGUI moneyText;

    [Tooltip("Текст описания испытания")]
    [SerializeField] private TextMeshProUGUI challengeText;

    [Tooltip("Префаб персонажа")]
    [SerializeField] private GameObject character;

    [Tooltip("Панель здоровья")]
    [SerializeField] private GameObject healthBar;

    [Tooltip("Панель маны")]
    [SerializeField] private GameObject maneBar;

    [Tooltip("Кнопка запуска игры")]
    [SerializeField] private GameObject playButton;

    [Tooltip("Загрузочный экран")]
    [SerializeField] private GameObject loadScreen;

    [Tooltip("Панель выбора испытаний")]
    [SerializeField] private GameObject challengeUI;
#pragma warning restore 0649

    public enum FireActButtonStateEnum
    {
        none,
        changeGun,
        NPC,
        weaponStore,
        portalToGame,
        tvAds,
        shootingRange
    };

    //Переменные состояния UI элементов
    public static FireActButtonStateEnum FireActButtonState;
    public static bool IsGamePausedState;
    public static bool IsWeaponStoreState;
    public static Vector3 SpawnPosition;

    //Скрипты персонажа
    private CharInfo charInfo;
    private CharGun charGun;
    private CharAction charAction;
    //Скрипты
    private SettingsInfo settingsInfo;
    private CurrentGameInfo currentGameInfo;

    //Переменные
    private float attackRate;
    private int manecost;
    private float nextAttack;
    private bool isAttackDown;
    private bool isAttackUp;
    public static bool isChange = true;

    public Transform currentWeapon;
    private AudioManager audioManager;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        Time.timeScale = 1f;

        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        StartUIActive();
        SetStartUI();

        if (SceneManager.GetActiveScene().name == "Game")
        {
            currentGameInfo.SetIsLobbyState(false);
            SpawnPosition = LevelGeneration.instance.StartSpawnLevel(currentGameInfo.challengeNumber);
        }

        character = Instantiate(character, SpawnPosition, Quaternion.identity);
        if (SceneManager.GetActiveScene().name == "Lobby")
            character.GetComponent<CharController>().speed = 5f;
        SetCharScripts();
        CheckFirstPlay();

        moneyText = moneyText.GetComponent<TextMeshProUGUI>();
        moneyText.text = charInfo.money.ToString();
        FireActButtonState = FireActButtonStateEnum.none;
        IsGamePausedState = false;
        IsWeaponStoreState = false;

        nextAttack = 0.0f;
    }

    private void StartUIActive()
    {
        IsGamePausedState = false;
    }


    private void SetStartUI()
    {
        pauseButton.GetComponent<MovementUI>().SetStart();
        moneyImage.GetComponent<MovementUI>().SetStart();
        healthBar.GetComponent<MovementUI>().SetStart();
        maneBar.GetComponent<MovementUI>().SetStart();

        ColorUtility.TryParseHtmlString("#" + settingsInfo.color, out Color newColor);

        fireActButton.GetComponent<Image>().color = newColor;
        joystick.transform.GetChild(0).GetComponent<Image>().color = newColor;
        joystick.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color = newColor;
        swapWeaponButton.GetComponent<Image>().color = newColor;
        currentWeaponImage.GetComponent<Image>().color = newColor;

        joystick.GetComponent<RectTransform>().anchoredPosition
          = new Vector3(settingsInfo.joystickPosition[0], settingsInfo.joystickPosition[1]);
        fireActButton.GetComponent<RectTransform>().anchoredPosition
          = new Vector3(settingsInfo.fireActButtonPosition[0], settingsInfo.fireActButtonPosition[1]);
        swapWeaponButton.GetComponent<RectTransform>().anchoredPosition
          = new Vector3(settingsInfo.swapWeaponButtonPosition[0], settingsInfo.swapWeaponButtonPosition[1]);

        pauseButton.GetComponent<MovementUI>().MoveToEnd();
        moneyImage.GetComponent<MovementUI>().MoveToEnd();
        healthBar.GetComponent<MovementUI>().MoveToEnd();
        maneBar.GetComponent<MovementUI>().MoveToEnd();
    }

    private void SetCharScripts()
    {
        charInfo = character.GetComponent<CharInfo>();
        charGun = character.GetComponent<CharGun>();
        charAction = character.GetComponent<CharAction>();
    }

    private void CheckFirstPlay()
    {
        if (MenuButtons.firstPlay)
        {
            charInfo.SetStartParametrs();
            SetCharAnim();
            charInfo.SaveChar();
        }
        else
        {
            if (currentGameInfo.LoadCurrentGame() && currentGameInfo.isLobby == false)
            {
                charInfo.LoadChar();
                SetCharAnim();
            }
            else
            {
                SaveSystem.DeleteCurrentGame();
                SceneManager.LoadScene("Menu");
            }
        }
    }

    private void SetCharAnim()
    {
        character.GetComponent<CharController>().CharacterRuntimeAnimatorController
            = Resources.Load<RuntimeAnimatorController>("Animations/Characters/" + charInfo.character + "/" + charInfo.skin + "/" + charInfo.skin)
                    as RuntimeAnimatorController;
    }


    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
            if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Home))
                OpenPause();
        if (isAttackDown)
        {
            AttackDown();
            PrepareAttack();
        }
        if (isAttackUp)
            AttackUp();

    }

    public void OpenPause()
    {
        audioManager.Play("ClickUI");
        Time.timeScale = 0f;
        IsGamePausedState = true;
        pause.SetActive(IsGamePausedState);
        HideChallengeUI();
        pausePanel.GetComponent<MovementUI>().MoveToEnd();
    }

    public void FireActStateDown()
    {
        switch (FireActButtonState)
        {
            case FireActButtonStateEnum.none:
                isAttackDown = true;
                break;
            case FireActButtonStateEnum.changeGun:
                charGun.ChangeGun();
                currentWeapon = character.transform.Find(charInfo.weapons[charGun.currentWeaponNumber]);
                if (currentWeapon.GetComponent<Weapon>().TypeOfAttack == WeaponData.AttackType.Bow)
                    CharController.isRotate = false;
                else
                    CharController.isRotate = true;
                break;
            case FireActButtonStateEnum.NPC:
                charAction.currentNPC.GetComponent<Character>().ShowPhrase();
                break;
            case FireActButtonStateEnum.weaponStore:
                OpenWeaponStore();
                break;
            case FireActButtonStateEnum.portalToGame:
                challengeUI.GetComponent<MovementUI>().MoveToEnd();
                break;
            case FireActButtonStateEnum.tvAds:
                AdsManager.AdShow();
                break;
            case FireActButtonStateEnum.shootingRange:
                ShootingRange.instance.StartGame();
                break;
        }
    }
    public void ChooseChallenge(int challengeNumber)
    {
        currentGameInfo.challengeNumber = challengeNumber;
        playButton.GetComponent<MovementUI>().MoveToEnd();
        challengeText.GetComponent<LocalizedText>().key = $@"Challenge{challengeNumber}";
        challengeText.GetComponent<LocalizedText>().SetLocalization();
    }

    public void HideChallengeUI()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            challengeUI.GetComponent<MovementUI>().MoveToStart();
            playButton.GetComponent<MovementUI>().MoveToStart();
        }
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void FireActStateUp()
    {
        switch (FireActButtonState)
        {
            case 0:
                isAttackUp = true;
                break;
        }
    }

    private void PrepareAttack()
    {
        if (charInfo.mane - manecost >= 0)
        {
            switch (currentWeapon.GetComponent<Weapon>().TypeOfAttack)
            {
                case WeaponData.AttackType.Bow:
                    currentWeapon.GetComponent<Bow>().Pulling();
                    CharController.isRotate = true;
                    break;
            }
        }
    }

    private void AttackDown()
    {
        if (charInfo.mane - manecost >= 0)
        {
            if (Time.time > nextAttack)
            {
                switch (currentWeapon.GetComponent<Weapon>().TypeOfAttack)
                {
                    case WeaponData.AttackType.Gun:
                        charInfo.SpendMana(manecost);
                        currentWeapon.GetComponent<Gun>().Shoot();
                        nextAttack = Time.time + attackRate;
                        break;
                    case WeaponData.AttackType.Sword:
                        charInfo.SpendMana(manecost);
                        currentWeapon.GetComponent<Sword>().Hit();
                        nextAttack = Time.time + attackRate;
                        break;
                }
            }
        }
    }

    public void AttackUp()
    {
        if (charInfo.mane - manecost >= 0 && isAttackUp)
        {
            switch (currentWeapon.GetComponent<Weapon>().TypeOfAttack)
            {
                case WeaponData.AttackType.Bow:
                    charInfo.SpendMana(manecost);
                    currentWeapon.GetComponent<Bow>().Shoot();
                    CharController.isRotate = false;
                    break;
            }
        }
        isAttackUp = false;
    }

    public void StopAttack()
    {
        isAttackDown = false;

        switch (currentWeapon.GetComponent<Weapon>().TypeOfAttack)
        {
            case WeaponData.AttackType.Sword:
                currentWeapon.GetComponent<Sword>().animator.SetBool("Attack", false);
                break;
        }
    }


    private void OpenWeaponStore()
    {
        IsWeaponStoreState = true;
        weaponStoreUI.SetActive(IsWeaponStoreState);

    }

    public void SetWeaponInfo(Weapon weapon)
    {
        attackRate = weapon.FireRate;
        manecost = weapon.Manecost;
    }

    public void SwapWeapon()
    {
        if (isChange)
        {
            if (WeaponSpawner.instance.countOfWeapon == 2)
            {
                if (charGun.currentWeaponNumber == 0)
                {
                    WeaponSpawner.instance.currentCharWeapon[charGun.currentWeaponNumber].SetActive(false);
                    charGun.currentWeaponNumber++;

                    WeaponSpawner.instance.currentCharWeapon[charGun.currentWeaponNumber].SetActive(true);
                    charGun.SwapWeapon();
                    currentWeapon = character.transform.Find(charInfo.weapons[charGun.currentWeaponNumber]);
                    currentWeaponImage.transform.GetChild(0).GetComponent<Image>().sprite
                        = WeaponSpawner.instance.currentCharWeapon[charGun.currentWeaponNumber]
                            .GetComponent<Weapon>().MainSprite;
                }
                else if (charGun.currentWeaponNumber == 1)
                {
                    WeaponSpawner.instance.currentCharWeapon[charGun.currentWeaponNumber].SetActive(false);
                    charGun.currentWeaponNumber--;

                    WeaponSpawner.instance.currentCharWeapon[charGun.currentWeaponNumber].SetActive(true);
                    charGun.SwapWeapon();
                    currentWeapon = character.transform.Find(charInfo.weapons[charGun.currentWeaponNumber]);
                    currentWeaponImage.transform.GetChild(0).GetComponent<Image>().sprite
                        = WeaponSpawner.instance.currentCharWeapon[charGun.currentWeaponNumber]
                            .GetComponent<Weapon>().MainSprite;

                }

                if (currentWeapon.GetComponent<Weapon>().TypeOfAttack == WeaponData.AttackType.Bow)
                    CharController.isRotate = false;
                else
                    CharController.isRotate = true;
            }
        }
    }

    public void ChangeWeaponButton()
    {
        WeaponSpawner.instance.currentCharWeapon[charGun.currentWeaponNumber].SetActive(true);
        currentWeaponImage.transform.GetChild(0).GetComponent<Image>().sprite
            = WeaponSpawner.instance.currentCharWeapon[charGun.currentWeaponNumber]
                        .GetComponent<Weapon>().MainSprite;
    }
}
