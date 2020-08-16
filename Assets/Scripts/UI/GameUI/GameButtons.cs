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

    [Tooltip("Кнопка паузы")]
    [SerializeField] private Button pauseButton;

    [Tooltip("Изображение текущего оружия")]
    [SerializeField] public GameObject currentWeaponImage;

    [Tooltip("UI картинки денег")]
    [SerializeField] private GameObject moneyImage;

    [Tooltip("Текст количества денег")]
    [SerializeField] private TextMeshProUGUI moneyText;

    [Tooltip("Текст описания испытания")]
    [SerializeField] private TextMeshProUGUI challengeText;

    [Tooltip("Текст в панели")]
    [SerializeField] private TextMeshProUGUI deathPanelText;

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

    [Tooltip("Панель смерти")]
    [SerializeField] private GameObject deathPanel;

    [Tooltip("Таймер до спауна")]
    [SerializeField] private GameObject spawnTimer;
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
    private CurrentGameInfo currentGameInfo;

    //Переменные
    private float attackRate;
    private int manecost;
    private float nextAttack;
    private bool isAttackDown;
    private bool isAttackUp;
    private int priceResurrect;
    private float startTime;
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
        // PlayerPrefs.DeleteAll();
        startTime = Time.time;
        Time.timeScale = 1f;

        GameObject.Find("UI_SpawnerHandler").GetComponent<UISpawner>().SetUI();
        pause.SetActive(false);
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        IsGamePausedState = false;
        SetStartUI();



        character = Instantiate(character, SpawnPosition, Quaternion.identity);
        SetCharScripts();

        if (SceneManager.GetActiveScene().name == "Game")
        {
            charInfo.LoadChar();
            currentGameInfo.LoadCurrentGame();
            //Debug.Log(currentGameInfo.challengeNumber);
        }

        if (SceneManager.GetActiveScene().name == "Game")
        {
            currentGameInfo.SetIsLobbyState(false);
            SpawnPosition = LevelGeneration.instance.StartSpawnLevel(currentGameInfo.challengeNumber);
            character.transform.position = SpawnPosition;
        }

        else if (SceneManager.GetActiveScene().name == "Lobby")
        {
            charInfo.SetStartParams();
            character.GetComponent<CharController>().speed = 7.5f;
        }
        SetCharAnim();

        moneyText = moneyText.GetComponent<TextMeshProUGUI>();
        moneyText.text = charInfo.money.ToString();
        FireActButtonState = FireActButtonStateEnum.none;
        IsGamePausedState = false;
        IsWeaponStoreState = false;
        nextAttack = 0.0f;
    }

    private void SetStartUI()
    {
        pauseButton.GetComponent<MovementUI>().SetStart();
        moneyImage.GetComponent<MovementUI>().SetStart();
        healthBar.GetComponent<MovementUI>().SetStart();
        maneBar.GetComponent<MovementUI>().SetStart();
        ShowHideControlUI(true);
    }

    private void SetCharScripts()
    {
        charInfo = character.GetComponent<CharInfo>();
        charGun = character.GetComponent<CharGun>();
        charAction = character.GetComponent<CharAction>();
    }

    private void SetCharAnim()
    {
        character.GetComponent<CharController>().CharacterRuntimeAnimatorController
            = Resources.Load<RuntimeAnimatorController>($"Animations/Characters/{charInfo.character}/{charInfo.skin}/{charInfo.skin}")
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
        pausePanel.GetComponent<MovementUI>().MoveToEnd();
        HideChallengeUI();
        if (SceneManager.GetActiveScene().name == "Lobby")
            ShootingRange.instance.CloseDifficultyPanel();
        ShowHideControlUI(false);
    }

    public void FireActStateDown()
    {
        if (!CharAction.isDeath)
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
                    ShootingRange.instance.ShowDifficultyPanel();
                    break;
            }
        }
    }

    public void ChooseChallenge(int challengeNumber)
    {
        audioManager.Play("ClickUI");
        currentGameInfo.challengeNumber = challengeNumber;
        challengeText.GetComponentInParent<MovementUI>().MoveToEnd();
        playButton.GetComponent<MovementUI>().MoveToEnd();
        challengeText.GetComponent<LocalizedText>().key = $@"ChallengeDescription{challengeNumber}";
        challengeText.GetComponent<LocalizedText>().SetLocalization();
    }

    public void HideChallengeUI()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            challengeText.GetComponentInParent<MovementUI>().MoveToStart();
            challengeText.GetComponent<LocalizedText>().key = "ChallengeText";
            challengeText.GetComponent<LocalizedText>().SetLocalization();
            challengeUI.GetComponent<MovementUI>().MoveToStart();
            playButton.GetComponent<MovementUI>().SetStart();
        }
    }

    public void ShowHideControlUI(bool isShow)
    {
        if (isShow)
        {
            pauseButton.GetComponent<MovementUI>().MoveToEnd();
            moneyImage.GetComponent<MovementUI>().MoveToEnd();
            healthBar.GetComponent<MovementUI>().MoveToEnd();
            maneBar.GetComponent<MovementUI>().MoveToEnd();
            if (SceneManager.GetActiveScene().name == "Game" && EnemySpawner.textTimer != 0)
                spawnTimer.GetComponent<MovementUI>().MoveToEnd();
        }
        else
        {
            pauseButton.GetComponent<MovementUI>().MoveToStart();
            moneyImage.GetComponent<MovementUI>().MoveToStart();
            healthBar.GetComponent<MovementUI>().MoveToStart();
            maneBar.GetComponent<MovementUI>().MoveToStart();
            if (SceneManager.GetActiveScene().name == "Game")
                spawnTimer.GetComponent<MovementUI>().MoveToStart();
        }

    }

    public void OpenDeathPanel()
    {
        if (CurrentGameInfo.instance.countResurrect != 0)
        {
            priceResurrect = (100 + ((int)((Time.time - startTime) * 2)) % 300);
            deathPanelText.text = priceResurrect.ToString();
            deathPanel.GetComponent<MovementUI>().MoveToEnd();
            Time.timeScale = 0f;
            IsGamePausedState = true;
            ShowHideControlUI(false);
        }
        else
            GoToFinishScene();
    }

    public void GoToFinishScene()
    {
        audioManager.Play("ClickUI");
        Time.timeScale = 1f;
        ProgressInfo.instance.currentCountShoots = charInfo.currentCountShoots;
        ProgressInfo.instance.currentCountKilledEnemies = charInfo.currentCountKilledEnemies;
        SceneManager.LoadScene("FinishGame");
    }

    public void ResurrectPlayerAd()
    {
        audioManager.Play("ClickUI");
        //Show ad
        charAction.Resurrect();
        deathPanel.GetComponent<MovementUI>().MoveToStart();
        Time.timeScale = 1f;
        IsGamePausedState = false;
        ShowHideControlUI(true);

    }

    public void ResurrectPlayerMoney()
    {
        if (ProgressInfo.instance.playerMoney >= priceResurrect)
        {
            ProgressInfo.instance.playerMoney -= priceResurrect;
            audioManager.Play("ClickUI");
            charAction.Resurrect();
            deathPanel.GetComponent<MovementUI>().MoveToStart();
            Time.timeScale = 1f;
            IsGamePausedState = false;
            ShowHideControlUI(true);
        }
    }

    public void GoToGame()
    {
        audioManager.Play("ClickUI");
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            charInfo.SaveChar();
            currentGameInfo.SaveCurrentGame();
        }
        SceneManager.LoadScene("Game");
    }

    public void FireActStateUp()
    {
        if (!CharAction.isDeath)
        {
            switch (FireActButtonState)
            {
                case 0:
                    isAttackUp = true;
                    break;
            }
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
                        charInfo.currentCountShoots++;
                        charInfo.SpendMana(manecost);
                        audioManager.Play(currentWeapon.GetComponent<Weapon>().WeaponName);
                        currentWeapon.GetComponent<Gun>().Shoot();
                        nextAttack = Time.time + attackRate;
                        break;
                    case WeaponData.AttackType.Sword:
                        charInfo.currentCountShoots++;
                        charInfo.SpendMana(manecost);
                        audioManager.Play(currentWeapon.GetComponent<Weapon>().WeaponName);
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
                    charInfo.currentCountShoots++;
                    charInfo.SpendMana(manecost);
                    audioManager.Play(currentWeapon.GetComponent<Weapon>().WeaponName);
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
