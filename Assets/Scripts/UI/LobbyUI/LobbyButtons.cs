using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyButtons : MonoBehaviour
{
    public Button goToGameButton;
    public Button buyButton;
    public Animator animator;

    public TextMeshProUGUI characterText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI maneText;
    public TextMeshProUGUI gunText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI priceText;

    private CurrentGameInfo currentGameInfo;
    private ProgressInfo progressInfo;
    private CharactersSpec charactersSpec;
    private CharactersSpec.Character charSpec;

    public RuntimeAnimatorController[] charactersAnimations;

    public int charCounter;
    public int charPrice;

    void Start()
    {
        charactersSpec = GameObject.Find("LobbyHandler").GetComponent<CharactersSpec>();
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        charactersSpec = GameObject.Find("LobbyHandler").GetComponent<CharactersSpec>();
        progressInfo = GameObject.Find("ProgressHandler").GetComponent<ProgressInfo>();
        charCounter = 0;

        ChooseCharacter();
        moneyText.text = progressInfo.playerMoney.ToString();
    }

    public void ChooseCharacter()
    {
        currentGameInfo.character = charactersAnimations[charCounter].name;
        SetSpecChar(currentGameInfo.character);
        SetInfoBar();
        ChoosenCharacterUI();
    }

    public void ChoosenCharacterUI()
    {
        animator.runtimeAnimatorController = charactersAnimations[charCounter];
        animator.SetFloat("Speed", 1);
        characterText.text = charactersAnimations[charCounter].name;
        priceText.text = charPrice.ToString();
        buyButton.gameObject.SetActive(!progressInfo.CharacterAccess(charactersAnimations[charCounter].name));
        GameBuyButtonAccess();
    }

    public void GameBuyButtonAccess()
    {
        if (progressInfo.CharacterAccess(charactersAnimations[charCounter].name))
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
        if (charCounter + 1 < charactersAnimations.Length)
        {
            charCounter++;
            ChooseCharacter();
        }
    }

    public void PrevChar()
    {
        if (charCounter - 1 >= 0)
        {
            charCounter--;
            ChooseCharacter();
        }
    }

    public void BuyCharacter()
    {
        if (progressInfo.playerMoney >= charPrice)
        {
            progressInfo.characters[charactersAnimations[charCounter].name] = true;
            progressInfo.playerMoney -= charPrice;
            moneyText.text = progressInfo.playerMoney.ToString();
            GameBuyButtonAccess();
            progressInfo.SaveProgress();
        }
    }
}
