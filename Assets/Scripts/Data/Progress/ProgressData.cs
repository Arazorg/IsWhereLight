using System.Collections.Generic;

[System.Serializable]
public class ProgressData
{
    public Dictionary<string, bool> characters;
    public Dictionary<string, int> secretCodes;
    public Dictionary<string, bool> achivments;
    public Dictionary<string, bool> amplifications;

    public Dictionary<string, int> forestLevelsStarsCount;

    public int playerMoney;
    public int countKilledEnemies;
    public int countShoots;
    public int countOfAmplificationPoint;
    public ProgressData(ProgressInfo progressInfo)
    {
        characters = progressInfo.characters;
        secretCodes = progressInfo.secretCodes;
        achivments = progressInfo.achivments;
        amplifications = progressInfo.amplifications;
        playerMoney = progressInfo.playerMoney;
        countKilledEnemies = progressInfo.countKilledEnemies;
        countShoots = progressInfo.countShoots;
        countOfAmplificationPoint = progressInfo.countOfAmplificationPoint;
    }
}
