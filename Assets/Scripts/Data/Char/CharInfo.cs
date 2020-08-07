using UnityEngine;

public class CharInfo : MonoBehaviour
{
    //Gameobjects
    private ManaBar manaBar;
    private HealthBar healthBar;
    private CurrentGameInfo currentGameInfo;
    private CharAction charAction;

    //Values
    public string character;
    public string skin;
    public string[] weapons;
    public int maxHealth;
    public int maxMane;
    public int health;
    public int mane;
    public int money;
    public int countResurrect;

    public void SetStartParametrs()
    {
        GetComponent();
        weapons = new string[2];
        character = currentGameInfo.character;
        skin = currentGameInfo.skin;
        weapons[0] = currentGameInfo.startWeapon;
        maxHealth = currentGameInfo.maxHealth;
        maxMane = currentGameInfo.maxMane;
        health = maxHealth;
        mane = maxMane;
        money = 0;
        countResurrect = 1;
        SetObjects();
    }

    private void SetObjects()
    {
        FindObjects();
        manaBar.SetMaxMin(mane, maxMane, 0);
        healthBar.SetMaxMin(health, maxHealth, 0);
        Camera.main.GetComponent<CameraFollow>().SetTarget(transform);
    }

    private void FindObjects()
    {
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        manaBar = GameObject.Find("Canvas").transform.Find("CharacterControlUI").transform.GetComponentInChildren<ManaBar>();
        healthBar = GameObject.Find("Canvas").transform.Find("CharacterControlUI").transform.GetComponentInChildren<HealthBar>();
    }

    public void SaveChar()
    {
        SaveSystem.SaveChar(this);
    }

    public void LoadChar()
    {
        GetComponent();
        CharData charData = SaveSystem.LoadChar();
        character = charData.character;
        skin = charData.skin;
        weapons = charData.weapons;
        maxHealth = charData.maxHealth;
        maxMane = charData.maxMane;
        health = charData.health;
        mane = charData.mane;
        money = charData.money;
        countResurrect = charData.countResurrect;
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
        if (mane + fillAmount > maxMane)
            mane = maxMane;
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
        if (health + healing > maxHealth)
            health = maxHealth;
        else
            health += healing;
        healthBar.SetHealth(health);
    }

    private void GetComponent()
    {
        charAction = GetComponent<CharAction>();
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
    }
}
