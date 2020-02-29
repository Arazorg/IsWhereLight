using UnityEngine;

public class SettingsButtons : MonoBehaviour
{
    public GameObject settings;
    public GameObject settingsButton;
    public GameObject interfaceSettings;
    public GameObject menu;
    private bool musicOn;
    private bool effectsOn;
    private AudioManager audioManager;
    private SettingsInfo settingsInfo;
    void Start()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        audioManager = FindObjectOfType<AudioManager>();
        musicOn = settingsInfo.musicOn;
        effectsOn = settingsInfo.effectsOn;
    }

    public void InterfaceSettingsOpen()
    {
        audioManager.Play("ClickUI");
        if (interfaceSettings.activeSelf == false)
        {
            menu.SetActive(false);
            interfaceSettings.SetActive(true);
        }
    }

    public void SettingsPanelClose()
    {
        settingsInfo.SaveSettings();
        audioManager.Play("ClickUI");
        if (settingsButton.activeSelf == false)
        {
            settings.SetActive(false);
            settingsButton.SetActive(true);
        }
    }

    public void MusicOnOff()
    {
        audioManager.Play("ClickUI");
        musicOn = !musicOn;
        if (musicOn)
            audioManager.On("Theme");
        else
            audioManager.Off("Theme");
        settingsInfo.musicOn = musicOn;
    }

    public void EffectsOnOff()
    {
        audioManager.Play("ClickUI");
        effectsOn = !effectsOn;
        if (effectsOn)
            audioManager.On("Effects");
        else
            audioManager.Off("Effects");
        settingsInfo.effectsOn = effectsOn;
    }
}
