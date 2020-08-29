﻿using System;
using UnityEngine;

public class CharController : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Аниматор персонажа")]
    [SerializeField] private Animator characterAnimator;

    //Character's scripts
    [Tooltip("CharInfo скрипт")]
    [SerializeField] private CharInfo charInfo;
    [Tooltip("CharGun скрипт")]
    [SerializeField] private CharGun charGun;

    //Character's variables
    [Tooltip("Скорость персонажа")]
    [SerializeField] public float speed;
#pragma warning restore 0649

    public static bool isRotate;
    public string currentTag;
    public RuntimeAnimatorController CharacterRuntimeAnimatorController
    {
        get { return characterAnimator.runtimeAnimatorController; }
        set { characterAnimator.runtimeAnimatorController = value; }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public GameObject closestEnemy = null;

    //UI
    private Joystick joystick;
    //Characters components
    private Rigidbody2D rb;

    //Character's guns variables
    private Transform gun;
    private float gunAngle;

    private bool m_FacingRight;
    private bool isStop;

    void Start()
    {
        isRotate = true;
        joystick = GameObject.Find($"Joystick").GetComponent<Joystick>();
        rb = GetComponent<Rigidbody2D>() as Rigidbody2D;
        charGun.SpawnStartWeapon();
        gun = transform.Find(charInfo.weapons[charGun.CurrentWeaponNumber]);
        if (gun.GetComponent<Weapon>().TypeOfAttack == WeaponData.AttackType.Bow) //ОШИБКА
            isRotate = false;
        m_FacingRight = true;
        isStop = false;
        currentTag = "Enemy";
    }

    void FixedUpdate()
    {
        if (!CharAction.isDeath)
        {
            characterAnimator.SetFloat("Speed", Math.Abs(joystick.Horizontal));
            rb.velocity = new Vector2(Mathf.Lerp(0, joystick.Horizontal * speed, 0.8f),
                                         Mathf.Lerp(0, joystick.Vertical * speed, 0.8f));
            if (!RotateGunToEnemy(currentTag))
            {
                closestEnemy = null;
                if (joystick.Horizontal > 0 && !m_FacingRight)
                    Flip();
                else if (joystick.Horizontal < 0 && m_FacingRight)
                    Flip();

                if (!isRotate && m_FacingRight && transform.Find(charInfo.weapons[charGun.CurrentWeaponNumber]).GetComponent<Weapon>().TypeOfAttack
                    == WeaponData.AttackType.Bow)
                    transform.Find(charInfo.weapons[charGun.CurrentWeaponNumber]).GetComponent<Bow>().SetAngle(true);
                else if (!isRotate && !m_FacingRight && transform.Find(charInfo.weapons[charGun.CurrentWeaponNumber]).GetComponent<Weapon>().TypeOfAttack
                    == WeaponData.AttackType.Bow)
                    transform.Find(charInfo.weapons[charGun.CurrentWeaponNumber]).GetComponent<Bow>().SetAngle(false);
                else
                {
                    if (joystick.Horizontal != 0 && joystick.Vertical != 0)
                    {
                        gunAngle = RotateGun();
                        isStop = false;
                    }
                    else
                    {
                        if (!isStop)
                        {
                            transform.Find(charInfo.weapons[charGun.CurrentWeaponNumber]).rotation
                                = Quaternion.Euler(new Vector3(0, 0, gunAngle));
                            isStop = true;
                        }
                    }
                }
            }
            else
            {
                // Debug.Log("Enemy");
                if (!isRotate && m_FacingRight && transform.Find(charInfo.weapons[charGun.CurrentWeaponNumber]).GetComponent<Weapon>().TypeOfAttack
                    == WeaponData.AttackType.Bow)
                    transform.Find(charInfo.weapons[charGun.CurrentWeaponNumber]).GetComponent<Bow>().SetAngle(true);
                else if (!isRotate && !m_FacingRight && transform.Find(charInfo.weapons[charGun.CurrentWeaponNumber]).GetComponent<Weapon>().TypeOfAttack
                    == WeaponData.AttackType.Bow)
                    transform.Find(charInfo.weapons[charGun.CurrentWeaponNumber]).GetComponent<Bow>().SetAngle(false);

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
    }

    private float RotateGun()
    {
        float gunAngle = -Mathf.Atan2(joystick.Horizontal, joystick.Vertical) * Mathf.Rad2Deg;
        if (isRotate)
            transform.Find(charInfo.weapons[charGun.CurrentWeaponNumber]).rotation
                = Quaternion.Euler(new Vector3(0, 0, gunAngle));
        return gunAngle;
    }

    private bool RotateGunToEnemy(string tag = "Enemy")
    {
        var enemies = GameObject.FindGameObjectsWithTag(tag);
        if (enemies.Length != 0)
        {
            closestEnemy = null;
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

            gun = transform.Find(charInfo.weapons[charGun.CurrentWeaponNumber]);
            Vector3 closeDirection = (closestEnemy.transform.position - transform.position);
            LayerMask layerMask
                = ~(1 << LayerMask.NameToLayer("Player") |
                        1 << LayerMask.NameToLayer("Ignore Raycast") |
                            1 << LayerMask.NameToLayer("Room"));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, closeDirection, Mathf.Infinity, layerMask);

            if (hit.collider != null)
            {
                if (hit.collider.tag == tag)
                {
                    gunAngle = -Mathf.Atan2(closestEnemy.transform.position.x - transform.position.x,
                                            closestEnemy.transform.position.y - transform.position.y)
                                                * Mathf.Rad2Deg;
                    if (isRotate)
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

    public void SetRbVelocityZero()
    {
        rb.velocity = Vector2.zero;
    }
}
