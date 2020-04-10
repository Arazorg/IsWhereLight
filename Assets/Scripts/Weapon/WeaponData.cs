using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Standart Weapon", fileName = "New Weapon")]
public class WeaponData : ScriptableObject
{
    public enum AttackType
    {
        Melee,
        Distant
    }

    [Tooltip("Основной аниматор")]
    [SerializeField] private RuntimeAnimatorController mainAnimator;
    public RuntimeAnimatorController MainAnimator
    {
        get { return mainAnimator; }
        protected set { }
    }


    [Tooltip("Название оружия")]
    [SerializeField] private string weaponName;
    public string WeaponName
    {
        get { return weaponName; }
        protected set { }
    }

    [Tooltip("Частота выстрелов оружия")]
    [SerializeField] private float fireRate;
    public float FireRate
    {
        get { return fireRate; }
        protected set { }
    }

    [Tooltip("Урон оружия")]
    [SerializeField] private int damage;
    public int Damage
    {
        get { return damage; }
        protected set { }
    }

    [Tooltip("Тип атаки оружия")]
    [SerializeField] private AttackType typeOfAttack;
    public AttackType TypeOfAttack
    {
        get { return typeOfAttack; }
        protected set { }
    }

    [Tooltip("Шанс критического урона")]
    [SerializeField] private float critChance;
    public float CritChance
    {
        get { return critChance; }
        protected set { }
    }

    [Tooltip("Затраты маны на использование")]
    [SerializeField] private int manecost;
    public int Manecost
    {
        get { return manecost; }
        protected set { }
    }

    [Tooltip("Снаряд оружия")]
    [SerializeField] private BulletData bullet;
    public BulletData Bullet
    {
        get { return bullet; }
        protected set { }
    }
}
