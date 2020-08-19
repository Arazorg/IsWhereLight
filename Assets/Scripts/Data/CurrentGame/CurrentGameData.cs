﻿[System.Serializable]
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
    public int currentWave = 0;
    public int countResurrect = 1;
    public bool canExit = true;

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
        currentWave = currentGameInfo.currentWave;
        countResurrect = currentGameInfo.countResurrect;
        canExit = currentGameInfo.canExit;
    }
}
