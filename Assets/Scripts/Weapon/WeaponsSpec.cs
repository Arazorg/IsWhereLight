using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsSpec : MonoBehaviour
{
    public struct Gun
    {
        public string name;
        public float speed;     //Bullet speed
        public float scatter;   //Bullet angle of scatter
        public float fireRate;  //Gun fire rate
        public float dmg;       //Gun damage
        public float crit;      //Gun critical chance
        public int mana;        //Gun's manecost
        public GameObject bulletPrefab;
    }

    public Dictionary<string, Gun> guns = new Dictionary<string, Gun>();

    void Awake()
    {
        guns.Add("0", new Gun()
        {
            name = "pistol",
            speed = 25f,
            scatter = 10f,
            fireRate = 0.5f,
            crit = 10f,
            dmg = 3f,
            mana = 2,
            bulletPrefab = GameObject.Find("GameHandler").transform.Find("Bullet").gameObject
        });

        guns.Add("1", new Gun()
        {
            name = "newPistol",
            speed = 40f,
            scatter = 3f,
            fireRate = 0.3f,
            crit = 15f,
            dmg = 5f,
            mana = 3,
            bulletPrefab = GameObject.Find("GameHandler").transform.Find("NewBullet").gameObject
        });
    }


}
