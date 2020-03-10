using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameButtons : MonoBehaviour
{
    //Classes
    private CurrentGameInfo currentGameInfo;
    private CharInfo charInfo;
    private CharShooting charShooting;
    private CharGun charGun;
    private CharAction charAction;
    private ManaBar manaBar;
    private SettingsInfo settingsInfo;
    

    //Values
    public GameObject pausePanel;
    public GameObject pauseButton;
    public GameObject settingsPanel;
    public Text moneyText;
    private GameObject joystick;
    private GameObject fireActButton;

    public static bool GameIsPaused = false;
    public float fireRate;
    public int mana;
    public int fireActButtonState;
    public static bool SettingsState;
    private WeaponsSpec.Gun gun;
    private float nextFire;
    private bool shooting;

    void Start()
    {
        Time.timeScale = 1f;
        SettingsState = false;
        pausePanel.SetActive(false);
        settingsPanel.SetActive(SettingsState);

        fireActButtonState = 0;
        nextFire = 0.0f;
        
        GameObject character = GameObject.Find("Character(Clone)");
        charInfo = character.GetComponent<CharInfo>();
        charShooting = character.GetComponent<CharShooting>();
        charAction = character.GetComponent<CharAction>();
        charGun = character.GetComponent<CharGun>();

        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        manaBar = GameObject.Find("Canvas").GetComponentInChildren<ManaBar>();

        joystick = GameObject.Find("Canvas").transform.Find("GameUI").transform.GetChild(0).gameObject as GameObject;
        fireActButton = GameObject.Find("Canvas").transform.Find("GameUI").transform.GetChild(1).gameObject as GameObject;

        joystick.GetComponent<RectTransform>().anchoredPosition
          = new Vector3(settingsInfo.joystickPosition[0], settingsInfo.joystickPosition[1]);
        fireActButton.GetComponent<RectTransform>().anchoredPosition
          = new Vector3(settingsInfo.fireActButtonPosition[0], settingsInfo.fireActButtonPosition[1]);
        moneyText.text = charInfo.money.ToString();
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

    public void FireActState()
    {
        switch (fireActButtonState)
        {
            case 0:
                shooting = true;
                break;
            case 1:
                charGun.ChangeGun();
                break;
            case 2:
                charAction.ChangeLevel();
                break;
        }
    }

    private void Fire()
    {
        if (manaBar.currentValue > 0 && shooting)
        {
            if (Time.time > nextFire)
            {
                manaBar.Spend(mana);
                charInfo.SpendMana(mana);
                charShooting.Shoot();
                nextFire = Time.time + fireRate;
            }
        }
    }

    public void StopFire()
    {
        shooting = false;
    }

    public void OpenPause()
    {
        GameIsPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void ClosePause()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        settingsInfo.SaveSettings();
        pausePanel.SetActive(false);
        SettingsState = false;
        settingsPanel.SetActive(SettingsState);
        pauseButton.SetActive(true);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        settingsInfo.SaveSettings();
        charInfo.SaveChar();
        SceneManager.LoadScene("Menu");
    }

    public void OpenCloseSettings()
    {
        settingsInfo.SaveSettings();
        SettingsState = !SettingsState;
        settingsPanel.SetActive(SettingsState);
    }

    public void Suicide()
    {
        SceneManager.LoadScene("FinishGame");
        FinishOfGameButton.finishGameMoney = charInfo.money;
    }

    public void PlusMoney()
    {
        charInfo.money++;
        moneyText.text = charInfo.money.ToString();
    }
}
