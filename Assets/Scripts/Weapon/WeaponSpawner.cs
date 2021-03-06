﻿using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public static WeaponSpawner instance;

#pragma warning disable 0649
    [Tooltip("Список настроек для оружия")]
    [SerializeField] private WeaponData[] weaponsSettings;

    [Tooltip("Ссылка на базовый префаб оружия")]
    [SerializeField] private List<GameObject> weaponsPrefabs;
#pragma warning restore 0649

    public GameObject[] currentCharWeapon = new GameObject[2];
    public int countOfWeapon;

    private GameButtons gameButtons;
    private GameObject spawnPrefab;
    private GameObject prefab;
    private WeaponData data;
    private CharInfo charInfo;
    private Weapon script;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        countOfWeapon = 0;
    }

    public void SetPrefab(string weaponName)
    {
        foreach (var data in weaponsSettings)
            if (data.WeaponName == weaponName)
                this.data = data;

        switch (data.TypeOfAttack)
        {
            case WeaponData.AttackType.Bow:
                spawnPrefab = weaponsPrefabs[0];
                break;
            case WeaponData.AttackType.Gun:
                spawnPrefab = weaponsPrefabs[1];
                break;
            case WeaponData.AttackType.Sword:
                spawnPrefab = weaponsPrefabs[2];
                break;
            case WeaponData.AttackType.Laser:
                spawnPrefab = weaponsPrefabs[3];
                break;
            case WeaponData.AttackType.ConstantLaser:
                spawnPrefab = weaponsPrefabs[4];
                break;
        }
    }

    public void Spawn(string weaponName, Transform spawnPosition, bool isAlly = false)
    {
        prefab = Instantiate(spawnPrefab, spawnPosition);
        script = prefab.GetComponent<Weapon>();
        if (isAlly)
            script.Init(data, true);
        else
            script.Init(data);
        prefab.name = weaponName;
        if (!isAlly)
        {
            prefab.GetComponent<SpriteRenderer>().sortingOrder = 3;
            prefab.transform.tag = "Gun";
            prefab.transform.localPosition = Vector3.zero;
        }
        else
        {
            var weaponScript = prefab.GetComponent<Weapon>();
            if (weaponScript.TypeOfAttack != WeaponData.AttackType.Sword)
            {
                var bulletSpawner = prefab.GetComponent<BulletSpawner>();
                bulletSpawner.SetBullet(weaponScript.CurrentBullet);
            }
            prefab.transform.tag = "GunKeep";
            prefab.GetComponent<SpriteRenderer>().sortingOrder = 3;
        }           
        prefab.SetActive(true);
        prefab.transform.rotation = Quaternion.Euler(0, 0, -90);   
        
    }

    public void Spawn(Transform transform, int currentWeaponNumber)
    {
        charInfo = GameObject.Find("CharInfoHandler").GetComponent<CharInfo>();

        currentCharWeapon[currentWeaponNumber] = Instantiate(spawnPrefab, transform);
        currentCharWeapon[currentWeaponNumber].GetComponent<Weapon>().Init(data, true);
        currentCharWeapon[currentWeaponNumber].name = data.WeaponName + currentWeaponNumber;
        currentCharWeapon[currentWeaponNumber].transform.tag = "GunKeep";
        currentCharWeapon[currentWeaponNumber].GetComponent<SpriteRenderer>().sortingOrder = 3;

        var weaponScript = currentCharWeapon[currentWeaponNumber].GetComponent<Weapon>();
        charInfo.weapons[currentWeaponNumber] = weaponScript.WeaponName + currentWeaponNumber;
        if (weaponScript.TypeOfAttack != WeaponData.AttackType.Sword)
        {
            var bulletSpawner = currentCharWeapon[currentWeaponNumber].GetComponent<BulletSpawner>();
            bulletSpawner.SetBullet(weaponScript.CurrentBullet);
        }

        gameButtons = GameObject.Find("Canvas").transform.Find("CharacterControlUI").GetComponent<GameButtons>();
        gameButtons.SetWeaponInfo(weaponScript);

        countOfWeapon++;
        if (countOfWeapon > 2)
            countOfWeapon = 2;
    }

    public void SwapWeapon(int currentWeaponNumber)
    {
        var weaponScript = currentCharWeapon[currentWeaponNumber].GetComponent<Weapon>();
        charInfo.weapons[currentWeaponNumber] = weaponScript.WeaponName + currentWeaponNumber;

        if (weaponScript.TypeOfAttack != WeaponData.AttackType.Sword)
        {
            var bulletSpawner = currentCharWeapon[currentWeaponNumber].GetComponent<BulletSpawner>();
            bulletSpawner.SetBullet(weaponScript.CurrentBullet);
        }

        gameButtons = GameObject.Find("Canvas").transform.Find("CharacterControlUI").GetComponent<GameButtons>();
        gameButtons.SetWeaponInfo(weaponScript);
    }

    public void Spawn(Vector3 position, Quaternion quaternion)
    {
        prefab = Instantiate(spawnPrefab, position, quaternion);
        script = prefab.GetComponent<Weapon>();
        script.Init(data);
        prefab.name = data.WeaponName;
        prefab.transform.tag = "Gun";
        prefab.SetActive(true);
        prefab.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
}
