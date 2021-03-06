﻿[System.Serializable]
public class SettingsData
{
    public float[] joystickPosition = new float[2];
    public float[] fireActButtonPosition = new float[2];
    public float[] swapWeaponButtonPosition = new float[2];
    public float[] skillButtonPosition = new float[2];
    public string color;
    public string joystickType;
    public string currentLocalization;
    public float musicVolume;
    public float effectsVolume;
    public bool musicOn;
    public bool effectsOn;
    public bool fpsOn;
    public bool isVibration;

    public SettingsData(SettingsInfo settingsInfo)
    {
        currentLocalization = settingsInfo.currentLocalization;
        musicOn = settingsInfo.musicOn;
        effectsOn = settingsInfo.effectsOn;
        musicVolume = settingsInfo.musicVolume;
        effectsVolume = settingsInfo.effectsVolume;
        fpsOn = settingsInfo.fpsOn;
        isVibration = settingsInfo.isVibration;
        color = settingsInfo.color;
        joystickType = settingsInfo.joystickType;
        joystickPosition = settingsInfo.joystickPosition;
        fireActButtonPosition = settingsInfo.fireActButtonPosition;
        swapWeaponButtonPosition = settingsInfo.swapWeaponButtonPosition;
        skillButtonPosition = settingsInfo.skillButtonPosition;
    }
}
