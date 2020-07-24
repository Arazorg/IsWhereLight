using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Список настроек для врагов")]
    [SerializeField] private List<EnemyData> enemySettings;

    [Range(1, 125)]
    [Tooltip("Количество объектов в пуле")]
    [SerializeField] private int enemyCount;

    [Tooltip("Ссылка на префабы врагов")]
    [SerializeField] private GameObject[] enemiesPrefabs;
#pragma warning restore 0649

    private GameObject enemyPrefab;


    /// <summary>
    /// Словарь для скриптов
    /// </summary>
    public static Dictionary<GameObject, Enemy> Enemies;
    private float nextSpawn = 24f;
    private readonly float spawnRate = 24f;

    int counter;

    private void Start()
    {
        counter = 0;
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
        }
    }

    private void SpawnFlock()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Spawn(enemySettings[Random.Range(0, enemySettings.Count)].EnemyName, counter);
            counter++;
        }

        foreach (var enemy in Enemies)
        {
            enemy.Key.SetActive(true);
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
                var prefab = Instantiate(enemyPrefab, new Vector3(Random.Range(10, 25), Random.Range(10, 25), 0), new Quaternion(0, 0, 0, 0));
                var script = prefab.GetComponent<Enemy>();
                prefab.name = "Enemy " + counter;
                prefab.SetActive(false);
                script.Init(data);
                prefab.GetComponent<SpriteRenderer>().sortingOrder = 2;
                //prefab.GetComponent<EnemyBulletSpawner>().SetBullet(script.dataOfBullet);
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
        Debug.Log("Death");
        Destroy(enemy);
        Enemies.Remove(enemy);
        //Spawn(enemySettings[Random.Range(0, enemySettings.Count)].EnemyName);
    }
}
