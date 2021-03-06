﻿using CodeMonkey.Utils;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public bool isMovement = true;
    public Transform CurrentTarget
    {
        get { return currentTarget; }
        set { currentTarget = value; }
    }
    private Transform currentTarget;

    private Enemy enemy;
    private EnemyMeleeAttack enemyMeleeAttack;
    private Rigidbody2D rb;

    private Vector3 roamPosition;
    private bool m_FacingRight;
    private float timeToNewRoam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        enemyMeleeAttack = GetComponent<EnemyMeleeAttack>();
        timeToNewRoam = float.MaxValue;
        SetFacing();
    }

    void Update()
    {
        if (!enemy.IsDeath && !enemy.isKnoking && !CharAction.isDeath)
        {
            if (currentTarget != null)
            {
                if (currentTarget.position.x - transform.position.x > 0 && !m_FacingRight)
                    Flip();
                else if (currentTarget.position.x - transform.position.x < 0 && m_FacingRight)
                    Flip();

                float distanceNewRoam = 1f;
                float newRoamTime = 1.5f;
                if (enemy.TypeOfAttack == EnemyData.AttackType.Melee)
                {
                    if (Vector3.Distance(transform.position, transform.position + roamPosition) < distanceNewRoam
                                        || Time.time > timeToNewRoam)
                    {
                        if (enemyMeleeAttack.IsAttack)
                        {
                            roamPosition = currentTarget.position - transform.position + (UtilsClass.GetRandomDir() / 10);
                        }
                        else
                        {
                            roamPosition = UtilsClass.GetRandomDir();
                            var cameraPosition = CalculateScreenSizeInWorldCoords();
                            timeToNewRoam = Time.time + newRoamTime;
                        }
                    }
                }
                else if (enemy.TypeOfAttack == EnemyData.AttackType.Distant)
                {
                    if (gameObject.tag != "Enemy")
                    {
                        roamPosition = currentTarget.position - transform.position;
                    }
                    else if (Vector3.Distance(transform.position, transform.position + roamPosition) < distanceNewRoam
                                        || Time.time > timeToNewRoam)
                    {
                        roamPosition = UtilsClass.GetRandomDir();
                        var cameraPosition = CalculateScreenSizeInWorldCoords();
                        timeToNewRoam = Time.time + newRoamTime;
                    }
                }

                roamPosition = roamPosition.normalized;
                if(isMovement)
                    rb.velocity = roamPosition * 5f;

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Wall")
        {
            timeToNewRoam = Time.time;
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Wall")
        {
            timeToNewRoam = Time.time;
        }
        if (collider.tag == "Destroyable")
        {
            if (enemy.TypeOfAttack == EnemyData.AttackType.Melee)
                enemyMeleeAttack.DestroyObstacle();
        }
    }

    private void SetFacing()
    {
        if (transform.localScale.x == 1)
            m_FacingRight = true;
        else
            m_FacingRight = false;
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private Vector2 CalculateScreenSizeInWorldCoords()
    {
        var cam = Camera.main;
        var p1 = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        var p2 = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane));
        var p3 = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
        float width = (p2 - p1).magnitude;
        float height = (p3 - p2).magnitude;
        Vector2 dimensions = new Vector2(width, height);
        return dimensions;
    }
}
