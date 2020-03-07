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

    private SettingsInfo settingsInfo;
    private GameButtons gameButtons;
    private WeaponsSpec weaponsSpec;
    private Bullet bullet;

    //UI GameObjects and UI
    private GameObject gunInfoBar;
    private Button fireActButton;
    private GameObject levelBar;
    private StringBuilder builder;

    //Gameobjects
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
        offsetGun = new Vector3(0, 0, 0);
        builder = new StringBuilder();
        StartGunCreate();
        gunInfoBar.SetActive(false);
        levelBar.GetComponentInChildren<Text>().text = settingsInfo.level.ToString();
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
        currentGun.transform.CompareTag("Gun");
        Instantiate(currentGun, gameObject.transform.position, currentGun.transform.rotation);
        Destroy(currentGun);
        currentGun = Instantiate(floorGun, gameObject.transform.position + offsetGun, Quaternion.identity);
        currentGun.transform.SetParent(gameObject.transform);
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
