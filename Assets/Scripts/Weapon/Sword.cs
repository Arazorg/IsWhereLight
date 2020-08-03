﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Маска врагов")]
    [SerializeField] private LayerMask enemyLayer;
#pragma warning restore 0649

    public Animator animator;
    private Weapon currentWeapon;
    private float timeToBlock;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentWeapon = GetComponent<Weapon>();
        animator.runtimeAnimatorController = currentWeapon.MainAnimator;
    }

    void Update()
    {
        if (Time.time < timeToBlock)
            gameObject.tag = "BlockAll";
        else
            gameObject.tag = "GunKeep";
    }

    public void Hit()
    {
        animator.SetBool("Attack", true);
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "Attack":
                    timeToBlock = Time.time + clip.length;
                    break;
            }
        }
        var enemies = Physics2D.OverlapCircleAll(currentWeapon.transform.position, currentWeapon.Radius, enemyLayer);
        foreach (var enemy in enemies)
        {
            var enemyScript = enemy.GetComponent<Enemy>();
            var currentAngle = -Mathf.Atan2(enemy.transform.position.x - transform.position.x,
                                         enemy.transform.position.y - transform.position.y) * Mathf.Rad2Deg;
            if (currentAngle > 0)
            {
                if (currentAngle <= transform.rotation.eulerAngles.z + currentWeapon.AttackAngle
                                                   && currentAngle >= transform.rotation.eulerAngles.z - currentWeapon.AttackAngle)
                {
                    if (enemy.transform.tag == "Destroyable")
                        Destroy(enemy.gameObject.transform.parent.gameObject);
                    else if (enemy.transform.tag == "Enemy")
                    {
                        enemyScript.GetDamage(currentWeapon.Damage, currentWeapon.CritChance);
                        enemyScript.Knoking(transform.position, 0.25f);
                    }
                }
            }
            else
            {
                if (currentAngle <= transform.rotation.eulerAngles.z + currentWeapon.AttackAngle - 360
                                                   && currentAngle >= transform.rotation.eulerAngles.z - currentWeapon.AttackAngle - 360)
                {
                    if (enemy.transform.tag == "Destroyable")
                        Destroy(enemy.gameObject.transform.parent.gameObject);
                    else if (enemy.transform.tag == "Enemy")
                    {
                        enemyScript.GetDamage(currentWeapon.Damage, currentWeapon.CritChance);
                        enemyScript.Knoking(transform.position, 0.25f);
                    }

                }
            }
        }
    }
}
