using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharData
{
    public int money;
    public int health;
    public int mane;
    public string gun;
    public string skin;

    public CharData(CharInfo charInfo)
    {
        money = charInfo.money;
        health = charInfo.health;
        mane = charInfo.mane;
        gun = charInfo.gun;
        skin = charInfo.skin;
    }
}
