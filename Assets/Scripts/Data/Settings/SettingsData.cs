[System.Serializable]
public class SettingsData
{
    public string currentLocalization;
    public bool musicOn;
    public bool effectsOn;
    public bool fpsOn;
    public string color;
    public string joystickType;

    public float[] joystickPosition = new float[2];
    public float[] fireActButtonPosition = new float[2];
    public float[] swapWeaponButtonPosition = new float[2];
    public float[] skillButtonPosition = new float[2];
    
    public SettingsData(SettingsInfo settingsInfo)
    {
        currentLocalization = settingsInfo.currentLocalization;
        musicOn = settingsInfo.musicOn;
        effectsOn = settingsInfo.effectsOn;
        fpsOn = settingsInfo.fpsOn;
        color = settingsInfo.color;
        joystickType = settingsInfo.joystickType;

        joystickPosition = settingsInfo.joystickPosition;
        fireActButtonPosition = settingsInfo.fireActButtonPosition;
        swapWeaponButtonPosition = settingsInfo.swapWeaponButtonPosition;
        skillButtonPosition = settingsInfo.skillButtonPosition;
    }
}
