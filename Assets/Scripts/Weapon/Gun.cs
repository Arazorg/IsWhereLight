using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private BulletSpawner bulletSpawner;
    private float bulletSpeed;
    private float bulletScatterAngle;

    private GameObject character;

    public void SetBulletInfo(Bullet bullet)
    {
        character = GameObject.Find("Character(Clone)");

        var charInfo = character.GetComponent<CharInfo>();
        int weaponNumber = character.GetComponent<CharGun>().currentWeaponNumber;

        bulletSpawner = GetComponent<BulletSpawner>();
        bulletSpeed = bullet.Speed;
        bulletScatterAngle = bullet.Scatter;
    }

    public void Shoot()
    {
        bulletSpawner.Spawn();
        Quaternion dir = Quaternion.AngleAxis(Random.Range(-bulletScatterAngle, bulletScatterAngle + 1), Vector3.forward);
        Rigidbody2D rb = bulletSpawner.currentWeaponBullet.GetComponent<Rigidbody2D>();
        rb.AddForce(dir * bulletSpawner.currentWeaponBullet.transform.up * bulletSpeed, ForceMode2D.Impulse);
    }
}
