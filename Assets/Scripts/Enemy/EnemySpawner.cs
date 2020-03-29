using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Tooltip("Список настроек для врагов")]
    [SerializeField] private List<EnemyData> enemySettings;

    [Tooltip("Количество объектов в пуле")]
    [SerializeField] private int enemyCount;

    [Tooltip("Ссылка на базовый префаб врага")]
    [SerializeField] private GameObject enemyPrefab;

    /// <summary>
    /// Словарь для скриптов
    /// </summary>
    public static Dictionary<GameObject, Enemy> Enemies;

    private void Start()
    {
        Enemies = new Dictionary<GameObject, Enemy>();
        for (int i = 0; i < enemyCount; i++)
        {
            Spawn(enemySettings[Random.Range(0, enemySettings.Count)].EnemyName);
        }

        foreach (var enemy in Enemies)
        {
            enemy.Key.SetActive(true);
        }

        Enemy.OnEnemyDeath += DestroyEnemy;
    }


    public void Spawn(string enemyName)
    {
        foreach (var data in enemySettings)
        {
            if (data.name == enemyName)
            {
                var prefab = Instantiate(enemyPrefab, new Vector3(Random.Range(-25, 25), Random.Range(-25, 25), 0), new Quaternion(0, 0, 0, 0));
                var script = prefab.GetComponent<Enemy>();
                prefab.SetActive(false);
                script.Init(data);
                prefab.transform.tag = "Enemy";
                prefab.GetComponent<SpriteRenderer>().sortingOrder = 2;
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
        Spawn(enemySettings[Random.Range(0, enemySettings.Count)].EnemyName);
    }
}
