using UnityEngine;
using UnityEngine.SceneManagement;

public class CharInfo : MonoBehaviour
{
    //Gameobjects
    private ManaBar manaBar;
    private HealthBar healthBar;
    private CurrentGameInfo currentGameInfo;
    //Values
    public int level;
    public int money;
    public int health;
    public int mane;
    public string gun;
    public string skin;
    public string character;

    void Start()
    {
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();

        if (MenuButtons.firstPlay)
        {
            SetStartParametrs();
            SaveChar();
            MenuButtons.firstPlay = false;
        }
        else
        {
            if(currentGameInfo.LoadCurrentGame())
                LoadChar();
            else
            {
                SaveSystem.DeleteCurrentGame();
                SceneManager.LoadScene("Menu");
            }            
        }

        manaBar = GameObject.Find("Canvas").GetComponentInChildren<ManaBar>();
        healthBar = GameObject.Find("Canvas").GetComponentInChildren<HealthBar>();
        SetObjects();
    }

    private void SetStartParametrs()
    {
        level = 1;
        money = currentGameInfo.startMoney;
        mane = currentGameInfo.maxMane;
        health = currentGameInfo.maxHealth;
        gun = currentGameInfo.startGun;
        character = currentGameInfo.character;
    }

    private void SetObjects()
    {
        manaBar.SetMana(mane, currentGameInfo.maxMane, 0);
        healthBar.SetHealth(health, currentGameInfo.maxHealth, 0);
    }

    public void SaveChar()
    {
        SaveSystem.SaveChar(this);
    }

    public void LoadChar()
    {
        CharData charData = SaveSystem.LoadChar();
        level = charData.level;
        health = charData.health;
        mane = charData.mane;
        gun = charData.gun;
        money = charData.money;
        character = charData.character;
    }

    public void SpendMana(int spendAmount)
    {
        if (mane - spendAmount < 0)
            mane = 0;
        else
            mane -= spendAmount;
    }

    public void FillMana(int fillAmount)
    {
        if (mane + fillAmount > currentGameInfo.maxMane)
            mane = currentGameInfo.maxMane;
        else
            mane += fillAmount;
    }

}
