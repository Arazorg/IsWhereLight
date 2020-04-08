using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDistantAttack : MonoBehaviour
{
    private EnemyBulletSpawner enemyBulletSpawner;

    CharInfo charInfo;
    private Animator animator;

    private float attackRange;
    [Tooltip("Player's layer")]
    [SerializeField] private LayerMask playerLayers;

    private int damage;
    private float bulletSpeed;
    private float bulletScatterAngle;

    public Transform shootTarget;
    public string targetTag;

    void Start()
    {
        transform.GetChild(0).position = transform.position;
        animator = GetComponent<Animator>();
        enemyBulletSpawner = GetComponent<EnemyBulletSpawner>();

        if (EnemySpawner.Enemies.ContainsKey(gameObject))
        {
            var enemy = GetComponent<Enemy>();
            damage = enemy.Damage;
            attackRange = enemy.AttackRange;

            var bulletData = enemy.dataOfBullet;
            bulletSpeed = bulletData.Speed;
            bulletScatterAngle = bulletData.Scatter;
            enemyBulletSpawner.SetBullet(bulletData);
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
}

