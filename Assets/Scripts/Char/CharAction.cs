using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharAction : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("Спрайт кнопки(атака)")]
    [SerializeField] private Sprite fireImage;

    [Tooltip("Спрайт кнопки(действие)")]
    [SerializeField] private Sprite actionImage;
#pragma warning restore 0649

    public static bool isDeath;

    public bool IsEnterFirst
    {
        get { return isEnterFirst; }
        set { isEnterFirst = value; }
    }
    private bool isEnterFirst;

    public bool IsPlayerHitted
    {
        get { return isPlayerHitted; }
        set { isPlayerHitted = value; }
    }
    private bool isPlayerHitted;

    public GameObject currentNPC;

    private Button fireActButton;
    private Transform characterControlUI;
    private CharInfo charInfo;
    private GameObject challengeUI;

    private float timeToOff;
    private float timeToDeathPanel;

    void Start()
    {
        isDeath = false;
        timeToDeathPanel = float.MaxValue;
        charInfo = GetComponent<CharInfo>();
        characterControlUI = GameObject.Find("Canvas").transform.Find("CharacterControlUI");
        fireActButton = characterControlUI.Find("FireActButton").GetComponent<Button>();
        if(SceneManager.GetActiveScene().name == "Lobby")
            challengeUI = characterControlUI.Find("ChallengeUI").gameObject;
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
        if (!isDeath && coll.isTrigger)
        {
            switch (coll.gameObject.name)
            {
                case "WeaponStore":
                    GameButtons.FireActButtonState = GameButtons.FireActButtonStateEnum.weaponStore;
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
                case "PortalToForest":
                    challengeUI.GetComponent<ChallengeUI>().OpenChallengeUI();
                    GetComponent<CharController>().SetZeroSpeed(true);
                    isDeath = true;
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
        AudioManager.instance.Play($"{charInfo.character}Death");
        GetComponent<Animator>().SetBool("Death", true);
        ColorUtility.TryParseHtmlString("#808080", out Color color);
        GetComponent<SpriteRenderer>().color = color;
        transform.Find(charInfo.weapons[GetComponent<CharGun>().CurrentWeaponNumber]).gameObject.SetActive(false);
        GetComponent<Rigidbody2D>().simulated = false;
        gameObject.tag = "IgnoreAll";
        timeToDeathPanel = Time.time + 1f;
    }

    public void Revive()
    {
        isDeath = false;
        CurrentGameInfo.instance.countResurrect--;
        charInfo.Healing(charInfo.maxHealth);
        charInfo.FillMana(charInfo.maxMane);
        AudioManager.instance.Play("Revive");
        GetComponent<Animator>().SetBool("Death", false);
        GetComponent<SpriteRenderer>().color = Color.white;
        transform.Find(charInfo.weapons[GetComponent<CharGun>().CurrentWeaponNumber]).gameObject.SetActive(true);
        GetComponent<Rigidbody2D>().simulated = true;
        gameObject.tag = "Player";
    }
}
