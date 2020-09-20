using UnityEngine;
using System.Collections.Generic;

public class SettingsInfo : MonoBehaviour
{
    public static SettingsInfo instance;
    public static Dictionary<string, float[]> startPositions = new Dictionary<string, float[]>();

    public float[] joystickPosition = new float[2];
    public float[] fireActButtonPosition = new float[2];
    public float[] swapWeaponButtonPosition = new float[2];
    public float[] skillButtonPosition = new float[2];
    public string currentLocalization;
    public string color;
    public string joystickType;
    public bool musicOn;
    public bool effectsOn;
    public bool fpsOn;
    public bool isVibration;

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
        isVibration = data.isVibration;
        color = data.color;
        joystickType = data.joystickType;
        joystickPosition = data.joystickPosition;
        fireActButtonPosition = data.fireActButtonPosition;
        swapWeaponButtonPosition = data.swapWeaponButtonPosition;

        if (data.skillButtonPosition != null)
            skillButtonPosition = data.skillButtonPosition;
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
        }
    }

    public void SetStartSettings()
    {
        InitDictionary();
        SetStartPositions();
        currentLocalization = "localizedText_en";
        musicOn = true;
        effectsOn = true;
        fpsOn = true;
        isVibration = true;
        color = "white";
        joystickType = "Dynamic";
    }

    private void InitDictionary()
    {
        startPositions["joystickPosition"] = new float[2] { 0, 0 };
        startPositions["fireActButtonPosition"] = new float[2] { -425, 275 };
        startPositions["swapWeaponButtonPosition"] = new float[2] { -185, -100 };
        startPositions["skillButtonPosition"] = new float[2] { -185, 150 };
        startPositions["hpBarPosition"] = new float[2] { 200, -100 };
        startPositions["maneBarPosition"] = new float[2] { 200, -175 };
    }

    private void SetStartPositions()
    {
        joystickPosition = startPositions["joystickPosition"];
        fireActButtonPosition = startPositions["fireActButtonPosition"];
        swapWeaponButtonPosition = startPositions["swapWeaponButtonPosition"];
        skillButtonPosition = startPositions["skillButtonPosition"];
    }
}
