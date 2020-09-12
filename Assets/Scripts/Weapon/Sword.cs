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
        var enemies = Physics2D.OverlapCircleAll(currentWeapon.transform.position, currentWeapon.Radius, 
                (1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("EnemyStatic")));
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
                        enemyScript.DestroyStaticEnemy();
                    else if (enemy.transform.tag == "Enemy")
                        enemyScript.GetDamage(currentWeapon.Damage, currentWeapon.CritChance, transform, currentWeapon.Knoking);
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
