using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    EnemyMeleeAttack enemyMeleeAttack;

    private Rigidbody2D rb;

    Enemy enemy;
    public float speed;
    public string target;
    public float fireRate;

    private Transform targetTransform;
    private Transform characterTransform;

    private float nextAttack;

    void Start()
    {
        enemyMeleeAttack = GetComponent<EnemyMeleeAttack>();
        characterTransform = GameObject.Find("Character(Clone)").transform;
        rb = GetComponent<Rigidbody2D>();

        enemy = GetComponent<Enemy>();
        target = enemy.Target;
        speed = enemy.Speed;
        fireRate = enemy.FireRate;

        nextAttack = 0.0f;
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void Update()
    {

        if (rb.velocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (rb.velocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);

        }

        if (Time.time > nextAttack)
        {
            nextAttack = Time.time + fireRate;
        }

    }

    private void GetTarget(string target)
    {
        if (target == "Player")
        {
            targetTransform = characterTransform;
        }
        else if (target == "Building")
        {
            targetTransform = GetNearestBuilding();
        }
    }

    private Transform GetNearestBuilding()
    {
        var buildings = GameObject.FindGameObjectsWithTag("Building");

        if (buildings.Length != 0)
        {
            GameObject closestBuilding = null;
            float distanceToBuilding = Mathf.Infinity;
            foreach (GameObject building in buildings)
            {
                Vector3 direction = building.transform.position - transform.position;
                float curDistance = direction.sqrMagnitude;
                if (curDistance < distanceToBuilding)
                {
                    closestBuilding = building;
                    distanceToBuilding = curDistance;
                }
            }
            return closestBuilding.transform;
        }
        else
        {
            return null;
        }
    }

    private Vector2 GetRoamingPosition()
    {
        return rb.position + GetRandomDir();
    }

    private Vector2 GetRandomDir()
    {
        return new Vector2(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f)).normalized;
    }
}