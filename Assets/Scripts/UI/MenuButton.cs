using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public static bool firstPlay;
    public Button continueButton;
    public Camera mainCam;
    private int level;

    void Start()
    {
        level = SaveSystem.LoadChar().level;
        continueButton.GetComponentInChildren<Text>().text += $"  {level}";
    }

    public void NewGame()
    {
        firstPlay = true;
        SceneManager.LoadScene("Game");
    }

    public void ContinueGame()
    {
        firstPlay = false;
        SceneManager.LoadScene("Game");
    }

    public void LinkToVk()
    {
        Application.OpenURL("https://vk.com/arazorg");
    }

    public void LinkToTwitter()
    {
        Application.OpenURL("https://twitter.com/arazorg");
    }

    public void MusicOnOff()
    {
        mainCam.GetComponent<AudioSource>().mute = !mainCam.GetComponent<AudioSource>().mute;
    }
}
