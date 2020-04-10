using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCreating : MonoBehaviour
{
    public GameObject character;
    private CurrentGameInfo currentGameInfo;

    void Start()
    {
        Instantiate(character, new Vector3(2,2,0), Quaternion.identity);
    }
}
