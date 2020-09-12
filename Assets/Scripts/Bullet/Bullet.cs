using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Префаб взрыва пули")]
    [SerializeField] private GameObject explosionPrefab;
#pragma warning restore 0649

    private Animator animator;
    private BulletData data;
    private SpriteRenderer bulletSprite;

    private bool isRemoveConstant;
    private bool isStartConstant;
    private float endSize;

    private readonly float speedOfLaserDisapperance = 4f; 

    /// <summary>
    /// Initialization of bullet
    /// </summary>
    /// <param name="data"></param>
    public void Init(BulletData data)
    {
        this.data = data;
        GetComponent<SpriteRenderer>().sprite = data.MainSprite;
        GetComponent<BoxCollider2D>().size = ColliderSize;
        GetComponent<BoxCollider2D>().offset = ColliderOffset;
        bulletSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        if (Animators.Count != 0)
            animator.runtimeAnimatorController = Animators[0];
        if (gameObject.tag.Contains("StandartGrenade") && gameObject != null)
        {
            Destroy(gameObject, DeleteTime);
        }
            
    }

    /// <summary>
    /// Sprite of current bullet
    /// </summary>
    public Sprite MainSprite
    {
        get { return data.MainSprite; }
        set { }
    }

    /// <summary>
    /// Collider size of current bullet
    /// </summary>
    public Vector2 ColliderSize
    {
        get { return data.ColliderSize; }
        set { }
    }

    /// <summary>
    /// Collider offset of current bullet
    /// </summary>
    public Vector2 ColliderOffset
    {
        get { return data.ColliderOffset; }
        set { }
    }

    /// <summary>
    /// Animators of current bullet
    /// </summary>
    public List<RuntimeAnimatorController> Animators
    {
        get { return data.Animators; }
        set { }
    }

    /// <summary>
    /// Type of current bullet
    /// </summary>
    public BulletData.BulletType TypeOfBullet
    {
        get { return data.TypeOfBullet; }
        set { }
    }

    /// <summary>
    /// Damage of current bullet
    /// </summary>
    public int Damage { get; set; }

    /// <summary>
    /// Critical chance of current bullet
    /// </summary>
    public float CritChance { get; set; }

    /// <summary>
    /// Knoking of current bullet
    /// </summary>
    public float Knoking { get; set; }

    /// <summary>
    /// Scatter of current bullet
    /// </summary>
    public float Scatter
    {
        get { return data.Scatter; }
        set { }
    }

    /// <summary>
    /// Speed of current bullet
    /// </summary>
    public float Speed
    {
        get { return data.Speed; }
        set { }
    }

    /// <summary>
    /// Time of delete of current bullet
    /// </summary>
    public float DeleteTime
    {
        get { return data.DeleteTime; }
        set { DeleteTime = value; }
    }

    void Update()
    {
        if (gameObject.tag.Contains("StandartLaser"))
        {
            if (bulletSprite.size.x - speedOfLaserDisapperance * Time.deltaTime > 0)
                bulletSprite.size -= new Vector2(speedOfLaserDisapperance * Time.deltaTime, 0);
            else
            {
                bulletSprite.size = new Vector2(0, bulletSprite.size.y);
                Destroy(gameObject);
            }
        }
        else if (gameObject.tag.Contains("Laser"))
        {
            if (isRemoveConstant)
            {
                isRemoveConstant = false;
                Destroy(gameObject);
            }
            if (isStartConstant)
            {
                if (bulletSprite.size.x + speedOfLaserDisapperance * Time.deltaTime < endSize)
                    bulletSprite.size += new Vector2(speedOfLaserDisapperance * Time.deltaTime, 0);
                else
                {
                    bulletSprite.size = new Vector2(endSize, bulletSprite.size.y);
                    isStartConstant = false;
                }
            }
        }
        else if (gameObject.tag.Contains("StandartGrenade") && animator.GetBool("Explosion") == false)
        {
            var speedOfGrenadeRotate = 10.5f;
            if (Time.timeScale != 0)
                transform.Rotate(0, 0, speedOfGrenadeRotate);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag != "GunKeep"
                && collider.tag != "StandartBullet"
                    && collider.tag != "StandartArrow"
                        && collider.tag != "HomingArrow"
                            && collider.tag != "StandartLaser"
                                && collider.tag != "ConstantLaser"
                                    && collider.tag != "EnemyBullet"
                                        && collider.tag != "IgnoreAll"
                                            && collider.tag != "NPC")
        {
            if (collider.tag == "Destroyable")
            {
                collider.GetComponent<Enemy>().DestroyStaticEnemy();
                if (!gameObject.tag.Contains("Laser") && !gameObject.tag.Contains("StandartGrenade"))
                    Destroy(gameObject);
                else if (gameObject.tag.Contains("StandartGrenade"))
                {
                    animator.SetBool("Explosion", true);
                    AudioManager.instance.Play("StandartGrenade");
                    CameraShaker.Instance.ShakeOnce(2f, 2f, .1f, 0.5f);
                    AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
                    float destroyTime = DeleteTime;
                    foreach (AnimationClip clip in clips)
                    {
                        switch (clip.name)
                        {
                            case "Explosion":
                                destroyTime = clip.length;
                                break;
                        }
                    }
                    Destroy(gameObject, destroyTime);
                }
            }
            else if (((gameObject.tag == "StandartBullet" || gameObject.tag == "HomingArrow") && collider.tag != "Player")
                        || (gameObject.tag == "EnemyBullet" && collider.tag != "Enemy"))
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
                Destroy(gameObject);
            }
            else if (gameObject.tag == "StandartArrow" && collider.tag != "Player")
            {
                if (!collider.isTrigger)
                {
                    gameObject.GetComponent<Rigidbody2D>().simulated = false;
                    var arrow = Instantiate(gameObject, transform.position, transform.rotation);
                    arrow.transform.parent = collider.transform;
                    arrow.tag = "IgnoreAll";
                    Destroy(gameObject);
                }
            }
            else if (((gameObject.tag == "StandartGrenade" && collider.tag != "Player" && collider.tag != "StandartGrenade")
                        || (gameObject.tag == "EnemyGrenade" && collider.tag != "Enemy" && collider.tag != "EnemyGrenade")))
            {
                animator.SetBool("Explosion", true);
                AudioManager.instance.Play("StandartGrenade");
                CameraShaker.Instance.ShakeOnce(2f, 2f, .1f, 0.5f);
                AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
                float destroyTime = DeleteTime;
                foreach (AnimationClip clip in clips)
                {
                    switch (clip.name)
                    {
                        case "Explosion":
                            destroyTime = clip.length;
                            break;
                    }
                }
                if (gameObject != null)
                {
                    var enemies = Physics2D.OverlapCircleAll(transform.position, 1f,
                        (1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("EnemyStatic")));
                    foreach (var enemy in enemies)
                    {
                        var enemyScript = enemy.GetComponent<Enemy>();
                        if (enemy.transform.tag == "Enemy")
                            enemyScript.GetDamage(Damage, CritChance, transform, 500f);
                    }
                    Destroy(gameObject, destroyTime);
                }
            }
        }
    }

    public void RemoveConstant()
    {
        isRemoveConstant = true;
    }

    public void StartConstant()
    {
        endSize = bulletSprite.size.x;
        bulletSprite.size = new Vector2(0, bulletSprite.size.y);
        isStartConstant = true;
    }
}
