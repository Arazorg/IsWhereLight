using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProgressData
{
    public Dictionary<string, bool> characters;
    public Dictionary<string, int> secretCodes;
    public Dictionary<string, bool> achivments;
    public int playerMoney;
    public int countKilledEnemies;
    public int countShoots;

    public ProgressData(ProgressInfo progressInfo)
    {
        characters = progressInfo.characters;
        secretCodes = progressInfo.secretCodes;
        achivments = progressInfo.achivments;
        playerMoney = progressInfo.playerMoney;
        countKilledEnemies = progressInfo.countKilledEnemies;
        countShoots = progressInfo.countShoots;
    }
}
