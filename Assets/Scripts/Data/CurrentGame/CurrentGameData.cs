[System.Serializable]
public class CurrentGameData
{
    public string character;
    public string skin;
    public string startWeapon;
    public string[] currentAmplifications = new string[4];
    public int maxHealth;
    public int maxMane;
    public bool wildMode;
    public bool isLobby;
    public int challengeNumber;
    public int currentWave = 0;
    public int countResurrect = 1;
    public string characterType;
    public float skillTime;

    public CurrentGameData(CurrentGameInfo currentGameInfo)
    {
        character = currentGameInfo.character;
        skin = currentGameInfo.skin;
        startWeapon = currentGameInfo.startWeapon;
        currentAmplifications = currentGameInfo.currentAmplifications;
        maxHealth = currentGameInfo.maxHealth;
        maxMane = currentGameInfo.maxMane;
        wildMode = currentGameInfo.wildMode;
        isLobby = currentGameInfo.isLobby;
        challengeNumber = currentGameInfo.challengeNumber;
        currentWave = currentGameInfo.currentWave;
        countResurrect = currentGameInfo.countResurrect;
        characterType = currentGameInfo.characterType;
        skillTime = currentGameInfo.skillTime;
    }
}
