using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharInfoBar : MonoBehaviour
{
    private CurrentGameInfo currentGameInfo;

    void Start()
    {
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
