using UnityEngine;
using System.Collections.Generic;

public class SettingsInfo : MonoBehaviour
{
    public static SettingsInfo instance;
    public static Dictionary<string, float[]> startPositions = new Dictionary<string, float[]>();

    public string currentLocalization;
    public bool musicOn;
    public bool effectsOn;
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

    public void SaveSettings()
    {
        SaveSystem.SaveSettings(this);
    }

    public void LoadSettings()
    {
        SettingsData settingsData = SaveSystem.LoadSettings();
        if (settingsData != null)
        {
            currentLocalization = settingsData.currentLocalization;
            musicOn = settingsData.musicOn;
            effectsOn = settingsData.effectsOn;
            joystickPosition = settingsData.joystickPosition;
            fireActButtonPosition = settingsData.fireActButtonPosition;
            swapWeaponButtonPosition = settingsData.swapWeaponButtonPosition;
            color = settingsData.color;
            joystickType = settingsData.joystickType;
        }
        else
            SetStartSettings();
    }

    public void InitDictionary()
    {
        startPositions["joystickPosition"] = new float[2] { 0, 0 };
        startPositions["fireActButtonPosition"] = new float[2] { -200, 250 };
        startPositions["hpBarPosition"] = new float[2] { 200, -100 };
        startPositions["maneBarPosition"] = new float[2] { 200, -175 };
        startPositions["swapWeaponButtonPosition"] = new float[2] { -200, 0 };
    }

    public void SetStartSettings()
    {
        currentLocalization = "localizedText_en";
        Debug.Log(currentLocalization);
        musicOn = true;
        effectsOn = true;
        SetStartPositions();
        SaveSettings();
        color = "white";
        joystickType = "Dynamic";
    }

    private void SetStartPositions()
    {
        joystickPosition = startPositions["joystickPosition"];
        fireActButtonPosition = startPositions["fireActButtonPosition"];
        swapWeaponButtonPosition = startPositions["swapWeaponButtonPosition"];
    }
}
