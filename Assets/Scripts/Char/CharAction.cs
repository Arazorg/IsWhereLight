
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharAction : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("CharInfo скрипт")]
    [SerializeField] private CharInfo charInfo;

    [Tooltip("Спрайт кнопки(атака)")]
    [SerializeField] private Sprite fireImage;

    [Tooltip("Спрайт кнопки(действие)")]
    [SerializeField] private Sprite actionImage;
#pragma warning restore 0649
    public bool isEnterFirst;
    public bool isPlayerHitted;
    public GameObject currentNPC;

    private float timeToOff;
    private Button fireActButton;
    private Transform characterControlUI;
    private float timeToDeathPanel;
    private float standartSpeed;
    public static bool isDeath;
    void Start()
    {
        timeToDeathPanel = float.MaxValue;
        characterControlUI = GameObject.Find("Canvas").transform.Find("CharacterControlUI");
        fireActButton = characterControlUI.Find("FireActButton").GetComponent<Button>();
    }

    void Update()
    {
        if (isDeath && Time.time > timeToDeathPanel)
        {
            characterControlUI.gameObject.GetComponent<GameButtons>().OpenDeathPanel();
            timeToDeathPanel = float.MaxValue;
        }
           
        PlayerHitted();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!isDeath)
        {
            switch (coll.gameObject.name)
            {
                case "WeaponStore":
                    GameButtons.FireActButtonState = GameButtons.FireActButtonStateEnum.weaponStore;
                    fireActButton.GetComponent<Image>().sprite = actionImage;
                    break;
                case "PortalToGame":
                    GameButtons.FireActButtonState = GameButtons.FireActButtonStateEnum.portalToGame;
                    fireActButton.GetComponent<Image>().sprite = actionImage;
                    break;
                case "TvAds":
                    GameButtons.FireActButtonState = GameButtons.FireActButtonStateEnum.tvAds;
                    fireActButton.GetComponent<Image>().sprite = actionImage;
                    break;
                case "ShootingRangeStartPosition":
                    GameButtons.FireActButtonState = GameButtons.FireActButtonStateEnum.shootingRange;
                    fireActButton.GetComponent<Image>().sprite = actionImage;
                    break;
            }

            switch (coll.gameObject.tag)
            {
                case "NPC":
                    GameButtons.FireActButtonState = GameButtons.FireActButtonStateEnum.NPC;
                    currentNPC = coll.gameObject;
                    fireActButton.GetComponent<Image>().sprite = actionImage;
                    break;
            }

            if (coll.tag == "EnemyBullet")
            {
                charInfo.Damage(coll.GetComponent<Bullet>().Damage);
                isPlayerHitted = true;
                isEnterFirst = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        fireActButton.GetComponent<Image>().sprite = fireImage;
    }

    public void PlayerHitted()
    {
        if (isPlayerHitted && !isDeath)
        {
            if (isEnterFirst)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
                timeToOff = Time.time + 0.1f;
                isEnterFirst = false;
            }
            else
            {
                if (Time.time > timeToOff)
                {
                    GetComponent<SpriteRenderer>().color = Color.white;
                    isPlayerHitted = false;
                    isEnterFirst = true;
                }
            }
        }
    }

    public void Death()
    {
        isDeath = true;
        GetComponent<Animator>().SetBool("Death", true);
        ColorUtility.TryParseHtmlString("#808080", out Color color);
        GetComponent<SpriteRenderer>().color = color;
        transform.GetChild(0).gameObject.SetActive(false);
        standartSpeed = GetComponent<CharController>().speed;
        GetComponent<CharController>().SetRbVelocityZero();
        gameObject.tag = "IgnoreAll";
        timeToDeathPanel = Time.time + 1f;
    }

    public void Resurrect()
    {
        isDeath = false;
        GetComponent<Animator>().SetBool("Death", false);
        GetComponent<SpriteRenderer>().color = Color.white;
        transform.GetChild(0).gameObject.SetActive(true);
        charInfo.countResurrect--;
        charInfo.Healing(charInfo.maxHealth);
        gameObject.tag = "Player";
    }
}
