using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharInfo : MonoBehaviour
{
    //Gameobjects
    private ManaBar manaBar;
    private HealthBar healthBar;
    private CurrentGameInfo currentGameInfo;
    private CharAction charAction;

    //Values
    public int level;
    public int money;
    public int health;
    public int mane;
    public string weapon;
    public string skin;
    public string character;

    public void SetStartParametrs()
    {
        charAction = GetComponent<CharAction>();
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        level = 1;
        money = currentGameInfo.startMoney;
        mane = currentGameInfo.maxMane;
        health = currentGameInfo.maxHealth;
        weapon = currentGameInfo.startWeapon;
        character = currentGameInfo.character;
        SetObjects();
    }

    private void SetObjects()
    {
        FindObjects();
        manaBar.SetMaxMin(mane, currentGameInfo.maxMane, 0);
        healthBar.SetMaxMin(health, currentGameInfo.maxHealth, 0);
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
        weapon = charData.weapon;
        money = charData.money;
        character = charData.character;
        SetObjects();
    }

    public void SpendMana(int manecost)
    {
        if (mane - manecost < 0)
            mane = 0;
        else
            mane -= manecost;
        manaBar.SetMane(mane);
    }

    public void FillMana(int fillAmount)
    {
        if (mane + fillAmount > currentGameInfo.maxMane)
            mane = currentGameInfo.maxMane;
        else
            mane += fillAmount;
    }

    public void Damage(int damage)
    {
        if (health - damage < 0)
            health = 0;
        else
            health -= damage;

        healthBar.SetHealth(health);

        if (health <= 0)
        {
            charAction.Death();
        }
    }

    public void Healing(int healing)
    {
        if (health + healing > currentGameInfo.maxHealth)
            health = currentGameInfo.maxHealth;
        else
            health += healing;
        healthBar.SetHealth(health);
    }

    private void FindObjects()
    {
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        manaBar = GameObject.Find("Canvas").transform.Find("GameUI").transform.GetComponentInChildren<ManaBar>();
        healthBar = GameObject.Find("Canvas").transform.Find("GameUI").transform.GetComponentInChildren<HealthBar>();
    }

}
