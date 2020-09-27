using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Standart Weapon", fileName = "New Weapon")]
public class WeaponData : ScriptableObject
{
    public enum AttackType
    {
        Gun,
        Sword,
        Bow,
        Laser,
        ConstantLaser
    }

    [System.Serializable]
    public struct WeaponShakeParametrs
    {
        public float magnitude;
        public float roughness;
        public float fadeInTime;
        public float fadeOutTime;
    }

#pragma warning disable 0649
    [Tooltip("Название оружия")]
    [SerializeField] private string weaponName;
    public string WeaponName
    {
        get { return weaponName; }
    }

    [Tooltip("Основной аниматор")]
    [SerializeField] private RuntimeAnimatorController mainAnimator;
    public RuntimeAnimatorController MainAnimator
    {
        get { return mainAnimator; }
    }

    [Tooltip("Изображение для оружия")]
    [SerializeField] private Sprite mainSprite;
    public Sprite MainSprite
    {
        get { return mainSprite; }
    }

    [Tooltip("Тип атаки оружия")]
    [SerializeField] private AttackType typeOfAttack;
    public AttackType TypeOfAttack
    {
        get { return typeOfAttack; }
    }

    [Tooltip("Смещение оружия")]
    [SerializeField] private Vector2 offset;
    public Vector2 Offset
    {
        get { return offset; }
    }

    [Tooltip("Смещение оружия во время атаки(вверх)")]
    [SerializeField] private Vector2 attackOffset;

    public Vector2 AttackOffset
    {
        get { return attackOffset; }
    }

    [Tooltip("Стандартный угол")]
    [SerializeField] private float standartAngle;
    public float StandartAngle
    {
        get { return standartAngle; }
    }

    [Tooltip("Угол атаки справа")]
    [SerializeField] private float attackAngleRight;
    public float AttackAngleRight
    {
        get { return attackAngleRight; }
    }

    [Tooltip("Угол атаки слева")]
    [SerializeField] private float attackAngleLeft;
    public float AttackAngleLeft
    {
        get { return attackAngleLeft; }
    }

    [Tooltip("Радиус атаки")]
    [SerializeField] private float radius;
    public float Radius
    {
        get { return radius; }
    }

    [Tooltip("Отбрасывание оружия")]
    [SerializeField] private float knoking;
    public float Knoking
    {
        get { return knoking; }
    }

    [Tooltip("Частота выстрелов оружия")]
    [SerializeField] private float fireRate;
    public float FireRate
    {
        get { return fireRate; }
    }

    [Tooltip("Урон оружия")]
    [SerializeField] private int damage;
    public int Damage
    {
        get { return damage; }
    }

    [Tooltip("Шанс критического урона")]
    [SerializeField] private float critChance;
    public float CritChance
    {
        get { return critChance; }
    }

    [Tooltip("Снаряд оружия")]
    [SerializeField] private BulletData currentBullet;
    public BulletData CurrentBullet
    {
        get { return currentBullet; }
    }


    [Tooltip("Затраты маны на использование")]
    [SerializeField] private int manecost;
    public int Manecost
    {
        get { return manecost; }
    }

    [Tooltip("Позиция спауна пули")]
    [SerializeField] private Vector2 firePointPosition;
    public Vector2 FirePointPosition
    {
        get { return firePointPosition; }
    }

    [Tooltip("Параметры тряски оружия")]
    [SerializeField] private WeaponShakeParametrs shakeParametrs;
    public WeaponShakeParametrs ShakeParametrs
    {
        get { return shakeParametrs; }
    }

#pragma warning restore 0649
}