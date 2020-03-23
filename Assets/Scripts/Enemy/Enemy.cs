using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyData data;

    /// <summary>
    /// Initialization of enemy
    /// </summary>
    /// <param name="data"></param>
    public void Init(EnemyData data)
    {
        this.data = data;
        attack = Attack;
        health = Health;
        GetComponent<Animator>().runtimeAnimatorController = data.MainAnimator;
        transform.tag = "Untagged";
        // GetComponent<Animator>().SetFloat("Speed", 1f);
    }

    private bool isEnemyHitted = false;
    private bool isEnterFirst = true;
    private float timeToOff;

    private void Update()
    {
        if (isEnemyHitted)
        {
            if (isEnterFirst)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                timeToOff = Time.time + 0.1f;
                isEnterFirst = false;
            }
            else
            {
                if (Time.time > timeToOff)
                {
                    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    isEnemyHitted = false;
                    isEnterFirst = true;
                }
            }
        }
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

    public static Action<GameObject> OnEnemyDeath;

    void Death()
    {
        OnEnemyDeath(gameObject);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "StandartBullet")
        {
            isEnemyHitted = true;
            health -= WeaponSpawner.currentCharWeapon.GetComponent<Weapon>().Damage;
        }

        if (health <= 0)
            Death();
    }

    void OnBecameVisible()
    {
        transform.tag = "Enemy";
    }

    void OnBecameInvisible()
    {
        transform.tag = "Untagged";
    }
}
