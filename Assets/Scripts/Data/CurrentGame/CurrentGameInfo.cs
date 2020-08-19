﻿using System.Collections;
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
    public int currentWave;
    public int countResurrect;
    public bool canExit;

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

    private void Init(CurrentGameData currentGameData)
    {
        SetStartParametrs();
        character = currentGameData.character;
        skin = currentGameData.skin;
        startWeapon = currentGameData.startWeapon;
        maxHealth = currentGameData.maxHealth;
        maxMane = currentGameData.maxMane;
        wildMode = currentGameData.wildMode;
        isLobby = currentGameData.isLobby;
        challengeNumber = currentGameData.challengeNumber;
        currentWave = currentGameData.currentWave;
        countResurrect = currentGameData.countResurrect;
        canExit = currentGameData.canExit;
    }

    private void SetStartParametrs()
    {
        currentWave = 1;
        countResurrect = 1;
        canExit = true;
    }

    public void SaveCurrentGame(string key = "currentGame")
    {
        string json = JsonUtility.ToJson(this);
        NewSaveSystem.Save(key, json);
    }

    public bool LoadCurrentGame(string key = "currentGame")
    {
        var currentGameString = NewSaveSystem.Load(key);
        if (currentGameString != null)
        {
            CurrentGameData saveObject = JsonUtility.FromJson<CurrentGameData>(currentGameString);
            Init(saveObject);
            return true;
        };
        return false;
    }

    public void SetIsLobbyState(bool isLobby)
    {
        this.isLobby = isLobby;
        SaveCurrentGame();
    }
}
