using System.Collections;
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

    public GameObject currentWeaponBullet;
    public Bullet currentBulletScript;

    private BulletData spawnBulletData;

    public void Spawn()
    {
        currentWeaponBullet = Instantiate(bulletsPrefabs[0], spawnPosition.position, spawnPosition.rotation);
        //currentWeaponBullet.transform.tag = "StandartBullet";
        currentBulletScript = currentWeaponBullet.GetComponent<Bullet>();
        currentBulletScript.Init(spawnBulletData);
        currentBulletScript.Damage = GetComponent<Weapon>().Damage;
        currentBulletScript.CritChance = GetComponent<Weapon>().CritChance;
        currentWeaponBullet.SetActive(true);
        Destroy(currentWeaponBullet, 5);
    }

    public void SetBullet(BulletData bulletData)
    {
        spawnPosition = transform.GetChild(0);
        spawnBulletData = bulletData;

        currentWeaponBullet = Instantiate(bulletsPrefabs[0], spawnPosition.position, spawnPosition.rotation);
       // currentWeaponBullet.transform.tag = "StandartBullet";
        currentBulletScript = currentWeaponBullet.GetComponent<Bullet>();
        currentBulletScript.Init(spawnBulletData);
        currentBulletScript.Damage = GetComponent<Weapon>().Damage;
        currentBulletScript.CritChance = GetComponent<Weapon>().CritChance;

        if(GetComponent<Gun>() != null)
            GetComponent<Gun>().SetBulletInfo(currentBulletScript);
        else if(GetComponent<Bow>() != null)
            GetComponent<Bow>().SetBulletInfo(currentBulletScript);

        Destroy(currentWeaponBullet);
    }
}
