using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyButtons : MonoBehaviour
{
    public Button goToMenuButton;
    private CurrentGameInfo currentGameInfo;

    void Start()
    {
        currentGameInfo = GameObject.Find("CurrentGameHandler").GetComponent<CurrentGameInfo>();
        currentGameInfo.SetStandartParametrs();
    }

    public void ChooseCharacter()
    {
        currentGameInfo.character = EventSystem.current.currentSelectedGameObject.name;
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
}
