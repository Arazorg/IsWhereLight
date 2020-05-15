﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [Tooltip("Ссылки на префабы пуль")]
    [SerializeField] private List<GameObject> bulletsPrefabs;

    [Tooltip("Список настроек для пуль")]
    [SerializeField] private List<BulletData> bulletsSettings;

    [Tooltip("Место спауна пули")]
    [SerializeField] private Transform spawnPosition;

    public GameObject currentWeaponBullet;
    public Bullet currentBulletScript;

    private BulletData spawnBulletData;

    public void Spawn()
    {
        currentWeaponBullet = Instantiate(bulletsPrefabs[0], spawnPosition.position, spawnPosition.rotation);
        currentWeaponBullet.transform.tag = "StandartBullet";
        currentBulletScript = currentWeaponBullet.GetComponent<Bullet>();
        currentBulletScript.Init(spawnBulletData);
        currentWeaponBullet.SetActive(true);
        Destroy(currentWeaponBullet, 5);
    }

    public void SetBullet(BulletData bulletData)
    {
        spawnPosition = transform.GetChild(0);
        spawnBulletData = bulletData;

        currentWeaponBullet = Instantiate(bulletsPrefabs[0], spawnPosition.position, spawnPosition.rotation);
        currentWeaponBullet.transform.tag = "StandartBullet";
        currentBulletScript = currentWeaponBullet.GetComponent<Bullet>();
        currentBulletScript.Init(spawnBulletData);

        currentBulletScript.Damage = GetComponent<Weapon>().Damage;
        currentBulletScript.CritChance = GetComponent<Weapon>().CritChance;

        GetComponent<Gun>().SetBulletInfo(currentBulletScript);
        Destroy(currentWeaponBullet);
    }
}
