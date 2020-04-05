using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explosionPrefab;
    private BulletData data;
    private CharInfo charInfo;
    private CharAction charAction;

    /// <summary>
    /// Initialization of bullet
    /// </summary>
    /// <param name="data"></param>
    public void Init(BulletData data)
    {
        GameObject character = GameObject.Find("Character(Clone)");
        charInfo = character.GetComponent<CharInfo>();
        charAction = character.GetComponent<CharAction>();

        this.data = data;
        damage = Damage;
        GetComponent<SpriteRenderer>().sprite = data.MainSprite;
    }

    /// <summary>
    /// Speed of current bullet
    /// </summary>
    private int damage;
    public int Damage
    {
        get
        {
            return data.Damage;
        }
        protected set { }
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
        if(gameObject.tag == "StandartBullet")
        {
            if (collider.tag == "Untagged" || collider.tag == "Enemy")
            {
                GameObject explosion = (GameObject)Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
                Destroy(gameObject);
            }
        }

        if (gameObject.tag == "EnemyBullet")
        {
            if (collider.tag == "Player")
            {
                charInfo.Damage(damage);
                charAction.isPlayerHitted = true;
                charAction.isEnterFirst = true;
                GameObject explosion = (GameObject)Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
                Destroy(gameObject);
            }
            else if(collider.tag == "Untagged")
            {
                GameObject explosion = (GameObject)Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
                Destroy(gameObject);
            }
        }
    }
}
