using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPos : MonoBehaviour
{
    public LayerMask whatIsRoom;
    public LevelGeneration levelGen;
    public GameObject closeRoom;

    void Update()
    {

        Collider2D room = Physics2D.OverlapCircle(transform.position, 1, whatIsRoom);
        if (room == null && levelGen.stopGeneration == true)
        {
            int rand = Random.Range(0, levelGen.rooms.Length);
            Instantiate(closeRoom, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
