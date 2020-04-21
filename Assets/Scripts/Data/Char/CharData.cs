using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharData
{
    public string character;
    public string skin;
    public string[] weapons;

    public int maxHealth;
    public int maxMane;
    public int health;
    public int mane;

    public int money;
    public int currentDay;

    public CharData(CharInfo charInfo)
    {
        character = charInfo.character;
        skin = charInfo.skin;
        weapons = charInfo.weapons;
        maxHealth = charInfo.maxHealth;
        maxMane = charInfo.maxMane;
        health = charInfo.health;
        mane = charInfo.mane;
        money = charInfo.money;
        currentDay = charInfo.currentDay;
    }
}
