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
                
                moveDir = (targetPosition - transform.position).normalized;
                rb.velocity = new Vector2(Mathf.Lerp(0, moveDir.x * speed, 1.1f),
                                     Mathf.Lerp(0, moveDir.y * speed, 1.1f));
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count)
                {
                    GetComponent<EnemyAI>().SetState(EnemyAI.State.Attack);
                    pathVectorList = null;
                }
            }
        }
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
