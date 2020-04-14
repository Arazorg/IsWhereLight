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
        //bulletSpawner.currentWeaponBullet.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, bulletScatterAngle));
        Quaternion dir = Quaternion.AngleAxis(Random.Range(-bulletScatterAngle, bulletScatterAngle+1), Vector3.forward);
        Rigidbody2D rb = bulletSpawner.currentWeaponBullet.GetComponent<Rigidbody2D>();
        rb.AddForce(dir * bulletSpawner.currentWeaponBullet.transform.up * bulletSpeed, ForceMode2D.Impulse);
    }


    public void SetBulletInfo(Bullet bullet)
    {
        bulletSpawner = transform.Find(GetComponent<CharInfo>().weapon).GetComponent<BulletSpawner>();
        bulletSpeed = bullet.Speed;
        bulletScatterAngle = bullet.Scatter;
    }


}

