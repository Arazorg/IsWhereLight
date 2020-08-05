using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGeneration : MonoBehaviour
{
    public static LevelGeneration instance;

#pragma warning disable 0649
    [Tooltip("Маска спаун поинтов")]
    [SerializeField] private LayerMask whatIsSpawnPoint;

    [Tooltip("Комнаты")]
    [SerializeField] public GameObject[] rooms;
#pragma warning restore 0649

    private Transform leftTop;
    private Transform rightBot;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            if (SceneManager.GetActiveScene().name == "Game")
                DontDestroyOnLoad(gameObject);
        }
    }

    public Vector3 StartSpawnLevel(int roomsNumber)
    {
        var startSpawn = transform.position;
        transform.position = startSpawn;
        var room = Instantiate(rooms[roomsNumber], transform.position, Quaternion.identity);
        leftTop = room.GetComponent<Room>().floorsTransformLeftTop;
        rightBot = room.GetComponent<Room>().floorsTransformRightBot;
        SpawnFloor();
        SetEnemySpawnPoints();
        return room.GetComponent<Room>().characterSpawnPosition.position;
    }

    private void SpawnFloor()
    {
        FloorSpawner floorSpawner = GameObject.Find("GameHandler").GetComponent<FloorSpawner>();
        floorSpawner.SetCorners(leftTop, rightBot);
        floorSpawner.StartSpawn();
    }
    private void SetEnemySpawnPoints()
    {
        EnemySpawner enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        List<Vector3> spawnPoints = new List<Vector3>();
        for (float x = leftTop.position.x + 0.5f; x <= rightBot.position.x; x++)
        {
            for (float y = rightBot.position.y + 0.5f; y <= leftTop.position.y; y++)
            {
                Collider2D[] spawnPoint = Physics2D.OverlapCircleAll(new Vector2(x,y), 1.5f, whatIsSpawnPoint);
                if (spawnPoint.Length == 0)
                    spawnPoints.Add(new Vector3(x, y, 0));
            }
        }
        enemySpawner.SpawnPoints = spawnPoints;
    }


}
