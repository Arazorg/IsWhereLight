using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharAction : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("CharInfo скрипт")]
    [SerializeField] private CharInfo charInfo;
#pragma warning restore 0649

    private float timeToOff;
    public bool isEnterFirst;
    public bool isPlayerHitted;

    void Start()
    {
        GameObject gameUI = GameObject.Find("Canvas").transform.Find("CharacterControlUI").gameObject;
    }

    void Update()
    {
        PlayerHitted();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        switch(coll.gameObject.name)
        {
            case "WeaponStore":
                GameButtons.FireActButtonState = GameButtons.FireActButtonStateEnum.weaponStore;
                break;
            case "PortalToGame":
                GameButtons.FireActButtonState = GameButtons.FireActButtonStateEnum.portalToGame;
                break;
            case "TvAds":
                GameButtons.FireActButtonState = GameButtons.FireActButtonStateEnum.tvAds;
                break;
        }

        if(coll.tag == "EnemyBullet")
        {
            charInfo.Damage(coll.GetComponent<Bullet>().Damage);
            isPlayerHitted = true;
            isEnterFirst = true;
        }
    }

    public void PlayerHitted()
    {
        if (isPlayerHitted)
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
        SceneManager.LoadScene("FinishGame");
        FinishOfGameButton.finishGameMoney = charInfo.money;
    }
}
