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
    public bool isDeath = false;

    /// <summary>
    /// Initialization of enemy
    /// </summary>
    /// <param name="data"></param>
    public void Init(EnemyData data)
    {
        this.data = data;
        health = Health;
        GetComponent<Animator>().runtimeAnimatorController = MainAnimator;
        gameObject.tag = "Untagged";
        if (!data.EnemyName.Contains("Target"))
            GetComponent<EnemyAI>().StartAI();
    }

    /// <summary>
    /// Animator of current enemy
    /// </summary>
    /// <param name="data"></param>
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
    /// FireRate of current enemy
    /// </summary>
    public float FireRate
    {
        get
        {
            return data.FireRate;
        }
        protected set { }
    }

    private void Update()
    {
        EnemyHitted();
    }

    private void EnemyHitted()
    {
        if (!isDeath)
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
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!isDeath)
        {
            if (coll.gameObject.tag == "StandartBullet" || coll.gameObject.tag == "StandartArrow")
            {
                var bullet = coll.gameObject.GetComponent<Bullet>();
                Knoking(coll.transform.position, bullet.Knoking);
                GetDamage(bullet.Damage, bullet.CritChance);
            }
        }
    }

    public void Knoking(Vector3 objectPosition, float weaponKnoking)
    {
        if (!isDeath)
        {
            transform.position += (transform.position - objectPosition).normalized * weaponKnoking;
        }
    }

    public void GetDamage(int damage, float critChance, float knoking = 0f)
    {
        if(!isDeath)
        {
            isEnemyHitted = true;

            bool isCriticalHit = UnityEngine.Random.Range(0, 100) < critChance;
            if (isCriticalHit)
                damage *= 2;
            health -= damage;
            PopupText.Create(transform.position, false, isCriticalHit, damage);
            if (health <= 0)
            {
                if (data.EnemyName.Contains("Target"))
                    ShootingRange.instance.Spawn(true);
                else
                {
                    GetComponent<Animator>().Play("Death");
                    isDeath = true;
                    ColorUtility.TryParseHtmlString("#808080", out Color color);
                    gameObject.tag = "IgnoreAll";
                    GetComponent<SpriteRenderer>().color = color;
                    Destroy(gameObject, 10f);
                }

            }
        }        
    }

    void OnBecameVisible()
    {
        if (!isDeath)
        {
            if (!data.EnemyName.Contains("Target"))
                gameObject.tag = "Enemy";
        }
    }

    void OnBecameInvisible()
    {
        if (!isDeath)
        {
            gameObject.tag = "Untagged";
        }
    }
}