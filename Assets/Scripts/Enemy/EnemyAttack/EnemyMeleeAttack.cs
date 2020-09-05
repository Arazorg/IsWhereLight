using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Player's layer")]
    [SerializeField] private LayerMask playerLayers;
#pragma warning restore 0649

    private CharInfo charInfo;
    private Animator animator;

    private int damage;
    private float attackRange;
    private float attackRate;
    private float attackAngle;
    private float timeToAttack;

    public bool isAttack;
    void Start()
    {
        animator = GetComponent<Animator>();
        GameObject player = GameObject.Find("Character(Clone)");
        charInfo = player.GetComponent<CharInfo>();
        timeToAttack = Time.time;

        var enemy = GetComponent<Enemy>();
        damage = enemy.Damage;
        attackRange = enemy.AttackRange;
        attackRate = enemy.FireRate;
        attackAngle = enemy.AttackAngle;
    }

    private void Update()
    {
        if (Time.time > timeToAttack && !GetComponent<Enemy>().IsDeath)
        {
            isAttack = true;
            Attack();
        }
        
    }
    
    public void DestroyObstacle()
    {
        var obstacles = Physics2D.OverlapCircleAll(transform.position, attackRange, 1 << LayerMask.NameToLayer("EnemyStatic"));
        animator.speed = 3f;
        animator.Play("Attack");
        animator.speed = 1f;
        foreach (var obstacle in obstacles)
        {
            obstacle.GetComponent<Enemy>().DestroyStaticEnemy();
        } 
    }

    public void Attack()
    {
        Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayers);
        foreach (Collider2D player in players)
        {
            timeToAttack = Time.time + attackRate;
            animator.Play("Attack");
            player.GetComponent<CharAction>().IsPlayerHitted = true;
            player.GetComponent<CharAction>().IsEnterFirst = true;
            charInfo.Damage(damage);
            
            var currentAngle = -Mathf.Atan2(player.transform.position.x - transform.position.x,
                                   player.transform.position.y - transform.position.y) * Mathf.Rad2Deg;
            if (currentAngle > 0)
            {
                if (currentAngle <= transform.rotation.eulerAngles.z + attackAngle
                                                   && currentAngle >= transform.rotation.eulerAngles.z - attackAngle)
                {
                    if (player.transform.tag == "Player")
                        charInfo.Damage(damage);
                }
                    
            }
            else
            {
                if (currentAngle <= transform.rotation.eulerAngles.z + attackAngle - 360
                                                      && currentAngle >= transform.rotation.eulerAngles.z - attackAngle - 360)
                    if (player.transform.tag == "Player")
                        charInfo.Damage(damage);
            }
            isAttack = false;
        }
    }
}
