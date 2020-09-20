using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseSettings : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("UI локализации")]
    [SerializeField] private GameObject localizationPanel;

    [Tooltip("UI паузы")]
    [SerializeField] private GameObject pausePanel;

    [Tooltip("Кнопка музыки")]
    [SerializeField] private Button musicButton;

    [Tooltip("Кнопка эффектов")]
    [SerializeField] private Button effectsButton;

    [Tooltip("Кнопка тряски экрана")]
    [SerializeField] private Button vibrationButton;

    [Tooltip("Текст настроек паузы")]
    [SerializeField] private TextMeshProUGUI settingsText;
#pragma warning restore 0649
    public static bool IsLocalizationPanelState;

    private SettingsInfo settingsInfo;
    private AudioManager audioManager;
    private LocalizationManager localizationManager;

    private bool musicOn;
    private bool effectsOn;
    private float timeToHint;
    private readonly float hintTime = 3f;

    void Start()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        localizationManager = GameObject.Find("LocalizationManager").GetComponent<LocalizationManager>();
        musicOn = settingsInfo.musicOn;
        effectsOn = settingsInfo.effectsOn;
        IsLocalizationPanelState = false;
        timeToHint = float.MaxValue;
    }

    void Update()
    {
        if (Time.realtimeSinceStartup > timeToHint)
            settingsText.GetComponent<MovementUI>().SetStart();
    }

    public void MusicOnOff()
    {        
        audioManager.Play("ClickUI");
        musicOn = !musicOn;
        if (musicOn)
        {
            ShowSettingsText("HintMusicOn");
            audioManager.On("Theme");
        }
        else
        {
            ShowSettingsText("HintMusicOff");
            audioManager.Off("Theme");
        }            
        settingsInfo.musicOn = musicOn;
        musicButton.GetComponentInChildren<ButtonImage>().SetSoundsSprite(musicOn);
    }

    public void EffectsOnOff()
    {
        audioManager.Play("ClickUI");
        effectsOn = !effectsOn;
        if (effectsOn)
        {
            ShowSettingsText("HintEffectsOn");
            audioManager.On("Effects");
        }
            
        else
        {
            ShowSettingsText("HintEffectsOff");
            audioManager.Off("Effects");
        }
            
        settingsInfo.effectsOn = effectsOn;
        effectsButton.GetComponentInChildren<ButtonImage>().SetSoundsSprite(effectsOn);
    }

    public void OpenCloseLocalizationPanel()
    {       
        audioManager.Play("ClickUI");
        IsLocalizationPanelState = !IsLocalizationPanelState;
        if (IsLocalizationPanelState)
        {
            localizationPanel.GetComponent<MovementUI>().MoveToEnd();
            pausePanel.GetComponent<MovementUI>().MoveToStart();
            settingsText.GetComponent<MovementUI>().SetStart();
        }
        else
        {
            localizationPanel.GetComponent<MovementUI>().MoveToStart();
            pausePanel.GetComponent<MovementUI>().MoveToEnd();
            settingsText.GetComponent<MovementUI>().MoveToEnd();
        }
    }

    public void ChangeLanguage(string fileName)
    {
        audioManager.Play("ClickUI");
        localizationManager.LoadLocalizedText(fileName);
    }
    
    public void SetStaticJoystick()
    {
        audioManager.Play("ClickUI");
        UISpawner.instance.DeleteJoystick();
        settingsInfo.joystickType = "Static";
        UISpawner.instance.SetUI(true);
        settingsInfo.SaveSettings();
        ShowSettingsText("HintStaticJoystick");
    }

    public void SetDynamicJoystick()
    {
        audioManager.Play("ClickUI");
        UISpawner.instance.DeleteJoystick();
        settingsInfo.joystickType = "Dynamic";
        UISpawner.instance.SetUI(true);
        settingsInfo.SaveSettings();
        ShowSettingsText("HintDynamicJoystick");
    }

    public void OnOffVibration()
    {
        audioManager.Play("ClickUI");
        settingsInfo.isVibration = !settingsInfo.isVibration;
        if(settingsInfo.isVibration)
            ShowSettingsText("HintVibrationOn");
        else
            ShowSettingsText("HintVibrationOff");
        settingsInfo.SaveSettings();
        vibrationButton.GetComponentInChildren<ButtonImage>().SetVibrationSprite(settingsInfo.isVibration);
    }

    public void ExportScreenshot()
    {
        audioManager.Play("ClickUI");
    }

    private void ShowSettingsText(string key)
    {
        localizationPanel.GetComponent<MovementUI>().MoveToStart();
        IsLocalizationPanelState = false;
        pausePanel.GetComponent<MovementUI>().MoveToEnd();
        settingsText.GetComponent<MovementUI>().MoveToEnd();
        settingsText.GetComponent<LocalizedText>().key = key;
        settingsText.GetComponent<LocalizedText>().SetLocalization();
        timeToHint = Time.realtimeSinceStartup + hintTime;
    }
}
