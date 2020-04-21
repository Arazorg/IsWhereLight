using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharAction : MonoBehaviour
{   
    private GameObject gunInfoBar;
    private Button fireActButton;
    private CharInfo charInfo;
    private CurrentGameInfo currentGameInfo;
    private SettingsInfo settingsInfo;
    private float timeToOff;
    public bool isEnterFirst;
    public bool isPlayerHitted;

    void Start()
    {
        GameObject gameUI = GameObject.Find("Canvas").transform.Find("CharacterControlUI").gameObject;
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        charInfo = GameObject.Find("Character(Clone)").GetComponent<CharInfo>();
        gunInfoBar = gameUI.transform.Find("GunInfoBar").gameObject;
        fireActButton = gameUI.transform.Find("FireActButton").GetComponent<Button>();
    }

    void Update()
    {
        PlayerHitted();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {

        if (coll.gameObject.tag == "WeaponStore")
        {
            GameButtons.FireActButtonState = 2;
            fireActButton.GetComponent<Image>().color = Color.magenta;
        }
        else if (coll.gameObject.tag == "Door")
        {
            GameButtons.FireActButtonState = 3;
            fireActButton.GetComponent<Image>().color = Color.yellow;
        }
    }

    public void PlayerHitted()
    {
        if (isPlayerHitted)
        {
            if (isEnterFirst)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
                timeToOff = Time.time + 0.05f;
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
