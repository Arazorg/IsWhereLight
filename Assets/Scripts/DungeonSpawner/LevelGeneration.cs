using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGeneration : MonoBehaviour
{
    public static LevelGeneration instance;
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

    public GameObject horizontal;
    public GameObject vertical;
    public GameObject[] horizontalCorridors;
    public GameObject[] verticalCorridors;

    public Transform[] startingPositions;
    public GameObject[] rooms; // index 0 --> LR, index 1 --> LRB, index 2 --> LRT, index 3 --> LRBT

    private int direction;
    public float moveIncrement;

    public float minX;
    public float maxX;
    public float maxY;
    public bool stopGeneration;

    private int topCounter;
    private float timeBtwSpawn;
    public float startTimeBtwSpawn;

    public LayerMask whatIsRoom;

    public Vector3 SpawnLevel()
    {
        foreach (var item in horizontalCorridors)
        {
            Instantiate(horizontal, item.transform.position, Quaternion.identity);
        }

        foreach (var item in verticalCorridors)
        {
            Instantiate(vertical, item.transform.position, Quaternion.identity);
        }
        int randStartingPos = Random.Range(0, startingPositions.Length);
        var startSpawn = startingPositions[randStartingPos].position;
        transform.position = startSpawn;

        Instantiate(rooms[1], transform.position, Quaternion.identity);

        direction = Random.Range(1, 6);

        return startSpawn;
    }

    private void Update()
    {
        if (timeBtwSpawn <= 0 && stopGeneration == false)
        {
            Move();
            timeBtwSpawn = startTimeBtwSpawn;
        }
        else
        {
            timeBtwSpawn -= Time.deltaTime;
        }
    }

    private void Move()
    {
        if (direction == 1 || direction == 2)
        {
            if (transform.position.x < maxX)
            {
                topCounter = 0;
                Vector2 pos = new Vector2(transform.position.x + moveIncrement, transform.position.y);
                transform.position = pos;

                int randRoom = Random.Range(1, rooms.Length);
                Instantiate(rooms[randRoom], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6);
                if (direction == 3)
                    direction = 2;
                else if (direction == 4)
                    direction = 5;
            }
            else
            {
                direction = 5;
            }
        }
        else if (direction == 3 || direction == 4)
        { // Move left !
            if (transform.position.x > minX)
            {
                topCounter = 0;
                Vector2 pos = new Vector2(transform.position.x - moveIncrement, transform.position.y);
                transform.position = pos;

                int randRoom = Random.Range(1, rooms.Length);
                Instantiate(rooms[randRoom], transform.position, Quaternion.identity);
                direction = Random.Range(3, 6);
            }
            else
            {
                direction = 5;
            }
        }
        else if (direction == 5)
        { // MoveTop
            topCounter++;
            if (transform.position.y < maxY)
            {
                Collider2D previousRoom = Physics2D.OverlapCircle(transform.position, 1, whatIsRoom);
                if (previousRoom.GetComponent<Room>().roomType != 1 && previousRoom.GetComponent<Room>().roomType != 3)
                {
                    if (topCounter >= 2)
                    {
                        previousRoom.GetComponent<Room>().RoomDestruction();
                        Instantiate(rooms[3], transform.position, Quaternion.identity);

                    }
                }
                else
                {
                    previousRoom.GetComponent<Room>().RoomDestruction();
                    int randRoomTopOpening = Random.Range(1, 4);
                    if (randRoomTopOpening == 2)
                    {
                        randRoomTopOpening = 1;
                    }
                    Instantiate(rooms[randRoomTopOpening], transform.position, Quaternion.identity);
                }

                Vector2 pos = new Vector2(transform.position.x, transform.position.y + moveIncrement);
                transform.position = pos;
                int randRoom = Random.Range(2, 4);
                Instantiate(rooms[randRoom], transform.position, Quaternion.identity);
                direction = Random.Range(1, 6);
            }
            else
            {
                stopGeneration = true;
            }
        }
    }
}
