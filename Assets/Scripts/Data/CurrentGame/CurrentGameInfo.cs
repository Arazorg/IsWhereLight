using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentGameInfo : MonoBehaviour
{
    public static CurrentGameInfo instance;

    public string character;
    public string skin;
    public string startWeapon;
    public string characterType;
    public string[] currentAmplifications;
    public int challengeNumber;
    public int currentWave;
    public int countResurrect;
    public float skillTime;
    public bool wildMode;
    public bool isLobby;

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void SetStartParametrs()
    {
        currentWave = 0;
        countResurrect = 1;
    }

    public void SetIsLobbyState(bool isLobby)
    {
        this.isLobby = isLobby;
    }
}
