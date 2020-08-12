using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProgressData
{
    public Dictionary<string, bool> characters;
    public Dictionary<string, int> secretCodes;
    public int playerMoney;
    public int countKilledEnemies;
    public int countShoots;

    public ProgressData(ProgressInfo progressInfo)
    {
        playerMoney = progressInfo.playerMoney;
        characters = progressInfo.characters;
        secretCodes = progressInfo.secretCodes;
        countKilledEnemies = progressInfo.countKilledEnemies;
        countShoots = progressInfo.countShoots;
    }
}
