using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShootingRange : MonoBehaviour
{
    public static ShootingRange instance;

#pragma warning disable 0649
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

    [Tooltip("UI Тира")]
    [SerializeField] private GameObject shootingRangeInfoUI;

    [Tooltip("Таймер тира")]
    [SerializeField] private GameObject shootingRangeTimer;

    [Tooltip("Текст таймера тира")]
    [SerializeField] private TextMeshProUGUI shootingRangeTimerText;
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
            shootingRangeInfoUI.GetComponent<MovementUI>().MoveToStart();
            result = 0;
            startStand.gameObject.SetActive(false);
            isGame = true;
            GameButtons.isChange = false;
            gameTimer = Time.time + gameDuration;
            charInfo.SpendMana(charInfo.mane - 30);
            player.GetComponent<CharController>().SetSpeed(0);
            player.transform.position = startStand.transform.position;
            Camera.main.orthographicSize = 7f;
            Camera.main.GetComponent<CameraFollow>().StopMove();
            Camera.main.transform.position = new Vector3(10, 22, -1);
        }
        else
        {
            PopupText.Create(startStand.transform.position + new Vector3(0, 1f, 0), true, false, -1, "GiveWeapon");
        }
    }

    private void OutputTime()
    {
        textTimer--;
        shootingRangeTimerText.text = textTimer.ToString(); 
    }

    public void StopGame()
    {
        CancelInvoke("OutputTime");
        shootingRangeInfoUI.GetComponent<MovementUI>().MoveToStart();
        shootingRangeTimer.GetComponent<MovementUI>().MoveToStart();
        startStand.gameObject.SetActive(true);
        GameButtons.isChange = true;
        isGame = false;
        player.GetComponent<CharController>().SetSpeed(startSpeed);
        player.GetComponent<CharInfo>().mane = startMane;
        Camera.main.orthographicSize = 5f;
        Camera.main.GetComponent<CameraFollow>().StartMove();
        Destroy(currentTarget);
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
            Debug.Log(result);
            Destroy(currentTarget);
        }
        else if (currentTarget != null)
            Destroy(currentTarget);

        spawnTimer = Time.time + spawnDuration;
        currentTarget = Instantiate(targetPrefab, targetStands[UnityEngine.Random.Range(0, targetStands.Length)].transform);
    }
}
