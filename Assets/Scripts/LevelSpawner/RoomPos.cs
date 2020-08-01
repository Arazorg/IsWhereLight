using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPos : MonoBehaviour
{
    public LayerMask whatIsRoom;
    public LevelGeneration levelGen;

    void Update()
    {
        Collider2D room = Physics2D.OverlapCircle(transform.position, 1, whatIsRoom);
        if (room != null)
            Destroy(gameObject);
    }
}
