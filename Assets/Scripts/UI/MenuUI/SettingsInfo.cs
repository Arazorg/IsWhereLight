using UnityEngine;

public class SettingsInfo : MonoBehaviour
{
    public bool musicOn;
    public bool effectsOn;
    //public Transform joystickPosition;
    //public Transform fireActButtonPosition;
    private SettingsInfo settingsInfo;
    void Start()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        if (MenuButtons.firstRun == true)
        {
            musicOn = true;
            effectsOn = true;
            SaveSettings();
            //joystickPosition.position = Vector3.zero;
            //fireActButtonPosition.position = Vector3.zero;
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
        SaveSystem.LoadSettings();

        musicOn = settingsInfo.musicOn;
        effectsOn = settingsInfo.effectsOn;
    }
}
