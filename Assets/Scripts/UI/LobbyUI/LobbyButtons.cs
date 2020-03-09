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
    public Button nextCharButton;
    public Button prevCharButton;
    public Animator animator;
    private CurrentGameInfo currentGameInfo;
    private CharactersSpec charactersSpec;
    private CharactersSpec.Character charSpec;

    public Text characterText;
    public Text healthText;
    public Text maneText;
    public Text gunText;

    private string[] characters;
    public int charCounter;
    void Start()
    {
        charactersSpec = GameObject.Find("LobbyHandler").GetComponent<CharactersSpec>();
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        charactersSpec = GameObject.Find("LobbyHandler").GetComponent<CharactersSpec>();
        charCounter = 0;
        characters = new string[] { "Knight", "Mage" };
        ChooseCharacter();
    }

    public void ChooseCharacter()
    {
        currentGameInfo.character = characters[charCounter];
        SetSpecChar(currentGameInfo.character);
        SetInfoBar();
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>
            ("Animations/" + characters[charCounter] + "/" + characters[charCounter]);
        Debug.Log("Animations/" + characters[charCounter] + "/" + characters[charCounter]);
        animator.SetFloat("Speed", 1);
        characterText.text = characters[charCounter];
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

    public void NextChar()
    {
        if (charCounter + 1 < characters.Length)
            charCounter++;
        ChooseCharacter();
    }

    public void PrevChar()
    {
        if(charCounter - 1 >= 0)
            charCounter--;
        ChooseCharacter();
    }
}
