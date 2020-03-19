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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "StandartBullet")
            health -= 5;
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
