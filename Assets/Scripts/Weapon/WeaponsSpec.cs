using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsSpec : MonoBehaviour
{
    public struct Gun
    {
        public float speed;     //Bullet speed
        public float scatter;   //Bullet angle of scatter
        public float fireRate;  //Gun fire rate
        public float dmg;       //Gun damage
        public float crit;      //Gun critical chance
        public int mana;        //Gun's manecost
        public GameObject gunPrefab;
        public GameObject bulletPrefab;
    }

    public Dictionary<string, Gun> guns = new Dictionary<string, Gun>();

    void Awake()
    {
        guns.Add("Staff", new Gun()
        {
            speed = 25f,
            scatter = 10f,
            fireRate = 0.5f,
            crit = 10f,
            dmg = 3f,
            mana = 2,
            gunPrefab = Resources.Load("Prefabs/Guns/Staff") as GameObject,
            bulletPrefab = Resources.Load<GameObject>("Prefabs/Guns/Bullets/Bullet")
            
        });

        guns.Add("NewStaff", new Gun()
        {
            speed = 40f,
            scatter = 3f,
            fireRate = 0.3f,
            crit = 15f,
            dmg = 5f,
            mana = 3,
            gunPrefab = Resources.Load("Prefabs/Guns/NewStaff") as GameObject,
            bulletPrefab = Resources.Load<GameObject>("Prefabs/Guns/Bullets/NewBullet")
        });
    }


}
