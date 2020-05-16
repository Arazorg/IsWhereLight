using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Standart Weapon", fileName = "New Weapon")]
public class WeaponData : ScriptableObject
{
    public enum AttackType
    {
        Gun,
        Sword,
        Bow
    }
#pragma warning disable 0649
    [Tooltip("Название оружия")]
    [SerializeField] private string weaponName;
    public string WeaponName
    {
        get
        {
            return weaponName;
        }
        set { }
    }

    [Tooltip("Основной аниматор")]
    [SerializeField] private RuntimeAnimatorController mainAnimator;
    public RuntimeAnimatorController MainAnimator
    {
        get
        {
            return mainAnimator;
        }
        set { }
    }

    [Tooltip("Изображение для оружия")]
    [SerializeField] private Sprite mainSprite;
    public Sprite MainSprite
    {
        get
        {
            return mainSprite;
        }
        set { }
    }

    [Tooltip("Тип атаки оружия")]
    [SerializeField] private AttackType typeOfAttack;
    public AttackType TypeOfAttack
    {
        get
        {
            return typeOfAttack;
        }
        set { }
    }

    [Tooltip("Размер колайдера")]
    [SerializeField] private Vector2 colliderSize;
    public Vector2 ColliderSize
    {
        get
        {
            return colliderSize;
        }
        set { }
    }

    [Tooltip("Сдвиг колайдера")]
    [SerializeField] private Vector2 colliderOffset;
    public Vector2 ColliderOffset
    {
        get
        {
            return colliderOffset;
        }
        set { }
    }

    [Tooltip("Радиус атаки")]
    [SerializeField] private float radius;
    public float Radius
    {
        get
        {
            return radius;
        }
        set { }
    }

    [Tooltip("Частота выстрелов оружия")]
    [SerializeField] private float fireRate;
    public float FireRate
    {
        get
        {
            return fireRate;
        }
        set { }
    }

    [Tooltip("Урон оружия")]
    [SerializeField] private int damage;
    public int Damage
    {
        get
        {
            return damage;
        }
        set { }
    }

    [Tooltip("Шанс критического урона")]
    [SerializeField] private float critChance;
    public float CritChance
    {
        get
        {
            return critChance;
        }
        set { }
    }

    [Tooltip("Шанс критического урона")]
    [SerializeField] private BulletData currentBullet;
    public BulletData CurrentBullet
    {
        get
        {
            return currentBullet;
        }
        set { }
    }


    [Tooltip("Затраты маны на использование")]
    [SerializeField] private int manecost;
    public int Manecost
    {
        get
        {
            return manecost;
        }
        set { }
    }
#pragma warning restore 0649
}