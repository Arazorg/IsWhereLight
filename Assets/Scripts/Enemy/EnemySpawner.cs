using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

#pragma warning disable 0649
    [Tooltip("Список настроек для врагов")]
    [SerializeField] private List<EnemyData> enemiesSettings;

    [Tooltip("Настройка для босса")]
    [SerializeField] private BossData bossSettings;

    [Tooltip("Средний процент элитных врагов на уровне")]
    [SerializeField] private int percentOfEliteEnemy;

    [Tooltip("Количество волн")]
    [SerializeField] private int countOfFlocks;

    [Range(0, 100)]
    [Tooltip("Количество объектов в пуле")]
    [SerializeField] private int enemiesCount;

    [Tooltip("Префабы места спауна врага")]
    [SerializeField] private GameObject enemySpawnPointPrefab;

    [Tooltip("Префабы врагов")]
    [SerializeField] private GameObject[] enemiesPrefabs;

    [Tooltip("Префабы боссов")]
    [SerializeField] private GameObject[] bossesPrefabs;

    [Tooltip("Таймер до спауна")]
    [SerializeField] private GameObject spawnTimer;

    [Tooltip("Текст таймера до спауна")]
    [SerializeField] private TextMeshProUGUI spawnTimerText;

    [Tooltip("Панель здоровья босса")]
    [SerializeField] public GameObject bossHpBar;

    [Tooltip("Текст имени босса")]
    [SerializeField] public GameObject bossNameText;
#pragma warning restore 0649

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

    public int textTimer;

    private GameObject enemyPrefab;
    private GameObject bossPrefab;

    private LevelData.LevelType levelType;
    private List<GameObject> enemies = new List<GameObject>();
    private List<Vector3> spawnPoints = new List<Vector3>();
    private Vector3 bossSpawnPoint = new Vector3();

    private int counter = 0;
    private int currentCountOfFlocks = 0;

    public void SetParameters(LevelInfo levelInfo, List<Vector3> spawnPoints)
    {
        levelType = levelInfo.TypeOfLevel;
        enemiesSettings = levelInfo.EnemiesSettings;
        countOfFlocks = levelInfo.CountOfFlocks;
        percentOfEliteEnemy = levelInfo.PercentOfEliteEnemy;
        enemiesCount = levelInfo.EnemiesCount;

        if (spawnPoints.Count != 0)
            SetSpawnPoints(spawnPoints);
        else
            this.spawnPoints.Add(Vector3.zero);

        textTimer = 5;
        spawnTimer.GetComponent<MovementUI>().MoveToEnd();
        InvokeRepeating("OutputTime", 1f, 1f);
        Invoke("DisableTimer", 5f);
        Invoke("FirstSpawn", 6f);
    }

    public void SetParameters(LevelInfo levelInfo)
    {
        levelType = levelInfo.TypeOfLevel;

        bossSpawnPoint = levelInfo.BossSpawnPoint;
        bossSettings = levelInfo.bossSetting;

        textTimer = 5;
        spawnTimer.GetComponent<MovementUI>().MoveToEnd();
        InvokeRepeating("OutputTime", 1f, 1f);
        Invoke("DisableTimer", 5f);
        Invoke("FirstSpawn", 6f);
    }

    private void SetSpawnPoints(List<Vector3> spawnPoints)
    {
        for (int i = 0; i < enemiesCount; ++i)
        {
            var point = spawnPoints[Random.Range(0, spawnPoints.Count)];
            spawnPoints.Remove(point);
            this.spawnPoints.Add(point);
        }
    }

    void Update()
    {
        if (counter != 0 && enemies.Count == 0)
        {
            if (currentCountOfFlocks < countOfFlocks)
            {
                CreateEnemySpawnPoints();
                Invoke("SpawnFlock", 1f);
            }
            else
            {
                GameObject.Find("Character(Clone)").GetComponent<CharController>().SetZeroSpeed(true);
                ProgressInfo.instance.SetLevelsForestStar(Regex.Replace(CurrentGameInfo.instance.challengeName, "[0-9]", "", RegexOptions.IgnoreCase));
                ProgressInfo.instance.SaveProgress();
                GameObject.Find("Canvas").transform.Find("EndGamePanel").GetComponent<EndGameUI>().OpenPanel(true);
            }
        }
    }

    private void DisableTimer()
    {
        spawnTimer.GetComponent<MovementUI>().MoveToStart();
        CancelInvoke("OutputTime");
        CreateEnemySpawnPoints();
    }

    private void FirstSpawn()
    {
        if (levelType != LevelData.LevelType.Boss)
            SpawnFlock();
        else
            SpawnBoss();
    }

    private void CreateEnemySpawnPoints()
    {
        if (levelType != LevelData.LevelType.Boss)
            foreach (var spawnPoint in spawnPoints)
                Destroy(Instantiate(enemySpawnPointPrefab, spawnPoint, Quaternion.identity), 0.75f);
    }

    private void SpawnFlock()
    {
        for (int i = 0; i < enemiesCount; i++)
        {
            var currentEnemy = Spawn(enemiesSettings[Random.Range(0, enemiesSettings.Count)].EnemyName, counter);
            if (currentEnemy != null)
                enemies.Add(currentEnemy);
            counter++;
        }
        currentCountOfFlocks++;
    }

    public GameObject Spawn(string enemyName, int counter)
    {
        GameObject currentEnemy = null;
        foreach (var data in enemiesSettings)
        {
            if (data.name == enemyName)
            {
                switch (data.TypeOfAttack)
                {
                    case EnemyData.AttackType.Distant:
                        enemyPrefab = enemiesPrefabs[0];
                        break;
                    case EnemyData.AttackType.Melee:
                        enemyPrefab = enemiesPrefabs[1];
                        break;
                }
                var currentSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                spawnPoints.Remove(currentSpawnPoint);
                currentEnemy = Instantiate(enemyPrefab, currentSpawnPoint, new Quaternion(0, 0, 0, 0));
                var script = currentEnemy.GetComponent<Enemy>();
                currentEnemy.name = "Enemy " + counter;
                currentEnemy.SetActive(true);
                script.Init(data);

                if (Random.Range(0, 101) < percentOfEliteEnemy)
                {
                    var scaleFactor = Random.Range(1.01f, 1.51f);
                    currentEnemy.transform.localScale = new Vector3(scaleFactor, scaleFactor);
                    script.AttackRange *= scaleFactor;

                    var parametrsFactor = Random.Range(1.01f, 2f);
                    script.Damage = (int)System.Math.Floor(script.Damage * parametrsFactor);
                    script.Health = (int)System.Math.Floor(script.Health * parametrsFactor);
                    script.Speed = (int)System.Math.Floor(script.Health * parametrsFactor);
                    script.FireRate *= Random.Range(0.75f, 1.01f);
                    script.Init(data);
                }
            }
        }
        return currentEnemy;
    }

    public GameObject SpawnBoss()
    {
        bossHpBar.GetComponent<MovementUI>().MoveToEnd();
        switch (bossSettings.TypeOfBoss)
        {
            case BossData.BossType.Melee:
                bossPrefab = bossesPrefabs[0];
                break;
        }
        GameObject currentBoss = Instantiate(bossPrefab, bossSpawnPoint, new Quaternion(0, 0, 0, 0));
        var script = currentBoss.GetComponent<Boss>();
        currentBoss.name = "Boss";
        currentBoss.SetActive(true);
        script.Init(bossSettings);
        bossNameText.GetComponent<LocalizedText>().key = "Boss" + script.EnemyName;
        enemies.Add(currentBoss);
        currentCountOfFlocks++;
        counter++;
        return currentBoss;
    }

    public void DeleteEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    private void OutputTime()
    {
        textTimer--;
        if (textTimer == 0)
            AudioManager.instance.Play("StartChallenge");
        AudioManager.instance.Play("TimerTick");
        spawnTimerText.text = textTimer.ToString();
    }
}
