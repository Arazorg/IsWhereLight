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
        Quaternion dir = Quaternion.AngleAxis(Random.Range(-bulletScatterAngle, bulletScatterAngle+1), Vector3.forward);
        Rigidbody2D rb = bulletSpawner.currentWeaponBullet.GetComponent<Rigidbody2D>();
        rb.AddForce(dir * bulletSpawner.currentWeaponBullet.transform.up * bulletSpeed, ForceMode2D.Impulse);
    }


    public void SetBulletInfo(Bullet bullet)
    {
        var charInfo = GetComponent<CharInfo>();
        int weaponNumber = GetComponent<CharGun>().currentWeaponNumber;
        var weapon = transform.Find((charInfo.weapons[weaponNumber]));

        bulletSpawner = weapon.GetComponent<BulletSpawner>();
        bulletSpeed = bullet.Speed;
        bulletScatterAngle = bullet.Scatter;
    }


}

