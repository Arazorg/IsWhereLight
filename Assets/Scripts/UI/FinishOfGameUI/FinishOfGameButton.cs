using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishOfGameButton : MonoBehaviour
{
    public void GoToMenu()
    {
        SaveSystem.DeleteCurrentGame();
        SceneManager.LoadScene("Menu");
    }
}
