using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class CharGun : MonoBehaviour
{
    //Classes
    public CharShooting charShooting;
    public CharInfo charInfo;

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
    private Transform currentGun;

    //Values
    private Vector3 offsetGun;

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
        weaponSpawner.Spawn(charInfo.gun, transform);
        currentGun = transform.GetChild(0);
        StartGunCreate();

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
        gunInfoBar.SetActive(false);
    }

    public void ChangeGun()
    {
        currentGun.transform.CompareTag("Gun");
        //Instantiate(currentGun, gameObject.transform.position, currentGun.transform.rotation);
        //Destroy(currentGun);
        currentGun.transform.SetParent(gameObject.transform);
        currentGun.transform.localScale = new Vector3(1, 1, 1);
        currentGun.transform.CompareTag("Untagged");
        charInfo.gun = currentGun.name;
        charShooting.firePoint = currentGun.transform.GetChild(0);
        Destroy(floorGun);
    }

    private void GetSpecGun(Collider2D coll)
    {
        var obj = coll.gameObject;
        if (WeaponSpawner.Weapons.ContainsKey(obj))
        {
            int damage = WeaponSpawner.Weapons[obj].Damage;
            float critChance = WeaponSpawner.Weapons[obj].CritChance;
            float fireRate = WeaponSpawner.Weapons[obj].FireRate;
            int manecost = WeaponSpawner.Weapons[obj].Manecost;
            BulletData bullet = WeaponSpawner.Weapons[obj].Bullet;

            gunInfoBar.GetComponentInChildren<Text>().text =
                $"{damage} DMG | " +
                $"{critChance}% CRIT | " +
                $"{manecost} MANA";
        }        
    }

    private void StartGunCreate()
    {
        currentGun.CompareTag("Untagged");
        currentGun.position += offsetGun;
        charShooting.firePoint = currentGun.transform.GetChild(0);
    }
}
