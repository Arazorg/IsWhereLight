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
    public int currentWave;
    public bool canExit;

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
        canExit = currentGameInfo.canExit;
    }
}
