using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class CharGun : MonoBehaviour
{
    //Classes
    public CharShooting charShooting;
    public CharInfo charInfo;

    private SettingsInfo settingsInfo;
    private GameButtons gameButtons;
    private WeaponsSpec weaponsSpec;
    private Bullet bullet;

    //UI GameObjects and UI
    private GameObject gunInfoBar;
    private Button fireActButton;
    private GameObject levelBar;

    //Gameobjects
    public Sprite pick_up_image;
    private Sprite fire_image;
    private GameObject startGun;
    private GameObject floorGun;
    private GameObject currentGun;

    //Values
    private WeaponsSpec.Gun gunSpec;
    private Vector3 offsetGun;

    void Start()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();

        GameObject gameHandler = GameObject.Find("GameHandler");
        bullet = gameHandler.GetComponent<Bullet>();
        weaponsSpec = gameHandler.GetComponent<WeaponsSpec>();

        GameObject gameUI = GameObject.Find("Canvas").transform.Find("GameUI").gameObject;
        gunInfoBar = gameUI.transform.Find("GunInfoBar").gameObject;
        levelBar = gameUI.transform.Find("LevelBar").gameObject;
        gameButtons = gameUI.GetComponent<GameButtons>();

        startGun = GetGunGameObject(charInfo.gun);
        fireActButton = GameObject.Find("FireActButton").GetComponent<Button>();
        offsetGun = new Vector3(0, -0.35f, 0);
        StartGunCreate();
        gunInfoBar.SetActive(false);
        levelBar.GetComponentInChildren<Text>().text = charInfo.level.ToString();

        fire_image = fireActButton.GetComponent<Image>().sprite;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Gun")
        {
            gameButtons.fireActButtonState = 1;//Change Gun
            WeaponsSpec.Gun floorGunInfo;
            floorGun = coll.gameObject;
            fireActButton.GetComponent<Image>().sprite = pick_up_image;
            //fireActButton.GetComponentInChildren<Text>().text = "Catch";
            gunInfoBar.SetActive(true);
            try
            {
                floorGunInfo = GetSpecGun(coll.gameObject.name.Substring
                    (0, coll.gameObject.name.IndexOf('(')).Replace(" ", string.Empty));
            }
            catch
            {
                floorGunInfo = GetSpecGun(coll.gameObject.name);
            }
            gunInfoBar.GetComponentInChildren<Text>().text = $"{floorGunInfo.dmg} DMG | {floorGunInfo.crit}% CRIT | {floorGunInfo.mana} MANA";
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        gameButtons.fireActButtonState = 0;//Fire
        fireActButton.GetComponent<Image>().sprite = fire_image;
        //fireActButton.GetComponentInChildren<Text>().text = "Fire";
        gunInfoBar.SetActive(false);
    }

    public void ChangeGun()
    {
        currentGun.transform.CompareTag("Gun");
        Instantiate(currentGun, gameObject.transform.position, currentGun.transform.rotation);
        Destroy(currentGun);
        currentGun = Instantiate(floorGun, gameObject.transform.position + offsetGun, Quaternion.identity);
        currentGun.transform.SetParent(gameObject.transform);
        currentGun.transform.localScale = new Vector3(1, 1, 1);
        currentGun.transform.CompareTag("Untagged");
        currentGun.name = (currentGun.name.Substring(0, currentGun.name.IndexOf('('))).Replace(" ", string.Empty);
        charInfo.gun = currentGun.name;
        SetSpecGun(currentGun.name);

        charShooting.firePoint = currentGun.transform.GetChild(0);
        Destroy(floorGun);
    }

    private GameObject GetGunGameObject(string name)
    {
        if (weaponsSpec.guns.TryGetValue(name, out gunSpec))
        {
            return gunSpec.gunPrefab;
        }
        else
        {
            Debug.Log($"Current name {name} don't exist");
            return null;
        }
    }

    private void SetSpecGun(string name)
    {
        if (weaponsSpec.guns.TryGetValue(name, out gunSpec))
        {
            bullet.dmg = gunSpec.dmg;
            bullet.crit = gunSpec.crit;
            gameButtons.mana = gunSpec.mana;
            gameButtons.fireRate = gunSpec.fireRate;
            charShooting.bulletSpeed = gunSpec.speed;
            charShooting.bulletScatterAngle = gunSpec.scatter;
            charShooting.bulletPrefab = gunSpec.bulletPrefab;
        }
        else
        {
            Debug.Log($"Current name {name} don't exist");
        }
    }

    private WeaponsSpec.Gun GetSpecGun(string name)
    {
        if (weaponsSpec.guns.TryGetValue(name, out WeaponsSpec.Gun currentGun))
        {
            return currentGun;
        }
        else
        {
            Debug.Log($"Current name {name} don't exist");
            return currentGun;
        }
    }

    private void StartGunCreate()
    {
        currentGun = Instantiate(startGun, gameObject.transform.position + offsetGun, Quaternion.identity);
        currentGun.transform.SetParent(gameObject.transform);
        currentGun.transform.CompareTag("Untagged");
        currentGun.name = (currentGun.name.Substring(0, currentGun.name.IndexOf('('))).Replace(" ", string.Empty);
        SetSpecGun(currentGun.name);
        charShooting.firePoint = currentGun.transform.GetChild(0);
    }


}
