using UnityEngine;

public class EnemyDistantAttack : MonoBehaviour
{
    public Transform ShootTarget
    {
        get { return shootTarget; }
        set 
        { 
            if(value != null)
                shootTarget = value; 
        }
    }
    public string TargetTag
    {
        get { return targetTag; }
        set
        {
            if (value != null)
                targetTag = value;
        }
    }
    private string targetTag;

    private Transform shootTarget;
    private Enemy enemy;
    private EnemyBulletSpawner enemyBulletSpawner;

    private float fireRate;
    private float bulletSpeed;
    private float bulletScatterAngle;
    private float timeToFire;

    void Start()
    {
        enemyBulletSpawner = GetComponent<EnemyBulletSpawner>();
        enemy = GetComponent<Enemy>();
        fireRate = enemy.FireRate;
        targetTag = enemy.Target;

        var bulletData = enemy.DataOfBullet;
        bulletSpeed = bulletData.Speed;
        bulletScatterAngle = bulletData.Scatter;
        enemyBulletSpawner.SetBullet(bulletData);
    }

    void Update()
    {
        if(Time.time > timeToFire && !enemy.IsDeath && gameObject.tag == "Enemy")
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

            LayerMask layerMask = ~(1 << LayerMask.NameToLayer("Enemy") |
                                        1 << LayerMask.NameToLayer("Ignore Raycast") |
                                            1 << LayerMask.NameToLayer("Room") |
                                                1 << LayerMask.NameToLayer("SpawnPoint"));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, closeDirection, distanceToPlayer, layerMask);

            if (hit.collider != null)
            {
                if ((hit.collider.tag == targetTag) || (hit.collider.tag == "Player"))
                    return true;
            }
        }
        else
            GetComponent<EnemyAI>().GetTarget(targetTag);
        return false;
    }

    private void Shoot()
    {
        AudioManager.instance.Play("EnemyDistantAttack");
        enemyBulletSpawner.Spawn();
        Quaternion dir = Quaternion.AngleAxis(Random.Range(-bulletScatterAngle, bulletScatterAngle + 1), Vector3.forward);
        Rigidbody2D rb = enemyBulletSpawner.currentEnemyBullet.GetComponent<Rigidbody2D>();
        Vector3 bulletRotate = dir * (shootTarget.position - enemyBulletSpawner.currentEnemyBullet.transform.position).normalized;
        enemyBulletSpawner.currentEnemyBullet.transform.rotation = Quaternion.Euler(bulletRotate.x, bulletRotate.y, bulletRotate.z);
        rb.AddForce(dir * (shootTarget.position - enemyBulletSpawner.currentEnemyBullet.transform.position).normalized * bulletSpeed, ForceMode2D.Impulse);
    }
}

