using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
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


    /// <summary>
    /// Speed of current bullet
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
    /// Scatter of current bullet
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

    void OnTriggerEnter2D(Collider2D collider)
    {
        
        if(gameObject.tag == "StandartBullet")
        {
            if (collider.tag == "Untagged" || collider.tag == "Enemy")
            {
                Destroy(gameObject);
            }
        }   
    }
}
