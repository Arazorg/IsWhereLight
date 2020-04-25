using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameButtons : MonoBehaviour
{
    [Tooltip("UI магазина оружия")]
    [SerializeField] private GameObject weaponStoreUI;

    [Tooltip("UI паузы")]
    [SerializeField] private GameObject pausePanel;

    [Tooltip("Джойстик")]
    [SerializeField] private GameObject joystick;

    [Tooltip("Кнопка паузы")]
    [SerializeField] private Button pauseButton;

    [Tooltip("Кнопка стрельбы и действий")]
    [SerializeField] private Button fireActButton;

    [Tooltip("Изображение текущего оружия")]
    public Image currentWeaponImage;

    [Tooltip("UI магазина оружия")]
    [SerializeField] private Text moneyText;

    //Переменные состояния UI элементов
    public static int FireActButtonState;
    public static bool IsGamePausedState;
    public static bool IsGamePausedPanelState;
    public static bool IsWeaponStoreState;

    //Скрипты персонажа
    private CharInfo charInfo;
    private CharShooting charShooting;
    private CharMelee charMelee;
    private CharBow charBow;
    private CharGun charGun;
    private CharAction charAction;

    //Скрипты
    private ManaBar manaBar;
    private SettingsInfo settingsInfo;
    private CurrentGameInfo currentGameInfo;

    //UI Скрипты
    private PauseUI pauseUI;
    private WeaponStoreUI weaponStore;
    private PauseSettings pauseSettingsUI;

    //Переменные
    public float attackRate;
    public int manecost;
    private float nextAttack;
    private bool isAttack;

    private GameObject character;
    public Transform currentWeapon;

    void Start()
    {
        Time.timeScale = 1f;

        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        manaBar = GameObject.Find("Canvas").GetComponentInChildren<ManaBar>();

        SetUIScripts();
        SetCharScripts();
        StartUIActive();
        SetStartUIPosition();
        CheckFirstPlay();

        moneyText.text = charInfo.money.ToString();

        FireActButtonState = 0;
        IsGamePausedState = false;
        IsGamePausedPanelState = false;
        IsWeaponStoreState = false;

        nextAttack = 0.0f;

    }


    private void SetUIScripts()
    {
        pauseUI = GameObject.Find("Canvas").GetComponentInChildren<PauseUI>();
        weaponStore = GameObject.Find("Canvas").GetComponentInChildren<WeaponStoreUI>();
        pauseSettingsUI = GameObject.Find("Canvas").GetComponentInChildren<PauseSettings>();
    }

    private void SetCharScripts()
    {
        character = GameObject.Find("Character(Clone)");
        charInfo = character.GetComponent<CharInfo>();
        charShooting = character.GetComponent<CharShooting>();
        charMelee = character.GetComponent<CharMelee>();
        charBow = character.GetComponent<CharBow>();
        charAction = character.GetComponent<CharAction>();
        charGun = character.GetComponent<CharGun>();
    }

    private void StartUIActive()
    {
        IsGamePausedPanelState = false;
        IsWeaponStoreState = false;
        pausePanel.SetActive(IsGamePausedPanelState);
        pausePanel.SetActive(IsWeaponStoreState);
        fireActButton.GetComponent<Image>().color = Color.red;
    }

    private void CheckFirstPlay()
    {
        if (MenuButtons.firstPlay)
        {
            charInfo.SetStartParametrs();
            charInfo.SaveChar();
            MenuButtons.firstPlay = false;
        }
        else
        {
            if (currentGameInfo.LoadCurrentGame())
                charInfo.LoadChar();
            else
            {
                SaveSystem.DeleteCurrentGame();
                SceneManager.LoadScene("Menu");
            }
        }
    }

    private void SetStartUIPosition()
    {
        joystick.GetComponent<RectTransform>().anchoredPosition
          = new Vector3(settingsInfo.joystickPosition[0], settingsInfo.joystickPosition[1]);
        fireActButton.GetComponent<RectTransform>().anchoredPosition
          = new Vector3(settingsInfo.fireActButtonPosition[0], settingsInfo.fireActButtonPosition[1]);
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Home))
            {
                OpenPause();
            }
        }
        Attack();
    }

    public void OpenPause()
    {
        Time.timeScale = 0f;
        IsGamePausedState = true;
        IsGamePausedPanelState = true;
        pausePanel.SetActive(IsGamePausedPanelState);
        pauseButton.gameObject.SetActive(false);
    }

    public void FireActState()
    {
        switch (FireActButtonState)
        {
            case 0:
                isAttack = true;
                break;
            case 1:
                charGun.ChangeGun();
                currentWeapon = character.transform.Find(charInfo.weapons[charGun.currentWeaponNumber]);
                charMelee.animator = character.transform.Find(charInfo.weapons[charGun.currentWeaponNumber]).GetComponent<Animator>();
                charBow.animator = character.transform.Find(charInfo.weapons[charGun.currentWeaponNumber]).GetComponent<Animator>();
                break;
            case 2:
                OpenWeaponStore();
                break;
            case 3:
                break;
            case 4:
                AdsManager.AdShow();
                break;
        }
    }

    private void OpenWeaponStore()
    {
        IsWeaponStoreState = true;
        weaponStoreUI.SetActive(IsWeaponStoreState);
    }

    private void Attack()
    {
        if (charInfo.mane - manecost >= 0 && isAttack 
            && currentWeapon.GetComponent<Weapon>().TypeOfAttack
                    != WeaponData.AttackType.Bow)
        {
            if (Time.time > nextAttack)
            {
                charInfo.SpendMana(manecost);
                if (currentWeapon.GetComponent<Weapon>().TypeOfAttack
                    == WeaponData.AttackType.Bullet)
                {
                    charShooting.Shoot();
                }
                else if (currentWeapon.GetComponent<Weapon>().TypeOfAttack
                    == WeaponData.AttackType.Melee)
                {
                    charMelee.Hit();
                }      
                nextAttack = Time.time + attackRate;
            }
        }

        else if (charInfo.mane - manecost >= 0 && isAttack
        && currentWeapon.GetComponent<Weapon>().TypeOfAttack
                == WeaponData.AttackType.Bow)
        {
            if (Time.time > nextAttack)
            {
                charBow.ArrowString();
            }
        }
    }

    public void BowAttack()
    {
        if (charInfo.mane - manecost >= 0 && isAttack)
        {
            charInfo.SpendMana(manecost);
            if (currentWeapon.GetComponent<Weapon>().TypeOfAttack == WeaponData.AttackType.Bow)
            {
                charShooting.Shoot();
                charBow.Shoot();
                isAttack = false;
                Debug.Log("стоп");
            }
        }
    }

    public void StopAttack()
    {
        if(currentWeapon.GetComponent<Weapon>().TypeOfAttack != WeaponData.AttackType.Bow)
            isAttack = false;
    }


    public void PlusMoney()
    {
        charInfo.money += 100;
        moneyText.text = charInfo.money.ToString();
    }

    public void Death()
    {
        charAction.Death();
    }

    public void SetWeaponInfo(Weapon weapon)
    {
        attackRate = weapon.FireRate;
        manecost = weapon.Manecost;
    }

    public void SwapWeapon()
    {
        if (WeaponSpawner.instance.countOfWeapon == 2)
        {
            if (charGun.currentWeaponNumber == 0)
            {
                WeaponSpawner.instance.currentWeaponScript[charGun.currentWeaponNumber].gameObject.SetActive(false);
                charGun.currentWeaponNumber++;

                WeaponSpawner.instance.currentWeaponScript[charGun.currentWeaponNumber].gameObject.SetActive(true);
                charGun.SwapWeapon();
                currentWeapon = character.transform.Find(charInfo.weapons[charGun.currentWeaponNumber]);
                charMelee.animator = character.transform.Find(charInfo.weapons[charGun.currentWeaponNumber]).GetComponent<Animator>();
                currentWeaponImage.sprite = WeaponSpawner.instance.currentWeaponScript[charGun.currentWeaponNumber].MainSprite;
            }
            else if (charGun.currentWeaponNumber == 1)
            {
                WeaponSpawner.instance.currentWeaponScript[charGun.currentWeaponNumber].gameObject.SetActive(false);
                charGun.currentWeaponNumber--;

                WeaponSpawner.instance.currentWeaponScript[charGun.currentWeaponNumber].gameObject.SetActive(true);
                charGun.SwapWeapon();
                currentWeapon = character.transform.Find(charInfo.weapons[charGun.currentWeaponNumber]);
                charMelee.animator = character.transform.Find(charInfo.weapons[charGun.currentWeaponNumber]).GetComponent<Animator>();
                currentWeaponImage.sprite = WeaponSpawner.instance.currentWeaponScript[charGun.currentWeaponNumber].MainSprite;

            }
        }
    }

    public void ChangeWeaponButton()
    {
        WeaponSpawner.instance.currentWeaponScript[charGun.currentWeaponNumber].gameObject.SetActive(true);
        currentWeaponImage.sprite = WeaponSpawner.instance.currentWeaponScript[charGun.currentWeaponNumber].MainSprite;
    }
}
