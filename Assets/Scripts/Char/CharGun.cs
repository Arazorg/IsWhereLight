using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;

public class CharGun : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Спрайт кнопки(поднятия оружия)")]
    [SerializeField] private Sprite pickUpImage;

    [Tooltip("Спрайт кнопки(атака)")]
    [SerializeField] private Sprite fireImage;
#pragma warning restore 0649

    public static bool isChange;

    public int CurrentWeaponNumber
    {
        get { return currentWeaponNumber; }
        set { currentWeaponNumber = value; }
    }
    private int currentWeaponNumber;

    private CharInfo charInfo;
    private GameObject gunInfoBar;
    private GameObject floorGun;
    private Button fireActButton;
    private GameButtons gameButtons;
    
    void Start()
    {
        var characterControlUI = GameObject.Find("Canvas").transform.Find("CharacterControlUI");
        gameButtons = characterControlUI.GetComponent<GameButtons>();
        gunInfoBar = characterControlUI.Find("GunInfoBar").gameObject;
        fireActButton = characterControlUI.Find("FireActButton").GetComponent<Button>();
        gunInfoBar.GetComponent<MovementUI>().SetStart();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Gun" && !CharAction.isDeath)
        {
            GameButtons.FireActButtonState = GameButtons.FireActButtonStateEnum.changeGun;
            floorGun = coll.gameObject;
            fireActButton.GetComponent<Image>().sprite = pickUpImage;
            gunInfoBar.GetComponent<MovementUI>().MoveToEnd();
            GetSpecGun(coll);
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        GameButtons.FireActButtonState = 0;
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
        }
        else
        {
            WeaponSpawner.instance.SetPrefab(floorGun.gameObject.GetComponent<Weapon>().WeaponName);
            WeaponSpawner.instance.Spawn(transform, 1);
            Destroy(floorGun.gameObject);
            gameButtons.SwapWeapon();
            WeaponSpawner.instance.SwapWeapon(1);
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
        int manecost = weapon.Manecost;
        gunInfoBar.GetComponentInChildren<TextMeshProUGUI>().text =
            $"{damage} DMG | {critChance}% CRIT | {manecost} MANA";
    }

    public void SpawnStartWeapon()
    {
        charInfo = GameObject.Find("CharInfoHandler").GetComponent<CharInfo>();
        var characterControlUI = GameObject.Find("Canvas").transform.Find("CharacterControlUI");
        gameButtons = characterControlUI.GetComponent<GameButtons>();
        WeaponSpawner.instance.countOfWeapon = 0;
        currentWeaponNumber = 0;
        var spawnWeapon = Regex.Replace(charInfo.weapons[currentWeaponNumber], "[0-9]", "", RegexOptions.IgnoreCase);
        WeaponSpawner.instance.SetPrefab(spawnWeapon);
        WeaponSpawner.instance.Spawn(transform, currentWeaponNumber);

        if (charInfo.weapons[1] != null && charInfo.weapons[1] != "")
        {
            WeaponSpawner.instance.currentCharWeapon[currentWeaponNumber].SetActive(false);
            currentWeaponNumber++;
            spawnWeapon = Regex.Replace(charInfo.weapons[currentWeaponNumber], "[0-9]", "", RegexOptions.IgnoreCase);
            WeaponSpawner.instance.SetPrefab(spawnWeapon);
            WeaponSpawner.instance.Spawn(transform, currentWeaponNumber);
        }

        gameButtons.currentWeaponImage.transform.GetChild(0).GetComponent<Image>().sprite =
            WeaponSpawner.instance.currentCharWeapon[currentWeaponNumber].GetComponent<Weapon>().MainSprite;
        gameButtons.currentWeapon = transform.Find(charInfo.weapons[currentWeaponNumber]);
        WeaponSpawner.instance.currentCharWeapon[currentWeaponNumber].SetActive(true);
    }
}
