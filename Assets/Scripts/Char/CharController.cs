using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharController : MonoBehaviour
{
    //UI
    private Joystick joystick;

    //Characters components
    private Rigidbody2D rb;
    [Tooltip("Аниматор персонажа")]
    [SerializeField] private Animator characterAnimator;

    public RuntimeAnimatorController CharacterRuntimeAnimatorController
    {
        get
        {
            return characterAnimator.runtimeAnimatorController;
        }
        set
        {
            characterAnimator.runtimeAnimatorController = value;
        }
    }

    //Character's scripts
    [Tooltip("CharInfo скрипт")]
    [SerializeField] private CharInfo charInfo;
    [Tooltip("CharGun скрипт")]
    [SerializeField] private CharGun charGun;

    //Character's guns variables
    private Transform gun;
    private float gunAngle;

    //Character's variables
    [Tooltip("Скорость персонажа")]
    [SerializeField] private float speed;

    private bool m_FacingRight;
    private bool isStop;

    void Start()
    {
        joystick = GameObject.Find("Dynamic Joystick").GetComponent<Joystick>();
        rb = GetComponent<Rigidbody2D>() as Rigidbody2D;
        gun = transform.Find(charInfo.weapons[charGun.currentWeaponNumber]);
        m_FacingRight = true;
        isStop = false;
    }


    void Update()
    {
        characterAnimator.SetFloat("Speed", Math.Abs(joystick.Horizontal));
        rb.velocity = new Vector2(Mathf.Lerp(0, joystick.Horizontal * speed, 0.8f),
                                     Mathf.Lerp(0, joystick.Vertical * speed, 0.8f));

        if (!RotateGunToEnemy())
        {
            if (joystick.Horizontal > 0 && !m_FacingRight)
                Flip();
            else if (joystick.Horizontal < 0 && m_FacingRight)
                Flip();

            if (joystick.Horizontal != 0 && joystick.Vertical != 0)
            {
                gunAngle = RotateGun();
                isStop = false;
            }
            else
            {
                if(!isStop)
                {
                    transform.Find(charInfo.weapons[charGun.currentWeaponNumber]).rotation = Quaternion.Euler(new Vector3(0, 0, gunAngle));
                    isStop = true;
                }
                    
            }
        }
        else
        {
            if (0 <= gunAngle && gunAngle <= 180)
            {
                m_FacingRight = false;
                transform.localScale = new Vector3(-1f, 1f, 1);
            }
            else
            {
                m_FacingRight = true;
                transform.localScale = new Vector3(1f, 1f, 1);
            }
        }
    }

    private float RotateGun()
    {
        float gunAngle = -Mathf.Atan2(joystick.Horizontal, joystick.Vertical) * Mathf.Rad2Deg;
        transform.Find(charInfo.weapons[charGun.currentWeaponNumber])
            .rotation = Quaternion.Euler(new Vector3(0, 0, gunAngle));
        return gunAngle;
    }

    private bool RotateGunToEnemy()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length != 0)
        {
            GameObject closestEnemy = null;
            float distanceToEnemy = Mathf.Infinity;
            foreach (var enemy in enemies)
            {
                Vector3 direction = enemy.transform.position - transform.position;
                float curDistance = direction.sqrMagnitude;
                if (curDistance < distanceToEnemy)
                {
                    closestEnemy = enemy;
                    distanceToEnemy = curDistance;
                }
            }
            gun = transform.Find(charInfo.weapons[charGun.currentWeaponNumber]);
            Vector3 closeDirection = (closestEnemy.transform.position - transform.position);

            LayerMask layerMask = ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Ignore Raycast"));

            RaycastHit2D hit = Physics2D.Raycast(gun.GetChild(0).transform.position, closeDirection, distanceToEnemy, layerMask);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Enemy")
                {
                    gunAngle = -Mathf.Atan2(closestEnemy.transform.position.x - transform.position.x,
                                            closestEnemy.transform.position.y - transform.position.y)
                                                * Mathf.Rad2Deg;
                    gun.rotation = Quaternion.Euler(new Vector3(0, 0, gunAngle));
                    return true;
                }
            }
        }
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
