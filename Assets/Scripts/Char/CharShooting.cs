using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharShooting : MonoBehaviour
{
    BulletSpawner bulletSpawner;

    //Values
    private float bulletSpeed;
    private float bulletScatterAngle;

    public void Shoot()
    {
        bulletSpawner.Spawn();
        Rigidbody2D rb = bulletSpawner.currentWeaponBullet.GetComponent<Rigidbody2D>();
        rb.AddForce(bulletSpawner.currentWeaponBullet.transform.up * bulletSpeed, ForceMode2D.Impulse);
    }

    public void SetBulletInfo(Bullet bullet)
    {
        bulletSpawner = transform.GetChild(0).GetComponent<BulletSpawner>();
        bulletSpeed = bullet.Speed;
        bulletScatterAngle = bullet.Scatter;
    }
}

