using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameButtons : MonoBehaviour
{
    public static GameButtons instance;

    public enum FireActButtonStateEnum
    {
        none,
        changeGun,
        NPC,
        weaponStore,
        tvAds,
        shootingRange
    };

#pragma warning disable 0649
    [Tooltip("UI магазина оружия")]
    [SerializeField] private GameObject weaponStoreUI;

    [Tooltip("UI паузы")]
    [SerializeField] private GameObject pause;

    [Tooltip("UI панель паузы")]
    [SerializeField] private GameObject pausePanel;

    [Tooltip("Панель доната")]
    [SerializeField] private GameObject donatePanel;

    [Tooltip("Кнопка паузы")]
    [SerializeField] private Button pauseButton;

    [Tooltip("Изображение текущего оружия")]
    [SerializeField] public GameObject currentWeaponImage;

    [Tooltip("UI картинки денег")]
    [SerializeField] private GameObject moneyImage;

    [Tooltip("Текст количества денег")]
    [SerializeField] private TextMeshProUGUI moneyText;

    [Tooltip("Текст в панели")]
    [SerializeField] private TextMeshProUGUI deathPanelText;

    [Tooltip("Текст денег в панели смерти")]
    [SerializeField] private TextMeshProUGUI deathPanelMoneyText;

    [Tooltip("Префаб персонажа")]
    [SerializeField] private GameObject character;

    [Tooltip("Панель здоровья")]
    [SerializeField] private GameObject healthBar;

    [Tooltip("Панель маны")]
    [SerializeField] private GameObject maneBar;

    [Tooltip("Загрузочный экран")]
    [SerializeField] private GameObject loadScreen;

    [Tooltip("Панель выбора испытаний")]
    [SerializeField] private GameObject challengeUI;

    [Tooltip("Панель смерти")]
    [SerializeField] private GameObject deathPanel;

    [Tooltip("Таймер до спауна")]
    [SerializeField] private GameObject spawnTimer;

    [Tooltip("Спрайт отката скилла")]
    [SerializeField] private Image skillButtonBar;
#pragma warning restore 0649

    public static FireActButtonStateEnum FireActButtonState;
    public static Vector3 SpawnPosition;
    public static bool IsGamePausedState;
    public static bool isChange = true;
    public static bool IsWeaponStoreState;
    public static bool isOpenPause = true;

    public Transform currentWeapon; // заменить на свойтсво(21 ссылка)

    private CharInfo charInfo;
    private CharGun charGun;
    private CharAction charAction;
    private CharSkills charSkills;
    private CurrentGameInfo currentGameInfo;
    private AudioManager audioManager;
    private Sound weaponSound;

    private float attackRate;
    private float[] nextAttack = new float[2];
    private float startTime;
    private float timeToSkill;
    private float startStringingTime;
    private bool isAttackDown;
    private bool isAttackUp;
    private bool isStaticAttack;
    private int priceResurrect;
    private int manecost;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    void Start()
    {
        startTime = Time.time;
        Time.timeScale = 1f;
        timeToSkill = float.MinValue;
        UISpawner.instance.SetUI();
        UISpawner.instance.IsStartFpsCounter = true;
        pause.SetActive(false);
        if (SceneManager.GetActiveScene().name == "Game")
            deathPanelMoneyText.gameObject.SetActive(false);
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        IsGamePausedState = false;
        SetStartUI();

        character = Instantiate(character, SpawnPosition, Quaternion.identity);
        SetCharScripts();

        if (SceneManager.GetActiveScene().name == "Game")
        {
            CharAmplifications.instance.SetAmplifications();
            currentGameInfo.SetIsLobbyState(false);
            SpawnPosition = LevelGeneration.instance.StartSpawnLevel(currentGameInfo.challengeName);
            character.transform.position = SpawnPosition;
        }
        charInfo.SetStartParams();


        UISpawner.instance.SetSkillButtonSprite(currentGameInfo.character);
        SetCharAnim();
        moneyText = moneyText.GetComponent<TextMeshProUGUI>();
        moneyText.text = charInfo.money.ToString();
        FireActButtonState = FireActButtonStateEnum.none;
        IsGamePausedState = false;
        IsWeaponStoreState = false;
        nextAttack[0] = 0.0f;
        nextAttack[1] = 0.0f;
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
        charInfo = GameObject.Find("CharInfoHandler").GetComponent<CharInfo>();
        charGun = character.GetComponent<CharGun>();
        charAction = character.GetComponent<CharAction>();
        charSkills = character.GetComponent<CharSkills>();
    }

    private void SetCharAnim()
    {
        character.GetComponent<CharController>().CharacterRuntimeAnimatorController
            = Resources.Load<RuntimeAnimatorController>($"Animations/Characters/{charInfo.character}/{charInfo.skin}/{charInfo.skin}")
                    as RuntimeAnimatorController;
    }


    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OpenPause();

        if (Application.platform == RuntimePlatform.Android)
            if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Home))
                OpenPause();
        if (isAttackDown && !CharAction.isDeath)
        {
            AttackDown();
            PrepareAttack();
        }

        if (isAttackUp && !CharAction.isDeath)
            AttackUp();

        if (Time.time < timeToSkill)
        {
            var currentPercent = Math.Abs(timeToSkill - Time.time) / currentGameInfo.skillTime;
            skillButtonBar.fillAmount = currentPercent;
        }
        else
            skillButtonBar.fillAmount = 0;
    }

    public void OpenPause()
    {
        if (isOpenPause)
        {
            audioManager.StopAllSounds();
            audioManager.Play("ClickUI");
            Time.timeScale = 0f;
            IsGamePausedState = true;
            pause.SetActive(IsGamePausedState);
            pause.GetComponent<PauseUI>().timeToClose = Time.realtimeSinceStartup + 0.25f;
            pausePanel.GetComponent<MovementUI>().MoveToEnd();
            if (SceneManager.GetActiveScene().name == "Lobby")
                ShootingRange.instance.CloseDifficultyPanel();
            ShowHideControlUI(false);
        }
    }

    public void FireActStateDown()
    {
        if (!CharAction.isDeath)
        {
            switch (FireActButtonState)
            {
                case FireActButtonStateEnum.none:
                    startStringingTime = Time.time;
                    isAttackDown = true;
                    break;
                case FireActButtonStateEnum.changeGun:
                    charGun.ChangeGun();
                    currentWeapon = character.transform.Find(charInfo.weapons[charGun.CurrentWeaponNumber]);
                    break;
                case FireActButtonStateEnum.NPC:
                    charAction.currentNPC.GetComponent<Character>().ShowPhrase();
                    break;
                case FireActButtonStateEnum.weaponStore:
                    OpenWeaponStore();
                    break;
                case FireActButtonStateEnum.tvAds:
                    AdsManager.instance.AdShow();
                    break;
                case FireActButtonStateEnum.shootingRange:
                    ShootingRange.instance.ShowDifficultyPanel();
                    break;
            }
        }
    }

    public void StartSkill()
    {
        if (Time.time > timeToSkill && !CharAction.isDeath)
        {
            charSkills.ChooseSkill(charInfo.character);
            timeToSkill = Time.time + currentGameInfo.skillTime;
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
            if (SceneManager.GetActiveScene().name == "Game" && EnemySpawner.instance.textTimer != 0)
                spawnTimer.GetComponent<MovementUI>().MoveToEnd();
            UISpawner.instance.HideShowFPS(true);
        }
        else
        {
            pauseButton.GetComponent<MovementUI>().MoveToStart();
            moneyImage.GetComponent<MovementUI>().MoveToStart();
            healthBar.GetComponent<MovementUI>().MoveToStart();
            maneBar.GetComponent<MovementUI>().MoveToStart();
            if (SceneManager.GetActiveScene().name == "Game")
                spawnTimer.GetComponent<MovementUI>().MoveToStart();
            UISpawner.instance.HideShowFPS(false);
        }

    }

    public void OpenDeathPanel()
    {
        if (CurrentGameInfo.instance.countResurrect != 0)
        {
            priceResurrect = (100 + ((int)((Time.time - startTime) * 2)) % 300);
            deathPanelText.text = priceResurrect.ToString();
            deathPanelMoneyText.gameObject.SetActive(true);
            deathPanelMoneyText.text = ProgressInfo.instance.playerMoney.ToString();
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
        audioManager.StopAllSounds();
        if (!CurrentGameInfo.instance.isWin)
            audioManager.Play("ClickUI");
        Time.timeScale = 1f;
        ProgressInfo.instance.currentCountShoots = charInfo.currentCountShoots;
        ProgressInfo.instance.currentCountKilledEnemies = charInfo.currentCountKilledEnemies;
        SceneManager.LoadScene("FinishGame");
    }

    public void RevivePlayerAd()
    {
        audioManager.Play("ClickUI");
        AdsManager.instance.AdShow(true);
    }

    public void RevivePlayerMoney()
    {
        if (ProgressInfo.instance.playerMoney >= priceResurrect)
        {
            ProgressInfo.instance.playerMoney -= priceResurrect;
            audioManager.Play("ClickUI");
            Revive();
        }
        else
            donatePanel.GetComponent<MovementUI>().MoveToEnd();
    }

    public void Revive()
    {
        charAction.Revive();
        deathPanel.GetComponent<MovementUI>().MoveToStart();
        deathPanelMoneyText.gameObject.SetActive(false);
        Time.timeScale = 1f;
        IsGamePausedState = false;
        ShowHideControlUI(true);
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
                    break;
            }
        }
    }

    private void AttackDown()
    {
        if (charInfo.mane - manecost >= 0)
        {
            if (Time.time > nextAttack[charGun.CurrentWeaponNumber])
            {
                var weaponScript = currentWeapon.GetComponent<Weapon>();
                CameraShaker.instance.ShakeOnce(weaponScript.ShakeParametrs.magnitude,
                                                    weaponScript.ShakeParametrs.roughness,
                                                         weaponScript.ShakeParametrs.fadeInTime,
                                                             weaponScript.ShakeParametrs.fadeOutTime);
                switch (currentWeapon.GetComponent<Weapon>().TypeOfAttack)
                {
                    case WeaponData.AttackType.Gun:
                        audioManager.Play(weaponScript.WeaponName);
                        charInfo.currentCountShoots++;
                        charInfo.SpendMana(manecost);
                        currentWeapon.GetComponent<Gun>().Shoot();
                        break;
                    case WeaponData.AttackType.Sword:
                        audioManager.Play(weaponScript.WeaponName);
                        charInfo.currentCountShoots++;
                        charInfo.SpendMana(manecost);
                        currentWeapon.GetComponent<Sword>().Hit();
                        break;
                    case WeaponData.AttackType.Laser:
                        audioManager.Play(weaponScript.WeaponName);
                        charInfo.currentCountShoots++;
                        charInfo.SpendMana(manecost);
                        currentWeapon.GetComponent<Laser>().Shoot();
                        break;
                    case WeaponData.AttackType.ConstantLaser:
                        if (weaponSound == null)
                            weaponSound = audioManager.Play(weaponScript.WeaponName, true);
                        charInfo.currentCountShoots++;
                        charInfo.SpendMana(manecost);
                        if (!isStaticAttack)
                            currentWeapon.GetComponent<ConstantLaser>().IsAttack = true;
                        if (currentWeapon.GetComponent<ConstantLaser>().IsAttack)
                            isStaticAttack = true;
                        break;
                    default:
                        break;
                }
                nextAttack[charGun.CurrentWeaponNumber] = Time.time + attackRate;
            }
            else
            {
                switch (currentWeapon.GetComponent<Weapon>().TypeOfAttack)
                {
                    case WeaponData.AttackType.Laser:
                        currentWeapon.GetComponent<Laser>().StopShoot();
                        break;
                    case WeaponData.AttackType.Gun:
                        currentWeapon.GetComponent<Gun>().StopShoot();
                        break;
                }
            }
        }
        else
            StopAttack();
    }

    public void AttackUp()
    {
        if (isAttackUp)
        {
            if (charInfo.mane - manecost >= 0)
            {
                switch (currentWeapon.GetComponent<Weapon>().TypeOfAttack)
                {
                    case WeaponData.AttackType.Bow:
                        charInfo.currentCountShoots++;
                        charInfo.SpendMana(manecost);
                        audioManager.Play(currentWeapon.GetComponent<Weapon>().WeaponName);
                        currentWeapon.GetComponent<Bow>().Shoot(Time.time - startStringingTime);
                        startStringingTime = 0;
                        StopAttack();
                        break;
                }
            }
            else
                StopAttack();
        }
        isAttackUp = false;
    }

    public void StopAttack()
    {
        isAttackDown = false;

        switch (currentWeapon.GetComponent<Weapon>().TypeOfAttack)
        {
            case WeaponData.AttackType.Sword:
                currentWeapon.GetComponent<Sword>().StopShoot();
                break;
            case WeaponData.AttackType.Laser:
                currentWeapon.GetComponent<Laser>().StopShoot();
                break;
            case WeaponData.AttackType.Gun:
                currentWeapon.GetComponent<Gun>().StopShoot();
                break;
            case WeaponData.AttackType.ConstantLaser:
                if (weaponSound != null)
                {
                    audioManager.Stop(weaponSound);
                    weaponSound = null;
                }

                currentWeapon.GetComponent<ConstantLaser>().StopShoot();
                isStaticAttack = false;
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
                AudioManager.instance.Play("WeaponSwap");
                if (charGun.CurrentWeaponNumber == 0)
                {
                    WeaponSpawner.instance.currentCharWeapon[charGun.CurrentWeaponNumber].SetActive(false);
                    charGun.CurrentWeaponNumber++;

                    WeaponSpawner.instance.currentCharWeapon[charGun.CurrentWeaponNumber].SetActive(true);
                    charGun.SwapWeapon();
                    currentWeapon = character.transform.Find(charInfo.weapons[charGun.CurrentWeaponNumber]);
                    currentWeaponImage.transform.GetChild(0).GetComponent<Image>().sprite
                        = WeaponSpawner.instance.currentCharWeapon[charGun.CurrentWeaponNumber]
                            .GetComponent<Weapon>().MainSprite;
                }
                else if (charGun.CurrentWeaponNumber == 1)
                {
                    WeaponSpawner.instance.currentCharWeapon[charGun.CurrentWeaponNumber].SetActive(false);
                    charGun.CurrentWeaponNumber--;

                    WeaponSpawner.instance.currentCharWeapon[charGun.CurrentWeaponNumber].SetActive(true);
                    charGun.SwapWeapon();
                    currentWeapon = character.transform.Find(charInfo.weapons[charGun.CurrentWeaponNumber]);
                    currentWeaponImage.transform.GetChild(0).GetComponent<Image>().sprite
                        = WeaponSpawner.instance.currentCharWeapon[charGun.CurrentWeaponNumber]
                            .GetComponent<Weapon>().MainSprite;
                }
            }
        }
    }

    public void ChangeWeaponButton()
    {
        WeaponSpawner.instance.currentCharWeapon[charGun.CurrentWeaponNumber].SetActive(true);
        currentWeaponImage.transform.GetChild(0).GetComponent<Image>().sprite
            = WeaponSpawner.instance.currentCharWeapon[charGun.CurrentWeaponNumber]
                        .GetComponent<Weapon>().MainSprite;
    }

}
