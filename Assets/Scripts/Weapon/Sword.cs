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
        var character = currentWeapon.transform.parent.position;
        var enemies = Physics2D.OverlapCircleAll(character, currentWeapon.Radius, (1 << LayerMask.NameToLayer("Enemy") | 
                                                                                        1 << LayerMask.NameToLayer("EnemyStatic")));
        foreach (var enemy in enemies)
        {
            var enemyScript = enemy.GetComponent<Enemy>();
            var currentAngle = -Mathf.Atan2(enemy.transform.position.x - character.x,
                                         enemy.transform.position.y - character.y) * Mathf.Rad2Deg;
            Debug.Log(currentAngle);
            if (currentAngle > 0)
            {
                if (currentAngle <= transform.rotation.eulerAngles.z + currentWeapon.AttackAngleRight
                                                   && currentAngle >= transform.rotation.eulerAngles.z - currentWeapon.AttackAngleRight)
                {
                    if (enemy.transform.tag == "Destroyable")
                        enemyScript.DestroyStaticEnemy();
                    else if (enemy.transform.tag == "Enemy" || enemy.transform.tag == "Thing")
                        enemyScript.GetDamage(currentWeapon.Damage, currentWeapon.CritChance, transform, currentWeapon.Knoking);
                }
            }
            else
            {
                if (currentAngle <= transform.rotation.eulerAngles.z + currentWeapon.AttackAngleLeft - 360
                                                   && currentAngle >= transform.rotation.eulerAngles.z - currentWeapon.AttackAngleLeft - 360)
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
