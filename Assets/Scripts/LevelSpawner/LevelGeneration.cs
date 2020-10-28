using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGeneration : MonoBehaviour
{
    public static LevelGeneration instance;

    [Serializable]
    public struct ChallengeType
    {
        public string name;
        public GameObject prefab;
    }

#pragma warning disable 0649
    [Tooltip("Маска спаун поинтов")]
    [SerializeField] private LayerMask whatIsSpawnPoint;

    [Tooltip("Комнаты")]
    [SerializeField] public ChallengeType[] challenges;

    [Tooltip("Спецификация куста")]
    [SerializeField] public EnemyData bushStaticTile;
#pragma warning restore 0649


    private Transform leftTop;
    private Transform rightBot;
    private GameObject currentRoom;

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

    public Vector3 StartSpawnLevel(string challengeName)
    {
        var startSpawn = transform.position;
        transform.position = startSpawn;
        foreach (var challenge in challenges)
            if(challenge.name == challengeName)
                currentRoom = Instantiate(challenge.prefab, transform.position, Quaternion.identity);
        leftTop = currentRoom.GetComponent<Room>().floorsTransformLeftTop;
        rightBot = currentRoom.GetComponent<Room>().floorsTransformRightBot;
        SpawnFloor();
        SetEnemySpawnPoints();
        return currentRoom.GetComponent<Room>().characterSpawnPosition.position;
    }

    private void SpawnFloor()
    {
        FloorSpawner floorSpawner = GameObject.Find("GameHandler").GetComponent<FloorSpawner>();
        floorSpawner.SetCorners(leftTop, rightBot);
        floorSpawner.StartSpawn();
    }

    private void SetEnemySpawnPoints()
    {
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
        var levelInfo = currentRoom.GetComponent<LevelInfo>();
        if (!(levelInfo.TypeOfLevel == LevelData.LevelType.Boss))
            EnemySpawner.instance.SetParameters(levelInfo, spawnPoints);
        else
        {
            Debug.Log("!");
            EnemySpawner.instance.SetParameters(levelInfo);
        }
            
    }
}
