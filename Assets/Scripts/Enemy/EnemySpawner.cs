using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Список настроек для врагов")]
    [SerializeField] private List<EnemyData> enemySettings;

    [Range(0, 125)]
    [Tooltip("Количество объектов в пуле")]
    [SerializeField] private int enemyCount;

    [Tooltip("Ссылка на префабы врагов")]
    [SerializeField] private GameObject[] enemiesPrefabs;

    [Tooltip("Таймер до спауна")]
    [SerializeField] private GameObject spawnTimer;

    [Tooltip("Текст таймера до спауна")]
    [SerializeField] private TextMeshProUGUI spawnTimerText;
#pragma warning restore 0649

    public static int textTimer;

    private GameObject enemyPrefab;

    private readonly float spawnRate = 24f;
    private float nextSpawn; 
    private int counter;

    private List<Vector3> spawnPoints = new List<Vector3>();
    public List<Vector3> SpawnPoints
    {
        get { return spawnPoints; }
        set 
        {
            if (value.Count != 0)
                spawnPoints = value;
            else
                spawnPoints.Add(Vector3.zero);
            textTimer = 5;
            spawnTimer.GetComponent<MovementUI>().MoveToEnd();
            InvokeRepeating("OutputTime", 1f, 1f);           
        }
    }

    void Start()
    {
        nextSpawn = Time.time + 5f;
    }

    void Update()
    {
        if (Time.time > nextSpawn)
        {
            SpawnFlock();
            nextSpawn = Time.time + spawnRate;
        }
    }

    private void SpawnFlock()
    {
        CancelInvoke("OutputTime");
        CurrentGameInfo.instance.currentWave++;
        spawnTimer.GetComponent<MovementUI>().MoveToStart();
        for (int i = 0; i < enemyCount; i++)
        {
            Spawn(enemySettings[Random.Range(0, enemySettings.Count)].EnemyName, counter);
            counter++;
        }
    }

    public void Spawn(string enemyName, int counter)
    {
        foreach (var data in enemySettings)
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
                var prefab = Instantiate(enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Count)], new Quaternion(0, 0, 0, 0));
                var script = prefab.GetComponent<Enemy>();
                prefab.name = "Enemy " + counter;
                prefab.SetActive(true);
                script.Init(data);
            }
        }
    }

    private void OutputTime()
    {
        textTimer--;
        if(textTimer == 0)
            AudioManager.instance.Play("StartChallenge");
        AudioManager.instance.Play("TimerTick");
        spawnTimerText.text = textTimer.ToString();
    }
}
