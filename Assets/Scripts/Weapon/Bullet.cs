using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Values
    private WeaponsSpec.Gun gun;
    public float dmg;
    public float crit;

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
