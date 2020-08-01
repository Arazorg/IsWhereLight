using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShootingRange : MonoBehaviour
{
    public static ShootingRange instance;

#pragma warning disable 0649
    [Tooltip("Список настроек для врагов")]
    [SerializeField] private List<EnemyData> enemySettings;

    [Tooltip("Пребаф цели")]
    [SerializeField] private GameObject targetPrefab;

    [Tooltip("Длительность игры")]
    [SerializeField] private float gameDuration;

    [Tooltip("Длительность спауна")]
    [SerializeField] private float spawnDuration;

    [Tooltip("Стэнд спауна оружия")]
    [SerializeField] private Transform weaponStand;

    [Tooltip("Место старта игры")]
    [SerializeField] private Transform startStand;

    [Tooltip("Стэнды спауна мишеней")]
    [SerializeField] private GameObject[] targetStands;

    [Tooltip("NPC тира")]
    [SerializeField] private GameObject shootingRangeNPC;

    [Tooltip("Таймер тира")]
    [SerializeField] private GameObject shootingRangeTimer;

    [Tooltip("Текст таймера тира")]
    [SerializeField] private TextMeshProUGUI shootingRangeTimerText;

    [Tooltip("Время нового привествия НПС")]
    [SerializeField] private float helloTime = 60f;

    [Tooltip("Время нового предупреждения НПС")]
    [SerializeField] private float giveWeaponTime = 2f;
#pragma warning restore 0649

    private float gameTimer;
    private float spawnTimer;
    private int textTimer = 0;
    private GameObject currentTarget;
    private bool isGame;
    private float startSpeed;
    private int startMane;
    private GameObject player;
    private CharInfo charInfo;
    private CharGun charGun;
    private int result;
    private int previousStand = -1;
    private float timeToHello = 0;
    private float timeToGiveWeapon = 0;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        isGame = false;
        SpawnShootingRangeWeapon();
    }

    void Update()
    {
        if ((Time.time > gameTimer || charInfo.mane == 0) && isGame)
            StopGame();
        if (isGame && Time.time > spawnTimer)
        {
            Spawn();
        }
    }

    public void StartGame()
    {
        player = GameObject.Find("Character(Clone)");
        startSpeed = player.GetComponent<CharController>().GetSpeed();
        charInfo = player.GetComponent<CharInfo>();
        charGun = player.GetComponent<CharGun>();
        startMane = player.GetComponent<CharInfo>().mane;

        if (charInfo.weapons[0] == "ShootingRangeWeapon0" ||
                    charInfo.weapons[1] == "ShootingRangeWeapon1")
        {
            textTimer = (int)gameDuration;
            InvokeRepeating("OutputTime", 1f, 1f);
            if (charInfo.weapons[0] == "ShootingRangeWeapon0" && charGun.currentWeaponNumber != 0)
                GameButtons.instance.SwapWeapon();
            else if (charInfo.weapons[1] == "ShootingRangeWeapon1" && charGun.currentWeaponNumber != 1)
                GameButtons.instance.SwapWeapon();
            shootingRangeTimer.GetComponent<MovementUI>().MoveToEnd();
            result = 0;
            startStand.gameObject.SetActive(false);
            isGame = true;
            GameButtons.isChange = false;
            gameTimer = Time.time + gameDuration;
            charInfo.mane = 30;
            charInfo.SpendMana(0);
            player.GetComponent<CharController>().SetSpeed(0);
            player.transform.position = startStand.transform.position;
            Camera.main.orthographicSize = 7f;
            Camera.main.GetComponent<CameraFollow>().StopMove();
            Camera.main.transform.position = new Vector3(-9, 10.75f, -1);
            PopupText.Create(shootingRangeNPC.transform.position + new Vector3(0, 1f, 0), true, false, -1, "ShootingRangeInfo", 5);
            SetCollider(true);
        }
        else
        {
            if (Time.time > timeToGiveWeapon && Time.time + helloTime - timeToHello > (int)PopupText.DISAPPEAR_TIMER_MAX_PHRASE + 1)
            {
                PopupText.Create(shootingRangeNPC.transform.position + new Vector3(0, 1f, 0), true, false, -1, "GiveWeapon");
                timeToGiveWeapon = Time.time + giveWeaponTime;
            }
            
        }
           
    }

    public void StopGame()
    {
        CancelInvoke("OutputTime");
        shootingRangeTimer.GetComponent<MovementUI>().MoveToStart();
        startStand.gameObject.SetActive(true);
        GameButtons.isChange = true;
        isGame = false;
        player.GetComponent<CharController>().SetSpeed(startSpeed);
        charInfo.mane = startMane;
        charInfo.SpendMana(0);
        Camera.main.orthographicSize = 5f;
        Camera.main.GetComponent<CameraFollow>().StartMove();
        Destroy(currentTarget);
        if (result > 5)
            PopupText.Create(shootingRangeNPC.transform.position + new Vector3(0, 1f, 0), true, false, -1, "GreatScore");
        else
            PopupText.Create(shootingRangeNPC.transform.position + new Vector3(0, 1f, 0), true, false, -1, "WeakScore");
        SetCollider(false);
    }

    private void OutputTime()
    {
        textTimer--;
        shootingRangeTimerText.text = textTimer.ToString();
    }

    private void SpawnShootingRangeWeapon()
    {
        WeaponSpawner.instance.SetPrefab("ShootingRangeWeapon");
        WeaponSpawner.instance.Spawn("ShootingRangeWeapon", weaponStand);
    }

    public void Spawn(bool isDeath = false)
    {
        if (currentTarget != null && isDeath)
        {
            result++;
            PopupText.Create(shootingRangeNPC.transform.position + new Vector3(0, 1f, 0), true, false, -1, "GreatShoot", 5);
            currentTarget.GetComponent<Animator>().SetBool("isDeath", true);
            Destroy(currentTarget, 0.5f);
        }
        else if (currentTarget != null)
            Destroy(currentTarget);


        spawnTimer = Time.time + spawnDuration;
        int currentStand;
        while (true)
        {
            currentStand = UnityEngine.Random.Range(0, targetStands.Length);
            if (currentStand == previousStand)
                continue;
            currentTarget = Instantiate(targetPrefab, targetStands[currentStand].transform);
            var script = currentTarget.GetComponent<Enemy>();
            var data = enemySettings[UnityEngine.Random.Range(0, enemySettings.Count)];
            script.Init(data);
            currentTarget.GetComponent<SpriteRenderer>().sortingOrder = 2;
            currentTarget.transform.position += new Vector3(0, 0.5f, 0);
            break;
        }
        previousStand = currentStand;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (Time.time > timeToHello && Time.time + giveWeaponTime - timeToGiveWeapon > (int)PopupText.DISAPPEAR_TIMER_MAX_PHRASE + 1)
        {
            PopupText.Create(shootingRangeNPC.transform.position + new Vector3(0, 1f, 0), true, false, -1, "Hello");
            timeToHello = Time.time + helloTime;
        }
    }

    private void SetCollider(bool isGame)
    {
        foreach (var stand in targetStands)
        {
            if(isGame)
            {
                stand.GetComponent<Collider2D>().isTrigger = true;
                stand.transform.tag = "IgnoreAll";
            }
            else
            {
                stand.GetComponent<Collider2D>().isTrigger = false;
                stand.transform.tag = "Untagged";
            }
            
        }
    }
}
