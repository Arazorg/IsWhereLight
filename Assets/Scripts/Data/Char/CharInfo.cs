using System.Collections;
using TMPro;
using UnityEngine;

public class CharInfo : MonoBehaviour
{
    public static CharInfo instance;

    private ManaBar manaBar;
    private HealthBar healthBar;
    private TextMeshProUGUI moneyText;
    private CurrentGameInfo currentGameInfo;
    private CharParametrs charParametrs;
    private CharAction charAction;
    private GameObject player;

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
    private float timeOfKnoking = float.MinValue;

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

    public void ReviveParametrs()
    {
        GetComponents();
        FindObjects();
        health = maxHealth;
        mane = maxMane;
        currentCountKilledEnemies = 0;
        currentCountShoots = 0;
        SetObjects();
    }

    private void FindObjects()
    {
        GetComponents();
        manaBar = GameObject.Find("Canvas").transform.Find("CharacterControlUI").transform.GetComponentInChildren<ManaBar>();
        healthBar = GameObject.Find("Canvas").transform.Find("CharacterControlUI").transform.GetComponentInChildren<HealthBar>();
        moneyText = GameObject.Find("Canvas").transform.Find("CharacterControlUI").transform.Find("MoneyImage").transform.Find("MoneyText").GetComponent<TextMeshProUGUI>();
    }

    private void SetObjects()
    {
        manaBar.SetMaxMin(maxMane, maxMane, 0);
        healthBar.SetMaxMin(maxHealth, maxHealth, 0);
        Camera.main.GetComponent<CameraShaker>().Target = GameObject.Find("Character(Clone)").transform;
    }

    private void GetComponents()
    {
        charAction = GameObject.Find("Character(Clone)").GetComponent<CharAction>();
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

    public void Damage(int damage, Transform objectTransform = null, float knoking = 0f)
    {
        if (player == null)
            player = GameObject.Find("Character(Clone)");
        if(!CharSkills.isUsingSkill)
        {
            if (health - damage < 0)
                health = 0;
            else
            {
                Knoking();
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

    private void Knoking()
    {
        if (!CharAction.isDeath && Time.time > timeOfKnoking)
        {
            CameraShaker.instance.ShakeOnce(1f, 1f, .2f, .15f);
            timeOfKnoking = Time.time + 0.75f;
            ShakeGameObject(gameObject, 0.15f, 0.075f);
        }
    }

    private bool isShaking = false;
    public IEnumerator ShakeGameObjectCOR(GameObject objectToShake, float totalShakeDuration, float decreasePoint)
    {
        if (decreasePoint >= totalShakeDuration)
        {
            Debug.LogError("decreasePoint must be less than totalShakeDuration...Exiting");
            yield break;
        }

        Transform objTransform = objectToShake.transform;
        Vector3 defaultPos = objTransform.position;
        Quaternion defaultRot = objTransform.rotation;

        float counter = 0f;
        const float speed = 0.1f;
        const float angleRot = 1.5f;

        while (counter < totalShakeDuration)
        {
            counter += Time.deltaTime;
            float decreaseSpeed = speed;

            Vector3 tempPosition = defaultPos + Random.insideUnitSphere * decreaseSpeed;
            tempPosition.z = defaultPos.z;

            objTransform.position = tempPosition;
            objTransform.rotation = defaultRot * Quaternion.AngleAxis(Random.Range(-angleRot, angleRot), new Vector3(0f, 0f, 1f));
            yield return null;

            if (counter >= decreasePoint)
            {
                counter = 0f;
                while (counter <= decreasePoint)
                {
                    counter += Time.deltaTime;
                    decreaseSpeed = Mathf.Lerp(speed, 0, counter / decreasePoint);
                    float decreaseAngle = Mathf.Lerp(angleRot, 0, counter / decreasePoint);

                    Vector3 tempPos = defaultPos + Random.insideUnitSphere * decreaseSpeed;
                    tempPos.z = defaultPos.z;
                    objTransform.position = tempPos;
                    objTransform.rotation = defaultRot * Quaternion.AngleAxis(Random.Range(-decreaseAngle, decreaseAngle), new Vector3(0f, 0f, 1f));

                    yield return null;
                }
                break;
            }
        }
        objTransform.position = defaultPos;
        objTransform.rotation = defaultRot;
        isShaking = false;
    }


    public void ShakeGameObject(GameObject objectToShake, float shakeDuration, float decreasePoint)
    {
        if (isShaking)
            return;
        isShaking = true;
        StartCoroutine(ShakeGameObjectCOR(objectToShake, shakeDuration, decreasePoint));
    }

    public void Healing(int healing)
    {
        if (health + healing > maxHealth)
            health = maxHealth;
        else
            health += healing;
        healthBar.SetHealth(health);
    }

    public void MoneyPlus(int money)
    {
        this.money += money;
        moneyText.text = this.money.ToString();
    }
}
