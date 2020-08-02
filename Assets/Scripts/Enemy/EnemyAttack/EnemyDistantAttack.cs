using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDistantAttack : MonoBehaviour
{
    private EnemyBulletSpawner enemyBulletSpawner;
    private Transform shootTarget;
    private string targetTag;
    private float fireRate;
    private float bulletSpeed;
    private float bulletScatterAngle;
    private float timeToFire;

    void Start()
    {
        enemyBulletSpawner = GetComponent<EnemyBulletSpawner>();

        var enemy = GetComponent<Enemy>();
        fireRate = enemy.FireRate;
        targetTag = enemy.Target;

        var bulletData = enemy.DataOfBullet;
        bulletSpeed = bulletData.Speed;
        bulletScatterAngle = bulletData.Scatter;
        enemyBulletSpawner.SetBullet(bulletData);
        timeToFire = 0f;

    }

    void Update()
    {
        if(Time.time > timeToFire)
        {
            Attack();
            timeToFire += fireRate;
        }
    }

    public void Attack()
    {
        if (GetTargetAccess())
            Shoot();
    }

    private bool GetTargetAccess()
    {
        if (shootTarget != null)
        {
            Vector3 direction = shootTarget.position - transform.position;
            float distanceToPlayer = direction.sqrMagnitude;

            Vector3 closeDirection = (shootTarget.transform.position - transform.position).normalized;

            LayerMask layerMask = ~(1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Ignore Raycast"));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, closeDirection, distanceToPlayer, layerMask);

            if (hit.collider != null)
            {
                if (hit.collider.tag == targetTag)
                    return true;
            }
        }
        return false;
    }

    private void Shoot()
    {
        enemyBulletSpawner.Spawn();
        Quaternion dir = Quaternion.AngleAxis(Random.Range(-bulletScatterAngle, bulletScatterAngle + 1), Vector3.forward);
        Rigidbody2D rb = enemyBulletSpawner.currentEnemyBullet.GetComponent<Rigidbody2D>();
        rb.AddForce(dir * (shootTarget.position - enemyBulletSpawner.currentEnemyBullet.transform.position).normalized * bulletSpeed, ForceMode2D.Impulse);
    }

    public void SetTarget(Transform shootTarget)
    {
        this.shootTarget = shootTarget;
    }
}

