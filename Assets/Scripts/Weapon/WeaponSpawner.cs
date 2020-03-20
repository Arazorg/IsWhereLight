using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [Tooltip("Список настроек для врагов")]
    [SerializeField] private List<WeaponData> weaponSettings;

    [Tooltip("Ссылка на базовый префаб врага")]
    [SerializeField] private GameObject weaponPrefab;

    [Tooltip("Место спауна оружия")]
    [SerializeField] private Transform[] spawnPositions;

    private GameObject prefab;
    public static GameObject currentCharWeapon;
    public static Weapon currentCharScript;
    private Weapon script;
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
                countOfWeapon += 1 % 2;
                script = prefab.GetComponent<Weapon>();
                script.Init(data);
                prefab.transform.tag = "Gun";
                prefab.SetActive(true);
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
                currentCharScript = currentCharWeapon.GetComponent<Weapon>();
                currentCharScript.Init(data);
                currentCharWeapon.transform.tag = "Untagged";
                currentCharWeapon.SetActive(false);
                Weapons.Add(currentCharWeapon, currentCharScript);
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
                Weapons.Add(prefab, script);
            }
        }
    }
}
