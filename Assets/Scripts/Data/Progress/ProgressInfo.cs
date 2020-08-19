﻿using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class ProgressInfo : MonoBehaviour
{
    public static ProgressInfo instance;

    public Dictionary<string, bool> characters;
    public Dictionary<string, int> secretCodes;
    public Dictionary<string, bool> achivments;
    public Dictionary<string, bool> weapons;

    public int playerMoney;
    public int countKilledEnemies;
    public int countShoots;

    public int currentCountKilledEnemies;
    public int currentCountShoots;

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

    private void Init(ProgressData data)
    {
        if (data.characters != null)
            characters = data.characters;
        if (data.secretCodes != null)
            secretCodes = data.secretCodes;
        if (data.achivments != null)
            achivments = data.achivments;
        if (data.weapons != null)
            weapons = data.weapons;

        playerMoney = data.playerMoney;
        countKilledEnemies = data.countKilledEnemies;
        countShoots = data.countShoots;
    }

    public void SaveProgress()
    {
        string json = JsonUtility.ToJson(this);
        json += $"\n{JsonConvert.SerializeObject(characters)}";
        json += $"\n{JsonConvert.SerializeObject(secretCodes)}";
        json += $"\n{JsonConvert.SerializeObject(achivments)}";
        json += $"\n{JsonConvert.SerializeObject(weapons)}";
        NewSaveSystem.Save("progressInfo", json);
    }

    public void LoadProgress()
    {
        SetStartProgress();
        var progressString = NewSaveSystem.Load("progressInfo");

        if (progressString != null)
        {
            var strings = progressString.Split(new char[] { '\n' });
            ProgressData data = JsonUtility.FromJson<ProgressData>(strings[0]);

            for (int i = 0; i < strings.Length; i++)
            {
                switch (i)
                {
                    case 1:
                        data.characters = JsonConvert.DeserializeObject<Dictionary<string, bool>>(strings[1]);
                        break;
                    case 2:
                        data.secretCodes = JsonConvert.DeserializeObject<Dictionary<string, int>>(strings[2]);
                        break;
                    case 3:
                        data.achivments = JsonConvert.DeserializeObject<Dictionary<string, bool>>(strings[3]);
                        break;
                    case 4:
                        data.weapons = JsonConvert.DeserializeObject<Dictionary<string, bool>>(strings[4]);
                        break;
                    default:
                        break;
                }
                Init(data);
            }
        }
    }

    public void SetStartProgress()
    {
        CharactersInit();
        SecretCodesInit();
        AchivmentsInit();
        WeaponsInit();
    }

    private void CharactersInit()
    {
        characters = new Dictionary<string, bool>
        {
            { "Legionnaire", true },
            { "Mage", false },
            { "Archer", false },
            { "Shooter", false },
            { "Doctor", false },
            { "Engineer", false }
        };
    }

    private void SecretCodesInit()
    {
        secretCodes = new Dictionary<string, int>
        {
            { "arazorg", 9999 },
            { "valerick", 10000 },
            { "banyuk", 10001 }
        };
    }

    private void AchivmentsInit()
    {
        achivments = new Dictionary<string, bool>
        {
            {"firstPlay", false },
            {"firstNewCharacter", false }
        };
    }

    private void WeaponsInit()
    {
        weapons = new Dictionary<string, bool>
        {
            {"pistol", true },
            {"staff", true }
        };
    }

    public bool CharacterAccess(string character)
    {
        if (characters[character])
            return true;
        else
            return false;
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
