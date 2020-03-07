using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentGameData : MonoBehaviour
{
    public bool wildMode;
    public int level;
    public int startMoney;
    public int maxHealth;
    public int maxMane;
    public string startGun;
    public string skin;

    public CurrentGameData(CurrentGameInfo currentGameInfo)
    {
        wildMode = currentGameInfo.wildMode;
        level = currentGameInfo.level;
        startMoney = currentGameInfo.startMoney;
        maxHealth = currentGameInfo.maxHealth;
        maxMane = currentGameInfo.maxMane;
        startGun = currentGameInfo.startGun;
        skin = currentGameInfo.skin;
    }
}
