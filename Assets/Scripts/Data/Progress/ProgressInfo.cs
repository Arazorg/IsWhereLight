using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressInfo : MonoBehaviour
{
    public static ProgressInfo instance;
    public Dictionary<string, bool> characters = new Dictionary<string, bool>();
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
        {
            playerMoney = progressData.playerMoney;
            characters = progressData.characters;
        }
        else
        {
            Debug.Log("!!!!!!!!!!");
            SetStartProgress();
        }
           
    }

    public void SetStartProgress()
    {
        playerMoney = 0;
        characters.Add("Knight", true);
        characters.Add("Mage", false);
        SaveProgress();
    }

    public bool CharacterAccess(string character)
    {
        if (characters[character])
            return true;
        else
            return false;
    }
}
