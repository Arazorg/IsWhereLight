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
    public Transform target;

    private BulletData bulletData;

    void Start()
    {
        animator = GetComponent<Animator>();
        GameObject player = GameObject.Find("Character(Clone)");
        enemyBulletSpawner = GetComponent<EnemyBulletSpawner>();
        if (EnemySpawner.Enemies.ContainsKey(gameObject))
        {
            var enemy = EnemySpawner.Enemies[gameObject];
            bulletData = EnemySpawner.Enemies[gameObject].dataOfBullet;
            damage = enemy.Attack;
            attackRange = enemy.AttackRange;
            bulletSpeed = bulletData.Speed;
            bulletScatterAngle = bulletData.Scatter;
            enemyBulletSpawner.SetBullet(bulletData);
        }
    }

    public void Attack()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, attackRange, playerLayers);
        if(player != null)
            Shoot();
    }

    private void Shoot()
    {
        enemyBulletSpawner.Spawn();
        Quaternion dir = Quaternion.AngleAxis(Random.Range(-bulletScatterAngle, bulletScatterAngle + 1), Vector3.forward);
        Rigidbody2D rb = enemyBulletSpawner.currentEnemyBullet.GetComponent<Rigidbody2D>();
        rb.AddForce(dir * (target.position - enemyBulletSpawner.currentEnemyBullet.transform.position).normalized * bulletSpeed, ForceMode2D.Impulse);
    }
}

