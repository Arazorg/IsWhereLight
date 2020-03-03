using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.SceneManagement;

public class CharGun : MonoBehaviour
{
    //Classes
    public CharShooting charShooting;
    public CharInfo charInfo;
    private GameButtons gameButtons;
    private WeaponsSpec weaponsSpec;
    private Bullet bullet;

    //UI GameObjects and UI
    public GameObject gunInfoBar;
    private GameObject levelBar;
    public Button fireActButton;
    private StringBuilder builder;

    //Gameobjects
    private GameObject startGun;
    private GameObject floorGun;
    private GameObject gun;

    //Values
    private WeaponsSpec.Gun gunSpec;
    private Vector3 offsetGun;

    void Start()
    {
        GameObject gameHandler = GameObject.Find("GameHandler");
        bullet = gameHandler.transform.Find("Bullet").GetComponent<Bullet>();
        weaponsSpec = gameHandler.GetComponent<WeaponsSpec>();
        gameButtons = gameHandler.GetComponent<GameButtons>();

        fireActButton = GameObject.Find("FireActButton").GetComponent<Button>();
        gunInfoBar = GameObject.Find("Canvas").transform.Find("GameUI").transform.Find("GunInfoBar").gameObject;
        levelBar = GameObject.Find("Canvas").transform.Find("GameUI").transform.Find("LevelBar").gameObject;

        startGun = GameObject.Find(charInfo.startGun);
        offsetGun = new Vector3(0, 0, 0);

        builder = new StringBuilder();

        StartGunCreate();
        gunInfoBar.SetActive(false);
        levelBar.GetComponentInChildren<Text>().text = charInfo.level.ToString();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Gun")
        {
            gameButtons.fireActButtonState = 1;//Change Gun
            WeaponsSpec.Gun floorGunInfo;
            floorGun = coll.gameObject;
            fireActButton.GetComponent<Image>().color = Color.green;
            fireActButton.GetComponentInChildren<Text>().text = "Catch";
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
            gunInfoBar.GetComponentInChildren<Text>().text = builder.Append($"{floorGunInfo.dmg} DMG | {floorGunInfo.crit}% CRIT | {floorGunInfo.mana} MANA").ToString();
            builder.Clear();
        }

        if (coll.gameObject.tag == "Portal")
        {
            gameButtons.fireActButtonState = 2;//Activate Portal
            fireActButton.GetComponent<Image>().color = Color.blue;
            fireActButton.GetComponentInChildren<Text>().text = "Enter";

        }

        if (coll.gameObject.tag == "Chest")
        {
            fireActButton.GetComponent<Image>().color = Color.yellow;
            fireActButton.GetComponentInChildren<Text>().text = "Open";
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        gameButtons.fireActButtonState = 0;//Fire
        fireActButton.GetComponent<Image>().color = Color.red;
        fireActButton.GetComponentInChildren<Text>().text = "Fire";
        gunInfoBar.SetActive(false);
    }

    public void ChangeGun()
    {
        gun.transform.CompareTag("Gun");
        Instantiate(gun, gameObject.transform.position, gun.transform.rotation);
        Destroy(gun);
        gun = Instantiate(floorGun, gameObject.transform.position + offsetGun, Quaternion.identity);
        gun.transform.SetParent(gameObject.transform);
        gun.transform.CompareTag("Untagged");
        gun.name = (gun.name.Substring(0, gun.name.IndexOf('('))).Replace(" ", string.Empty);
        charInfo.startGun = gun.name;
        SetSpecGun(gun.name);
        charShooting.firePoint = gun.transform.GetChild(0);
        Destroy(floorGun);
    }

    public void ChangeLevel()
    {
        charInfo.level++;
        charInfo.SaveChar();
        SceneManager.LoadScene("Game");
    }

    public void OpenChest()
    {
        int numberOfGun = Random.Range(0, 2);
        GameObject creatingGun = GameObject.Find(numberOfGun.ToString());
        Instantiate(creatingGun, transform.position, transform.rotation);
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
        if (weaponsSpec.guns.TryGetValue(name, out WeaponsSpec.Gun gun))
        {
            return gun;
        }
        else
        {
            Debug.Log($"Current name {name} don't exist");
            return gun;
        }
    }

    private void StartGunCreate()
    {
        gun = Instantiate(startGun, gameObject.transform.position + offsetGun, Quaternion.identity);
        gun.transform.SetParent(gameObject.transform);
        gun.transform.CompareTag("Untagged");
        gun.name = (gun.name.Substring(0, gun.name.IndexOf('('))).Replace(" ", string.Empty);
        SetSpecGun(gun.name);
        charShooting.firePoint = gun.transform.GetChild(0);
    }


}
