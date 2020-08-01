using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explosionPrefab;
    private BulletData data;

    /// <summary>
    /// Initialization of bullet
    /// </summary>
    /// <param name="data"></param>
    public void Init(BulletData data)
    {
        this.data = data;
        GetComponent<SpriteRenderer>().sprite = data.MainSprite;
    }

    public Sprite MainSprite
    {
        get
        {
            return data.MainSprite;
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
    /// Scatter of current bullet
    /// </summary>
    public float Scatter
    {
        get
        {
            return data.Scatter;
        }
        protected set { }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag != "Player" 
            && collider.tag != "GunKeep" 
                && collider.tag != "StandartBullet" 
                    && collider.tag != "StandartArrow"
                        && collider.tag != "EnemyBullet"
                            && collider.tag != "IgnoreAll"
                                && collider.tag != "NPC")
        {
            if (collider.tag == "Destroyable")
            {
                Destroy(collider.gameObject.transform.parent.gameObject);
                Destroy(gameObject);
            }
            else if (gameObject.tag == "StandartBullet")
            {
                GameObject explosion = (GameObject)Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
                Destroy(gameObject);
            }
            else if(gameObject.tag == "StandartArrow")
            {
                var arrow = Instantiate(gameObject, transform.position, transform.rotation);
                arrow.tag = "IgnoreAll";
                Destroy(gameObject);
                Destroy(arrow, 10);
            } 
        }
    }
}
