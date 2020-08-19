using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private bool m_FacingRight;
    private Transform currentTarget ;

    void Start()
    {
        SetFacing();
    }

    void Update()
    {
        if(!GetComponent<Enemy>().IsDeath)
        {
            if (currentTarget != null)
            {
                if (currentTarget.position.x - transform.position.x > 0 && !m_FacingRight)
                    Flip();

                else if (currentTarget.position.x - transform.position.x < 0 && m_FacingRight)
                    Flip();
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

    public void SetCurrentTarget(Transform target)
    {
        currentTarget = target;
    }
}
