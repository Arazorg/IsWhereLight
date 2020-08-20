﻿using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Tooltip("Смещение текста над оружием")]
    [SerializeField] public Vector3 offsetText;

    private WeaponData data;
    private PopupText currentPhrase = null;

    /// <summary>
    /// Initialization of weapon
    /// </summary>
    /// <param name="data"></param>
    public void Init(WeaponData data)
    {
        this.data = data;
        critChance = CritChance;

        GetComponent<Animator>().runtimeAnimatorController = MainAnimator;
        GetComponent<SpriteRenderer>().sprite = MainSprite;
        transform.localPosition += (Vector3)Offset;
        transform.rotation = Quaternion.Euler(0,0,StandartAngle);
    }

    public string WeaponName
    {
        get
        {
            return data.WeaponName;
        }
    }

    public WeaponData.AttackType TypeOfAttack
    {
        get
        {
            return data.TypeOfAttack;
        }
    }

    public Sprite MainSprite
    {
        get
        {
            return data.MainSprite;
        }
    }

    public RuntimeAnimatorController MainAnimator
    {
        get
        {
            return data.MainAnimator;
        }
    }
    public Vector2 Offset
    {
        get
        {
            return data.Offset;
        }
    }

    public Vector2 AttackOffset
    {
        get
        {
            return data.AttackOffset;
        }
    }

    public float StandartAngle
    {
        get
        {
            return data.StandartAngle;
        }
    }

    public float Knoking
    {
        get
        {
            return data.Knoking;
        }
    }

    public float AttackAngle
    {
        get
        {
            return data.AttackAngle;
        }
    }

    public float Radius
    {
        get
        {
            return data.Radius;
        }
    }

    public float FireRate
    {
        get
        {
            return data.FireRate;
        }
    }

    public int Damage
    {
        get
        {
            return data.Damage;
        }
    }

    private float critChance;
    public float CritChance
    {
        get
        {
            return data.CritChance;
        }
        set
        {
            critChance = value;
        }
    }

    public int Manecost
    {
        get
        {
            return data.Manecost;
        }
    }

    public BulletData CurrentBullet
    {
        get
        {
            return data.CurrentBullet;
        }
    }

    public Vector2 firePointPosition
    {
        get
        {
            return data.FirePointPosition;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (transform.tag == "Gun" && coll.tag == "Player")
        {
            currentPhrase = PopupText.Create(transform.position + offsetText, true, false, -1, WeaponName, 3.75f, true);
        }
    }

    void OnTriggerExit2D()
    {
        if(currentPhrase != null)
            currentPhrase.DeletePhrase();
    }
}
