using UnityEngine;

public class CurrentGameInfo : MonoBehaviour
{
    public static CurrentGameInfo instance;

    public string character;
    public string skin;
    public string startWeapon;
    public string characterType;
    public string[] currentAmplifications;
    public string challengeName;
    public int currentWave;
    public int countResurrect;
    public float skillTime;
    public bool wildMode;
    public bool isLobby;
    public bool isWin;

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
