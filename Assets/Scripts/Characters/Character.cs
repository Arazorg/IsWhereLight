using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour, IPointerDownHandler
{
#pragma warning disable 0649
    [Tooltip("Параметры персонажа")]
    [SerializeField] private CharacterData data;

    [Tooltip("UI Выбора персонажа")]
    [SerializeField] private CharacterChooseUI characterChooseUI;

    [Tooltip("UI Управления персонажа")]
    [SerializeField] private GameObject characterControlUI;

    [Tooltip("UI лобби")]
    [SerializeField] private GameObject lobbyUI;

    [Tooltip("Смещение текста над NPC")]
    [SerializeField] private Vector3 offsetText;

    [Tooltip("Время нового привествия НПС")]
    [SerializeField] private float phraseTime = 3f;

    [Tooltip("Время нового привествия НПС")]
    [SerializeField] private float helloTime = 60f;

    [Tooltip("Время фразы НПС о выстреле")]
    [SerializeField] private float shootPhraseTime = 2f;

    [Tooltip("Лист фраз NPC")]
    [SerializeField] private List<string> NPC_Phrases;

    [Tooltip("Размер камеры при выбранном персонаже")]
    [SerializeField] private float cameraSizeCharacter;
#pragma warning restore 0649

    public GameObject playerCharacter;
    private float timeToHello = 0;
    private float timeToPhrase = 0;
    private float timeToShootPhrase = 0;
    private int lastPhrase = -1;
    private bool m_FacingRight;

    void Start()
    {
        offsetText = new Vector3(0, 0.85f, 0);
        if (transform.localScale.x == 1)
            m_FacingRight = true;
        else
            m_FacingRight = false;

        playerCharacter = null;
        characterChooseUI.gameObject.SetActive(false);
        Init();
    }

    /// <summary>
    /// Initialization of character
    /// </summary>
    /// <param name="data"></param>
    public void Init()
    {
        GetComponent<Animator>().runtimeAnimatorController = data.Animations[0];
    }

    /// <summary>
    /// Class of character
    /// </summary>
    public string CharacterClass
    {
        get
        {
            return data.CharacterClass;
        }
        protected set { }
    }

    /// <summary>
    /// Character's animations
    /// </summary>
    public RuntimeAnimatorController[] Animations
    {
        get
        {
            return data.Animations;
        }
        protected set { }
    }

    public int MaxHealth
    {
        get
        {
            return data.MaxHealth;
        }
        protected set { }
    }

    public int MaxMane
    {
        get
        {
            return data.MaxMane;
        }
        protected set { }
    }

    public string StartWeapon
    {
        get
        {
            return data.StartWeapon;
        }
        protected set { }
    }

    public int Price
    {
        get
        {
            return data.Price;
        }
        protected set { }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!characterControlUI.gameObject.activeSelf)
        {
            lobbyUI.GetComponent<LobbyUI>().HideLobby();
            CameraZoom();
            characterChooseUI.gameObject.SetActive(true);
            characterChooseUI.ChooseCharacter(this, GetComponent<Animator>());
        }
    }

    private void CameraZoom()
    {
        Camera.main.orthographicSize = cameraSizeCharacter;
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if (Time.time > timeToHello &&
                 (Time.time + phraseTime - timeToPhrase > (int)PopupText.DISAPPEAR_TIMER_MAX_PHRASE + 1 &&
                    Time.time + shootPhraseTime - timeToShootPhrase > (int)PopupText.DISAPPEAR_TIMER_MAX_PHRASE + 1))
            {
                PopupText.Create(transform.position + offsetText, true, false, -1, "Hello");
                timeToHello = Time.time + helloTime;
            }

            if ((transform.position - coll.transform.position).x < 0 && !m_FacingRight)
                Flip();
            else if ((transform.position - coll.transform.position).x > 0 && m_FacingRight)
                Flip();
        }
        else if (Time.time > timeToShootPhrase &&
                    (coll.gameObject.tag == "StandartBullet" || coll.gameObject.tag == "StandartArrow") &&
                            (Time.time + helloTime - timeToHello > (int)PopupText.DISAPPEAR_TIMER_MAX_PHRASE + 1 &&
                                    Time.time + phraseTime - timeToPhrase > (int)PopupText.DISAPPEAR_TIMER_MAX_PHRASE + 1))
        {
            PopupText.Create(transform.position + offsetText, true, false, -1, "Shoot");
            timeToShootPhrase = Time.time + shootPhraseTime;
        }
    }

    public void ShowPhrase()
    {
        if (Time.time > timeToPhrase &&
                (Time.time + helloTime - timeToHello > (int)PopupText.DISAPPEAR_TIMER_MAX_PHRASE + 1 &&
                    Time.time + shootPhraseTime - timeToShootPhrase > (int)PopupText.DISAPPEAR_TIMER_MAX_PHRASE + 1))
        {
            int phrase = Random.Range(0, NPC_Phrases.Count);
            while (phrase == lastPhrase)
                phrase = Random.Range(0, NPC_Phrases.Count);
            lastPhrase = phrase;
            PopupText.Create(transform.position + offsetText, true, false, -1, NPC_Phrases[phrase]);
            timeToPhrase = Time.time + phraseTime;
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Vector3 scale = transform.localScale;
            if ((transform.position - coll.transform.position).x < 0 && !m_FacingRight)
            {
                m_FacingRight = true;
                scale.x = 1;
            }
            else if ((transform.position - coll.transform.position).x > 0 && m_FacingRight)
            {
                m_FacingRight = false;
                scale.x = -1;
            }
            transform.localScale = scale;
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
