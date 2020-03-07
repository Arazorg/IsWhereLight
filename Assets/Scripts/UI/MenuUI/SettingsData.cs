using UnityEngine;

[System.Serializable]
public class SettingsData
{
    public int level;
    public bool musicOn;
    public bool effectsOn;
    public float[] joystickPosition = new float[2];
    public float[] fireActButtonPosition = new float[2];
    public float[] hpBarPosition = new float[2];
    public float[] maneBarPosition = new float[2];

    public SettingsData(SettingsInfo settingsInfo)
    {
        level = settingsInfo.level;
        musicOn = settingsInfo.musicOn;
        effectsOn = settingsInfo.effectsOn;
        joystickPosition = settingsInfo.joystickPosition;
        fireActButtonPosition = settingsInfo.fireActButtonPosition;
        hpBarPosition = settingsInfo.hpBarPosition;
        maneBarPosition = settingsInfo.maneBarPosition;
    }
}
