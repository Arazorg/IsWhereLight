using UnityEngine;

public class ConstantLaser : MonoBehaviour
{
    public Animator animator;
    private Weapon weapon;
    private bool isStart;
    private bool isAttack;

    public bool IsAttack
    {
        get
        {
            return isAttack; 
        }
        set {
            isAttack = value;
            timeToDamage = Time.time + GetComponent<Weapon>().FireRate;
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
    private float timeToDamage;

    void Start()
    {
        if(!isAttack)
            timeToDamage = float.MaxValue;
        animator = GetComponent<Animator>();
        weapon = GetComponent<Weapon>();
        transform.GetChild(0).localPosition = weapon.firePointPosition;
    }

    public void SetBulletInfo(Bullet bullet)
    {
        bulletSpawner = GetComponent<BulletSpawner>();
    }

    void Update()
    {
        if(isAttack && !CharAction.isDeath)
        {
            Shoot();
            if(Time.time > timeToDamage)
            {
                var closestEnemy = GetComponentInParent<CharController>().closestEnemy;
                timeToDamage = Time.time + weapon.FireRate;
                if(closestEnemy != null)
                    closestEnemy.GetComponent<Enemy>()
                        .GetDamage(weapon.Damage, weapon.CritChance, closestEnemy.transform, weapon.Knoking);
            }
            
        }
            
    }

    public void Shoot()
    {
        Vector3 enemyPosition = Vector3.zero;
        animator.SetBool("Attack", true);
        try
        {
            enemyPosition = GetComponentInParent<CharController>().closestEnemy.transform.position;
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

            var laserScale = new Vector3(0.8f, (enemyPosition - transform.GetChild(0).position).magnitude);
            if (isStart)
            {
                bullet.GetComponent<Bullet>().StartConstant();
                isStart = false;
            }
            bullet.GetComponent<SpriteRenderer>().size = laserScale;
            bullet.transform.position
            = new Vector3((enemyPosition.x + transform.GetChild(0).position.x) / 2,
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
        timeToDamage = float.MaxValue;
        isAttack = false;
        if (bullet != null)
        {
            bullet.GetComponent<Bullet>().RemoveConstant();
            bullet = null;
        }
    }
}
