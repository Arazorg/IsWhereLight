using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    BulletSpawner bulletSpawner;
    GameButtons gameButtons;

    [Tooltip("Список настроек для оружия")]
    [SerializeField] private List<WeaponData> weaponSettings;

    [Tooltip("Ссылка на базовый префаб оружия")]
    [SerializeField] private GameObject weaponPrefab;

    [Tooltip("Место спауна оружия")]
    [SerializeField] private Transform[] spawnPositions;

    private GameObject prefab;
    private Weapon script;

    public static GameObject currentCharWeapon;
    public static Weapon currentWeaponScript;
    
    private int countOfWeapon;

    public static Dictionary<GameObject, Weapon> Weapons;

    private void Start()
    {
        Weapons = new Dictionary<GameObject, Weapon>();
        countOfWeapon = 0;
    }


    public void Spawn(string weaponName)
    {
        foreach (var data in weaponSettings)
        {
            if (data.name == weaponName)
            {
                prefab = Instantiate(weaponPrefab, spawnPositions[countOfWeapon]);
                countOfWeapon = (countOfWeapon + 1) % 2;
                script = prefab.GetComponent<Weapon>();
                script.Init(data);
                prefab.transform.tag = "Gun";
                prefab.SetActive(true);
                prefab.GetComponent<SpriteRenderer>().sortingOrder = 0;
                Weapons.Add(prefab, script);
            }
        }
    }

    public void Spawn(string weaponName, Transform transform)
    {
        foreach (var data in weaponSettings)
        {
            if (data.name == weaponName)
            {
                currentCharWeapon = Instantiate(weaponPrefab, transform);
                currentWeaponScript = currentCharWeapon.GetComponent<Weapon>();
                currentWeaponScript.Init(data);
                currentCharWeapon.transform.tag = "Untagged";
                currentCharWeapon.SetActive(false);
                currentCharWeapon.GetComponent<SpriteRenderer>().sortingOrder = 2;
                Weapons.Add(currentCharWeapon, currentWeaponScript);

                bulletSpawner = currentCharWeapon.GetComponent<BulletSpawner>();
                gameButtons = GameObject.Find("Canvas").transform.Find("GameUI").GetComponent<GameButtons>();
                bulletSpawner.SetBullet(currentWeaponScript.Bullet);
                gameButtons.SetWeaponInfo(currentWeaponScript);
            }
        }   
    }

    public void Spawn(string weaponName, Vector3 position, Quaternion quaternion)
    {
        foreach (var data in weaponSettings)
        {
            if (data.name == weaponName)
            {
                prefab = Instantiate(weaponPrefab, position, quaternion);
                script = prefab.GetComponent<Weapon>();
                script.Init(data);
                prefab.transform.tag = "Gun";
                prefab.SetActive(true);
                prefab.GetComponent<SpriteRenderer>().sortingOrder = 0;
                Weapons.Add(prefab, script);
            }
        }
    }
}
