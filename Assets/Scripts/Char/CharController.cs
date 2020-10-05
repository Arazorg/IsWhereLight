using System;
using UnityEngine;

public class CharController : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Аниматор персонажа")]
    [SerializeField] private Animator characterAnimator;

    [Tooltip("CharGun скрипт")]
    [SerializeField] private CharGun charGun;

    [Tooltip("Скорость персонажа")]
    [SerializeField] private float speed;
#pragma warning restore 0649

    public RuntimeAnimatorController CharacterRuntimeAnimatorController
    {
        get { return characterAnimator.runtimeAnimatorController; }
        set { characterAnimator.runtimeAnimatorController = value; }
    }

    public GameObject ClosestEnemy
    {
        get { return closestEnemy; }
        set { closestEnemy = value; }
    }
    private GameObject closestEnemy = null;

    public Joystick JousticVal
    {
        get { return joystick; }
        set { joystick = value; }
    }
    public Joystick joystick;
    private Rigidbody2D rb;
    private Transform gun;
    private CharInfo charInfo;

    private float gunAngle;
    private bool m_FacingRight;
    private bool isStop;
    private string currentTag;
    private float startSpeed;

    void Start()
    {
        charInfo = GameObject.Find("CharInfoHandler").GetComponent<CharInfo>();
        joystick = GameObject.Find($"Joystick").GetComponent<Joystick>();
        rb = GetComponent<Rigidbody2D>() as Rigidbody2D;
        charGun.SpawnStartWeapon();
        gun = transform.Find(charInfo.weapons[charGun.CurrentWeaponNumber]);
        m_FacingRight = true;
        isStop = false;
        currentTag = "Enemy";
        speed = GameObject.Find("CharParametrsHandler").GetComponent<CharParametrs>().CharSpeed;
        startSpeed = GameObject.Find("CharParametrsHandler").GetComponent<CharParametrs>().CharSpeed;
    }

    void FixedUpdate()
    {
        if (!CharAction.isDeath && !GetComponent<CharSkills>().isLegionnaireSkill)
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
                = ~(1 << LayerMask.NameToLayer("Ally") |
                        1 << LayerMask.NameToLayer("Ignore Raycast") |
                            1 << LayerMask.NameToLayer("Player") |
                                1 << LayerMask.NameToLayer("Room") |
                                    1 << LayerMask.NameToLayer("SpawnPoint"));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, closeDirection, Mathf.Infinity, layerMask);
            if (hit.collider != null)
            {
                if (hit.collider.tag == tag)
                {
                    gunAngle = -Mathf.Atan2(closestEnemy.transform.position.x - transform.position.x,
                                                closestEnemy.transform.position.y - transform.position.y) * Mathf.Rad2Deg;
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

    public void SetZeroSpeed(bool isZero)
    {
        if (isZero)
            speed = 0;
        else
            speed = startSpeed;
    }
}
