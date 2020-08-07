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
    public int challengeNumber;
    public int countKilledEnemy;
    public int countShoots = 0;
    public int currentWave = 0;

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
        if(!isLobby)
            SaveSystem.SaveCurrentGame(this);
    }

    public void SetIsLobbyState(bool isLobby)
    {
        this.isLobby = isLobby;
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
            challengeNumber = currentGameData.challengeNumber;
            countKilledEnemy = currentGameData.countKilledEnemy;
            countShoots = currentGameData.countShoots;
            currentWave = currentGameData.currentWave;
            return true;
        }
        else
            return false;  
    }
}
