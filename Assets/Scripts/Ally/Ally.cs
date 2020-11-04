using UnityEngine;

public class Ally : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Спецификация союзника")]
    [SerializeField] public AllyData data;
#pragma warning restore 0649

    public bool IsDeath
    {
        get { return isDeath; }
        set { isDeath = value; }
    }
    private bool isDeath = false;

    public string CurrentTag
    {
        get { return currentTag; }
        set { currentTag = value; }
    }
    private string currentTag;

    private GameObject closestEnemy = null;
    private Transform weapon;
    private Animator animator;

    private float gunAngle;
    private float timeToShoot;
    private float shootTime;
    private bool m_FacingRight;

    public RuntimeAnimatorController MainAnimator
    {
        get
        {
            return data.MainAnimator;
        }
        protected set { }
    }
    public int LayerOrder
    {
        get
        {
            return data.LayerOrder;
        }
        protected set { }
    }

    /// <summary>
    /// Attack of current enemy
    /// </summary>
    public int Speed
    {
        get
        {
            return data.Speed;
        }
        protected set { }
    }

    /// <summary>
    /// Health of current enemy
    /// </summary>
    private int health;
    public int Health
    {
        get
        {
            return data.Health;
        }
        protected set { }
    }

    /// <summary>
    /// Target of current enemy
    /// </summary>
    public string Target
    {
        get
        {
            return data.Target;
        }
        protected set { }
    }

    /// <summary>
    /// Name of current enemy
    /// </summary>
    public string EnemyName
    {
        get
        {
            return data.AllyName;
        }
        protected set { }
    }

    public Vector2 ActionColliderSize
    {
        get
        {
            return data.ActionColliderSize;
        }
        protected set { }
    }
    public Vector2 ActionColliderOffset
    {
        get
        {
            return data.ActionColliderOffset;
        }
        protected set { }
    }

    public Vector2 ColliderSize
    {
        get
        {
            return data.СolliderSize;
        }
        protected set { }
    }

    public Vector2 ColliderOffset
    {
        get
        {
            return data.ColliderOffset;
        }
        protected set { }
    }

    public string AllyWeaponName
    {
        get
        {
            return data.AllyWeaponName;
        }
        protected set { }
    }

    public void Init(AllyData data)
    {
        this.data = data;
        health = Health;

        foreach (var collider in GetComponents<BoxCollider2D>())
        {
            if (collider.isTrigger)
            {
                collider.offset = ActionColliderOffset;
                collider.size = ActionColliderSize;
            }
            else
            {
                collider.offset = ColliderOffset;
                collider.size = ColliderSize;
            }
        }

        GetComponent<SpriteRenderer>().sortingOrder = LayerOrder;
        gameObject.tag = "Untagged";
        GetComponent<Animator>().runtimeAnimatorController = MainAnimator;
    }

    void Start()
    {
        if (data != null)
            Init(data);

        m_FacingRight = true;
        timeToShoot = float.MinValue;

        WeaponSpawner.instance.SetPrefab(AllyWeaponName);
        WeaponSpawner.instance.Spawn(AllyWeaponName, transform, true);

        weapon = transform.GetChild(0);
        animator = GetComponent<Animator>();
        weapon.localPosition = weapon.GetComponent<Weapon>().FirePointPosition;
        shootTime = weapon.GetComponent<Weapon>().FireRate;
    }

    void Update()
    {
        if (!isDeath)
        {
            if (RotateGunToEnemy() && closestEnemy != null)
            {
                if (closestEnemy.transform.position.x - transform.position.x > 0 && !m_FacingRight)
                    Flip();
                else if (closestEnemy.transform.position.x - transform.position.x < 0 && m_FacingRight)
                    Flip();
                if (Time.time > timeToShoot)
                {
                    var weaponScript = weapon.GetComponent<Weapon>();
                    CameraShaker.instance.ShakeOnce(weaponScript.ShakeParametrs.magnitude,
                                                        weaponScript.ShakeParametrs.roughness,
                                                             weaponScript.ShakeParametrs.fadeInTime,
                                                                 weaponScript.ShakeParametrs.fadeOutTime);
                    switch (weapon.GetComponent<Weapon>().TypeOfAttack)
                    {
                        case WeaponData.AttackType.Gun:
                            AudioManager.instance.Play(weaponScript.WeaponName);
                            weapon.GetComponent<Gun>().Shoot();
                            break;
                        case WeaponData.AttackType.Sword:
                            AudioManager.instance.Play(weaponScript.WeaponName);
                            weapon.GetComponent<Sword>().Hit();
                            break;
                        case WeaponData.AttackType.Laser:
                            AudioManager.instance.Play(weaponScript.WeaponName);
                            weapon.GetComponent<Laser>().Shoot();
                            break;
                        default:
                            break;
                    }
                    timeToShoot = Time.time + shootTime;
                }
            }
            else
            {
                var weaponScript = weapon.GetComponent<Weapon>();
                switch (weapon.GetComponent<Weapon>().TypeOfAttack)
                {
                    case WeaponData.AttackType.Gun:
                        weapon.GetComponent<Gun>().StopShoot();
                        break;
                    case WeaponData.AttackType.Sword:
                        weapon.GetComponent<Sword>().StopShoot();
                        break;
                    case WeaponData.AttackType.Laser:
                        weapon.GetComponent<Laser>().StopShoot();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void Death()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        float deathTime = 0;
        Destroy(transform.GetChild(0).gameObject);
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "Death":
                    deathTime = clip.length;
                    break;
            }
        }
        animator.SetBool("isDeath", true);
        isDeath = true;
        Destroy(gameObject, deathTime);
    }

    private bool RotateGunToEnemy(string tag = "Enemy")
    {
        var enemies = GameObject.FindGameObjectsWithTag(tag);
        if (enemies.Length != 0)
        {
            closestEnemy = null;
            float distanceToEnemy = Mathf.Infinity;
            float minDistanceToEnemy = 1f;
            foreach (var enemy in enemies)
            {
                Vector3 direction = enemy.transform.position - transform.position;
                float curDistance = direction.sqrMagnitude;
                if (curDistance < distanceToEnemy && curDistance > minDistanceToEnemy)
                {
                    closestEnemy = enemy;
                    distanceToEnemy = curDistance;
                }
            }

            weapon = transform.GetChild(0);
            Vector3 closeDirection = (closestEnemy.transform.position - transform.position);
            LayerMask layerMask
                = ~(1 << LayerMask.NameToLayer("Ally") |
                        1 << LayerMask.NameToLayer("Ignore Raycast") |
                            1 << LayerMask.NameToLayer("Player") |
                                1 << LayerMask.NameToLayer("Room") |
                                    1 << LayerMask.NameToLayer("SpawnPoint"));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, closeDirection, Mathf.Infinity, layerMask);

            if (hit.collider != null)
            {
                if (hit.collider.tag == tag)
                {
                    gunAngle = -Mathf.Atan2(closestEnemy.transform.position.x - transform.position.x,
                                            closestEnemy.transform.position.y - transform.position.y)
                                                * Mathf.Rad2Deg;
                    weapon.rotation = Quaternion.Euler(new Vector3(0, 0, gunAngle));

                    return true;
                }
            }
        }
        return false;
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnBecameVisible()
    {
        if (!isDeath)
            gameObject.tag = "Ally";
    }

    void OnBecameInvisible()
    {
        gameObject.tag = "Untagged";
    }
}
