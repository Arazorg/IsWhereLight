using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseSettings : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("UI локализации")]
    [SerializeField] private GameObject localizationPanel;

    [Tooltip("Панель громкости звуков")]
    [SerializeField] private GameObject soundVolumePanel;

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

    [Tooltip("Слайдер музыки")]
    [SerializeField] private Slider sliderMusic;

    [Tooltip("Слайдер эффектов")]
    [SerializeField] private Slider sliderEffects;
#pragma warning restore 0649
    public static bool IsLocalizationPanelState;

    private SettingsInfo settingsInfo;
    private AudioManager audioManager;
    private LocalizationManager localizationManager;
    private float timeToHint;
    private readonly float hintTime = 3f;

    void Start()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        localizationManager = GameObject.Find("LocalizationManager").GetComponent<LocalizationManager>();
        sliderMusic.value = settingsInfo.musicVolume;
        sliderEffects.value = settingsInfo.effectsVolume;
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
        settingsInfo.musicOn = !settingsInfo.musicOn;
        if (settingsInfo.musicOn)
        {
            soundVolumePanel.GetComponent<MovementUI>().MoveToEnd();
            audioManager.On("Theme");
        }
        else
        {
            if (!settingsInfo.effectsOn)
                soundVolumePanel.GetComponent<MovementUI>().MoveToStart();
            audioManager.Off("Theme");
        }
        settingsText.GetComponent<MovementUI>().MoveToStart();
        musicButton.GetComponentInChildren<ButtonImage>().SetSoundsSprite(settingsInfo.musicOn);
    }

    public void EffectsOnOff()
    {
        audioManager.Play("ClickUI");
        settingsInfo.effectsOn = !settingsInfo.effectsOn;
        if (settingsInfo.effectsOn)
        {
            soundVolumePanel.GetComponent<MovementUI>().MoveToEnd();
            audioManager.On("Effects");
        }          
        else
        {
            if(!settingsInfo.musicOn)
                soundVolumePanel.GetComponent<MovementUI>().MoveToStart();
            audioManager.Off("Effects");
        }
        settingsText.GetComponent<MovementUI>().MoveToStart();
        effectsButton.GetComponentInChildren<ButtonImage>().SetSoundsSprite(settingsInfo.effectsOn);
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
        StartCoroutine(CaptureScreen());
    }

    public IEnumerator CaptureScreen()
    {
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
        yield return new WaitForEndOfFrame();
        ScreenCapture.CaptureScreenshot(Application.persistentDataPath + @"/SomeLevel.png");
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
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

    public void SetMusic()
    {   
        settingsInfo.musicVolume = sliderMusic.value;
    }

    public void SetEffects()
    {
        settingsInfo.effectsVolume = sliderEffects.value;
    }
}
