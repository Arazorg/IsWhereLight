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
        attack = Attack;
        health = Health;
        target = Target;

        GetComponent<Animator>().runtimeAnimatorController = data.MainAnimator;

        transform.tag = "Untagged";
    }

    /// <summary>
    /// Attack of current enemy
    /// </summary>
    private int speed;
    public int Speed
    {
        get
        {
            return data.Speed;
        }
        protected set { }
    }

    /// <summary>
    /// Attack of current enemy
    /// </summary>
    private int attack;
    public int Attack
    {
        get
        {
            return data.Attack;
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
    private string target;
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
    private string enemyName;
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
    private float fireRate;
    public float FireRate
    {
        get
        {
            return data.FireRate;
        }
        protected set { }
    }

    /// <summary>
    /// Current target of current enemy
    /// </summary>
    public Transform curTarget;
    public Vector3 positionCurTarget;
    public bool moveTo;
    

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
            int damage = WeaponSpawner.currentCharWeapon.GetComponent<Weapon>().Damage;
            isEnemyHitted = true;
            bool isCriticalHit = UnityEngine.Random.Range(0, 100) < WeaponSpawner.currentCharWeapon.GetComponent<Weapon>().CritChance;
            if (isCriticalHit)
                damage *= 2;
            health -= damage;
            PopupDamage.Create(transform.position, damage, isCriticalHit);
        }


        if (health <= 0)
            Death();
    }

    void OnBecameVisible()
    {
        gameObject.tag = "Enemy";
    }

    void OnBecameInvisible()
    {
        gameObject.tag = "Untagged";
    }

    public static Enemy GetClosestEnemy(Vector3 position, float maxRange)
    {
        Enemy closest = null;
        foreach (Enemy enemy in EnemySpawner.Enemies.Values)
        {
            if (Vector3.Distance(position, enemy.GetPosition()) <= maxRange)
            {
                if (closest == null)
                {
                    closest = enemy;
                }
                else
                {
                    if (Vector3.Distance(position, enemy.GetPosition()) < Vector3.Distance(position, closest.GetPosition()))
                    {
                        closest = enemy;
                    }
                }
            }
        }
        return closest;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}