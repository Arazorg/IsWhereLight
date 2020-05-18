using UnityEngine;

[System.Serializable]
public class SettingsData
{
    public string currentLocalization;
    public bool musicOn;
    public bool effectsOn;
    public float[] joystickPosition = new float[2];
    public float[] fireActButtonPosition = new float[2];
    public string color;

    public SettingsData(SettingsInfo settingsInfo)
    {
        currentLocalization = settingsInfo.currentLocalization;
        musicOn = settingsInfo.musicOn;
        effectsOn = settingsInfo.effectsOn;
        joystickPosition = settingsInfo.joystickPosition;
        fireActButtonPosition = settingsInfo.fireActButtonPosition;
        color = settingsInfo.color;
    }
}
