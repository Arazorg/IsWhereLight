﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour, IPointerDownHandler
{
    [Tooltip("Параметры персонажа")]
    [SerializeField] private CharacterData data;

    [Tooltip("UI Выбора персонажа")]
    [SerializeField] private CharacterChooseUI characterChooseUI;

    [Tooltip("UI Управления персонажа")]
    [SerializeField] private GameObject characterControlUI;

    [Tooltip("UI лобби")]
    [SerializeField] private GameObject lobbyUI;

    public GameObject playerCharacter;
    private bool m_FacingRight;

    void Start()
    {
        m_FacingRight = true;
        playerCharacter = null;
        characterChooseUI.gameObject.SetActive(false);
        Init();
    }

    void Update()
    {
        if(playerCharacter != null)
        {
            UpdateFlip();
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void UpdateFlip()
    {
        if ((transform.position - playerCharacter.transform.position).x < 0 && !m_FacingRight)
            Flip();
        else if ((transform.position - playerCharacter.transform.position).x > 0 && m_FacingRight)
            Flip();
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

    /// <summary>
    /// Character's animations
    /// </summary>
    public string[] SkinsNames
    {
        get
        {
            return data.SkinsNames;
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
            lobbyUI.SetActive(false);
            CameraZoom();
            characterChooseUI.gameObject.SetActive(true);
            characterChooseUI.ChooseCharacter(this, GetComponent<Animator>());
        }
    }

    private void CameraZoom()
    {
        Camera.main.orthographicSize = 1.3f;
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
            PopupDamage.Create(transform.position, true, false, -1, "Hello");
    }
}