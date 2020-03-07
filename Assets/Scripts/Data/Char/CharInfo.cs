using UnityEngine;

public class CharInfo : MonoBehaviour
{
    //Gameobjects
    private ManaBar manaBar;
    private HealthBar healthBar;
    private CurrentGameInfo currentGameInfo;
    //Values
    public int money;
    public int health;
    public int mane;
    public string gun;
    public string skin;

    void Start()
    {
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();

        if (MenuButtons.firstPlay == true)
        {
            Debug.Log("LOQIEEAN");
            SetStartParametrs();
        }
        else
        {
            LoadChar();
        }

        manaBar = GameObject.Find("Canvas").GetComponentInChildren<ManaBar>();
        healthBar = GameObject.Find("Canvas").GetComponentInChildren<HealthBar>();
        manaBar.SetMana(mane, currentGameInfo.maxMane, 0);
        healthBar.SetHealth(health, currentGameInfo.maxHealth, 0);
    }

    public void SetStartParametrs()
    {
        money = currentGameInfo.startMoney;
        mane = currentGameInfo.maxMane;
        health = currentGameInfo.maxHealth;
        gun = currentGameInfo.startGun;
        skin = currentGameInfo.skin;
        MenuButtons.firstPlay = false;
    }

    public void SaveChar()
    {
        SaveSystem.SaveChar(this);
    }

    public void LoadChar()
    {
        CharData data = SaveSystem.LoadChar();
        health = data.health;
        mane = data.mane;
        gun = data.gun;
        money = data.money;
        skin = data.skin;
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
