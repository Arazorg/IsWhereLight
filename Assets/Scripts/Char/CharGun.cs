using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro;
using System.Text.RegularExpressions;

public class CharGun : MonoBehaviour
{
    //Classes
    public CharInfo charInfo;
    public CharMelee charMelee;

    private SettingsInfo settingsInfo;
    private GameButtons gameButtons;
    private Bullet bullet;

    //UI GameObjects and UI
    public Sprite pick_up_image;
    private Sprite fire_image;
    private GameObject gunInfoBar;
    private Button fireActButton;
    private GameObject currentDayBar;

    //Gameobjects
    private GameObject floorGun;

    //Values
    [Tooltip("Смещение дальнего оружия")]
    [SerializeField] private Vector3 offsetGunDistant;

    [Tooltip("Смещение ближнего оружия")]
    [SerializeField] private Vector3 offsetGunMelee;
    public int currentWeaponNumber;

    void Start()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();

        GameObject gameUI = GameObject.Find("Canvas").transform.Find("CharacterControlUI").gameObject;
        gunInfoBar = gameUI.transform.Find("GunInfoBar").gameObject;
        currentDayBar = gameUI.transform.Find("LevelBar").gameObject;
        gameButtons = gameUI.GetComponent<GameButtons>();
        fireActButton = GameObject.Find("FireActButton").GetComponent<Button>();

        offsetGunDistant = new Vector3(0, -0.35f, 0);
        offsetGunMelee = new Vector3(-0.2f, -0.35f, 0);

        currentWeaponNumber = 0;
        var spawnWeapon = Regex.Replace(charInfo.weapons[currentWeaponNumber], "[0-9]", "", RegexOptions.IgnoreCase);
        WeaponSpawner.instance.Spawn(spawnWeapon, transform, currentWeaponNumber);
        SetWeaponParam();
        if (charInfo.weapons[1] != null)
        {
            Debug.Log("have 2" + charInfo.weapons[1]);
            currentWeaponNumber++;
            spawnWeapon = Regex.Replace(charInfo.weapons[currentWeaponNumber], "[0-9]", "", RegexOptions.IgnoreCase);
            WeaponSpawner.instance.Spawn(spawnWeapon, transform, currentWeaponNumber);
            SetWeaponParam();
        }
        gameButtons.currentWeaponImage.sprite = WeaponSpawner.instance.currentWeaponScript[currentWeaponNumber].MainSprite;

        GameObject character = GameObject.Find("Character(Clone)");
        gameButtons.currentWeapon = character.transform.Find(charInfo.weapons[currentWeaponNumber]);
        charMelee.animator = character.transform.Find(charInfo.weapons[currentWeaponNumber]).GetComponent<Animator>();
        WeaponSpawner.instance.currentCharWeapon[currentWeaponNumber].SetActive(true);

        gunInfoBar.SetActive(false);
        currentDayBar.GetComponentInChildren<Text>().text = charInfo.currentDay.ToString();
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
        if (WeaponSpawner.instance.countOfWeapon == 2)
        {
            WeaponSpawner.instance.currentCharWeapon[currentWeaponNumber].transform.SetParent(null);
            WeaponSpawner.instance.Spawn(WeaponSpawner.instance.currentCharWeapon[currentWeaponNumber].GetComponent<Weapon>().WeaponName,
                                   gameObject.transform.position,
                                       Quaternion.identity);
            Destroy(WeaponSpawner.instance.currentCharWeapon[currentWeaponNumber]);

            WeaponSpawner.instance.Spawn(floorGun.gameObject.GetComponent<Weapon>().WeaponName, 
                                    transform, currentWeaponNumber);
            gameButtons.ChangeWeaponButton();
            SwapWeapon();
            Destroy(floorGun.gameObject);
            SetWeaponParam();
        }
        else
        {
            WeaponSpawner.instance.Spawn(floorGun.gameObject.GetComponent<Weapon>().WeaponName,
                                    transform, 1);
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
        BulletData bullet = weapon.Bullet;

        gunInfoBar.GetComponentInChildren<Text>().text =
                    $"{damage} DMG | " +
                    $"{critChance}% CRIT | " +
                    $"{manecost} MANA";
    }

    
    private void SetWeaponParam()
    {
        if (WeaponSpawner.instance.currentCharWeapon[currentWeaponNumber].GetComponent<Weapon>().TypeOfAttack
            == WeaponData.AttackType.Distant)
            WeaponSpawner.instance.currentCharWeapon[currentWeaponNumber].transform.position
                = transform.position + offsetGunDistant;

        else if (WeaponSpawner.instance.currentCharWeapon[currentWeaponNumber].GetComponent<Weapon>().TypeOfAttack
            == WeaponData.AttackType.Melee)
        {
            WeaponSpawner.instance.currentCharWeapon[currentWeaponNumber].transform.position
                = transform.position + offsetGunMelee;
        }
            
    }
}
