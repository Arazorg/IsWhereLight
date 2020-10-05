﻿using UnityEngine;

public class CharInfo : MonoBehaviour
{
    public static CharInfo instance;

    private ManaBar manaBar;
    private HealthBar healthBar;
    private CurrentGameInfo currentGameInfo;
    private CharParametrs charParametrs;
    private CharAction charAction;

    public string character;
    public string skin;
    public string[] weapons;
    public int maxHealth;
    public int maxMane;
    public int health;
    public int mane;
    public int money;
    public int currentCountKilledEnemies;
    public int currentCountShoots;

    private readonly float damageSoundTime = 0.75f;
    private float timeToDamageSound;
    private bool isDamageSound;

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        if(Time.time > timeToDamageSound)
        {
            isDamageSound = true;
            timeToDamageSound = Time.time + damageSoundTime;
        }
    }

    public void SetStartParams()
    {
        GetComponents();
        FindObjects();
        weapons = new string[2];
        character = currentGameInfo.character;
        skin = currentGameInfo.skin;
        weapons[0] = currentGameInfo.startWeapon;
        weapons[1] = null;
        maxHealth = charParametrs.CharHp;
        maxMane = charParametrs.CharMane;
        health = maxHealth;
        mane = maxMane;
        money = 0;
        currentCountKilledEnemies = 0;
        currentCountShoots = 0;
        SetObjects();
    }

    private void FindObjects()
    {
        GetComponents();
        manaBar = GameObject.Find("Canvas").transform.Find("CharacterControlUI").transform.GetComponentInChildren<ManaBar>();
        healthBar = GameObject.Find("Canvas").transform.Find("CharacterControlUI").transform.GetComponentInChildren<HealthBar>();
    }

    private void SetObjects()
    {
        manaBar.SetMaxMin(maxMane, maxMane, 0);
        healthBar.SetMaxMin(maxHealth, maxHealth, 0);
        Camera.main.GetComponent<CameraShaker>().Target = GameObject.Find("Character(Clone)").transform;
    }

    private void GetComponents()
    {
        charAction = GetComponent<CharAction>();
        charParametrs = GameObject.Find("CharParametrsHandler").GetComponent<CharParametrs>();
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
        manaBar.SetMane(mane);
    }

    public void Damage(int damage)
    {
        if(!GetComponent<CharSkills>().isLegionnaireSkill)
        {
            if (health - damage < 0)
                health = 0;
            else
            {
                charAction.IsPlayerHitted = true;
                charAction.IsEnterFirst = true;
                if (isDamageSound)
                {
                    AudioManager.instance.Play($"{character}Damage");
                    isDamageSound = false;
                }
                health -= damage;
            }
            healthBar.SetHealth(health);

            if (health <= 0)
            {
                charAction.Death();
            }
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
