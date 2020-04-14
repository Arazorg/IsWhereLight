using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    BulletSpawner bulletSpawner;
    GameButtons gameButtons;
    CharInfo charInfo;

    [Tooltip("Список настроек для оружия")]
    [SerializeField] private List<WeaponData> weaponSettings;

    [Tooltip("Ссылка на базовый префаб оружия")]
    [SerializeField] private GameObject weaponPrefab;

    [Tooltip("Место спауна оружия")]
    [SerializeField] private Transform[] spawnPositions;

    private GameObject prefab;
    private Weapon script;

    public static GameObject[] currentCharWeapon = new GameObject[2];
    public static Weapon[] currentWeaponScript = new Weapon[2];
    public static int countOfWeapon;
    private int countOfStand;

    public static Dictionary<GameObject, Weapon> Weapons;

    private void Start()
    {
        charInfo = GameObject.Find("Character(Clone)").GetComponent<CharInfo>();
        Weapons = new Dictionary<GameObject, Weapon>();
        countOfWeapon = 0;
        countOfStand = 0;
    }


    public void Spawn(string weaponName)
    {
        foreach (var data in weaponSettings)
        {
            if (data.name == weaponName)
            {
                prefab = Instantiate(weaponPrefab, spawnPositions[countOfStand]);
                countOfStand = (countOfStand + 1) % 3;
                script = prefab.GetComponent<Weapon>();
                script.Init(data);
                prefab.name = weaponName;
                prefab.transform.tag = "Gun";
                prefab.SetActive(true);
                prefab.GetComponent<SpriteRenderer>().sortingOrder = 2;
                Weapons.Add(prefab, script);
            }
        }
    }

    public void Spawn(string weaponName, Transform transform, int currentWeaponNumber)
    {
        foreach (var data in weaponSettings)
        {
            if (data.name == weaponName)
            {
                currentCharWeapon[currentWeaponNumber] = Instantiate(weaponPrefab, transform);
                currentWeaponScript[currentWeaponNumber] = currentCharWeapon[currentWeaponNumber].GetComponent<Weapon>();
                currentCharWeapon[currentWeaponNumber].GetComponent<Weapon>().Init(data);
                currentCharWeapon[currentWeaponNumber].name = weaponName + currentWeaponNumber;
                currentCharWeapon[currentWeaponNumber].transform.tag = "Untagged";
                currentCharWeapon[currentWeaponNumber].SetActive(false);
                currentCharWeapon[currentWeaponNumber].GetComponent<SpriteRenderer>().sortingOrder = 3;
                Weapons.Add(currentCharWeapon[currentWeaponNumber], currentCharWeapon[currentWeaponNumber].GetComponent<Weapon>());

                charInfo.weapon = currentCharWeapon[currentWeaponNumber].GetComponent<Weapon>().WeaponName + currentWeaponNumber;

                bulletSpawner = currentCharWeapon[currentWeaponNumber].GetComponent<BulletSpawner>();
                gameButtons = GameObject.Find("Canvas").transform.Find("GameUI").GetComponent<GameButtons>();

                bulletSpawner.SetBullet(currentWeaponScript[currentWeaponNumber].Bullet);
                gameButtons.SetWeaponInfo(currentCharWeapon[currentWeaponNumber].GetComponent<Weapon>());
                
                countOfWeapon++;
                if (countOfWeapon > 2)
                    countOfWeapon = 2;
                
            }
        }   
    }

    public void SwapWeapon(int currentWeaponNumber)
    {
        charInfo.weapon = currentCharWeapon[currentWeaponNumber].GetComponent<Weapon>().WeaponName + currentWeaponNumber;
        bulletSpawner = currentCharWeapon[currentWeaponNumber].GetComponent<BulletSpawner>();
        gameButtons = GameObject.Find("Canvas").transform.Find("GameUI").GetComponent<GameButtons>();
        bulletSpawner.SetBullet(currentWeaponScript[currentWeaponNumber].Bullet);
        gameButtons.SetWeaponInfo(currentCharWeapon[currentWeaponNumber].GetComponent<Weapon>());
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
                prefab.name = weaponName;
                prefab.transform.tag = "Gun";
                prefab.SetActive(true);
                prefab.GetComponent<SpriteRenderer>().sortingOrder = 2;
                Weapons.Add(prefab, script);
            }
        }
    }
}
