using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyButtons : MonoBehaviour
{
    public Button goToMenuButton;
    private CurrentGameInfo currentGameInfo;
    private CharactersSpec charactersSpec;
    private CharactersSpec.Character charSpec;
    public Text healthText;
    public Text maneText;
    public Text gunText;

    void Start()
    {
        charactersSpec = GameObject.Find("LobbyHandler").GetComponent<CharactersSpec>();
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        charactersSpec = GameObject.Find("LobbyHandler").GetComponent<CharactersSpec>();
        SetSpecChar("Knight");
        SetInfoBar();
    }

    public void ChooseCharacter()
    {
        currentGameInfo.character = EventSystem.current.currentSelectedGameObject.name;
        SetSpecChar(currentGameInfo.character);
        SetInfoBar();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GoToGame()
    {
        currentGameInfo.SaveCurrentGame();
        SceneManager.LoadScene("Game");
    }

    public void SetSpecChar(string name)
    {
        if (charactersSpec.characters.TryGetValue(name, out charSpec))
        {
            currentGameInfo.maxHealth = charSpec.maxHealth;
            currentGameInfo.maxMane = charSpec.maxMane;
            currentGameInfo.startGun = charSpec.startGun;
        }
        else
        {
            Debug.Log($"Current name {name} don't exist");
        }
    }

    private void SetInfoBar()
    {
        healthText.text = "Health : " + currentGameInfo.maxHealth.ToString();
        maneText.text = "Mane : " + currentGameInfo.maxMane.ToString();
        gunText.text = "Gun : " + currentGameInfo.startGun.ToString();
    }
}
