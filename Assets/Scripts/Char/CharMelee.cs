using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMelee : MonoBehaviour
{
    public LayerMask enemyLayer;
    public Animator animator;
    private float timeToOff;
    private float attackTime;
    private bool isAttack;

    void Start()
    {
        timeToOff = Time.time;
        attackTime = Time.time;
    }

    void Update()
    {
        if (timeToOff < Time.time)
        {
            animator.SetBool("Attack", false);
        }

        if(attackTime < Time.time)
        {
            isAttack = true;
        }
    }

    public void Hit()
    {
        if(isAttack)
        {
            isAttack = false;
            animator.SetBool("Attack", true);
            timeToOff = Time.time + 0.25f;
            var enemies = Physics2D.OverlapCircleAll(transform.position, 0.7f, enemyLayer);
            foreach (var enemy in enemies)
            {
                enemy.gameObject.GetComponent<Enemy>().GetDamage();
            }
            attackTime = Time.time + 0.225f;
        }

        
    }
}
