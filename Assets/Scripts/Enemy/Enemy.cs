using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyData data;

    private bool isEnemyHitted = false;
    private bool isEnterFirst = true;
    private float timeToOff;

    /// <summary>
    /// Initialization of enemy
    /// </summary>
    /// <param name="data"></param>
    public void Init(EnemyData data)
    {
        this.data = data;
        health = Health;
        GetComponent<Animator>().runtimeAnimatorController = MainAnimator;
        transform.tag = "Untagged";
    }

    public RuntimeAnimatorController MainAnimator
    {
        get
        {
            return data.MainAnimator;
        }
        protected set { }
    }

    /// <summary>
    /// Attack of current enemy
    /// </summary>
    public int Speed
    {
        get
        {
            return data.Speed;
        }
        protected set { }
    }


    /// <summary>
    /// Type of attack of current enemy
    /// </summary>
    public EnemyData.AttackType TypeOfAttack
    {
        get
        {
            return data.TypeOfAttack;
        }
        protected set { }
    }

    /// <summary>
    /// Attack of current enemy
    /// </summary>
    public int Damage
    {
        get
        {
            return data.Damage;
        }
        protected set { }
    }

    /// <summary>
    /// Attack range of current enemy
    /// </summary>
    public float AttackRange
    {
        get
        {
            return data.AttackRange;
        }
        protected set { }
    }

    /// <summary>
    /// BulletData of current enemy
    /// </summary>
    public BulletData DataOfBullet
    {
        get
        {
            return data.DataOfBullet;
        }
        protected set { }
    }

    /// <summary>
    /// Health of current enemy
    /// </summary>
    private int health;
    public int Health
    {
        get
        {
            return data.Health;
        }
        protected set { }
    }

    /// <summary>
    /// Target of current enemy
    /// </summary>
    public string Target
    {
        get
        {
            return data.Target;
        }
        protected set { }
    }

    /// <summary>
    /// Name of current enemy
    /// </summary>
    public string EnemyName
    {
        get
        {
            return data.EnemyName;
        }
        protected set { }
    }

    /// <summary>
    /// Name of current enemy
    /// </summary>
    public float FireRate
    {
        get
        {
            return data.FireRate;
        }
        protected set { }
    }

    public Transform curTarget;
    public Vector3 positionCurTarget;

    private void Update()
    {
        EnemyHitted();
    }

    public static Action<GameObject> OnEnemyDeath;

    void Death()
    {
        OnEnemyDeath(gameObject);
    }

    private void EnemyHitted()
    {
        if (isEnemyHitted)
        {
            if (isEnterFirst)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
                timeToOff = Time.time + 0.05f;
                isEnterFirst = false;
            }
            else
            {
                if (Time.time > timeToOff)
                {
                    GetComponent<SpriteRenderer>().color = Color.white;
                    isEnemyHitted = false;
                    isEnterFirst = true;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "StandartBullet")
        {
            var bullet = coll.gameObject.GetComponent<Bullet>();
            GetDamage(bullet.Damage, bullet.Damage);
        }

        if (health <= 0)
            Death();
    }

    public void GetDamage(int damage, float critChance)
    {
        isEnemyHitted = true;
        bool isCriticalHit = UnityEngine.Random.Range(0, 100) < critChance;
        if (isCriticalHit)
            damage *= 2;
        health -= damage;
        PopupDamage.Create(transform.position, false, isCriticalHit, damage);
    }

    void OnBecameVisible()
    {
        gameObject.tag = "Enemy";
    }

    void OnBecameInvisible()
    {
        gameObject.tag = "Untagged";
    }
}