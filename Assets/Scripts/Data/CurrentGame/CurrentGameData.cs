using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurrentGameData
{
    public string character;
    public string skin;
    public string startWeapon;
    public int maxHealth;
    public int maxMane;
    public bool wildMode;
    public bool isLobby;
    public int challengeNumber;
    public int countKilledEnemy;
    public int countShoots;
    public int currentWave;

    public CurrentGameData(CurrentGameInfo currentGameInfo)
    {
        character = currentGameInfo.character;
        skin = currentGameInfo.skin;
        startWeapon = currentGameInfo.startWeapon;
        maxHealth = currentGameInfo.maxHealth;
        maxMane = currentGameInfo.maxMane;
        wildMode = currentGameInfo.wildMode;
        isLobby = currentGameInfo.isLobby;
        challengeNumber = currentGameInfo.challengeNumber;
        countKilledEnemy = currentGameInfo.countKilledEnemy;
        countShoots = currentGameInfo.countShoots;
        currentWave = currentGameInfo.currentWave;
    }
}
