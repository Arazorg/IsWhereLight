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

    void Start()
    {
        GameObject gameUI = GameObject.Find("Canvas").transform.Find("GameUI").gameObject;
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        charInfo = GameObject.Find("Character(Clone)").GetComponent<CharInfo>();
        gunInfoBar = gameUI.transform.Find("GunInfoBar").gameObject;
        fireActButton = gameUI.transform.Find("FireActButton").GetComponent<Button>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {

        if (coll.gameObject.tag == "WeaponStore")
        {
            GameButtons.FireActButtonState = 2;
            fireActButton.GetComponent<Image>().color = Color.magenta;
        }

        if (coll.gameObject.tag == "Chest")
        {
            GameButtons.FireActButtonState = 3;
            fireActButton.GetComponent<Image>().color = Color.yellow;
        }
        if (coll.gameObject.tag == "Enemy")
        {
            var obj = coll.gameObject;
            if (EnemySpawner.Enemies.ContainsKey(obj))
            {
                int spend = EnemySpawner.Enemies[obj].Attack;
                Debug.Log(spend);
                charInfo.SpendHealth(spend);
                if (charInfo.health < 0)
                {
                    Death();
                }
            }
        }
    }

    public void Death()
    {
        SceneManager.LoadScene("FinishGame");
        FinishOfGameButton.finishGameMoney = charInfo.money;
    }

    public void ChangeLevel()
    {
        charInfo.level++;
        settingsInfo.SaveSettings();
        charInfo.SaveChar();
        SceneManager.LoadScene("Game");
    }
}
