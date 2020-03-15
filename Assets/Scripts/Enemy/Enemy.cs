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
        //GetComponent<Animator>().runtimeAnimatorController = data.MainAnimator;
    }

    /// <summary>
    /// Attack of current enemy
    /// </summary>
    public int Attack
    {
        get
        {
            return data.Attack;
        }
        protected set { }
    }

    public int Health
    {
        get
        {
            return data.Health;
        }
        protected set { }
    }

    public static Action<GameObject> OnEnemyDeath;

    void OnBecameVisible()
    {
        transform.tag = "Enemy";
    }
    void OnBecameInvisible()
    {
        transform.tag = "Untagged";
    }
}
