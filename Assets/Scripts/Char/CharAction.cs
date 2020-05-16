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

    private GameObject gunInfoBar;
    private Button fireActButton;

    private SettingsInfo settingsInfo;

    private float timeToOff;
    public bool isEnterFirst;
    public bool isPlayerHitted;

    void Start()
    {
        GameObject gameUI = GameObject.Find("Canvas").transform.Find("CharacterControlUI").gameObject;
        gunInfoBar = gameUI.transform.Find("GunInfoBar").gameObject;
        fireActButton = gameUI.transform.Find("FireActButton").GetComponent<Button>();

        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
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
                GameButtons.FireActButtonState = 2;
                fireActButton.GetComponent<Image>().color = Color.magenta;
                break;
            case "Door":
                GameButtons.FireActButtonState = 3;
                fireActButton.GetComponent<Image>().color = Color.yellow;
                break;
            case "TV":
                GameButtons.FireActButtonState = 4;
                fireActButton.GetComponent<Image>().color = Color.yellow;
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
