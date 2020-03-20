using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private BulletData data;

    /// <summary>
    /// Initialization of weapon
    /// </summary>
    /// <param name="data"></param>
    public void Init(BulletData data)
    {
        this.data = data;
        GetComponent<SpriteRenderer>().sprite = data.MainSprite;
    }

    /// <summary>
    /// Crit chance of current weapon
    /// </summary>
    private float speed;
    public float Speed
    {
        get
        {
            return data.Speed;
        }
        protected set { }
    }

    /// <summary>
    /// Manecost of current weapon
    /// </summary>
    private float scatter;
    public float Scatter
    {
        get
        {
            return data.Scatter;
        }
        protected set { }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(gameObject.tag == "StandartBullet")
        {
            if (collision.collider.tag == "Untagged" || collision.collider.tag == "Enemy")
            {
                Destroy(gameObject);
            }
        }   
    }
}
