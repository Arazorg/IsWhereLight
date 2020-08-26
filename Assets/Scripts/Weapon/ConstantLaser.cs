using UnityEngine;

public class ConstantLaser : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Элемент в конце и начале лазера")]
    [SerializeField] private GameObject startEndElement;
#pragma warning restore 0649

    public Animator animator;
    private bool isAttack;
    private bool isStart;

    public bool IsAttack
    {
        get
        {
            return isAttack; 
        }
        set {
            isAttack = value;
            if (GetComponentInParent<CharController>().closestEnemy != null)
            {
                bulletSpawner.Spawn(transform);
                bullet = bulletSpawner.CurrentWeaponBullet;
                bullet.tag = "ConstantLaser";
                isStart = true;
            }
            else
                isAttack = false;
        }
    }

    private BulletSpawner bulletSpawner;
    private GameObject bullet;
    private GameObject startElement;
    private GameObject endElement;
    private float bulletScatterAngle;

    void Start()
    {
        animator = GetComponent<Animator>();
        transform.GetChild(0).localPosition = GetComponent<Weapon>().firePointPosition;
    }

    public void SetBulletInfo(Bullet bullet)
    {
        bulletSpawner = GetComponent<BulletSpawner>();
        bulletScatterAngle = bullet.Scatter;
    }

    void Update()
    {
        if(isAttack)
            Shoot();
    }

    public void Shoot()
    {
        animator.SetBool("Attack", true);

        if (gameObject.GetComponent<Weapon>().TypeOfAttack == WeaponData.AttackType.ConstantLaser &&
                GetComponentInParent<CharController>().closestEnemy != null)
        {
            LayerMask layerMask
                = ~(1 << LayerMask.NameToLayer("Player") |
                        1 << LayerMask.NameToLayer("Ignore Raycast") |
                             1 << LayerMask.NameToLayer("Room"));

           // RaycastHit2D hit = Physics2D.Raycast(transform.GetChild(0).position, transform.up, Mathf.Infinity, layerMask);
            Vector3 enemyTransform = GetComponentInParent<CharController>().closestEnemy.transform.position;
            var laserScale = new Vector3(0.8f, (enemyTransform - transform.GetChild(0).position).magnitude);

            if(isStart)
            {
                bullet.GetComponent<Bullet>().StartConstant();
                isStart = false;
            }
            bullet.GetComponent<SpriteRenderer>().size = laserScale;
            bullet.GetComponent<BoxCollider2D>().size = laserScale + new Vector3(0, 0.33f);
            bullet.transform.position
            = new Vector3((enemyTransform.x + transform.GetChild(0).position.x) / 2,
                            (enemyTransform.y + transform.GetChild(0).position.y) / 2);

            bullet.transform.rotation = transform.rotation;
        }
    }

    public void StopShoot()
    {
        isAttack = false;
        bullet.GetComponent<Bullet>().RemoveConstant();
    }
}
