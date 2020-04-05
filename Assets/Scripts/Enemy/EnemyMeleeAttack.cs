﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    private CharInfo charInfo;
    private Animator animator;

    [Tooltip("Точка атаки")]
    [SerializeField] Transform attackPoint;
    [Tooltip("Радиус атаки")]
    public float attackRange;
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
        {
            damage = EnemySpawner.Enemies[gameObject].Attack;
            attackRange = EnemySpawner.Enemies[gameObject].AttackRange;
        } 
    }

    public void Attack()
    {
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
}