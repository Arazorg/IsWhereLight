using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public Animator animator;
    private BulletSpawner bulletSpawner;
    private float bulletSpeed;
    private float bulletScatterAngle;

    private GameObject character;

    void Start()
    {
        animator = GetComponent<Animator>();
        character = GameObject.Find("Character(Clone)");
    }

    public void SetBulletInfo(Bullet bullet)
    {
        var charInfo = character.GetComponent<CharInfo>();
        int weaponNumber = character.GetComponent<CharGun>().currentWeaponNumber;
        var weapon = transform.Find((charInfo.weapons[weaponNumber]));

        bulletSpawner = weapon.GetComponent<BulletSpawner>();
        bulletSpeed = bullet.Speed;
        bulletScatterAngle = bullet.Scatter;
    }

    public void Pulling()
    {
        animator.Play("BowStringing");
    }

    public void Shoot()
    {
        animator.Play("BowIdle");
        bulletSpawner.Spawn();
        Quaternion dir = Quaternion.AngleAxis(Random.Range(-bulletScatterAngle, bulletScatterAngle + 1), Vector3.forward);
        Rigidbody2D rb = bulletSpawner.currentWeaponBullet.GetComponent<Rigidbody2D>();
        rb.AddForce(dir * bulletSpawner.currentWeaponBullet.transform.up * bulletSpeed, ForceMode2D.Impulse);
    }
}
