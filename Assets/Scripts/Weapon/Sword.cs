using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Animator animator;
    private Weapon currentWeapon;
#pragma warning disable 0649
    [Tooltip("Маска врагов")]
    [SerializeField] private LayerMask enemyLayer;
#pragma warning restore 0649

    void Start()
    {
        animator = GetComponent<Animator>();
        currentWeapon = GetComponent<Weapon>();
        animator.runtimeAnimatorController = currentWeapon.MainAnimator;
    }

    public void Hit()

    {
        animator.SetBool("Attack", true);
        var enemies = Physics2D.OverlapCircleAll(currentWeapon.transform.position, currentWeapon.Radius, enemyLayer);
        foreach (var enemy in enemies)
        {

            var currentAngle = -Mathf.Atan2(enemy.transform.position.x - transform.position.x,
                                         enemy.transform.position.y - transform.position.y) * Mathf.Rad2Deg;
            if (currentAngle <= transform.rotation.eulerAngles.z + currentWeapon.AttackAngle
                    && currentAngle >= transform.rotation.eulerAngles.z - currentWeapon.AttackAngle)
                Debug.Log("HIT!");
                //enemy.GetComponent<Enemy>().GetDamage(currentWeapon.Damage, currentWeapon.CritChance);
        }
    }

}
