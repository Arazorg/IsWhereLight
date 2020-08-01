using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Точка атаки")]
    [SerializeField] Transform attackPoint;

    [Tooltip("Player's layer")]
    [SerializeField] private LayerMask playerLayers;
#pragma warning restore 0649

    private CharInfo charInfo;
    private Animator animator;

    private int damage;
    private float attackRange;
    private float timeToOff;

    public Transform hitTarget;
    public string targetTag;

    void Start()
    {
        animator = GetComponent<Animator>();
        GameObject player = GameObject.Find("Character(Clone)");
        charInfo = player.GetComponent<CharInfo>();
        timeToOff = Time.time;

        var enemy = GetComponent<Enemy>();
        damage = enemy.Damage;
        attackRange = enemy.AttackRange;
    }

    private void Update()
    {
        GetTargetAccess();

        if (timeToOff < Time.time)
        {
            animator.SetBool("TargetClose", false);
        }
    }

    public void Attack()
    {
        Collider2D[] players = Physics2D.OverlapCircleAll(attackPoint.position, 0.5f, playerLayers);

        foreach (Collider2D player in players)
        {
            timeToOff = Time.time + 1f;
            animator.SetBool("TargetClose", true);
            player.GetComponent<CharAction>().isPlayerHitted = true;
            player.GetComponent<CharAction>().isEnterFirst = true;
            charInfo.Damage(damage);
        }
    }

    private void GetTargetAccess()
    {
        if (hitTarget != null)
        {
            Vector3 direction = hitTarget.position - transform.position;
            float distanceToPlayer = direction.sqrMagnitude;
            Vector3 closeDirection = (hitTarget.transform.position - transform.position).normalized;
            attackPoint.localPosition = closeDirection;
        }
    }
}
