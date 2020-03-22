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
    public string weapon;
    public string skin;
    public string character;

    public CharData(CharInfo charInfo)
    {
        level = charInfo.level;
        money = charInfo.money;
        health = charInfo.health;
        mane = charInfo.mane;
        weapon = charInfo.weapon;
        skin = charInfo.skin;
        character = charInfo.character;
    }
}
