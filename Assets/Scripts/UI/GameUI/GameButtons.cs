using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameButtons : MonoBehaviour
{
    [Tooltip("UI магазина оружия")]
    [SerializeField] private GameObject weaponStoreUI;

    [Tooltip("UI паузы")]
    [SerializeField] private GameObject pausePanel;

    [Tooltip("UI магазина оружия")]
    [SerializeField] private Text moneyText;

    [Tooltip("Джойстик")]
    [SerializeField] private GameObject joystick;

    [Tooltip("Кнопка паузы")]
    [SerializeField] private Button pauseButton;

    [Tooltip("Кнопка стрельбы и действий")]
    [SerializeField] private Button fireActButton;


    //Переменные состояния
    public static int FireActButtonState;
    public static bool IsGamePausedState;
    public static bool IsGamePausedPanelState;
    public static bool IsWeaponStoreState;

    //Скрипты персонажа
    private CharInfo charInfo;
    private CharShooting charShooting;
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

    public float fireRate;
    public int manecost;
    private float nextFire;
    private bool shooting;

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

        nextFire = 0.0f;
        
    }


    private void SetUIScripts()
    {
        pauseUI = GameObject.Find("Canvas").GetComponentInChildren<PauseUI>();
        weaponStore = GameObject.Find("Canvas").GetComponentInChildren<WeaponStoreUI>();
        pauseSettingsUI = GameObject.Find("Canvas").GetComponentInChildren<PauseSettings>();

    }

    private void SetCharScripts()
    {
        GameObject character = GameObject.Find("Character(Clone)");
        charInfo = character.GetComponent<CharInfo>();
        charShooting = character.GetComponent<CharShooting>();
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

        Fire();
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
                shooting = true;
                break;
            case 1:
                charGun.ChangeGun();
                break;
            case 2:
                OpenWeaponStore();
                break;
        }
    }

    private void OpenWeaponStore()
    {
        IsWeaponStoreState = true;
        weaponStoreUI.SetActive(IsWeaponStoreState);
    }

    private void Fire()
    {
        if (manaBar.currentValue > 0 && shooting)
        {
            if (Time.time > nextFire)
            {
                manaBar.Spend(manecost);
                charInfo.SpendMana(manecost);
                charShooting.Shoot();
                nextFire = Time.time + fireRate;
            }
        }
    }

    public void StopFire()
    {
        shooting = false;
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
        fireRate = weapon.FireRate;
        manecost = weapon.Manecost;
    }
}
