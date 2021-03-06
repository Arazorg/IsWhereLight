﻿using UnityEngine;

public class Weapon : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Смещение текста над оружием")]
    [SerializeField] public Vector3 offsetText;
#pragma warning restore 0649

    private WeaponData data;
    //private PopupText currentPhrase = null;

    /// <summary>
    /// Initialization of weapon
    /// </summary>
    /// <param name="data"></param>
    public void Init(WeaponData data, bool isUsed = false)
    {
        this.data = data;
        critChance = CritChance;

        GetComponent<Animator>().runtimeAnimatorController = MainAnimator;
        GetComponent<SpriteRenderer>().sprite = MainSprite;
        if(isUsed)
            transform.localPosition += (Vector3)Offset;
        transform.rotation = Quaternion.Euler(0, 0, StandartAngle);
    }

    public string WeaponName
    {
        get { return data.WeaponName; }
    }

    public WeaponData.AttackType TypeOfAttack
    {
        get { return data.TypeOfAttack; }
    }

    public Sprite MainSprite
    {
        get { return data.MainSprite; }
    }

    public RuntimeAnimatorController MainAnimator
    {
        get { return data.MainAnimator; }
    }

    public Vector2 Offset
    {
        get { return data.Offset; }
    }

    public Vector2 AttackOffset
    {
        get { return data.AttackOffset; }
    }

    public float StandartAngle
    {
        get { return data.StandartAngle; }
    }

    public float Knoking
    {
        get { return data.Knoking; }
    }

    public float AttackAngleRight
    {
        get { return data.AttackAngleRight; }
    }

    public float AttackAngleLeft
    {
        get { return data.AttackAngleLeft; }
    }

    public float Radius
    {
        get { return data.Radius; }
    }

    public float FireRate
    {
        get { return data.FireRate; }
    }

    public int Damage
    {
        get { return data.Damage; }
    }

    public float CritChance
    {
        get { return data.CritChance; }
        set { critChance = value; }
    }
    private float critChance;

    public int Manecost
    {
        get { return data.Manecost; }
    }

    public BulletData CurrentBullet
    {
        get { return data.CurrentBullet; }
    }

    public Vector2 FirePointPosition
    {
        get { return data.FirePointPosition; }
    }

    public WeaponData.WeaponShakeParametrs ShakeParametrs
    {
        get { return data.ShakeParametrs; }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        //if (transform.tag == "Gun" && coll.tag == "Player")
        //    currentPhrase = PopupText.Create(transform.position + offsetText, true, false, -1, WeaponName, 3.75f, true);
    }

    void OnTriggerExit2D()
    {
        //if (currentPhrase != null)
        //    currentPhrase.DeletePhrase();
    }
}
