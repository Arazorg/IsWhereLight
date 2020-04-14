using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    //public static BulletSpawner instance;

    CharShooting charShooting;

    [Tooltip("Ссылка на базовый префаб пули")]
    [SerializeField] private GameObject bulletPrefab;

    [Tooltip("Место спауна пули")]
    [SerializeField] private Transform spawnPosition;


    public GameObject currentWeaponBullet;
    public Bullet currentBulletScript;

    private BulletData bulletData;

    public void Spawn()
    {
        currentWeaponBullet = Instantiate(bulletPrefab, spawnPosition.position, spawnPosition.rotation);
        currentWeaponBullet.transform.tag = "StandartBullet";
        currentBulletScript = currentWeaponBullet.GetComponent<Bullet>();
        currentBulletScript.Init(bulletData);
        currentWeaponBullet.SetActive(true);
        Destroy(currentWeaponBullet, 5);
    }

    public void SetBullet(BulletData _bulletData)
    {
        spawnPosition = transform.GetChild(0);
        bulletData = _bulletData;

        if (charShooting == null)
            charShooting = GameObject.Find("Character(Clone)").GetComponent<CharShooting>();

        currentWeaponBullet = Instantiate(bulletPrefab, spawnPosition.position, spawnPosition.rotation);
        currentWeaponBullet.transform.tag = "StandartBullet";
        currentBulletScript = currentWeaponBullet.GetComponent<Bullet>();
        currentBulletScript.Init(bulletData);
        charShooting.SetBulletInfo(currentBulletScript);
        Destroy(currentWeaponBullet);
    }
}
