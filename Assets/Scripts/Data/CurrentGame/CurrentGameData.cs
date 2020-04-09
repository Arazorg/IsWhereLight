using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurrentGameData
{
    public string character;
    public string skin;
    public string startWeapon;
    public bool wildMode;
    public int startMoney;
    public int maxHealth;
    public int maxMane;

    public CurrentGameData(CurrentGameInfo currentGameInfo)
    {
        character = currentGameInfo.character;
        wildMode = currentGameInfo.wildMode;
        startMoney = currentGameInfo.startMoney;
        maxHealth = currentGameInfo.maxHealth;
        maxMane = currentGameInfo.maxMane;
        startWeapon = currentGameInfo.startWeapon;
        skin = currentGameInfo.skin;
    }
}
