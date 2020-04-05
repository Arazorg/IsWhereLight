using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Tooltip("Список настроек для врагов")]
    [SerializeField] private List<EnemyData> enemySettings;

    [Range(1, 125)]
    [Tooltip("Количество объектов в пуле")]
    [SerializeField] private int enemyCount;

    [Tooltip("Ссылка на базовый префаб врага")]
    [SerializeField] private GameObject enemyPrefab;

    LightingManager lightingManager;

    /// <summary>
    /// Словарь для скриптов
    /// </summary>
    public static Dictionary<GameObject, Enemy> Enemies;
    private bool isSpawn;
    private float nextSpawn = 24f;
    private float spawnRate = 24f;

    private void Start()
    {
        lightingManager = GameObject.Find("GameHandler").GetComponent<LightingManager>();
        Enemies = new Dictionary<GameObject, Enemy>();
        SpawnFlock();
        Enemy.OnEnemyDeath += DestroyEnemy;
    }

    private void Update()
    {
        if (Time.time > nextSpawn)
        {
            SpawnFlock();
            nextSpawn = Time.time + spawnRate;
            isSpawn = false;
        }
    }

    private void SpawnFlock()
    {
        int counter = 0;
        for (int i = 0; i < enemyCount; i++)
        {
            Spawn(enemySettings[Random.Range(0, enemySettings.Count)].EnemyName, counter);
            counter++;
        }

        foreach (var enemy in Enemies)
        {
            enemy.Key.SetActive(true);
        }
        enemyCount += 7;
    }

    public void Spawn(string enemyName, int counter)
    {
        foreach (var data in enemySettings)
        {
            if (data.name == enemyName)
            {
                var prefab = Instantiate(enemyPrefab, new Vector3(Random.Range(10, 25), Random.Range(10, 25), 0), new Quaternion(0, 0, 0, 0));
                var script = prefab.GetComponent<Enemy>();
                prefab.name = "Enemy " + counter;
                prefab.SetActive(false);
                script.Init(data);
                prefab.transform.tag = "Enemy";
                prefab.GetComponent<SpriteRenderer>().sortingOrder = 2;
                prefab.GetComponent<EnemyBulletSpawner>().SetBullet(script.dataOfBullet);
                Enemies.Add(prefab, script);
            }
        }
    }

    /// <summary>
    /// Удаление врага и генерация нового
    /// </summary>
    /// <param name="enemy"></param>
    private void DestroyEnemy(GameObject enemy)
    {
        Destroy(enemy);
        Enemies.Remove(enemy);
        //Spawn(enemySettings[Random.Range(0, enemySettings.Count)].EnemyName);
    }
}
