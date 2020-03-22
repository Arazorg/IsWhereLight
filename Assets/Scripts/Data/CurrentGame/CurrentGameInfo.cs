using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentGameInfo : MonoBehaviour
{
    public static CurrentGameInfo instance;

    public string character;
    public string skin;
    public string startWeapon;
    public bool wildMode;

    public int startMoney;
    public int maxHealth;
    public int maxMane;

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

            wildMode = currentGameData.wildMode;
            startMoney = currentGameData.startMoney;
            maxHealth = currentGameData.maxHealth;
            maxMane = currentGameData.maxMane;
            return true;
        }
        else
        {
            return false;
        }     
    }
}
