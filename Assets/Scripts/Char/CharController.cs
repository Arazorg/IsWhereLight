using System;
using UnityEngine;

public class CharController : MonoBehaviour
{
    //UI
    private Joystick joystick;
    //Characters components
    private Rigidbody2D rb;

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
    [SerializeField] private float speed;
#pragma warning restore 0649

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

    //Character's guns variables
    private Transform gun;
    private float gunAngle;

    private bool m_FacingRight;
    private bool isStop;

    public static bool isRotate;

    void Start()
    {
        isRotate = true;

        joystick = GameObject.Find("Dynamic Joystick").GetComponent<Joystick>();
        rb = GetComponent<Rigidbody2D>() as Rigidbody2D;
        gun = transform.Find(charInfo.weapons[charGun.currentWeaponNumber]);
        if (gun.GetComponent<Weapon>().TypeOfAttack == WeaponData.AttackType.Bow)
            isRotate = false;

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
            Debug.Log("Don't Enemy");
           // Debug.Log("Not Enemy");
            if (joystick.Horizontal > 0 && !m_FacingRight)
                Flip();
            else if (joystick.Horizontal < 0 && m_FacingRight)
                Flip();

            if (!isRotate && m_FacingRight && transform.Find(charInfo.weapons[charGun.currentWeaponNumber]).GetComponent<Weapon>().TypeOfAttack
                == WeaponData.AttackType.Bow)
                transform.Find(charInfo.weapons[charGun.currentWeaponNumber]).GetComponent<Bow>().SetAngle(true);
            else if (!isRotate && !m_FacingRight && transform.Find(charInfo.weapons[charGun.currentWeaponNumber]).GetComponent<Weapon>().TypeOfAttack
                == WeaponData.AttackType.Bow)
                transform.Find(charInfo.weapons[charGun.currentWeaponNumber]).GetComponent<Bow>().SetAngle(false);

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
                        transform.Find(charInfo.weapons[charGun.currentWeaponNumber]).rotation
                            = Quaternion.Euler(new Vector3(0, 0, gunAngle));
                        isStop = true;
                    }
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
        if (isRotate)
            transform.Find(charInfo.weapons[charGun.currentWeaponNumber]).rotation
                = Quaternion.Euler(new Vector3(0, 0, gunAngle));
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
            LayerMask layerMask = ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("Room"));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, closeDirection, Mathf.Infinity, layerMask);
            Debug.Log(hit.collider.name);

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Enemy")
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
        else
        {
            Debug.Log(0);
            
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
