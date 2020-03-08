using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCreating : MonoBehaviour
{
    public GameObject character;
    public Transform startPosition;
    private CurrentGameInfo currentGameInfo;

    void Start()
    {
        Instantiate(character, startPosition.position, Quaternion.identity);
    }
}
