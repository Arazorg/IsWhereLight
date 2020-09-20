using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

#pragma warning disable 0649
    [Tooltip("Префаб взрыва статического врага")]
    [SerializeField] private GameObject explosionPrefab;

    [Tooltip("Специфиикация врага")]
    [SerializeField] private EnemyData enemyData;
#pragma warning restore 0649

    public bool IsDeath
    {
        get { return isDeath; }
        set { isDeath = value; }
    }
    private bool isDeath = false;

    private EnemyData data;
    private bool isEnemyHitted = false;
    private bool isEnterFirst = true;
    private float timeToOff;
    public bool isKnoking;
    private float timeOfKnoking;

    /// <summary>
    /// Initialization of enemy
    /// </summary>
    /// <param name="data"></param>
    public void Init(EnemyData data)
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
        if (!data.EnemyName.Contains("Static") && !EnemyName.Contains("Punchbag"))
            GetComponent<Animator>().runtimeAnimatorController = MainAnimator;
        else if(!EnemyName.Contains("Punchbag"))
            gameObject.tag = "Destroyable";

        timeOfKnoking = float.MaxValue;
        if (!data.EnemyName.Contains("Target") && 
                !data.EnemyName.Contains("Static") &&
                    !data.EnemyName.Contains("Thing") &&
                        !data.EnemyName.Contains("Punchbag"))
            GetComponent<EnemyAI>().StartAI();
    }
    
    void Start()
    {
        if (enemyData != null)
            Init(enemyData);
    }

    /// <summary>
    /// Animator of current enemy
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
    /// Type of attack of current enemy
    /// </summary>
    public EnemyData.AttackType TypeOfAttack
    {
        get
        {
            return data.TypeOfAttack;
        }
        protected set { }
    }

    /// <summary>
    /// Attack of current enemy
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
    /// Attack range of current enemy
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
    /// Attack angle of current enemy
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
    /// BulletData of current enemy
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
            return data.EnemyName;
        }
        protected set { }
    }

    /// <summary>
    /// FireRate of current enemy
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

    private void Update()
    {
        if (Time.time > timeOfKnoking)
            isKnoking = false;
        EnemyHitted();
    }

    private void EnemyHitted()
    {
        if (!isDeath)
        {
            if (isEnemyHitted)
            {
                if (isEnterFirst)
                {
                    GetComponent<SpriteRenderer>().color = Color.red;
                    timeToOff = Time.time + 0.05f;
                    isEnterFirst = false;
                }
                else
                {
                    if (Time.time > timeToOff)
                    {
                        GetComponent<SpriteRenderer>().color = Color.white;
                        isEnemyHitted = false;
                        isEnterFirst = true;
                    }
                }
            }
        }
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
                if (transform.tag == "Destroyable")
                    DestroyStaticEnemy();
                else
                {
                    var bullet = coll.gameObject.GetComponent<Bullet>();
                    GetDamage(bullet.Damage, bullet.CritChance, bullet.transform, bullet.Knoking);
                }
            }
        }
    }

    public void GetDamage(int damage, float critChance, Transform objectTransform = null, float knoking = 0f)
    {
        if(!isDeath)
        {
            bool isCriticalHit = UnityEngine.Random.Range(0, 100) < critChance;
            if (isCriticalHit)
                damage *= 2;
            health -= damage;
            if (!EnemyName.Contains("Static") && !EnemyName.Contains("Punchbag"))
                Knoking(objectTransform.position, knoking);
            if(EnemyName.Contains("Punchbag"))
            {
                if (objectTransform.position.x > transform.position.x)
                {
                    GetComponent<Animator>().Play("DamageLeft");
                }
                    
                if (objectTransform.position.x < transform.position.x)
                    GetComponent<Animator>().Play("DamageRight");
            }
            if(!EnemyName.Contains("Thing"))
            {
                isEnemyHitted = true;
                PopupText.Create(transform.position, false, isCriticalHit, damage);
                if (health <= 0)
                {
                    if (EnemyName.Contains("Target"))
                        ShootingRange.instance.Spawn(true);
                    else
                    {
                        AudioManager.instance.Play($"EnemyDeath{UnityEngine.Random.Range(0, 2)}");
                        GetComponent<Animator>().Play("Death");
                        foreach (var collider2D in gameObject.GetComponents<BoxCollider2D>())
                            Destroy(collider2D);

                        isDeath = true;
                        GetComponent<Rigidbody2D>().simulated = false;

                        ColorUtility.TryParseHtmlString("#808080", out Color color);
                        gameObject.tag = "IgnoreAll";
                        gameObject.layer = 2;
                        GetComponent<SpriteRenderer>().color = color;
                        GetComponent<EnemyAI>().Character.GetComponent<CharInfo>().currentCountKilledEnemies++;
                    }

                }
            }
        }
    }

    private void Knoking(Vector3 objectPosition, float weaponKnoking)
    {
        if (!isDeath)
        {
            GetComponent<Rigidbody2D>().AddForce((transform.position - objectPosition).normalized * weaponKnoking);
            isKnoking = true;
            timeOfKnoking = Time.time + 1f;
        }
    }

    public void DestroyStaticEnemy()
    {
        AudioManager.instance.Play("BushDestroy");
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
        Destroy(gameObject);
    }

    void OnBecameVisible()
    {
        if (!isDeath && gameObject.tag != "Destroyable")
        {
            if (!data.EnemyName.Contains("Target") && !EnemyName.Contains("Thing"))
                gameObject.tag = "Enemy";
            else if(EnemyName.Contains("Thing"))
                gameObject.tag = "Thing";
        }
    }

    void OnBecameInvisible()
    {
        if (!isDeath && gameObject.tag != "Destroyable")
        {
            gameObject.tag = "Untagged";
        }
    }
}