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
    public Button goToGameButton;
    public Button nextCharButton;
    public Button prevCharButton;
    public Button buyButton;
    public Animator animator;
    private CurrentGameInfo currentGameInfo;
    private ProgressInfo progressInfo;
    private CharactersSpec charactersSpec;
    private CharactersSpec.Character charSpec;

    public TextMeshProUGUI characterText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI maneText;
    public TextMeshProUGUI gunText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI priceText;

    private string[] characters;
    public int charCounter;
    public int charPrice;

    void Start()
    {
        charactersSpec = GameObject.Find("LobbyHandler").GetComponent<CharactersSpec>();
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        charactersSpec = GameObject.Find("LobbyHandler").GetComponent<CharactersSpec>();
        progressInfo = GameObject.Find("ProgressHandler").GetComponent<ProgressInfo>();
        charCounter = 0;
        characters = new string[] { "Knight", "Mage" };
        ChooseCharacter();
        moneyText.text = progressInfo.playerMoney.ToString();
    }

    public void ChooseCharacter()
    {
        currentGameInfo.character = characters[charCounter];
        SetSpecChar(currentGameInfo.character);
        SetInfoBar();
        ChoosenCharacterUI();
    }

    public void ChoosenCharacterUI()
    {
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>
            ("Animations/" + characters[charCounter] + "/" + characters[charCounter]);
        animator.SetFloat("Speed", 1);
        characterText.text = characters[charCounter];
        priceText.text = charPrice.ToString();
        buyButton.gameObject.SetActive(!progressInfo.CharacterAccess(characters[charCounter]));
        GameBuyButtonAccess();
    }

    public void GameBuyButtonAccess()
    {
        if (progressInfo.CharacterAccess(characters[charCounter]))
        {
            goToGameButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);
        }   
        else
        {
            goToGameButton.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(true);
        }       
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
            currentGameInfo.startWeapon = charSpec.startWeapon;
            charPrice = charSpec.price;
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
        gunText.text = "Gun : " + currentGameInfo.startWeapon.ToString();
    }

    public void NextChar()
    {
        if (charCounter + 1 < characters.Length)
        {
            charCounter++;
            ChooseCharacter();
        }
    }

    public void PrevChar()
    {
        if(charCounter - 1 >= 0)
        {      
            charCounter--;
            ChooseCharacter();
        }   
    }

    public void BuyCharacter()
    {
        if (progressInfo.playerMoney >= charPrice)
        {
            progressInfo.characters[characters[charCounter]] = true;
            progressInfo.playerMoney -= charPrice;
            moneyText.text = progressInfo.playerMoney.ToString();
            GameBuyButtonAccess();
            progressInfo.SaveProgress();
        }           
    }
}
