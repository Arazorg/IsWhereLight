using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    CharInfo charInfo;

    private Animator animator;

    [Tooltip("Точка атаки")]
    [SerializeField] Transform attackPoint;
    [Tooltip("Радиус атаки")]
    [SerializeField] float attackRange = 0.5f;
    [Tooltip("Player's layer")]
    [SerializeField] private LayerMask playerLayers;

    private int damage;
    private bool isAttackState;

    void Start()
    {
        isAttackState = false;
        animator = GetComponent<Animator>();
        GameObject player = GameObject.Find("Character(Clone)");
        charInfo = player.GetComponent<CharInfo>();

        if (EnemySpawner.Enemies.ContainsKey(gameObject))
            damage = EnemySpawner.Enemies[gameObject].Attack;
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            isAttackState = true;
            Attack();
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            isAttackState = false;
            Attack();
        }
    }

    void Attack()
    {
        Debug.Log("attack");
        Collider2D[] players = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);

        foreach (Collider2D player in players)
        {
            player.GetComponent<CharAction>().isPlayerHitted = true;
            player.GetComponent<CharAction>().isEnterFirst = true;

            if (EnemySpawner.Enemies.ContainsKey(gameObject))
            {
                int damage = EnemySpawner.Enemies[gameObject].Attack;
                charInfo.Damage(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
