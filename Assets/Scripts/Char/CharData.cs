﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharData
{
    public int health;
    public int mana;
    public string startGun;
    
    public CharData(CharInfo charInfo)
    {
        health = charInfo.health;
        mana = charInfo.mana;
        startGun = charInfo.startGun;
    }
}
