using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Tooltip("Стэнд награды")]
    [SerializeField] private Transform rewardStand;

    [Tooltip("Место старта игры")]
    [SerializeField] private Transform startStand;

    [Tooltip("Стэнды спауна мишеней")]
    [SerializeField] private GameObject[] targetStands;

    [Tooltip("NPC тира")]
    [SerializeField] private GameObject shootingRangeNPC;

    [Tooltip("Панель выбора сложности в тире")]
    [SerializeField] private GameObject shootingRangeUI;

    [Tooltip("Текст таймера тира")]
    [SerializeField] private TextMeshProUGUI shootingRangeTimerText;

    [Tooltip("Текст описания сложности игры в тире")]
    [SerializeField] private TextMeshProUGUI shootingRangeText;

    [Tooltip("Кнопка начала игры")]
    [SerializeField] private Button playButton;
#pragma warning restore 0649

    private GameObject player;
    private CharInfo charInfo;
    private CharGun charGun;
    private GameObject currentTarget;
    private PopupText currentPhrase = null;
    private AudioManager audioManager;

    private float gameTimer;
    private float spawnTimer;
    private float startSpeed;

    private int textTimer = 0;
    private int startMane;
    private int result;
    private int previousStand = -1;
    private int difficultyLevel;

    private bool isGame;
    private bool isHello = false;

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
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        isGame = false;
        SpawnShootingRangeWeapon();
    }

    void Update()
    {
        if ((Time.time > gameTimer || charInfo.mane == 0) && isGame)
            StopGame();
        if (isGame && Time.time > spawnTimer)
            Spawn();
    }

    public void ShowDifficultyPanel()
    {
        player = GameObject.Find("Character(Clone)");
        charInfo = player.GetComponent<CharInfo>();
        if (charInfo.weapons[0] == "Shooting Range Weapon0" ||
                    charInfo.weapons[1] == "Shooting Range Weapon1")
            shootingRangeUI.GetComponent<MovementUI>().MoveToEnd();
        else
        {
            if (currentPhrase != null)
                currentPhrase.DeletePhrase();
            currentPhrase = PopupText.Create(shootingRangeNPC.transform.position 
                    + new Vector3(0, 1f, 0), true, false, -1, "GiveWeapon");
        }

    }

    public void CloseDifficultyPanel()
    {
        shootingRangeUI.GetComponent<MovementUI>().MoveToStart();
        shootingRangeText.GetComponentInParent<MovementUI>().MoveToStart();
        playButton.GetComponent<MovementUI>().SetStart();
    }

    public void ChooseDifficulty(int difficultyLevel)
    {
        audioManager.Play("ClickUI");
        this.difficultyLevel = difficultyLevel;
        if (difficultyLevel == 1)
            spawnDuration = 2f;
        else
            spawnDuration = 1f;
        shootingRangeText.GetComponentInParent<MovementUI>().MoveToEnd();
        shootingRangeText.GetComponent<LocalizedText>().key = $@"ShootingRangeDifficulty{difficultyLevel}";
        shootingRangeText.GetComponent<LocalizedText>().SetLocalization();
        playButton.GetComponent<MovementUI>().MoveToEnd();
    }

    public void StartGame()
    {
        shootingRangeUI.GetComponent<MovementUI>().MoveToStart();
        shootingRangeText.GetComponentInParent<MovementUI>().MoveToStart();
        playButton.GetComponent<MovementUI>().MoveToStart();
        startSpeed = player.GetComponent<CharController>().Speed;
        charGun = player.GetComponent<CharGun>();
        startMane = player.GetComponent<CharInfo>().mane;
        textTimer = (int)gameDuration;
        InvokeRepeating("OutputTime", 1f, 1f);
        Debug.Log(charInfo.weapons[1]);
        if (charInfo.weapons[0] == "Shooting Range Weapon0" && charGun.CurrentWeaponNumber != 0)
            GameButtons.instance.SwapWeapon();
        else if (charInfo.weapons[1] == "Shooting Range Weapon1" && charGun.CurrentWeaponNumber != 1)
            GameButtons.instance.SwapWeapon();
        shootingRangeTimerText.gameObject.SetActive(true);
        shootingRangeTimerText.GetComponent<MovementUI>().MoveToEnd();
        result = 0;
        startStand.gameObject.SetActive(false);
        isGame = true;
        GameButtons.isChange = false;
        gameTimer = Time.time + gameDuration;
        if (difficultyLevel == 1)
            charInfo.mane = 30;
        else
            charInfo.mane = 60;
        charInfo.SpendMana(0);
        player.GetComponent<CharController>().Speed = 0;
        player.transform.position = startStand.transform.position;
        Camera.main.orthographicSize = 7f;
        Camera.main.GetComponent<CameraFollow>().IsMove = false;
        Camera.main.transform.position = new Vector3(-15, 11.25f, -1);
        if (currentPhrase != null)
            currentPhrase.DeletePhrase();
        currentPhrase = PopupText.Create(shootingRangeNPC.transform.position 
            + new Vector3(0, 1f, 0), true, false, -1, "ShootingRangeInfo", 5);
        SetCollider(true);
    }

    public void StopGame()
    {
        CancelInvoke("OutputTime");
        shootingRangeTimerText.GetComponent<MovementUI>().MoveToStart();
        shootingRangeTimerText.gameObject.SetActive(false);
        startStand.gameObject.SetActive(true);
        GameButtons.isChange = true;
        isGame = false;
        player.GetComponent<CharController>().Speed = startSpeed;
        charInfo.mane = startMane;
        charInfo.SpendMana(0);
        Camera.main.orthographicSize = 5f;
        Camera.main.GetComponent<CameraFollow>().IsMove = true;
        Destroy(currentTarget);
        if (currentPhrase != null)
            currentPhrase.DeletePhrase();
        if (difficultyLevel == 1)
            Result(5);
        else
            Result(10);

        SetCollider(false);
    }

    private void Result(int countShots)
    {
        WeaponSpawner.instance.SetPrefab("Healing Beam Blaster");
        WeaponSpawner.instance.Spawn("Healing Beam Blaster", rewardStand);
        if (result > countShots)
        {
            currentPhrase = PopupText.Create(shootingRangeNPC.transform.position
                + new Vector3(0, 1f, 0), true, false, -1, $"GreatScore{UnityEngine.Random.Range(0, 5)}");
        }   
        else
            currentPhrase = PopupText.Create(shootingRangeNPC.transform.position
                + new Vector3(0, 1f, 0), true, false, -1, $"WeakScore{UnityEngine.Random.Range(0, 3)}");
    }
    private void OutputTime()
    {
        textTimer--;
        shootingRangeTimerText.text = textTimer.ToString();
    }

    private void SpawnShootingRangeWeapon()
    {
        WeaponSpawner.instance.SetPrefab("Shooting Range Weapon");
        WeaponSpawner.instance.Spawn("Shooting Range Weapon", weaponStand);
    }

    public void Spawn(bool isDeath = false)
    {
        if (currentTarget != null && isDeath)
        {
            result++;
            if (currentPhrase != null)
                currentPhrase.DeletePhrase();
            currentPhrase = PopupText.Create(shootingRangeNPC.transform.position 
                    + new Vector3(0, 1f, 0), true, false, -1, $"GreatShoot{UnityEngine.Random.Range(0, 5)}", 5);
            currentTarget.GetComponent<Animator>().SetBool("isDeath", true);
            AnimationClip[] clips = currentTarget.GetComponent<Animator>().runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                switch (clip.name)
                {
                    case "Death":
                        Destroy(currentTarget, clip.length);
                        break;
                }
            }
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
        if (!isHello)
        {
            if (currentPhrase != null)
                currentPhrase.DeletePhrase();
            currentPhrase = PopupText.Create
                (shootingRangeNPC.transform.position 
                    + new Vector3(0, 1f, 0), true, false, -1, $"Hello{UnityEngine.Random.Range(0, 6)}");
            isHello = true;
        }
    }

    private void SetCollider(bool isGame)
    {
        foreach (var stand in targetStands)
        {
            if (isGame)
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
