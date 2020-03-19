using UnityEngine;
using UnityEngine.UI;

public class SettingsButtons : MonoBehaviour
{
    public GameObject settings;
    public GameObject localization;
    public GameObject secretCode;
    public GameObject interfaceSettings;
    public GameObject menu;
    public Button settingsButton;

    private AudioManager audioManager;
    private SettingsInfo settingsInfo;
    private LocalizationManager localizationManager;

    private bool musicOn;
    private bool effectsOn;
    public static bool IsLocalizationPanelState = false;

    void Start()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        localizationManager = GameObject.Find("LocalizationManager").GetComponent<LocalizationManager>();
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
        if (settingsButton.gameObject.activeSelf == false)
        {
            settings.SetActive(false);
            settingsButton.gameObject.SetActive(true);
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
        settingsInfo.SaveSettings();
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
        settingsInfo.SaveSettings();
    }

    public void SecretCodePanelOpen()
    {
        secretCode.SetActive(true);
    }

    public void SecretCodePanelClose()
    {
        secretCode.SetActive(false);
    }

    public void OpenCloseLocalizationPanel()
    {
        IsLocalizationPanelState = !IsLocalizationPanelState;
        localization.SetActive(IsLocalizationPanelState);
    }

    public void ChangeLanguage(string fileName)
    {
        localizationManager.LoadLocalizedText(fileName);
    }
}