using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

#pragma warning disable 0649
    [Tooltip("Список настроек для врагов")]
    [SerializeField] private List<EnemyData> enemiesSettings;

    [Tooltip("Настройка для босса")]
    [SerializeField] private BossData bossSettings;

    [Tooltip("Количество волн")]
    [SerializeField] private int countOfFlocks;

    [Range(0, 100)]
    [Tooltip("Количество объектов в пуле")]
    [SerializeField] private int enemiesCount;

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

    private float nextSpawn;
    private int counter;
    private int currentCountOfFlocks = 0;

    public void SetParameters(LevelInfo levelInfo, List<Vector3> spawnPoints)
    {
        levelType = levelInfo.TypeOfLevel;
        nextSpawn = Time.time + 5f;
        if (spawnPoints.Count != 0)
            this.spawnPoints = spawnPoints;
        else
            this.spawnPoints.Add(Vector3.zero);

        enemiesSettings = levelInfo.EnemiesSettings;
        countOfFlocks = levelInfo.CountOfFlocks;
        enemiesCount = levelInfo.EnemiesCount;

        textTimer = 5;
        spawnTimer.GetComponent<MovementUI>().MoveToEnd();
        InvokeRepeating("OutputTime", 1f, 1f);
    }

    public void SetParameters(LevelInfo levelInfo)
    {
        levelType = levelInfo.TypeOfLevel;
        nextSpawn = Time.time + 5f;

        bossSpawnPoint = levelInfo.BossSpawnPoint;
        bossSettings = levelInfo.bossSetting;

        textTimer = 5;
        spawnTimer.GetComponent<MovementUI>().MoveToEnd();
        InvokeRepeating("OutputTime", 1f, 1f);
    }

    void Update()
    {
        if (Time.time > nextSpawn)
        {
            spawnTimer.GetComponent<MovementUI>().MoveToStart();
            CancelInvoke("OutputTime");
            if (levelType != LevelData.LevelType.Boss)
                SpawnFlock();
            else
                SpawnBoss();
            nextSpawn = int.MaxValue;
        }

        if (counter != 0 && enemies.Count == 0)
        {
            if (currentCountOfFlocks < countOfFlocks)
                SpawnFlock();
            else
            {
                CurrentGameInfo.instance.isWin = true;
                ProgressInfo.instance.SetLevelsForestStar(Regex.Replace(CurrentGameInfo.instance.challengeName, "[0-9]", "", RegexOptions.IgnoreCase));
                GameButtons.instance.GoToFinishScene();
                Destroy(gameObject);
            }

        }
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
                currentEnemy = Instantiate(enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Count)], new Quaternion(0, 0, 0, 0));
                var script = currentEnemy.GetComponent<Enemy>();
                currentEnemy.name = "Enemy " + counter;
                currentEnemy.SetActive(true);
                script.Init(data);
            }
        }
        return currentEnemy;
    }

    public GameObject SpawnBoss()
    {
        GameObject currentBoss = null;
        bossHpBar.GetComponent<MovementUI>().MoveToEnd();
        switch (bossSettings.TypeOfBoss)
        {
            case BossData.BossType.Melee:
                bossPrefab = bossesPrefabs[0];
                break;
        }
        currentBoss = Instantiate(bossPrefab, bossSpawnPoint, new Quaternion(0, 0, 0, 0));
        var script = currentBoss.GetComponent<Boss>();
        currentBoss.name = "Boss";
        currentBoss.SetActive(true);
        script.Init(bossSettings);

        return currentBoss;
    }

    private void OutputTime()
    {
        textTimer--;
        if (textTimer == 0)
            AudioManager.instance.Play("StartChallenge");
        AudioManager.instance.Play("TimerTick");
        spawnTimerText.text = textTimer.ToString();
    }

    public void DeleteEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
}
