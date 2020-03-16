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
    public void Init(EnemyData _data)
    {
        data = _data;
        health = Health;
        GetComponent<Animator>().runtimeAnimatorController = MainAnimator;
       // GetComponent<Animator>().SetFloat("Speed", 1f);
    }

    /// <summary>
    /// Animator of current enemy
    /// </summary>
    private Animator mainAnimator;
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
        protected set
        {
            health = data.Health;
        }
    }

    public static Action<GameObject> OnEnemyDeath;

    void Death()
    {
        OnEnemyDeath(gameObject);
        Debug.Log("death of enemy");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("damage" + health);
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
