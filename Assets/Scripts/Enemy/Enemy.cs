﻿using System.Collections;
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

    private static float timeToBushDestroySound;
    private float timeToOff;
    private bool isEnemyHitted;
    private bool isEnterFirst;
    public bool isKnoking;
    public bool isPlayerInCollider;

    /// <summary>
    /// Initialization of enemy
    /// </summary>
    /// <param name="data"></param>
    public void Init(EnemyData data)
    {
        this.data = data;
        health = data.Health;
        speed = data.Speed;
        damage = data.Damage;
        fireRate = data.FireRate;
        attackRange = data.AttackRange;

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
        else if (!EnemyName.Contains("Punchbag"))
            gameObject.tag = "Destroyable";

        isEnemyHitted = false;
        isEnterFirst = true;
        isPlayerInCollider = false;

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
        get { return data.MainAnimator; }
        set { }
    }
    public int LayerOrder
    {
        get { return data.LayerOrder; }
        set { }
    }

    /// <summary>
    /// Type of attack of current enemy
    /// </summary>
    public EnemyData.AttackType TypeOfAttack
    {
        get { return data.TypeOfAttack; }
        set { }
    }

    /// <summary>
    /// Health of current enemy
    /// </summary>
    private int health;
    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    /// <summary>
    /// Attack of current enemy
    /// </summary>
    private int speed;
    public int Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    /// <summary>
    /// Attack of current enemy
    /// </summary>
    private int damage;
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    /// <summary>
    /// FireRate of current enemy
    /// </summary>
    private float fireRate;
    public float FireRate
    {
        get { return fireRate; }
        set { fireRate = value; }
    }

    /// <summary>
    /// Attack range of current enemy
    /// </summary>
    private float attackRange;
    public float AttackRange
    {
        get { return attackRange; }
        set { attackRange = value; }
    }


    /// <summary>
    /// Attack angle of current enemy
    /// </summary>
    public float AttackAngle
    {
        get { return data.AttackAngle; }
        set { }
    }

    /// <summary>
    /// BulletData of current enemy
    /// </summary>
    private BulletData dataOfBullet;
    public BulletData DataOfBullet
    {
        get { return data.DataOfBullet; }
        set { }
    }


    /// <summary>
    /// Target of current enemy
    /// </summary>
    public string Target
    {
        get { return data.Target; }
        set { }
    }

    /// <summary>
    /// Name of current enemy
    /// </summary>
    public string EnemyName
    {
        get { return data.EnemyName; }
        set { }
    }

    public Vector2 ActionColliderSize
    {
        get { return data.ActionColliderSize; }
        set { }
    }
    public Vector2 ActionColliderOffset
    {
        get { return data.ActionColliderOffset; }
        set { }
    }

    public Vector2 ColliderSize
    {
        get { return data.СolliderSize; }
        set { }
    }

    public Vector2 ColliderOffset
    {
        get
        {
            return data.ColliderOffset;
        }
        set { }
    }

    private void Update()
    {
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
            if (!EnemyName.Contains("Static") && !EnemyName.Contains("Punchbag"))
                Knoking();
            if (EnemyName.Contains("Punchbag"))
            {
                AudioManager.instance.Play("PunchbagDamage");
                if (objectTransform.position.x > transform.position.x)
                    GetComponent<Animator>().Play("DamageLeft");

                if (objectTransform.position.x < transform.position.x)
                    GetComponent<Animator>().Play("DamageRight");
            }

            if (!EnemyName.Contains("Thing"))
            {
                isEnemyHitted = true;
                PopupText.Create(transform.position, false, isCriticalHit, damage);
                if (health <= 0)
                    Death();
            }
        }
    }

    private void Death()
    {
        if (EnemyName.Contains("Target"))
            ShootingRange.instance.Spawn(true);
        else
        {
            AudioManager.instance.Play($"EnemyDeath{Random.Range(0, 2)}");
            GetComponent<Animator>().Play("Death");
            foreach (var collider2D in gameObject.GetComponents<BoxCollider2D>())
                Destroy(collider2D);

            isDeath = true;
            GetComponent<EnemyAI>().StopAI();
            GetComponent<Rigidbody2D>().simulated = false;

            ColorUtility.TryParseHtmlString("#808080", out Color color);
            gameObject.tag = "IgnoreAll";
            GetComponent<SpriteRenderer>().sortingOrder = 1;
            GetComponent<SpriteRenderer>().color = color;
            CharInfo.instance.currentCountKilledEnemies++;
            EnemySpawner.instance.DeleteEnemy(gameObject);
        }
    }

    private void Knoking()
    {
        if (!isDeath) { }
    }

    private bool isShaking = false;
    public IEnumerator ShakeGameObjectCOR(GameObject objectToShake, float totalShakeDuration, float decreasePoint)
    {
        if (decreasePoint >= totalShakeDuration)
        {
            Debug.LogError("decreasePoint must be less than totalShakeDuration...Exiting");
            yield break;
        }

        Transform objTransform = objectToShake.transform;
        Vector3 defaultPos = objTransform.position;
        Quaternion defaultRot = objTransform.rotation;

        float counter = 0f;
        const float speed = 0.175f;
        const float angleRot = 1.5f;

        while (counter < totalShakeDuration)
        {
            counter += Time.deltaTime;
            float decreaseSpeed = speed;

            Vector3 tempPosition = defaultPos + Random.insideUnitSphere * decreaseSpeed;
            tempPosition.z = defaultPos.z;

            objTransform.position = tempPosition;
            objTransform.rotation = defaultRot * Quaternion.AngleAxis(Random.Range(-angleRot, angleRot), new Vector3(0f, 0f, 1f));
            yield return null;

            if (counter >= decreasePoint)
            {
                counter = 0f;
                while (counter <= decreasePoint)
                {
                    counter += Time.deltaTime;
                    decreaseSpeed = Mathf.Lerp(speed, 0, counter / decreasePoint);
                    float decreaseAngle = Mathf.Lerp(angleRot, 0, counter / decreasePoint);

                    Vector3 tempPos = defaultPos + Random.insideUnitSphere * decreaseSpeed;
                    tempPos.z = defaultPos.z;
                    objTransform.position = tempPos;
                    objTransform.rotation = defaultRot * Quaternion.AngleAxis(Random.Range(-decreaseAngle, decreaseAngle), new Vector3(0f, 0f, 1f));

                    yield return null;
                }
                break;
            }
        }
        objTransform.position = defaultPos;
        objTransform.rotation = defaultRot;
        isShaking = false;
    }


    public void ShakeGameObject(GameObject objectToShake, float shakeDuration, float decreasePoint)
    {
        if (isShaking)
            return;
        isShaking = true;
        StartCoroutine(ShakeGameObjectCOR(objectToShake, shakeDuration, decreasePoint));
    }

    public void DestroyStaticEnemy()
    {
        var bushDestroySoundTime = 0.1f;
        if (Time.time > timeToBushDestroySound)
        {
            AudioManager.instance.Play("BushDestroy");
            timeToBushDestroySound = Time.time + bushDestroySoundTime;
        }
        Destroy(Instantiate(explosionPrefab, transform.position, Quaternion.identity),
                     explosionPrefab.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
        Destroy(gameObject);
    }

    void OnBecameVisible()
    {
        if (!isDeath && gameObject.tag != "Destroyable")
        {
            if (!data.EnemyName.Contains("Target") && !EnemyName.Contains("Thing"))
                gameObject.tag = "Enemy";
            else if (EnemyName.Contains("Thing"))
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