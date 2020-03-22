using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressInfo : MonoBehaviour
{
    public static ProgressInfo instance;

    public Dictionary<string, bool> characters = new Dictionary<string, bool>();
    public Dictionary<string, int> secretCodes = new Dictionary<string, int>();
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
            secretCodes = progressData.secretCodes;
        }
        else
        {
            SetStartProgress();
        }
    }

    public void SetStartProgress()
    {
        playerMoney = 0;
        CharactersInit();
        SecretCodesInit();
        SaveProgress();
    }

    public bool CharacterAccess(string character)
    {
        if (characters[character])
            return true;
        else
            return false;
    }

    private void CharactersInit()
    {
        characters.Add("Knight", true);
        characters.Add("Mage", false);
    }

    private void SecretCodesInit()
    {
        secretCodes.Add("arazorg", 999);
        secretCodes.Add("valerick", 1000);
    }

    public int CheckSecretCode(string code)
    {
        int money;
        if (secretCodes.ContainsKey(code))
        {
            if (secretCodes[code] != 0)
            {
                playerMoney += secretCodes[code];
                money = secretCodes[code];
                secretCodes[code] = 0;
                SaveProgress();
                return money;
            }
            return 0;
        }
        return -1;
    }
}
