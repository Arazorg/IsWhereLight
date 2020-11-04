using UnityEngine;

public class Boss : MonoBehaviour
{
    public bool IsDeath
    {
        get { return isDeath; }
        set { isDeath = value; }
    }
    private bool isDeath = false;
    public bool isPlayerInCollider;

    private BossData data;
    private HealthBar healthBar;

    /// <summary>
    /// Initialization of boss
    /// </summary>
    /// <param name="data"></param>
    public void Init(BossData data)
    {
        this.data = data;
        health = Health;
        healthBar = GameObject.Find("Canvas").transform.GetComponentInChildren<HealthBar>();
        healthBar.SetMaxMin(health, health, 0);
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
        isPlayerInCollider = false;
        GetComponent<SpriteRenderer>().sortingOrder = LayerOrder;
        gameObject.tag = "Untagged";
        GetComponent<Animator>().runtimeAnimatorController = MainAnimator;
        GetComponent<BossAI>().StartAI();
    }

    /// <summary>
    /// Animator of current boss
    /// </summary>
    /// <param name="data"></param>
    public RuntimeAnimatorController MainAnimator
    {
        get
        {
            return data.MainAnimator;
        }
        protected set { }
    }

    /// <summary>
    /// Layer order of current boss
    /// </summary>
    public int LayerOrder
    {
        get
        {
            return data.LayerOrder;
        }
        protected set { }
    }

    /// <summary>
    /// Attack of current boss
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
    /// Type of attack of current boss
    /// </summary>
    public BossData.BossType TypeOfBoss
    {
        get
        {
            return data.TypeOfBoss;
        }
        protected set { }
    }

    /// <summary>
    /// Attack of current boss
    /// </summary>
    public int Damage
    {
        get
        {
            return data.Damage;
        }
        protected set { }
    }

    /// <summary>
    /// Attack range of current boss
    /// </summary>
    public float AttackRange
    {
        get
        {
            return data.AttackRange;
        }
        protected set { }
    }

    /// <summary>
    /// Attack angle of current boss
    /// </summary>
    public float AttackAngle
    {
        get
        {
            return data.AttackAngle;
        }
        protected set { }
    }

    /// <summary>
    /// BulletData of current boss
    /// </summary>
    public BulletData DataOfBullet
    {
        get
        {
            return data.DataOfBullet;
        }
        protected set { }
    }

    /// <summary>
    /// Health of current boss
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
    /// Target of current boss
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
    /// Name of current boss
    /// </summary>
    public string EnemyName
    {
        get
        {
            return data.EnemyName;
        }
        protected set { }
    }

    /// <summary>
    /// FireRate of current boss
    /// </summary>
    public float FireRate
    {
        get
        {
            return data.FireRate;
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

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!isDeath)
        {
            if (coll.gameObject.tag == "StandartBullet"
                || coll.gameObject.tag == "StandartArrow"
                    || coll.gameObject.tag == "HomingArrow"
                        || coll.gameObject.tag == "StandartLaser")
            {
                var bullet = coll.gameObject.GetComponent<Bullet>();
                GetDamage(bullet.Damage, bullet.CritChance, bullet.transform, bullet.Knoking);
            }

            if (coll.gameObject.tag == "Player")
                isPlayerInCollider = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
            isPlayerInCollider = false;
    }

    public void GetDamage(int damage, float critChance, Transform objectTransform = null, float knoking = 0f)
    {
        if (!isDeath)
        {
            bool isCriticalHit = Random.Range(0, 100) < critChance;
            if (isCriticalHit)
                damage *= 2;
            health -= damage;
            healthBar.SetHealth(health);
            PopupText.Create(transform.position, false, isCriticalHit, damage);
            if (health <= 0)
            {
                health = 0;
                healthBar.GetComponent<MovementUI>().MoveToStart();
                Death();
            }

        }
    }

    private void Death()
    {
        AudioManager.instance.Play($"{EnemyName}Death");
        GetComponent<Animator>().Play("Death");
        isDeath = true;
        //GetComponent<Rigidbody2D>().simulated = false;

        ColorUtility.TryParseHtmlString("#808080", out Color color);
        gameObject.tag = "IgnoreAll";
        GetComponent<SpriteRenderer>().sortingOrder = 1;
        GetComponent<SpriteRenderer>().color = color;
        //CharInfo.instance.currentCountKilledEnemies++;
        EnemySpawner.instance.DeleteEnemy(gameObject);
    }

    void OnBecameVisible()
    {
        if (!isDeath)
            gameObject.tag = "Enemy";
    }

    void OnBecameInvisible()
    {
        gameObject.tag = "Untagged";
    }
}
