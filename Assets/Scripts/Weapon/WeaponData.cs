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

    [Tooltip("Смещение оружия")]
    [SerializeField] private Vector2 offset;
    public Vector2 Offset
    {
        get
        {
            return offset;
        }
        set { }
    }

    [Tooltip("Смещение оружия во время атаки(вверх)")]
    [SerializeField] private Vector2 attackOffset;

    public Vector2 AttackOffset
    {
        get
        {
            return attackOffset;
        }
        set { }
    }

    [Tooltip("Стандартный угол")]
    [SerializeField] private float standartAngle;
    public float StandartAngle
    {
        get
        {
            return standartAngle;
        }
        set { }
    }

    [Tooltip("Угол атаки")]
    [SerializeField] private float attackAngle;
    public float AttackAngle
    {
        get
        {
            return attackAngle;
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

    [Tooltip("Отбрасывание оружия")]
    [SerializeField] private float knoking;
    public float Knoking
    {
        get
        {
            return knoking;
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

    [Tooltip("Снаряд оружия")]
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