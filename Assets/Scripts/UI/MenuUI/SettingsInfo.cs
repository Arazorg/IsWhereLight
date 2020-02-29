using UnityEngine;

public class SettingsInfo : MonoBehaviour
{
    public static SettingsInfo instance;
    public bool musicOn;
    public bool effectsOn;
    //public Transform joystickPosition;
    //public Transform fireActButtonPosition;
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
        SettingsData settingsData = SaveSystem.LoadSettings();

        musicOn = settingsData.musicOn;
        effectsOn = settingsData.effectsOn;
    }
}
