using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishOfGameButton : MonoBehaviour
{
    public static int finishGameMoney;
    private ProgressInfo progressInfo;
    private CharacterInfo charInfo;

    void Start()
    {
        progressInfo = GameObject.Find("ProgressHandler").GetComponent<ProgressInfo>();
        progressInfo.playerMoney += finishGameMoney;
        progressInfo.SaveProgress();
    }

    public void GoToMenu()
    {
        SaveSystem.DeleteCurrentGame();
        SceneManager.LoadScene("Menu");
    }
}
