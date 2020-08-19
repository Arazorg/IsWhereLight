using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;

public class CharGun : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("CharInfo скрипт")]
    [SerializeField] private CharInfo charInfo;

    [Tooltip("Спрайт кнопки(поднятия оружия)")]
    [SerializeField] private Sprite pickUpImage;

    [Tooltip("Спрайт кнопки(атака)")]
    [SerializeField] private Sprite fireImage;

    [Tooltip("Смещение дальнего оружия")]
    [SerializeField] private Vector3 offsetGunDistant;

    [Tooltip("Смещение ближнего оружия")]
    [SerializeField] private Vector3 offsetGunMelee;
#pragma warning restore 0649

    public static bool isChange;
    public int CurrentWeaponNumber
    {
        get { return currentWeaponNumber; }
        set { currentWeaponNumber = value; }
    }
    private int currentWeaponNumber;

    //UI
    private GameObject gunInfoBar;
    private Button fireActButton;
    //Gameobjects
    private GameObject floorGun;
    //Scripts 
    private GameButtons gameButtons;

    void Start()
    {
        var characterControlUI = GameObject.Find("Canvas").transform.Find("CharacterControlUI");
        gameButtons = characterControlUI.GetComponent<GameButtons>();
        gunInfoBar = characterControlUI.Find("GunInfoBar").gameObject;
        fireActButton = characterControlUI.Find("FireActButton").GetComponent<Button>();
        SpawnStartWeapon();
        gunInfoBar.GetComponent<MovementUI>().SetStart();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Gun" && !CharAction.isDeath)
        {
            GameButtons.FireActButtonState = GameButtons.FireActButtonStateEnum.changeGun;//Change Gun
            floorGun = coll.gameObject;
            fireActButton.GetComponent<Image>().sprite = pickUpImage;
            gunInfoBar.GetComponent<MovementUI>().MoveToEnd();
            GetSpecGun(coll);
        }
    }


    void OnTriggerExit2D(Collider2D coll)
    {
        GameButtons.FireActButtonState = 0;//Fire
        fireActButton.GetComponent<Image>().sprite = fireImage;

        if (coll.gameObject.tag == "Gun")
            gunInfoBar.GetComponent<MovementUI>().MoveToStart();
    }

    public void ChangeGun()
    {

        if (WeaponSpawner.instance.countOfWeapon == 2)
        {
            WeaponSpawner.instance.currentCharWeapon[currentWeaponNumber].transform.SetParent(null);

            WeaponSpawner.instance.SetPrefab(WeaponSpawner.instance.currentCharWeapon[currentWeaponNumber].GetComponent<Weapon>().WeaponName);
            WeaponSpawner.instance.Spawn(gameObject.transform.position, Quaternion.identity);

            Destroy(WeaponSpawner.instance.currentCharWeapon[currentWeaponNumber]);

            WeaponSpawner.instance.SetPrefab(floorGun.gameObject.GetComponent<Weapon>().WeaponName);
            WeaponSpawner.instance.Spawn(transform, currentWeaponNumber);

            gameButtons.ChangeWeaponButton();
            SwapWeapon();
            Destroy(floorGun.gameObject);
            SetWeaponParam();
        }
        else
        {
            WeaponSpawner.instance.SetPrefab(floorGun.gameObject.GetComponent<Weapon>().WeaponName);
            WeaponSpawner.instance.Spawn(transform, 1);
            Destroy(floorGun.gameObject);
            gameButtons.SwapWeapon();
            WeaponSpawner.instance.SwapWeapon(1);
            SetWeaponParam();
        }

    }

    public void SwapWeapon()
    {
        WeaponSpawner.instance.SwapWeapon(currentWeaponNumber);
    }

    private void GetSpecGun(Collider2D coll)
    {
        var weapon = coll.gameObject.GetComponent<Weapon>();

        int damage = weapon.Damage;
        float critChance = weapon.CritChance;
        float fireRate = weapon.FireRate;
        int manecost = weapon.Manecost;
        BulletData bullet = weapon.CurrentBullet;
        gunInfoBar.GetComponentInChildren<TextMeshProUGUI>().text =
            $"{damage} DMG | {critChance}% CRIT | {manecost} MANA";
    }

    private void SetWeaponParam()
    {
        if (WeaponSpawner.instance.currentCharWeapon[currentWeaponNumber].GetComponent<Weapon>().TypeOfAttack
            == WeaponData.AttackType.Gun)
            WeaponSpawner.instance.currentCharWeapon[currentWeaponNumber].transform.position
                = transform.position + offsetGunDistant;

        else if (WeaponSpawner.instance.currentCharWeapon[currentWeaponNumber].GetComponent<Weapon>().TypeOfAttack
                == WeaponData.AttackType.Sword)
            WeaponSpawner.instance.currentCharWeapon[currentWeaponNumber].transform.position
            = transform.position + offsetGunMelee;
    }

    private void SpawnStartWeapon()
    {
        WeaponSpawner.instance.countOfWeapon = 0;
        currentWeaponNumber = 0;
        var spawnWeapon = Regex.Replace(charInfo.weapons[currentWeaponNumber], "[0-9]", "", RegexOptions.IgnoreCase);
        WeaponSpawner.instance.SetPrefab(spawnWeapon);
        WeaponSpawner.instance.Spawn(transform, currentWeaponNumber);
        SetWeaponParam();

        if (charInfo.weapons[1] != null && charInfo.weapons[1] != "")
        {
            WeaponSpawner.instance.currentCharWeapon[currentWeaponNumber].SetActive(false);
            currentWeaponNumber++;
            spawnWeapon = Regex.Replace(charInfo.weapons[currentWeaponNumber], "[0-9]", "", RegexOptions.IgnoreCase);
            WeaponSpawner.instance.SetPrefab(spawnWeapon);
            WeaponSpawner.instance.Spawn(transform, currentWeaponNumber);
            SetWeaponParam();
        }

        gameButtons.currentWeaponImage.transform.GetChild(0).GetComponent<Image>().sprite =
            WeaponSpawner.instance.currentCharWeapon[currentWeaponNumber].GetComponent<Weapon>().MainSprite;

        gameButtons.currentWeapon = transform.Find(charInfo.weapons[currentWeaponNumber]);
        WeaponSpawner.instance.currentCharWeapon[currentWeaponNumber].SetActive(true);
    }
}
