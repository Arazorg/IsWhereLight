using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    [Tooltip("Размер камеры при выбранном персонаже")]
    [SerializeField] private float cameraSizeCharacter;
#pragma warning restore 0649

    public GameObject playerCharacter;
    private int lastPhrase = -1;
    private bool isHello = false;
    private bool m_FacingRight;
    private PopupText currentPhrase;

    private float timePhrase = 0.6f;
    private float timeIgnore = 20f;
    private float phraseTimer;
    private int phrasesCount;
    private bool isIgnore;
    private AudioManager audioManager; 

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        offsetText = new Vector3(0, 0.85f, 0);
        if (transform.localScale.x == 1)
            m_FacingRight = true;
        else
            m_FacingRight = false;

        playerCharacter = null;
        characterChooseUI.gameObject.SetActive(false);
        phrasesCount = 0;
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

    public string CharacterType
    {
        get
        {
            return data.CharacterType;
        }
        protected set { }
    }

    public float SkillTime
    {
        get
        {
            return data.SkillTime;
        }
        protected set { }
    }

    void Update()
    {
        if (phrasesCount > 5 && !isIgnore)
        {
            if (currentPhrase != null)
                currentPhrase.DeletePhrase();
            currentPhrase = PopupText.Create(transform.position + offsetText, true, false, -1, $"ShutUp{Random.Range(0, 6)}");
            phraseTimer = Time.time + timeIgnore;
            isIgnore = true;
        }
        if (Time.time > phraseTimer)
        {
            isIgnore = false;
            phrasesCount = 0;
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!characterControlUI.gameObject.activeSelf)
        {
            audioManager.Play("ClickUI");
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
            if (!isHello)
            {
                if (currentPhrase != null)
                    currentPhrase.DeletePhrase();
                currentPhrase = PopupText.Create(transform.position + offsetText, true, false, -1, $"Hello{Random.Range(0, 6)}");
                isHello = true;
            }

            if (!isIgnore)
            {
                if ((transform.position - coll.transform.position).x < 0 && !m_FacingRight)
                    Flip();
                else if ((transform.position - coll.transform.position).x > 0 && m_FacingRight)
                    Flip();
            }
            else
            {
                if (currentPhrase != null)
                    currentPhrase.DeletePhrase();
                currentPhrase = 
                    PopupText.Create(transform.position + offsetText, true, false, -1, $"GoAway{Random.Range(0, 4)}");
                if ((transform.position - coll.transform.position).x < 0 && m_FacingRight)
                    Flip();
                else if ((transform.position - coll.transform.position).x > 0 && !m_FacingRight)
                    Flip();
            }

        }

        if (coll.gameObject.tag.Contains("Bullet") 
                || coll.gameObject.tag.Contains("Arrow")
                    || coll.gameObject.tag.Contains("Laser"))
        {
            if (currentPhrase != null)
                currentPhrase.DeletePhrase();
            currentPhrase = PopupText.Create(transform.position
                + offsetText, true, false, -1, $"{CharacterClass}ShootReaction{Random.Range(0, 2)}");
        }
    }

    public void ShowPhrase()
    {
        if (!isIgnore)
        {
            int phrase = Random.Range(0, 8);
            while (phrase == lastPhrase)
                phrase = Random.Range(0, 8);
            lastPhrase = phrase;
            if (currentPhrase != null)
                currentPhrase.DeletePhrase();
            currentPhrase = PopupText.Create(transform.position 
                + offsetText, true, false, -1, $"{CharacterClass}Phrase{phrase}");
            phraseTimer = Time.time + timePhrase;
            phrasesCount++;
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Vector3 scale = transform.localScale;
            if (!isIgnore)
            {
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
            else
            {
                if ((transform.position - coll.transform.position).x < 0 && m_FacingRight)
                {
                    m_FacingRight = false;
                    scale.x = -1;
                }
                else if ((transform.position - coll.transform.position).x > 0 && !m_FacingRight)
                {
                    m_FacingRight = true;
                    scale.x = 1;
                }
                transform.localScale = scale;
            }
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
