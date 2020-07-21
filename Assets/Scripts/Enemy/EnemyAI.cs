using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        ChaseTarget,
        Attack
    }
    public State state;

    [SerializeField] private LayerMask enemyLayer;

    private Transform targetTransform;
    private GameObject character;

    Enemy enemy;
    EnemyPathfindingMovement enemyPathfindingMovement;
    EnemyMeleeAttack enemyMeleeAttack;
    EnemyDistantAttack enemyDistantAttack;

    private float speed;
    private string targetTag;
    private float fireRate;
    private EnemyData.AttackType typeOfAttack;
    private float attackRange;

    private float nextAttack;

    void Awake()
    {
        enemyMeleeAttack = GetComponent<EnemyMeleeAttack>();
        enemyDistantAttack = GetComponent<EnemyDistantAttack>();
        enemyPathfindingMovement = GetComponent<EnemyPathfindingMovement>();
        enemy = GetComponent<Enemy>();

        character = GameObject.Find("Character(Clone)");
    }

    void Start()
    {
        targetTag = enemy.Target;
        speed = enemy.Speed;
        fireRate = enemy.FireRate;
        typeOfAttack = enemy.TypeOfAttack;
        attackRange = enemy.AttackRange;

        enemyPathfindingMovement.attackRange = attackRange;
        enemyPathfindingMovement.speed = speed;

        nextAttack = 0.0f;
        state = State.ChaseTarget;

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        GetTarget(targetTag);

        enemyPathfindingMovement.SetTargetPosition(targetTransform.position);
        enemyPathfindingMovement.target = targetTransform.gameObject;

        if (typeOfAttack == EnemyData.AttackType.Distant)
        {
            enemyDistantAttack.targetTag = targetTag;
            enemyDistantAttack.shootTarget = targetTransform;
        }
        else if (typeOfAttack == EnemyData.AttackType.Melee)
        {
            enemyMeleeAttack.targetTag = targetTag;
            enemyMeleeAttack.hitTarget = targetTransform;
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

    public void SetState(State state)
    {
        this.state = state;
    }

    private void GetTarget(string targetTag)
    {
        if (targetTag == "Player")
        {
            targetTransform = character.transform;
        }
        else if (targetTag == "Building")
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
}