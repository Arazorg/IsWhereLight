using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGeneration : MonoBehaviour
{
    public static LevelGeneration instance;

#pragma warning disable 0649
    [Tooltip("Комнаты")]
    [SerializeField] public GameObject[] rooms;
#pragma warning restore 0649

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
        FloorSpawner floorSpawner = GameObject.Find("GameHandler").GetComponent<FloorSpawner>();
        var room = Instantiate(rooms[roomsNumber], transform.position, Quaternion.identity);

        floorSpawner.SetCorners
            (room.GetComponent<Room>().floorsTransformLeftTop,
               room.GetComponent<Room>().floorsTransformRightBot);
        floorSpawner.StartSpawn();
        return room.GetComponent<Room>().characterSpawnPosition.position;
    }
}
