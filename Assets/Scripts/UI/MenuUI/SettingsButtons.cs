using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButtons : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("UI панели локализации")]
    [SerializeField] private GameObject localizationPanel;

    [Tooltip("UI панели секретного кода")]
    [SerializeField] private GameObject secretCodePanel;

    [Tooltip("UI панели настроек интерфейса")]
    [SerializeField] private GameObject interfaceSettingsPanel;

    [Tooltip("UI панели благодарностей")]
    [SerializeField] private GameObject thanksPanel;

    [Tooltip("UI панели громкости музыки")]
    [SerializeField] private GameObject musicVolumePanel;

    [Tooltip("UI панели громкости эффектов")]
    [SerializeField] private GameObject effectsVolumePanel;

    [Tooltip("UI панели меню")]
    [SerializeField] private GameObject menuPanel;

    [Tooltip("Кнопка настроек")]
    [SerializeField] private Button settingsButton;

    [Tooltip("Поля ввода секретного кода")]
    [SerializeField] private InputField secretCodeField;

    [Tooltip("Текст полученных денег")]
    [SerializeField] private TextMeshProUGUI moneyPlusText;

    [Tooltip("Изображение монетки")]
    [SerializeField] private Image moneyImage;

    [Tooltip("Кнопка музыки")]
    [SerializeField] private Button musicButton;

    [Tooltip("Кнопка эффектов")]
    [SerializeField] private Button effectsButton;

    [Tooltip("Слайдер музыки")]
    [SerializeField] private Slider sliderMusic;

    [Tooltip("Слайдер эффектов")]
    [SerializeField] private Slider sliderEffects;

    [Tooltip("Кнопка выхода")]
    [SerializeField] private Button exitButton;
#pragma warning restore 0649

    public bool IsLocalizationPanelState = false;
    public bool IsSecretPanelState = false;
    public bool IsThanksPanelState = false;
    public bool IsInterfacePanelState = false;

    private AudioManager audioManager;
    private SettingsInfo settingsInfo;
    private LocalizationManager localizationManager;
    private ProgressInfo progressInfo;

    private bool musicOn;
    private bool effectsOn;

    void Start()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        localizationManager = GameObject.Find("LocalizationManager").GetComponent<LocalizationManager>();
        progressInfo = GameObject.Find("ProgressHandler").GetComponent<ProgressInfo>();
        audioManager = FindObjectOfType<AudioManager>();

        musicOn = settingsInfo.musicOn;
        effectsOn = settingsInfo.effectsOn;       

        sliderMusic.value = settingsInfo.musicVolume;
        sliderEffects.value = settingsInfo.effectsVolume;
        moneyImage.gameObject.SetActive(false);
    }

    public void InterfaceSettingsOpen()
    {
        audioManager.Play("ClickUI");
        InterfaceDrag.isDraging = true;       
        SettingsPanelClose();
        menuPanel.GetComponent<MenuButtons>().AllPanelHide();
        menuPanel.GetComponent<MovementUI>().MoveToEnd();
        interfaceSettingsPanel.GetComponent<MovementUI>().MoveToEnd();
        IsInterfacePanelState = true;
    }

    public void ThanksPanelOpenClose()
    {
        IsThanksPanelState = !IsThanksPanelState;
        audioManager.Play("ClickUI");
        if (IsThanksPanelState)
        {
            SettingsPanelClose();
            menuPanel.GetComponent<MenuButtons>().AllPanelHide();
            thanksPanel.GetComponent<MovementUI>().MoveToEnd();
            exitButton.GetComponent<MovementUI>().MoveToStart();
        }
        else
        {
            menuPanel.GetComponent<MenuButtons>().SettingsPanelOpenClose();
            thanksPanel.GetComponent<MovementUI>().MoveToStart();
        }
    }

    public void SettingsPanelClose()
    {
        audioManager.Play("ClickUI");
        GetComponent<MovementUI>().MoveToStart();
        secretCodePanel.GetComponent<MovementUI>().MoveToStart();
        localizationPanel.GetComponent<MovementUI>().MoveToStart();
        musicVolumePanel.GetComponent<MovementUI>().MoveToStart();
        effectsVolumePanel.GetComponent<MovementUI>().MoveToStart();     
    }

    public void MusicOnOff()
    {
        audioManager.Play("ClickUI");
        musicOn = !musicOn;
        if (musicOn)
        {
            musicVolumePanel.GetComponent<MovementUI>().MoveToEnd();
            audioManager.On("Theme");
        }
        else
        {
            musicVolumePanel.GetComponent<MovementUI>().MoveToStart();
            audioManager.Off("Theme");
        }

        settingsInfo.musicOn = musicOn;
        settingsInfo.SaveSettings();
        musicButton.GetComponentInChildren<ButtonImage>().SetSoundsSprite(musicOn);
    }

    public void EffectsOnOff()
    {
        audioManager.Play("ClickUI");
        effectsOn = !effectsOn;
        if (effectsOn)
        {
            effectsVolumePanel.GetComponent<MovementUI>().MoveToEnd();
            audioManager.On("Effects");
        }
        else
        {
            effectsVolumePanel.GetComponent<MovementUI>().MoveToStart();
            audioManager.Off("Effects");
        }

        settingsInfo.effectsOn = effectsOn;
        settingsInfo.SaveSettings();
        effectsButton.GetComponentInChildren<ButtonImage>().SetSoundsSprite(effectsOn);
    }

    public void SecretCodePanelOpenClose()
    {
        audioManager.Play("ClickUI");
        IsSecretPanelState = !IsSecretPanelState;
        if (IsSecretPanelState)
        {
            secretCodePanel.GetComponent<MovementUI>().MoveToEnd();
            effectsVolumePanel.GetComponent<MovementUI>().MoveToStart();
            musicVolumePanel.GetComponent<MovementUI>().MoveToStart();
        }
        else
        {
            secretCodePanel.GetComponent<MovementUI>().MoveToStart();
            if (effectsOn)
                effectsVolumePanel.GetComponent<MovementUI>().MoveToEnd();
            if (musicOn)
                musicVolumePanel.GetComponent<MovementUI>().MoveToEnd();
        }
        IsLocalizationPanelState = false;
        localizationPanel.GetComponent<MovementUI>().MoveToStart();
        moneyPlusText.text = "";
        secretCodeField.text = "";
        moneyImage.gameObject.SetActive(false);

    }

    public void LocalizationPanelOpenClose()
    {
        audioManager.Play("ClickUI");
        IsLocalizationPanelState = !IsLocalizationPanelState; //ошибка при выходе в меню из лобби
        if (IsLocalizationPanelState)
        {
            localizationPanel.GetComponent<MovementUI>().MoveToEnd();
            effectsVolumePanel.GetComponent<MovementUI>().MoveToStart();
            musicVolumePanel.GetComponent<MovementUI>().MoveToStart();
        }          
        else
        {
            localizationPanel.GetComponent<MovementUI>().MoveToStart();
            if (musicOn)
                musicVolumePanel.GetComponent<MovementUI>().MoveToEnd();
            if (effectsOn)
                effectsVolumePanel.GetComponent<MovementUI>().MoveToEnd();
            
        }
            
        IsSecretPanelState = false;
        secretCodePanel.GetComponent<MovementUI>().MoveToStart();
        moneyPlusText.text = "";
        secretCodeField.text = "";
        moneyImage.gameObject.SetActive(false);
    }

    public void ChangeLanguage(string fileName)
    {
        audioManager.Play("ClickUI");
        localizationManager.LoadLocalizedText(fileName);
    }

    public void CheckSecretCode()
    {
        int money;
        audioManager.Play("ClickUI");
        money = progressInfo.CheckSecretCode(secretCodeField.text);
        if (money != 0 && money != -1)
        {
            moneyPlusText.text = "+" + money.ToString();
            moneyImage.gameObject.SetActive(true);
        }
        else if (money == 0)
        {
            moneyPlusText.GetComponent<LocalizedText>().key = "AlreadyUseCode";
            moneyPlusText.GetComponent<LocalizedText>().SetLocalization();
            moneyImage.gameObject.SetActive(false);
        }
        else
        {
            moneyPlusText.GetComponent<LocalizedText>().key = "CodeDontExist";
            moneyPlusText.GetComponent<LocalizedText>().SetLocalization();
            moneyImage.gameObject.SetActive(false);
        }
    }

    public void CheckMusicEffectsPanels()
    {
        if (musicOn)
            musicVolumePanel.GetComponent<MovementUI>().MoveToEnd();
        else
            musicVolumePanel.GetComponent<MovementUI>().MoveToStart();

        if (effectsOn)
            effectsVolumePanel.GetComponent<MovementUI>().MoveToEnd();
        else
            effectsVolumePanel.GetComponent<MovementUI>().MoveToStart();
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