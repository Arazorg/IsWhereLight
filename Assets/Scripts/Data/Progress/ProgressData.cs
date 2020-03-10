using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProgressData
{
    public int playerMoney;

    public ProgressData(ProgressInfo progressInfo)
    {
        playerMoney = progressInfo.playerMoney;
    }
}
