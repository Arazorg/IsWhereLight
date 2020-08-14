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
    public int currentWave = 0;
    public bool canExit = true;

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
        character = currentGameData.character;
        skin = currentGameData.skin;
        startWeapon = currentGameData.startWeapon;
        maxHealth = currentGameData.maxHealth;
        maxMane = currentGameData.maxMane;
        wildMode = currentGameData.wildMode;
        isLobby = currentGameData.isLobby;
        challengeNumber = currentGameData.challengeNumber;
        currentWave = currentGameData.currentWave;
        canExit = currentGameData.canExit;
    }

    public void SaveCurrentGame()
    {
        string json = JsonUtility.ToJson(this);
        NewSaveSystem.Save("currentGame", json);
    }

    public bool LoadCurrentGame()
    {
        var currentGameString = NewSaveSystem.Load("currentGame");
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
