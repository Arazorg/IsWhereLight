using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentGameInfo : MonoBehaviour
{
    public static CurrentGameInfo instance;

    public string character;
    public string skin;
    public string startWeapon;
    public int maxHealth;
    public int maxMane;
    public bool wildMode;
    public bool isLobby;

    void Awake()
    {       
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SaveCurrentGame()
    {
        SaveSystem.SaveCurrentGame(this);
    }

    public bool LoadCurrentGame()
    {
        CurrentGameData currentGameData = SaveSystem.LoadCurrentGame();
        if(currentGameData != null)
        {
            character = currentGameData.character;
            skin = currentGameData.skin;
            startWeapon = currentGameData.startWeapon;
            maxHealth = currentGameData.maxHealth;
            maxMane = currentGameData.maxMane;
            wildMode = currentGameData.wildMode;
            isLobby = currentGameData.isLobby;
            return true;
        }
        else
        {
            return false;
        }     
    }
}
