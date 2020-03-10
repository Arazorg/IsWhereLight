using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressInfo : MonoBehaviour
{
    public static ProgressInfo instance;
    public int playerMoney;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SaveProgress()
    {
        SaveSystem.SaveProgress(this);
    }

    public void LoadProgress()
    {
        ProgressData progressData = SaveSystem.LoadProgress();
        if (progressData != null)
            playerMoney = progressData.playerMoney;
        else
            SetStartProgress();
    }

    public void SetStartProgress()
    {
        playerMoney = 0;
        SaveProgress();
    }
}
