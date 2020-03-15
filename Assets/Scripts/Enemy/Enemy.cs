using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyData data;
    private Transform target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

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
        get { Debug.Log(data.Speed);
            return data.Attack;
        }
        protected set { }
    }

    public float Health
    {
        get {
            Debug.Log(data.Speed);
            return data.Health;
        }
        protected set { }
    }

    public float Speed
    {
        get {
            Debug.Log(data.Speed);
            return data.Speed;
        }
        protected set { }
    }

    public static Action<GameObject> OnEnemyDeath;

    private void FixedUpdate()
    {
        if (Health < 0 && OnEnemyDeath != null)
            OnEnemyDeath(gameObject);

        Vector3 direction = transform.position - target.position;
        float curDistance = direction.sqrMagnitude;
        if (transform.tag == "Enemy" && curDistance > 10f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, Speed * Time.deltaTime);
        }
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
