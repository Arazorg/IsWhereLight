using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Tooltip("Смещение текста над оружием")]
    [SerializeField] public Vector3 offsetText;

    private WeaponData data;

    /// <summary>
    /// Initialization of weapon
    /// </summary>
    /// <param name="data"></param>
    public void Init(WeaponData data)
    {
        this.data = data;
        critChance = CritChance;
        colliderSize = ColliderSize;
        colliderOffset = ColliderOffset;

        SetColliderSettings();
        GetComponent<Animator>().runtimeAnimatorController = MainAnimator;
        GetComponent<SpriteRenderer>().sprite = data.MainSprite;
    }

    private void SetColliderSettings()
    {
        GetComponent<BoxCollider2D>().size = new Vector2(colliderSize.x, colliderSize.y);
        GetComponent<BoxCollider2D>().offset = new Vector2(colliderOffset.x, colliderOffset.y);
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

    private Vector2 colliderSize;
    public Vector2 ColliderSize
    {
        get
        {
            return data.ColliderSize;
        }
    }

    private Vector2 colliderOffset;
    public Vector2 ColliderOffset
    {
        get
        {
            return data.ColliderOffset;
        }
    }

    private float radius;
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

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (transform.tag == "Gun" && coll.tag == "Player")
        {
            PopupDamage.Create(transform.position + offsetText, true, false, -1, WeaponName);
        }
    }
}
