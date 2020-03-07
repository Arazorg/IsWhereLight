using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentGameInfo : MonoBehaviour
{
    public static CurrentGameInfo instance;

    public bool wildMode;
    public int level;
    public int startMoney;
    public int maxHealth;
    public int maxMane;
    public string startGun;
    public string skin;

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
        SetStandartGame();
    }

    private void SetStandartGame()
    {
        wildMode = false;
        level = 1;
        startMoney = 0;
        maxHealth = 5;
        maxMane = 100;
        startGun = "Staff";
        skin = "KhightSkin";
    }
}
