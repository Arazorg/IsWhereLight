﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Ссылки на префабы пуль")]
    [SerializeField] private List<GameObject> bulletsPrefabs;

    [Tooltip("Список настроек для пуль")]
    [SerializeField] private List<BulletData> bulletsSettings;

    [Tooltip("Место спауна пули")]
    [SerializeField] private Transform spawnPosition;
#pragma warning restore 0649
    private GameObject currentWeaponBullet;
    private Bullet currentBulletScript;
    private BulletData spawnBulletData;

    public void Spawn()
    {
        currentWeaponBullet = Instantiate(bulletsPrefabs[0], spawnPosition.position, spawnPosition.rotation);
        //currentWeaponBullet.transform.tag = "StandartBullet";
        currentBulletScript = currentWeaponBullet.GetComponent<Bullet>();
        currentBulletScript.Init(spawnBulletData);
        currentBulletScript.Damage = GetComponent<Weapon>().Damage;
        currentBulletScript.CritChance = GetComponent<Weapon>().CritChance;
        currentBulletScript.Knoking = GetComponent<Weapon>().Knoking;
        currentWeaponBullet.SetActive(true);
        Destroy(currentWeaponBullet, 5);
    }

    public void SetBullet(BulletData bulletData)
    {
        spawnPosition = transform.GetChild(0);
        spawnBulletData = bulletData;

        currentWeaponBullet = Instantiate(bulletsPrefabs[0], spawnPosition.position, spawnPosition.rotation);
        currentBulletScript = currentWeaponBullet.GetComponent<Bullet>();
        currentBulletScript.Init(spawnBulletData);
        currentBulletScript.Damage = GetComponent<Weapon>().Damage;
        currentBulletScript.CritChance = GetComponent<Weapon>().CritChance;
        currentBulletScript.Knoking = GetComponent<Weapon>().Knoking;

        if (GetComponent<Gun>() != null)
            GetComponent<Gun>().SetBulletInfo(currentBulletScript);
        else if(GetComponent<Bow>() != null)
            GetComponent<Bow>().SetBulletInfo(currentBulletScript);
        else if(GetComponent<Laser>() != null)
            GetComponent<Laser>().SetBulletInfo(currentBulletScript);

        Destroy(currentWeaponBullet);
    }

    public void SetDamageCrit(float coof)
    {
        currentBulletScript.Damage = (int)(currentBulletScript.Damage * coof);
        if (currentBulletScript.Damage < 1)
            currentBulletScript.Damage = 1;
        currentBulletScript.CritChance *= coof;
    }

    public GameObject GetBullet()
    {
        return currentWeaponBullet;
    }

    public Bullet GetBulletScript()
    {
        return currentBulletScript;
    }
}
