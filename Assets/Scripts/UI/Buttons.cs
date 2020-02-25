using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //Classes
    public CharInfo charInfo;
    public CharShooting charShooting;
    public CharGun charGun;
    public ManaBar manaBar;

    //Values
    private WeaponsSpec.Gun gun;
    public float fireRate;
    public int mana;
    private float nextFire;
    private bool shooting;

    void Start()
    {
        charInfo = GameObject.Find("Character(Clone)").GetComponent<CharInfo>();
        GameObject character = GameObject.Find("Character(Clone)");
        charShooting = character.GetComponent<CharShooting>();
        charGun = character.GetComponent<CharGun>();
        nextFire = 0.0f;
    }

    void Update()
    {
        if (manaBar.currentValue > 0 && shooting)
        {
            if (Time.time > nextFire)
            {
                manaBar.Spend(mana);
                charInfo.SpendMana(mana);
                charShooting.Shoot();
                nextFire = Time.time + fireRate;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (gameObject.name)
        {
            case "FireActButton":
                if (gameObject.GetComponent<Image>().color == Color.green)
                {
                    charGun.ChangeGun();
                }
                else if (gameObject.GetComponent<Image>().color == Color.blue)
                {
                    charGun.ChangeLevel();
                }
                else if (gameObject.GetComponent<Image>().color == Color.yellow)
                {
                    charGun.OpenChest();
                }
                else
                {
                    shooting = true;
                }
                break;
            case "MenuButton":
                charInfo.SaveChar();
                SceneManager.LoadScene("Lobby");
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        shooting = false;
    }

    void OnApplicationQuit()
    {
        charInfo.SaveChar();
    }
}
