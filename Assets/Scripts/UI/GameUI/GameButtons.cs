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

    [Tooltip("Префаб персонажа")]
    [SerializeField] private GameObject character;
    public Transform currentWeapon;

    void Start()
    {
        Time.timeScale = 1f;

        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        manaBar = GameObject.Find("Canvas").GetComponentInChildren<ManaBar>();

        SetUIScripts();
        var animator = Resources.Load("");

        StartUIActive();
        SetStartUIPosition();

        character = Instantiate(character, new Vector3(2, 2, 0), Quaternion.identity);
        SetCharScripts();
        CheckFirstPlay();
        

        moneyText.text = charInfo.money.ToString();

        FireActButtonState = 0;
        IsGamePausedState = false;
        IsGamePausedPanelState = false;
        IsWeaponStoreState = false;

        nextAttack = 0.0f;

    }

    private void StartUIActive()
    {
        IsGamePausedPanelState = false;
        IsWeaponStoreState = false;
        pausePanel.SetActive(IsGamePausedPanelState);
        pausePanel.SetActive(IsWeaponStoreState);
        fireActButton.GetComponent<Image>().color = Color.red;
    }


    private void SetStartUIPosition()
    {
        joystick.GetComponent<RectTransform>().anchoredPosition
          = new Vector3(settingsInfo.joystickPosition[0], settingsInfo.joystickPosition[1]);
        fireActButton.GetComponent<RectTransform>().anchoredPosition
          = new Vector3(settingsInfo.fireActButtonPosition[0], settingsInfo.fireActButtonPosition[1]);

    }

    private void SetUIScripts()
    {
        pauseUI = GameObject.Find("Canvas").GetComponentInChildren<PauseUI>();
        weaponStore = GameObject.Find("Canvas").GetComponentInChildren<WeaponStoreUI>();
        pauseSettingsUI = GameObject.Find("Canvas").GetComponentInChildren<PauseSettings>();
    }

    private void SetCharScripts()
    {
        charInfo = character.GetComponent<CharInfo>();
        charAction = character.GetComponent<CharAction>();
        charGun = character.GetComponent<CharGun>();
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
            if (currentGameInfo.LoadCurrentGame())
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
            = Resources.Load<RuntimeAnimatorController>("Animations/Characters/" + charInfo.character + "/" + charInfo.skin + "/" + charInfo.skin) as RuntimeAnimatorController;
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
                Debug.Log("start Attack");
                break;
            case 1:
                charGun.ChangeGun();
                currentWeapon = character.transform.Find(charInfo.weapons[charGun.currentWeaponNumber]);
                break;
            case 2:
                OpenWeaponStore();
                break;
            case 3:
                SceneManager.LoadScene("Game");
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
        if (charInfo.mane - manecost >= 0 && isAttack)
        {
            if (Time.time > nextAttack)
            {
                charInfo.SpendMana(manecost);
                switch (currentWeapon.GetComponent<Weapon>().TypeOfAttack)
                {
                    case WeaponData.AttackType.Gun:
                        currentWeapon.GetComponent<Gun>().Shoot();
                        break;
                    case WeaponData.AttackType.Sword:
                        currentWeapon.GetComponent<Sword>().Hit();
                        break;
                    case WeaponData.AttackType.Bow:
                        currentWeapon.GetComponent<Bow>().Shoot();
                        break;
                }
                nextAttack = Time.time + attackRate;
            }
        }
    }

    public void StopAttack()
    {
        if (currentWeapon.GetComponent<Weapon>().TypeOfAttack != WeaponData.AttackType.Bow)
            isAttack = false;

        switch (currentWeapon.GetComponent<Weapon>().TypeOfAttack)
        {
            case WeaponData.AttackType.Sword:
                currentWeapon.GetComponent<Sword>().animator.SetBool("Attack", false);
                break;
        }
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
                WeaponSpawner.instance.currentCharWeapon[charGun.currentWeaponNumber].SetActive(false);
                charGun.currentWeaponNumber++;

                WeaponSpawner.instance.currentCharWeapon[charGun.currentWeaponNumber].SetActive(true);
                charGun.SwapWeapon();
                currentWeapon = character.transform.Find(charInfo.weapons[charGun.currentWeaponNumber]);
                currentWeaponImage.sprite 
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
                currentWeaponImage.sprite 
                    = WeaponSpawner.instance.currentCharWeapon[charGun.currentWeaponNumber]
                        .GetComponent<Weapon>().MainSprite;

            }
        }
    }

    public void ChangeWeaponButton()
    {
        WeaponSpawner.instance.currentCharWeapon[charGun.currentWeaponNumber].SetActive(true);
        currentWeaponImage.sprite
            = WeaponSpawner.instance.currentCharWeapon[charGun.currentWeaponNumber]
                        .GetComponent<Weapon>().MainSprite;
    }
}
