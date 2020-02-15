using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharShooting : MonoBehaviour
{
    //Gameobjects
    public Transform firePoint;
    public GameObject bulletPrefab;

    //Values
    public float bulletSpeed;
    public float bulletScatterAngle;

    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletSpeed, ForceMode2D.Impulse);
    }
}

