using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;

    public enum State
    {
        ChaseTarget,
        Attack
    }

    EnemyMeleeAttack enemyMeleeAttack;
    EnemyDistantAttack enemyDistantAttack;
    EnemyPathfindingMovement enemyPathfindingMovement;

    private Rigidbody2D rb;

    Enemy enemy;
    public float speed;
    public string target;
    public float fireRate;
    public EnemyData.AttackType typeOfAttack;
    private float attackRange;
    private float nextAttack;
    private State state;

    private Transform targetTransform;
    private GameObject character;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyMeleeAttack = GetComponent<EnemyMeleeAttack>();
        enemyDistantAttack = GetComponent<EnemyDistantAttack>();
        enemyPathfindingMovement = GetComponent<EnemyPathfindingMovement>();
        enemy = GetComponent<Enemy>();
        character = GameObject.Find("Character(Clone)");
    }

    void Start()
    {
        target = enemy.Target;
        speed = enemy.Speed;
        fireRate = enemy.FireRate;
        typeOfAttack = enemy.TypeOfAttack;
        attackRange = enemy.AttackRange;

        nextAttack = 0.0f;
        state = State.ChaseTarget;

        InvokeRepeating("UpdatePath", 0f, .25f);
    }

    public void SetState(State state)
    {
        this.state = state;
    }

    void UpdatePath()
    {
        GetTarget(target);
        enemyPathfindingMovement.attackRange = attackRange;
        enemyPathfindingMovement.SetTargetPosition(targetTransform.position);
        enemyPathfindingMovement.target = targetTransform.gameObject;
        if (typeOfAttack == EnemyData.AttackType.Distant)
        {
            enemyDistantAttack.target = targetTransform;
        }
        else if (typeOfAttack == EnemyData.AttackType.Melee)
        {
            enemyMeleeAttack.attackRange = attackRange;
        }
    }

    void Update()
    {
        if (Time.time > nextAttack && state == State.Attack)
        {
            switch (typeOfAttack)
            {
                case EnemyData.AttackType.Melee:
                    enemyMeleeAttack.Attack();
                    nextAttack = Time.time + fireRate;
                    break;
                case EnemyData.AttackType.Distant:
                    enemyDistantAttack.Attack();
                    nextAttack = Time.time + fireRate;
                    break;
            }
        }
    }

    private void GetTarget(string target)
    {
        if (target == "Player")
        {
            targetTransform = character.transform;
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
        return new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

    public Vector3 GetCurrentPosition()
    {
        return transform.position;
    }
}