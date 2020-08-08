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
    private bool IsLocalizationPanelState = false;
    private bool IsSecretPanelState = false;
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
        SettingsPanelClose();
        menuPanel.GetComponent<MenuButtons>().AllPanelHide();
        menuPanel.GetComponent<MovementUI>().MoveToEnd();
        interfaceSettingsPanel.GetComponent<MovementUI>().MoveToEnd();
    }

    public void SettingsPanelClose()
    {
        settingsInfo.SaveSettings();
        audioManager.Play("ClickUI");
        settingsButton.GetComponent<MovementUI>().MoveToEnd();
        gameObject.GetComponent<MovementUI>().MoveToStart();
        secretCodePanel.GetComponent<MovementUI>().MoveToStart();
        localizationPanel.GetComponent<MovementUI>().MoveToStart();
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
        if (IsSecretPanelState)
            secretCodePanel.GetComponent<MovementUI>().MoveToEnd();
        else
            secretCodePanel.GetComponent<MovementUI>().MoveToStart();
        IsLocalizationPanelState = false;
        localizationPanel.GetComponent<MovementUI>().MoveToStart();
        moneyPlusText.text = "";

    }

    public void LocalizationPanelOpenClose()
    {
        audioManager.Play("ClickUI");
        IsLocalizationPanelState = !IsLocalizationPanelState; //ошибка при выходе в меню из лобби
        if (IsLocalizationPanelState)
            localizationPanel.GetComponent<MovementUI>().MoveToEnd();
        else
            localizationPanel.GetComponent<MovementUI>().MoveToStart();
        IsSecretPanelState = false;
        secretCodePanel.GetComponent<MovementUI>().MoveToStart();
        moneyPlusText.text = "";
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
        }
        else if (money == 0)
        {
            moneyPlusText.GetComponent<LocalizedText>().key = "AlreadyUseCode";
            moneyPlusText.GetComponent<LocalizedText>().SetLocalization();
        }
        else
        {
            moneyPlusText.GetComponent<LocalizedText>().key = "CodeDontExist";
            moneyPlusText.GetComponent<LocalizedText>().SetLocalization();
        }

    }
}