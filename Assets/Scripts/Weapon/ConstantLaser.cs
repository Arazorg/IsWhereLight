using UnityEngine;

public class ConstantLaser : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Элемент в конце и начале лазера")]
    [SerializeField] private GameObject startEndElement;
#pragma warning restore 0649

    public Animator animator;

    private bool isStart;
    private bool isAttack;
    public bool IsAttack
    {
        get
        {
            return isAttack; 
        }
        set {
            isAttack = value;
            timeToDamage = Time.time + GetComponent<Weapon>().FireRate;
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
    private float timeToDamage;

    void Start()
    {
        if(!isAttack)
            timeToDamage = float.MaxValue;
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
        if(isAttack && !CharAction.isDeath)
        {
            Shoot();
            if(Time.time > timeToDamage)
            {
                //вынести наружу перменную врага и перменные оружия получать раз, компомент, изменить цвета для союзаных построек, добавить лечение и восстанволение мпанв текст
                timeToDamage = Time.time + GetComponent<Weapon>().FireRate;
                GetComponentInParent<CharController>().closestEnemy.
                    GetComponent<Enemy>().GetDamage(GetComponent<Weapon>().Damage, 
                                                        GetComponent<Weapon>().CritChance, 
                                                            GetComponent<Weapon>().Knoking);
            }
            
        }
            
    }

    public void Shoot()
    {
        Vector3 enemyTransform = Vector3.zero;
        animator.SetBool("Attack", true);
        try
        {
            enemyTransform = GetComponentInParent<CharController>().closestEnemy.transform.position;
        }
        catch
        {
            if (bullet != null)
            {
                bullet.GetComponent<Bullet>().RemoveConstant();
                bullet = null;
            }
        }

        if (GetComponentInParent<CharController>().closestEnemy != null)
        {
            if(bullet == null)
            {
                bulletSpawner.Spawn(transform);
                bullet = bulletSpawner.CurrentWeaponBullet;
                bullet.tag = "ConstantLaser";
                isStart = true;
            }

            var laserScale = new Vector3(0.8f, (enemyTransform - transform.GetChild(0).position).magnitude);
            if (isStart)
            {
                bullet.GetComponent<Bullet>().StartConstant();
                isStart = false;
            }
            bullet.GetComponent<SpriteRenderer>().size = laserScale;
            bullet.transform.position
            = new Vector3((enemyTransform.x + transform.GetChild(0).position.x) / 2,
                            (enemyTransform.y + transform.GetChild(0).position.y) / 2);

            bullet.transform.rotation = transform.rotation;
        }
        else
        {
            if (bullet != null)
            {
                bullet.GetComponent<Bullet>().RemoveConstant();
                bullet = null;
            }
        }

    }
    public void StopShoot()
    {
        timeToDamage = float.MaxValue;
        isAttack = false;
        if (bullet != null)
        {
            bullet.GetComponent<Bullet>().RemoveConstant();
            bullet = null;
        }
    }
}
