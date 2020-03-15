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
        GetComponent<Animator>().runtimeAnimatorController = data.MainAnimator;
    }

    /// <summary>
    /// Attack of current enemy
    /// </summary>
    public float Attack
    {
        get { Debug.Log(data.Attack);
            return data.Attack;
        }
        protected set { }
    }

    public float Health
    {
        get {
            Debug.Log(data.Health);
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
