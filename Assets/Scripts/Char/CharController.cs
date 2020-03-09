using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class CharController : MonoBehaviour
{
    public Joystick joystick;
    public Animator animator;
    public float speed = 0;
    private float gunAngle;
    private Transform gun;
    private Rigidbody2D rb;
    private GameObject[] enemies;
    private bool m_FacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>() as Rigidbody2D;
        joystick = GameObject.Find("Dynamic Joystick").GetComponent<Joystick>();
        gun = transform.GetChild(0); //Current gun
    }

    void Update()
    {
        if (!RotateGunToEnemy())
        {
            if (MoveChar())
            {
                gunAngle = RotateGun();
            }
            else
            {
                transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 0, gunAngle));
            }
        }        
    }

    private float RotateGun()
    {
        float gunAngle = -Mathf.Atan2(joystick.Horizontal, joystick.Vertical) * Mathf.Rad2Deg;
        transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 0, gunAngle));
        return gunAngle;
    }

    private bool RotateGunToEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length != 0)
        {
            GameObject closestEnemy = null;
            float distanceToEnemy = Mathf.Infinity;
            foreach (GameObject enemy in enemies)
            {
                Vector3 direction = enemy.transform.position - transform.position;
                float curDistance = direction.sqrMagnitude;
                if (curDistance < distanceToEnemy)
                {
                    closestEnemy = enemy;
                    distanceToEnemy = curDistance;
                }
            }

            Vector3 closeDirection = (closestEnemy.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, closeDirection, distanceToEnemy, 1 << LayerMask.NameToLayer("Target"));

            if (hit.collider.tag == "Enemy")
            {
                float gunAngle = -Mathf.Atan2(closestEnemy.transform.position.x - transform.position.x, 
                    closestEnemy.transform.position.y - transform.position.y) * Mathf.Rad2Deg;
                gun.rotation = Quaternion.Euler(new Vector3(0, 0, gunAngle));
                return true;
            }
        }
        return false;
    }

    private bool MoveChar()
    {
        animator.SetFloat("Speed", Math.Abs(joystick.Horizontal));
        if (joystick.Horizontal > 0 && !m_FacingRight)
            Flip();
        else if (joystick.Horizontal < 0 && m_FacingRight)
            Flip();

        rb.velocity = new Vector2(Mathf.Lerp(0, joystick.Horizontal* speed, 0.8f),
                                     Mathf.Lerp(0, joystick.Vertical * speed, 0.8f));
        
        if (new Vector2(joystick.Horizontal, joystick.Vertical) != Vector2.zero)
            return true;
        return false;
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
