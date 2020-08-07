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
   
    /*
    public string TargetTag
    {
        get { return targetTag; }
        set { targetTag = value; }
    }
    private string targetTag;
    */

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

    }

    private void Update()
    {
        if (Time.time > timeToAttack && !GetComponent<Enemy>().isDeath)
        {
            Attack();
        }
    }

    public void Attack()
    {
        Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayers);

        foreach (Collider2D player in players)
        {
            timeToAttack = Time.time + attackRate;
            animator.Play("Attack");
            player.GetComponent<CharAction>().isPlayerHitted = true;
            player.GetComponent<CharAction>().isEnterFirst = true;
            charInfo.Damage(damage);

            var currentAngle = -Mathf.Atan2(player.transform.position.x - transform.position.x,
                                   player.transform.position.y - transform.position.y) * Mathf.Rad2Deg;
            if (currentAngle > 0)
            {
                if (currentAngle <= transform.rotation.eulerAngles.z + attackAngle
                                                   && currentAngle >= transform.rotation.eulerAngles.z - attackAngle)
                {
                    if (player.transform.tag == "Player")
                    {
                        charInfo.Damage(damage);
                    }
                }
            }
            else
            {
                if (currentAngle <= transform.rotation.eulerAngles.z + attackAngle - 360
                                                      && currentAngle >= transform.rotation.eulerAngles.z - attackAngle - 360)
                {
                    if (player.transform.tag == "Player")
                    {
                        charInfo.Damage(damage);
                    }
                }
            }
        }
    }
}
