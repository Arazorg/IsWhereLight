﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private WeaponData data;

    /// <summary>
    /// Initialization of weapon
    /// </summary>
    /// <param name="data"></param>
    public void Init(WeaponData data)
    {
        this.data = data;
        GetComponent<SpriteRenderer>().sprite = data.MainSprite;
    }

    public static Action<GameObject> OnAllyWeaponChange;
    public static Action<GameObject> OnWeaponBuy;

    /// <summary>
    /// Fire rate of current weapon
    /// </summary>
    private float fireRate;
    public float FireRate
    {
        get
        {
            return data.FireRate;
        }
        protected set { }
    }

    /// <summary>
    /// Damage of current weapon
    /// </summary>
    private int damage;
    public int Damage
    {
        get
        {
            return data.Damage;
        }
        protected set { }
    }

    /// <summary>
    /// Crit chance of current weapon
    /// </summary>
    private float critChance;
    public float CritChance
    {
        get
        {
            return data.CritChance;
        }
        protected set { }
    }

    /// <summary>
    /// Manecost of current weapon
    /// </summary>
    private int manecost;
    public int Manecost
    {
        get
        {
            return data.Manecost;
        }
        protected set { }
    }

    /// <summary>
    /// Bullet of current weapon
    /// </summary>
    private BulletData bullet;
    public BulletData Bullet
    {
        get
        {
            return data.Bullet;
        }
        protected set { }
    }
}
