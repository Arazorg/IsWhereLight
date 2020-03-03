using UnityEngine;
using System.Collections.Generic;

public class SettingsInfo : MonoBehaviour
{
    public static SettingsInfo instance;
    public static Dictionary<string, float[]> startPositions = new Dictionary<string, float[]>();

    public bool musicOn;
    public bool effectsOn;
    public float[] joystickPosition = new float[2];
    public float[] fireActButtonPosition = new float[2];
    public float[] hpBarPosition = new float[2];
    public float[] maneBarPosition = new float[2];

    void Awake()
    {
        InitDictionary();

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (MenuButtons.firstRun == true)
        {
            musicOn = true;
            effectsOn = true;
            SetStartPositions();
            SaveSettings();
        }
        else
        {
            LoadSettings();
        }
    }

    public void SaveSettings()
    {
        SaveSystem.SaveSettings(this);
    }

    public void LoadSettings()
    {
        SettingsData settingsData = SaveSystem.LoadSettings();

        musicOn = settingsData.musicOn;
        effectsOn = settingsData.effectsOn;
        joystickPosition = settingsData.joystickPosition;
        fireActButtonPosition = settingsData.fireActButtonPosition;
        hpBarPosition = settingsData.hpBarPosition;
        maneBarPosition = settingsData.maneBarPosition;
    }
    private void InitDictionary()
    {
        startPositions["joystickPosition"] = new float[2] { 0, 0 };
        startPositions["fireActButtonPosition"] = new float[2] { -200, 250 };
        startPositions["hpBarPosition"] = new float[2] { 200, -100 };
        startPositions["maneBarPosition"] = new float[2] { 200, -175 };
    }
    private void SetStartPositions()
    {
        joystickPosition = startPositions["joystickPosition"];
        fireActButtonPosition = startPositions["fireActButtonPosition"];
        hpBarPosition = startPositions["hpBarPosition"];
        maneBarPosition = startPositions["maneBarPosition"];
    }

}
