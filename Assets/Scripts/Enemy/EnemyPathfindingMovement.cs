using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfindingMovement : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;

    public float speed;
    public GameObject target;
    public float attackRange;
    
    private int currentPathIndex;
    private List<Vector3> pathVectorList;

    private Rigidbody2D rb;
    private Vector3 moveDir;
    private bool m_FacingRight;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        m_FacingRight = true;
    }

    void Update()
    {
        UpdateFlip();
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (pathVectorList != null)
        {
            GetComponent<EnemyAI>().SetState(EnemyAI.State.ChaseTarget);
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > attackRange)
            {
                moveDir = (targetPosition - transform.position + (Vector3)CalculateMove(transform, GetEnemies(0.5f))).normalized;
                rb.velocity = new Vector2(Mathf.Lerp(0, moveDir.x * speed, 1.1f),
                                     Mathf.Lerp(0, moveDir.y * speed, 1.1f));
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count)
                {
                    GetComponent<EnemyAI>().SetState(EnemyAI.State.Attack);
                    moveDir = (transform.position + (Vector3)CalculateMove(transform, GetEnemies(0.5f))).normalized;
                    rb.velocity = new Vector2(Mathf.Lerp(0, moveDir.x * speed, 1.1f),
                                         Mathf.Lerp(0, moveDir.y * speed, 1.1f));
                    pathVectorList = null;
                }
            }
        }
    }

    private List<Transform> GetEnemies(float radius)
    {
        List<Transform> enemies = new List<Transform>();

        foreach (var enemyColl in Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer))
        {
            enemies.Add(enemyColl.transform);
        }
        return enemies;
    }

    private Vector2 CalculateMove(Transform enemy, List<Transform> enemies)
    {
        if (enemies.Count == 0)
            return Vector2.zero;
        Vector2 avoidanceMove = Vector2.zero;

        foreach (var curEnemy in enemies)
        {
            if (curEnemy != null)
                avoidanceMove += (Vector2)(enemy.position - curEnemy.position);
        }
        return avoidanceMove;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;

        pathVectorList = Pathfinding.Instance.FindPath(transform.position, targetPosition);
        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void UpdateFlip()
    {
        if ((transform.position - target.transform.position).x < 0 && !m_FacingRight)
            Flip();
        else if ((transform.position - target.transform.position).x > 0 && m_FacingRight)
            Flip();
    }

}
