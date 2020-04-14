using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro;

public class CharGun : MonoBehaviour
{
    //Classes
    public CharInfo charInfo;
    public CharMelee charMelee;

    private WeaponSpawner weaponSpawner;
    private SettingsInfo settingsInfo;
    private GameButtons gameButtons;
    private Bullet bullet;

    //UI GameObjects and UI
    public Sprite pick_up_image;
    private Sprite fire_image;
    private GameObject gunInfoBar;
    private Button fireActButton;
    private GameObject levelBar;

    //Gameobjects
    private GameObject floorGun;

    //Values
    [Tooltip("Смещение оружия")]
    [SerializeField] private Vector3 offsetGun;
    public int currentWeaponNumber;

    void Start()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        bullet = GameObject.Find("GameHandler").GetComponent<Bullet>();
        weaponSpawner = GameObject.Find("WeaponSpawner").GetComponent<WeaponSpawner>();

        GameObject gameUI = GameObject.Find("Canvas").transform.Find("GameUI").gameObject;
        gunInfoBar = gameUI.transform.Find("GunInfoBar").gameObject;
        levelBar = gameUI.transform.Find("LevelBar").gameObject;
        gameButtons = gameUI.GetComponent<GameButtons>();
        fireActButton = GameObject.Find("FireActButton").GetComponent<Button>();

        offsetGun = new Vector3(0, -0.35f, 0);
        currentWeaponNumber = 0;
        weaponSpawner.Spawn(charInfo.weapon, transform, currentWeaponNumber);
        gameButtons.currentWeaponImage.sprite = WeaponSpawner.currentWeaponScript[currentWeaponNumber].MainSprite;

        GameObject character = GameObject.Find("Character(Clone)");
        gameButtons.currentWeapon = character.transform.Find(charInfo.weapon);
        charMelee.animator = character.transform.Find(charInfo.weapon).GetComponent<Animator>();
        SetWeaponParam();
        WeaponSpawner.currentCharWeapon[currentWeaponNumber].SetActive(true);

        gunInfoBar.SetActive(false);
        levelBar.GetComponentInChildren<Text>().text = charInfo.level.ToString();
        fire_image = fireActButton.GetComponent<Image>().sprite;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Gun")
        {
            GameButtons.FireActButtonState = 1;//Change Gun
            floorGun = coll.gameObject;
            fireActButton.GetComponent<Image>().sprite = pick_up_image;
            fireActButton.GetComponent<Image>().color = Color.blue;
            gunInfoBar.SetActive(true);
            GetSpecGun(coll);
        }
    }


    void OnTriggerExit2D(Collider2D coll)
    {
        GameButtons.FireActButtonState = 0;//Fire
        fireActButton.GetComponent<Image>().sprite = fire_image;
        fireActButton.GetComponent<Image>().color = Color.red;

        if (coll.gameObject.tag == "Gun")
        {
            gunInfoBar.SetActive(false);
        }
    }

    public void ChangeGun()
    {
        if (WeaponSpawner.countOfWeapon == 2)
        {
            WeaponSpawner.currentCharWeapon[currentWeaponNumber].transform.SetParent(null);
            weaponSpawner.Spawn(WeaponSpawner.currentCharWeapon[currentWeaponNumber].GetComponent<Weapon>().WeaponName,
                                   gameObject.transform.position,
                                       Quaternion.identity);
            Destroy(WeaponSpawner.currentCharWeapon[currentWeaponNumber]);

            weaponSpawner.Spawn(floorGun.gameObject.GetComponent<Weapon>().WeaponName, 
                                    transform, currentWeaponNumber);
            gameButtons.ChangeWeaponButton();
            SwapWeapon();
            Destroy(floorGun.gameObject);
            SetWeaponParam();
        }
        else
        {
            weaponSpawner.Spawn(floorGun.gameObject.GetComponent<Weapon>().WeaponName,
                                    transform, 1);
            Destroy(floorGun.gameObject);
            gameButtons.SwapWeapon();
            weaponSpawner.SwapWeapon(1);
            SetWeaponParam();
        }
    }

    public void SwapWeapon()
    {
        weaponSpawner.SwapWeapon(currentWeaponNumber);
    }

    private void GetSpecGun(Collider2D coll)
    {
        var weapon = coll.gameObject.GetComponent<Weapon>();

        int damage = weapon.Damage;
        float critChance = weapon.CritChance;
        float fireRate = weapon.FireRate;
        int manecost = weapon.Manecost;
        BulletData bullet = weapon.Bullet;

        gunInfoBar.GetComponentInChildren<Text>().text =
                    $"{damage} DMG | " +
                    $"{critChance}% CRIT | " +
                    $"{manecost} MANA";
    }

    private void SetWeaponParam()
    {
        WeaponSpawner.currentCharWeapon[currentWeaponNumber].transform.position = transform.position + offsetGun;
    }
}
