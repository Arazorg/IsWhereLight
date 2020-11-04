using UnityEngine;

public class ConstantLaser : MonoBehaviour
{
    public bool IsAttack
    {
        get { return isAttack; }
        set
        {
            if (value)
                animator.SetBool("Attack", true);
            isAttack = value;
            timeToDamage = Time.time + GetComponent<Weapon>().FireRate;
            if (GetComponentInParent<CharController>().ClosestEnemy != null)
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
    
    private Animator animator;
    private Weapon weapon;
    private BulletSpawner bulletSpawner;
    private GameObject bullet;

    private bool isStart;
    private bool isAttack;
    private float timeToDamage;

    void Start()
    {
        if (!isAttack)
            timeToDamage = float.MaxValue;
        animator = GetComponent<Animator>();
        weapon = GetComponent<Weapon>();
        transform.GetChild(0).localPosition = weapon.FirePointPosition;
    }

    public void SetBulletInfo(Bullet bullet)
    {
        bulletSpawner = GetComponent<BulletSpawner>();
    }

    void Update()
    {
        if (isAttack && !CharAction.isDeath)
        {
            Shoot();
            if (Time.time > timeToDamage)
            {
                var closestEnemy = GetComponentInParent<CharController>().ClosestEnemy;
                timeToDamage = Time.time + weapon.FireRate;
                if (closestEnemy != null)
                {
                    var bossScript = closestEnemy.GetComponent<Boss>();
                    var enemyScript = closestEnemy.GetComponent<Enemy>();
                    if((enemyScript != null))
                        enemyScript.GetDamage(weapon.Damage, weapon.CritChance, closestEnemy.transform, weapon.Knoking);
                    else if (bossScript != null)
                        bossScript.GetDamage(weapon.Damage, weapon.CritChance, closestEnemy.transform, weapon.Knoking);
                }
            }
        }
    }

    public void Shoot()
    {
        Vector3 enemyPosition = Vector3.zero;
        try
        {
            enemyPosition = GetComponentInParent<CharController>().ClosestEnemy.transform.position;
        }
        catch
        {
            if (bullet != null)
            {
                bullet.GetComponent<Bullet>().RemoveConstant();
                bullet = null;
            }
        }

        if (GetComponentInParent<CharController>().ClosestEnemy != null)
        {
            if (bullet == null)
            {
                bulletSpawner.Spawn(transform);
                bullet = bulletSpawner.CurrentWeaponBullet;
                bullet.tag = "ConstantLaser";
                isStart = true;
            }

            var laserScale = new Vector3(0.8f, (enemyPosition - transform.GetChild(0).position).magnitude);
            if (isStart)
            {
                bullet.GetComponent<Bullet>().StartConstant();
                isStart = false;
            }
            bullet.GetComponent<SpriteRenderer>().size = laserScale;
            bullet.transform.position = new Vector3((enemyPosition.x + transform.GetChild(0).position.x) / 2,
                                                        (enemyPosition.y + transform.GetChild(0).position.y) / 2);
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
        animator.SetBool("Attack", false);
        timeToDamage = float.MaxValue;
        isAttack = false;
        if (bullet != null)
        {
            bullet.GetComponent<Bullet>().RemoveConstant();
            bullet = null;
        }
    }
}
