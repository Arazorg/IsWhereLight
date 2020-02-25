using UnityEngine;

[System.Serializable]
public class SettingsData
{
    public bool musicOn;
    public bool effectsOn;
   // public Transform joystickPosition;
   // public Transform fireActButtonPosition;

    public SettingsData(SettingsInfo settingsInfo)
    {
        musicOn = settingsInfo.musicOn;
        effectsOn = settingsInfo.effectsOn;
       // joystickPosition = settingsInfo.joystickPosition;
       // fireActButtonPosition = settingsInfo.fireActButtonPosition;
    }
}
