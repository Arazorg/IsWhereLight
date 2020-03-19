using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public void SetStartParametrs()
    {
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        level = 1;
        money = currentGameInfo.startMoney;
        mane = currentGameInfo.maxMane;
        health = currentGameInfo.maxHealth;
        gun = currentGameInfo.startGun;
        character = currentGameInfo.character;
        SetObjects();
    }

    private void SetObjects()
    {
        FindObjects();
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
        SetObjects();
    }

    public void SpendMana(int spendAmount)
    {
        if (mane - spendAmount < 0)
            mane = 0;
        else
            mane -= spendAmount;
        manaBar.Spend(spendAmount);
    }

    public void FillMana(int fillAmount)
    {
        if (mane + fillAmount > currentGameInfo.maxMane)
            mane = currentGameInfo.maxMane;
        else
            mane += fillAmount;
    }

    public void SpendHealth(int spendAmount)
    {
        if (health - spendAmount < 0)
            health = 0;
        else
            health -= spendAmount;
        healthBar.Damage(spendAmount);
    }

    private void FindObjects()
    {
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        manaBar = GameObject.Find("Canvas").transform.Find("GameUI").transform.GetComponentInChildren<ManaBar>();
        healthBar = GameObject.Find("Canvas").transform.Find("GameUI").transform.GetComponentInChildren<HealthBar>();
    }

}
