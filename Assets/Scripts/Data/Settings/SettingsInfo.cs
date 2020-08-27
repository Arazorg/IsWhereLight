using UnityEngine;
using System.Collections.Generic;

public class SettingsInfo : MonoBehaviour
{
    public static SettingsInfo instance;
    public static Dictionary<string, float[]> startPositions = new Dictionary<string, float[]>();

    public string currentLocalization;
    public bool musicOn;
    public bool effectsOn;
    public bool fpsOn;
    public float[] joystickPosition = new float[2];
    public float[] fireActButtonPosition = new float[2];
    public float[] swapWeaponButtonPosition = new float[2];
    public string color;
    public string joystickType;

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

    private void Init(SettingsData data)
    {
        currentLocalization = data.currentLocalization;
        musicOn = data.musicOn;
        effectsOn = data.effectsOn;
        fpsOn = data.fpsOn;
        joystickPosition = data.joystickPosition;
        fireActButtonPosition = data.fireActButtonPosition;
        swapWeaponButtonPosition = data.swapWeaponButtonPosition;
        color = data.color;
        joystickType = data.joystickType;
    }

    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(this);
        NewSaveSystem.Save("settings", json);
    }

    public void LoadSettings()
    {
        SetStartSettings();
        var settingsString = NewSaveSystem.Load("settings");
        if (settingsString != null)
        {
            SettingsData saveObject = JsonUtility.FromJson<SettingsData>(settingsString);
            Init(saveObject);
        };
    }

    public void SetStartSettings()
    {
        InitDictionary();
        currentLocalization = "localizedText_en";
        SetStartPositions();
        musicOn = true;
        effectsOn = true;
        fpsOn = true;
        color = "white";
        joystickType = "Dynamic";
    }


    private void InitDictionary()
    {
        startPositions["joystickPosition"] = new float[2] { 0, 0 };
        startPositions["fireActButtonPosition"] = new float[2] { -200, 250 };
        startPositions["hpBarPosition"] = new float[2] { 200, -100 };
        startPositions["maneBarPosition"] = new float[2] { 200, -175 };
        startPositions["swapWeaponButtonPosition"] = new float[2] { -200, 0 };
    }


    private void SetStartPositions()
    {
        joystickPosition = startPositions["joystickPosition"];
        fireActButtonPosition = startPositions["fireActButtonPosition"];
        swapWeaponButtonPosition = startPositions["swapWeaponButtonPosition"];
    }
}
