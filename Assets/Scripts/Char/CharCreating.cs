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
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        Debug.Log(currentGameInfo.skin);
        Sprite test = Resources.Load("Sprites/" + currentGameInfo.skin + ".png") as Sprite;
        character.GetComponent<SpriteRenderer>().sprite = test;
    }
}
