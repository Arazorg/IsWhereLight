using System;
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
        GetComponent<Animator>().runtimeAnimatorController = data.MainAnimator;
        GetComponent<SpriteRenderer>().sprite = data.MainSprite;
        GetComponentInChildren<LocalizedText>().key = data.name;
    }

    /// <summary>
    /// Name of weapon
    /// </summary>
    private string weaponName;
    public string WeaponName
    {
        get
        {
            return data.WeaponName;
        }
        protected set { }
    }

    public Sprite MainSprite
    {
        get
        {
            return data.MainSprite;
        }
        protected set { }
    }

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

    public WeaponData.AttackType TypeOfAttack
    {
        get
        {
            return data.TypeOfAttack;
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
