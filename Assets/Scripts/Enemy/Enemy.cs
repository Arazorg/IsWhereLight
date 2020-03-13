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
        GetComponent<Animator>().runtimeAnimatorController = data.MainAnimator;
    }

    /// <summary>
    /// Attack of current enemy
    /// </summary>
    public float Attack
    {
        get { return data.Attack; }
        protected set { }
    }

    public static Action<GameObject> OnEnemyDeath;

    private void FixedUpdate()
    {
        
    }
}
