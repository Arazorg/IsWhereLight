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

    [Tooltip("UI панели меню")]
    [SerializeField] private GameObject menuPanel;

    [Tooltip("Кнопка настроек")]
    [SerializeField] private Button settingsButton;

    [Tooltip("Поля ввода секретного кода")]
    [SerializeField] private InputField secretCodeField;

    [Tooltip("Текст полученных денег")]
    [SerializeField] private TextMeshProUGUI moneyPlusText;
#pragma warning restore 0649

    //Скрипты
    private AudioManager audioManager;
    private SettingsInfo settingsInfo;
    private LocalizationManager localizationManager;
    private ProgressInfo progressInfo;

    //Переменные состояния игры
    public static bool IsLocalizationPanelState = false;
    public static bool IsSecretPanelState = false;
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
    }

    public void InterfaceSettingsOpen()
    {
        audioManager.Play("ClickUI");
        if (interfaceSettingsPanel.activeSelf == false)
        {
            menuPanel.GetComponent<MenuButtons>().AllPanelClose();
            menuPanel.SetActive(false);
            interfaceSettingsPanel.SetActive(true);
        }
    }

    public void SettingsPanelClose()
    {
        settingsInfo.SaveSettings();
        audioManager.Play("ClickUI");
        if (settingsButton.gameObject.activeSelf == false)
        {
            MenuButtons.IsSettingPanelState = !MenuButtons.IsSettingPanelState;
            gameObject.SetActive(MenuButtons.IsSettingPanelState);
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

    public void SecretCodePanelOpenClose()
    {
        audioManager.Play("ClickUI");
        IsSecretPanelState = !IsSecretPanelState;
        secretCodePanel.SetActive(IsSecretPanelState);

        IsLocalizationPanelState = false;
        localizationPanel.SetActive(IsLocalizationPanelState);       
    }

    public void LocalizationPanelOpenClose()
    {
        audioManager.Play("ClickUI");
        IsLocalizationPanelState = !IsLocalizationPanelState;
        localizationPanel.SetActive(IsLocalizationPanelState);

        IsSecretPanelState = false;
        secretCodePanel.SetActive(IsSecretPanelState);     
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
            moneyPlusText.fontSize = 64;
            moneyPlusText.text = "+" + money.ToString();
        }
        else if (money == 0)
        {
            moneyPlusText.fontSize = 36;
            moneyPlusText.text = "You already use this code";
        }
        else
        {
            moneyPlusText.fontSize = 36;
            moneyPlusText.text = "This code does not exist";
        }

    }
}