using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButtons : MonoBehaviour
{
    //Classes
    private CharInfo charInfo;
    private CharShooting charShooting;
    private CharGun charGun;
    private ManaBar manaBar;

    //Values
    public GameObject pausePanel;
    public GameObject pauseButton;
    public GameObject settingsPanel;

    public static bool GameIsPaused = false;
    public float fireRate;
    public int mana;
    public int fireActButtonState;
    private WeaponsSpec.Gun gun;
    private float nextFire;
    private bool shooting;
    private bool settingsState;

    void Start()
    {
        Time.timeScale = 1f;
        settingsState = false;
        pausePanel.SetActive(false);
        settingsPanel.SetActive(settingsState);
        fireActButtonState = 0;
        GameObject character = GameObject.Find("Character(Clone)");
        charInfo = character.GetComponent<CharInfo>();
        charShooting = character.GetComponent<CharShooting>();
        charGun = character.GetComponent<CharGun>();
        manaBar = GameObject.Find("Canvas").GetComponentInChildren<ManaBar>();
        nextFire = 0.0f;
    }
    void Update()
    {
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
                charGun.ChangeLevel();
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
        pausePanel.SetActive(false);
        settingsState = false;
        settingsPanel.SetActive(settingsState);
        pauseButton.SetActive(true);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        charInfo.SaveChar();
        SceneManager.LoadScene("Menu");
    }

    public void OpenCloseSettings()
    {
        settingsState = !settingsState;
        settingsPanel.SetActive(settingsState);      
    }
    
}
