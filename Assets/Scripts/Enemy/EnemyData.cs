﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Standart Enemy", fileName = "New Enemy")]
public class EnemyData : ScriptableObject
{
    public enum AttackType
    {
        Melee,
        Distant
    }
#pragma warning disable 0649
    [Tooltip("Основной аниматор")]
    [SerializeField] private RuntimeAnimatorController mainAnimator;
    public RuntimeAnimatorController MainAnimator
    {
        get { return mainAnimator; }
        protected set { }
    }

    [Tooltip("Скорость врага")]
    [SerializeField] private int speed;
    public int Speed
    {
        get { return speed; }
        protected set { }
    }

    [Tooltip("Здоровье врага")]
    [SerializeField] private int health;
    public int Health
    {
        get { return health; }
    }

    [Tooltip("Тип атаки врага")]
    [SerializeField] private AttackType typeOfAttack;
    public AttackType TypeOfAttack
    {
        get { return typeOfAttack; }
        protected set { }
    }

    [Tooltip("Атака врага")]
    [SerializeField] private int damage;
    public int Damage
    {
        get { return damage; }
        protected set { }
    }

    [Tooltip("Радиус атаки врага")]
    [SerializeField] private float attackRange;
    public float AttackRange
    {
        get { return attackRange; }
        protected set { }
    }

    [Tooltip("Угол атаки врага")]
    [SerializeField] private float attackAngle;
    public float AttackAngle
    {
        get { return attackAngle; }
        protected set { }
    }

    [Tooltip("Пуля врага")]
    [SerializeField] private BulletData dataOfBullet;
    public BulletData DataOfBullet
    {
        get { return dataOfBullet; }
        protected set { }
    }

    [Tooltip("Цель атаки врага")]
    [SerializeField] private string target;
    public string Target
    {
        set
        {
            target = value;
        }
        get { return target; }
    }

    [Tooltip("Имя врага")]
    [SerializeField] private string enemyName;
    public string EnemyName
    {
        get { return enemyName; }
    }

    [Tooltip("Частота атаки врага")]
    [SerializeField] private float fireRate;
    public float FireRate
    {
        get { return fireRate; }
        protected set { }
    }
#pragma warning restore 0649
}