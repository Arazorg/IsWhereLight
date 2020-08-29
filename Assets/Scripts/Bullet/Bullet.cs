using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explosionPrefab;
    private BulletData data;
    private SpriteRenderer bulletSprite;
    private bool isRemoveConstant;
    private bool isStartConstant;
    private float endSize;

    /// <summary>
    /// Initialization of bullet
    /// </summary>
    /// <param name="data"></param>
    public void Init(BulletData data)
    {
        this.data = data;
        GetComponent<SpriteRenderer>().sprite = data.MainSprite;
        if (Animators.Count != 0)
            GetComponent<Animator>().runtimeAnimatorController = Animators[0];
        bulletSprite = GetComponent<SpriteRenderer>();
    }

    public Sprite MainSprite
    {
        get
        {
            return data.MainSprite;
        }
        protected set { }
    }

    public List<RuntimeAnimatorController> Animators
    {
        get
        {
            return data.Animators;
        }
        protected set { }
    }

    public BulletData.BulletType TypeOfBullet
    {
        get
        {
            return data.TypeOfBullet;
        }
    }

    public int Damage { get; set; }
    public float CritChance { get; set; }
    public float Knoking { get; set; }

    public float Scatter
    {
        get
        {
            return data.Scatter;
        }
        set { }
    }

    /// <summary>
    /// Speed of current bullet
    /// </summary>
    public float Speed
    {
        get
        {
            return data.Speed;
        }
        protected set { }
    }

    /// <summary>
    /// Time of delete of current bullet
    /// </summary>
    public float DeleteTime
    {
        get
        {
            return data.DeleteTime;
        }
        set
        {
            DeleteTime = value;
        }
    }

    void Update()
    {
        if (gameObject.tag.Contains("StandartLaser"))
        {
            if (bulletSprite.size.x - 4f * Time.deltaTime > 0)
                bulletSprite.size -= new Vector2(4f * Time.deltaTime, 0);
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
                if (bulletSprite.size.x + 4f * Time.deltaTime < endSize)
                    bulletSprite.size += new Vector2(4f * Time.deltaTime, 0);
                else
                {
                    bulletSprite.size = new Vector2(endSize, bulletSprite.size.y);
                    isStartConstant = false;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag != "GunKeep"
                && collider.tag != "StandartBullet"
                    && collider.tag != "StandartArrow"
                        && collider.tag != "StandartLaser"
                            && collider.tag != "ConstantLaser"
                                && collider.tag != "EnemyBullet"
                                    && collider.tag != "IgnoreAll"
                                        && collider.tag != "NPC")
        {
            if (collider.tag == "Destroyable")
            {
                Destroy(collider.gameObject.transform.parent.gameObject);
                if (!gameObject.tag.Contains("Laser"))
                    Destroy(gameObject);
            }
            else if ((gameObject.tag == "StandartBullet" && collider.tag != "Player")
                        || (gameObject.tag == "EnemyBullet" && collider.tag != "Enemy"))
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
                Destroy(gameObject);
            }
            else if (gameObject.tag == "StandartArrow" && collider.tag != "Player")
            {
                var arrow = Instantiate(gameObject, transform.position, transform.rotation);
                arrow.transform.parent = collider.transform;
                arrow.tag = "IgnoreAll";
                Destroy(gameObject);
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
