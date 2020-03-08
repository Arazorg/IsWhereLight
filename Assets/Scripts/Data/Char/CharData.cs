using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharData
{
    public int level;
    public int money;
    public int health;
    public int mane;
    public string gun;
    public string skin;
    public string character;

    public CharData(CharInfo charInfo)
    {
        level = charInfo.level;
        money = charInfo.money;
        health = charInfo.health;
        mane = charInfo.mane;
        gun = charInfo.gun;
        skin = charInfo.skin;
        character = charInfo.character;
    }
}
