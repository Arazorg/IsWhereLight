using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProgressData
{
    public int playerMoney;

    public Dictionary<string, bool> characters = new Dictionary<string, bool>();
    public Dictionary<string, int> secretCodes = new Dictionary<string, int>();

    public ProgressData(ProgressInfo progressInfo)
    {
        playerMoney = progressInfo.playerMoney;
        characters = progressInfo.characters;
        secretCodes = progressInfo.secretCodes;
    }
}
