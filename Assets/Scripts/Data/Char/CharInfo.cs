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
    public int currentCountKilledEnemies;
    public int currentCountShoots;

    public void Init(CharData data)
    {
        character = data.character;
        skin = data.skin;
        weapons = data.weapons;
        maxHealth = data.maxHealth;
        maxMane = data.maxMane;
        health = data.health;
        mane = data.mane;
        money = data.money;
        countResurrect = data.countResurrect;
        currentCountKilledEnemies = data.currentCountKilledEnemies;
        currentCountShoots = data.currentCountShoots;
    }
    public void SetStartParams()
    {
        FindObjects();
        weapons = new string[2];
        character = currentGameInfo.character;
        skin = currentGameInfo.skin;
        weapons[0] = currentGameInfo.startWeapon;
        weapons[1] = null;
        maxHealth = currentGameInfo.maxHealth;
        maxMane = currentGameInfo.maxMane;
        health = maxHealth;
        mane = maxMane;
        money = 0;
        countResurrect = 1;
        currentCountKilledEnemies = 0;
        currentCountShoots = 0;
        SetObjects();
    }

    public void SaveChar()
    {
        string json = JsonUtility.ToJson(this);
        NewSaveSystem.Save("character", json);
    }

    public void LoadChar()
    {
        GetComponents();
        SetStartParams();
        var charString = NewSaveSystem.Load("character");
        if (charString != null)
        {
            CharData saveObject = JsonUtility.FromJson<CharData>(charString);
            Init(saveObject);
        };
        SetObjects();
    }

    private void FindObjects()
    {
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        manaBar = GameObject.Find("Canvas").transform.Find("CharacterControlUI").transform.GetComponentInChildren<ManaBar>();
        healthBar = GameObject.Find("Canvas").transform.Find("CharacterControlUI").transform.GetComponentInChildren<HealthBar>();
    }

    private void SetObjects()
    {
        manaBar.SetMaxMin(mane, maxMane, 0);
        healthBar.SetMaxMin(health, maxHealth, 0);
        Camera.main.GetComponent<CameraFollow>().SetTarget(transform);
    }

    private void GetComponents()
    {
        charAction = GetComponent<CharAction>();
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
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
}
