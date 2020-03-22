using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Tooltip("Список настроек для врагов")]
    [SerializeField] private List<EnemyData> enemySettings;

    [Tooltip("Количество объектов в пуле")]
    [SerializeField] private int poolCount;

    [Tooltip("Ссылка на базовый префаб врага")]
    [SerializeField] private GameObject enemyPrefab;

    [Tooltip("Время между спауном врагов")]
    [SerializeField] private float spawnTime;

    /// <summary>
    /// Словарь для скриптов
    /// </summary>
    public static Dictionary<GameObject, Enemy> Enemies;
    private Queue<GameObject> currentEnemies;

    private void Start()
    {
        Enemies = new Dictionary<GameObject, Enemy>();
        currentEnemies = new Queue<GameObject>();

        for (int i = 0; i < poolCount; ++i)
        {
            var prefab = Instantiate(enemyPrefab, new Vector3(Random.Range(-25,25), Random.Range(-25, 25), 0), new Quaternion(0,0,0,0));
            var script = prefab.GetComponent<Enemy>();
            prefab.SetActive(false);
            Enemies.Add(prefab, script);
            currentEnemies.Enqueue(prefab);
        }
        Enemy.OnEnemyDeath += ReturnEnemy;
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        if(spawnTime == 0)
        {
            Debug.LogError("Не выставленно время спауна, заданное стандартное время - 1 сек.");
            spawnTime = 1;
        }
        while(true)
        {
            yield return new WaitForSeconds(spawnTime);
            if(currentEnemies.Count > 0)
            {
                //получение компонентов и активация врага
                var enemy = currentEnemies.Dequeue();
                var script = Enemies[enemy];
                enemy.SetActive(true);
                int rand = Random.Range(0, enemySettings.Count);
                script.Init(enemySettings[rand]);
            }
        }
    }
    /// <summary>
    /// Возврат объекта обратно в пул и подготовка к повторному использованию
    /// </summary>
    /// <param name="enemy"></param>
    private void ReturnEnemy(GameObject _enemy)
    {
        _enemy.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
        _enemy.SetActive(false);
        currentEnemies.Enqueue(_enemy);
    }
}
