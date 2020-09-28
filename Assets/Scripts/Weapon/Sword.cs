using System;
using UnityEngine;

public class Sword : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Маска врагов")]
    [SerializeField] private LayerMask enemyLayer;
#pragma warning restore 0649

    private Animator animator;
    private Weapon currentWeapon;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentWeapon = GetComponent<Weapon>();
        animator.runtimeAnimatorController = currentWeapon.MainAnimator;
    }

    public void Hit()
    {
        animator.SetBool("Attack", true);
        var character = currentWeapon.transform.parent;
        var enemies = Physics2D.OverlapCircleAll(transform.position, currentWeapon.Radius, (1 << LayerMask.NameToLayer("Enemy") |
                                                                                        1 << LayerMask.NameToLayer("EnemyStatic")));
        foreach (var enemy in enemies)
        {
            var enemyScript = enemy.GetComponent<Enemy>();
            var currentAngle = (-Mathf.Atan2(enemy.transform.position.x - transform.position.x,
                                         enemy.transform.position.y - transform.position.y) * Mathf.Rad2Deg);
            if (currentAngle > 0)
                currentAngle *= -1;

            if (character.localScale.x == 1)
            {
                if ((transform.localEulerAngles.z - 360) - currentWeapon.AttackAngleRight <= currentAngle
                                               && currentAngle <= (transform.localEulerAngles.z - 360) + currentWeapon.AttackAngleLeft)
                {
                    if (enemy.transform.tag == "Destroyable")
                        enemyScript.DestroyStaticEnemy();
                    else if (enemy.transform.tag == "Enemy" || enemy.transform.tag == "Thing")
                        enemyScript.GetDamage(currentWeapon.Damage, currentWeapon.CritChance, transform, currentWeapon.Knoking);
                }
            }
            else
            {
                if ((transform.localEulerAngles.z - 360) - currentWeapon.AttackAngleRight <= currentAngle
                                                   && currentAngle <= (transform.localEulerAngles.z - 360) + currentWeapon.AttackAngleRight)
                {
                    if (enemy.transform.tag == "Destroyable")
                        enemyScript.DestroyStaticEnemy();
                    else if (enemy.transform.tag == "Enemy" || enemy.transform.tag == "Thing")
                        enemyScript.GetDamage(currentWeapon.Damage, currentWeapon.CritChance, transform, currentWeapon.Knoking);
                }
            }
        }
    }

    public void StopShoot()
    {
        animator.SetBool("Attack", false);
    }
}
