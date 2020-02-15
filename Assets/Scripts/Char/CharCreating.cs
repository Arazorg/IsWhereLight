using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCreating : MonoBehaviour
{
    public GameObject character;
    public Transform startPosition;

    void Start()
    {
        Instantiate(character, startPosition.position, Quaternion.identity);
    }
}
