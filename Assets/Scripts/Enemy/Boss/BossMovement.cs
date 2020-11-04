using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public bool isMovement = true;
    public Transform CurrentTarget
    {
        get { return currentTarget; }
        set { currentTarget = value; }
    }
    private Transform currentTarget;

    private Boss boss;
    private Rigidbody2D rb;
    private Vector3 roamPosition;

    private bool m_FacingRight;
    private float timeToNewRoam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boss = GetComponent<Boss>();
        timeToNewRoam = float.MinValue;
        SetFacing();
    }

    void Update()
    {
        if (!boss.IsDeath && !CharAction.isDeath)
        {
            if (currentTarget != null)
            {
                if (currentTarget.position.x - transform.position.x > 0 && !m_FacingRight)
                    Flip();
                else if (currentTarget.position.x - transform.position.x < 0 && m_FacingRight)
                    Flip();

                float newRoamTime = 0.85f;

                if (Time.time > timeToNewRoam)
                {
                    roamPosition = UtilsClass.GetRandomDir();
                    timeToNewRoam = Time.time + newRoamTime;
                }

                roamPosition = roamPosition.normalized;
                if (isMovement)
                    rb.velocity = roamPosition * 5f;
                else
                    Debug.Log("no target");

            }                
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Wall")
            timeToNewRoam = Time.time;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Wall" || collider.tag == "Destroyable")
            timeToNewRoam = Time.time;
    }
}
