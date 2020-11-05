using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Player's layer")]
    [SerializeField] private LayerMask playerLayers;
#pragma warning restore 0649
    public bool IsAttack
    {
        get { return isAttack; }
    }
    private bool isAttack;

    private CharInfo charInfo;
    private Animator animator;

    private int damage;
    private float attackRange;
    private float attackRate;
    private float attackAngle;
    private float timeToAttack;

    void Start()
    {
        animator = GetComponent<Animator>();
        charInfo = GameObject.Find("CharInfoHandler").GetComponent<CharInfo>();
        timeToAttack = Time.time;

        var enemy = GetComponent<Enemy>();
        damage = enemy.Damage;
        attackRange = enemy.AttackRange;
        attackRate = enemy.FireRate;
        attackAngle = enemy.AttackAngle;
    }

    private void Update()
    {
        if (Time.time > timeToAttack && !GetComponent<Enemy>().IsDeath)
        {
            isAttack = true;
            Attack();
        }      
    }
    
    public void DestroyObstacle()
    {
        var obstacles = Physics2D.OverlapCircleAll(transform.position, attackRange, 1 << LayerMask.NameToLayer("EnemyStatic"));
        animator.speed = 3f;
        animator.Play("Attack");
        animator.speed = 1f;
        foreach (var obstacle in obstacles)
        {
            obstacle.GetComponent<Enemy>().DestroyStaticEnemy();
        } 
    }

    public void Attack()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayers);
        foreach (Collider2D target in targets)
        {
            timeToAttack = Time.time + attackRate;
            animator.Play("Attack");
            AudioManager.instance.Play("EnemyMeleeAttack");
            charInfo.Damage(damage);
            
            var currentAngle = -Mathf.Atan2(target.transform.position.x - transform.position.x,
                                   target.transform.position.y - transform.position.y) * Mathf.Rad2Deg;
            if (currentAngle > 0)
            {
                if (currentAngle <= transform.rotation.eulerAngles.z + attackAngle
                                                   && currentAngle >= transform.rotation.eulerAngles.z - attackAngle)
                {
                    if (target.transform.tag == "Player")
                        charInfo.Damage(damage);
                    else if (target.transform.tag == "Ally")
                        target.GetComponent<Ally>().Damage(damage, 15);
                }
                    
            }
            else
            {
                if (currentAngle <= transform.rotation.eulerAngles.z + attackAngle - 360
                                                      && currentAngle >= transform.rotation.eulerAngles.z - attackAngle - 360)
                    if (target.transform.tag == "Player")
                        charInfo.Damage(damage);
                    else if (target.transform.tag == "Ally")
                        target.GetComponent<Ally>().Damage(damage, 15);
            }
            isAttack = false;
        }
    }
}
