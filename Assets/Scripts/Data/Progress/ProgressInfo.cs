using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class ProgressInfo : MonoBehaviour
{
    public static ProgressInfo instance;

    public Dictionary<string, bool> characters;
    public Dictionary<string, int> secretCodes;
    public Dictionary<string, bool> achivments;
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
        characters = data.characters;
        secretCodes = data.secretCodes;
        achivments = data.achivments;
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
            data.characters = JsonConvert.DeserializeObject<Dictionary<string, bool>>(strings[1]);
            data.secretCodes = JsonConvert.DeserializeObject<Dictionary<string, int>>(strings[2]);
            Init(data);
        }
    }

    public void SetStartProgress()
    {
        CharactersInit();
        SecretCodesInit();
        AchivmentsInit();
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
