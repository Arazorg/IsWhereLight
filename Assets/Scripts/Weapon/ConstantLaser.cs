﻿using UnityEngine;

public class ConstantLaser : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Элемент в конце и начале лазера")]
    [SerializeField] private GameObject startEndElement;
#pragma warning restore 0649

    public Animator animator;
    private bool isAttack;
    private bool isStart;

    public bool IsAttack
    {
        get
        {
            return isAttack; 
        }
        set {
            isAttack = value;
            if (GetComponentInParent<CharController>().closestEnemy != null)
            {
                bulletSpawner.Spawn(transform);
                bullet = bulletSpawner.CurrentWeaponBullet;
                bullet.tag = "ConstantLaser";
                isStart = true;
            }
            else
                isAttack = false;
        }
    }

    private BulletSpawner bulletSpawner;
    private GameObject bullet;
    private GameObject startElement;
    private GameObject endElement;
    private float bulletScatterAngle;

    void Start()
    {
        animator = GetComponent<Animator>();
        transform.GetChild(0).localPosition = GetComponent<Weapon>().firePointPosition;
    }

    public void SetBulletInfo(Bullet bullet)
    {
        bulletSpawner = GetComponent<BulletSpawner>();
        bulletScatterAngle = bullet.Scatter;
    }

    void Update()
    {
        if(isAttack && !CharAction.isDeath)
            Shoot();
    }

    public void Shoot()
    {
        Vector3 enemyTransform = Vector3.zero;
        animator.SetBool("Attack", true);
        try
        {
            enemyTransform = GetComponentInParent<CharController>().closestEnemy.transform.position;
        }
        catch
        {
            if (bullet != null)
            {
                bullet.GetComponent<Bullet>().RemoveConstant();
                bullet = null;
            }
        }

        if (GetComponentInParent<CharController>().closestEnemy != null)
        {
            if(bullet == null)
            {
                bulletSpawner.Spawn(transform);
                bullet = bulletSpawner.CurrentWeaponBullet;
                bullet.tag = "ConstantLaser";
                isStart = true;
            }

            LayerMask layerMask
                = ~(1 << LayerMask.NameToLayer("Player") |
                        1 << LayerMask.NameToLayer("Ignore Raycast") |
                             1 << LayerMask.NameToLayer("Room"));

            var laserScale = new Vector3(0.8f, (enemyTransform - transform.GetChild(0).position).magnitude);
            if (isStart)
            {
                bullet.GetComponent<Bullet>().StartConstant();
                isStart = false;
            }
            bullet.GetComponent<SpriteRenderer>().size = laserScale;
            bullet.GetComponent<BoxCollider2D>().size = laserScale + new Vector3(0, 0.33f);
            bullet.transform.position
            = new Vector3((enemyTransform.x + transform.GetChild(0).position.x) / 2,
                            (enemyTransform.y + transform.GetChild(0).position.y) / 2);

            bullet.transform.rotation = transform.rotation;
        }
        else
        {
            if (bullet != null)
            {
                bullet.GetComponent<Bullet>().RemoveConstant();
                bullet = null;
            }
        }

    }
    public void StopShoot()
    {
        isAttack = false;
        if (bullet != null)
        {
            bullet.GetComponent<Bullet>().RemoveConstant();
            bullet = null;
        }
    }
}
