using UnityEngine;

public class EnemyDistantAttack : MonoBehaviour
{
    public Transform ShootTarget
    {
        get { return shootTarget; }
        set { shootTarget = value; }
    }
    private Transform shootTarget;
    private EnemyBulletSpawner enemyBulletSpawner;
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
    }

    void Update()
    {
        if(Time.time > timeToFire && !GetComponent<Enemy>().IsDeath && gameObject.tag == "Enemy")
        {
            Attack();
            timeToFire = Time.time + fireRate;
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
        Vector3 bulletRotate = dir * (shootTarget.position - enemyBulletSpawner.currentEnemyBullet.transform.position).normalized;
        enemyBulletSpawner.currentEnemyBullet.transform.rotation = Quaternion.Euler(bulletRotate.x, bulletRotate.y, bulletRotate.z);
        rb.AddForce(dir * (shootTarget.position - enemyBulletSpawner.currentEnemyBullet.transform.position).normalized * bulletSpeed, ForceMode2D.Impulse);
    }
}

