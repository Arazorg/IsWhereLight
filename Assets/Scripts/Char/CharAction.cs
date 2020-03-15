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
    private GameButtons gameButtons;

    void Start()
    {
        GameObject gameUI = GameObject.Find("Canvas").transform.Find("GameUI").gameObject;
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        charInfo = GameObject.Find("Character(Clone)").GetComponent<CharInfo>();
        gameButtons = gameUI.GetComponent<GameButtons>();
        gunInfoBar = gameUI.transform.Find("GunInfoBar").gameObject;
        fireActButton = gameUI.transform.Find("FireActButton").GetComponent<Button>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Portal")
        {
            gameButtons.fireActButtonState = 2;//Activate Portal
            fireActButton.GetComponent<Image>().color = Color.blue;
            //fireActButton.GetComponentInChildren<Text>().text = "Enter";

        }

        if (coll.gameObject.tag == "Chest")
        {
            fireActButton.GetComponent<Image>().color = Color.yellow;
           // fireActButton.GetComponentInChildren<Text>().text = "Open";
        }
    }

    public void ChangeLevel()
    {
        charInfo.level++;
        settingsInfo.SaveSettings();
        charInfo.SaveChar();
        SceneManager.LoadScene("Game");
    }

    public void OpenChest()
    {
        int numberOfGun = Random.Range(0, 2);
        GameObject creatingGun = GameObject.Find(numberOfGun.ToString());
        Instantiate(creatingGun, transform.position, transform.rotation);
    }
}
