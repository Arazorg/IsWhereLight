using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceSettings : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("UI панели меню")]
    [SerializeField] private GameObject menuPanel;

    [Tooltip("UI панели цвета")]
    [SerializeField] private GameObject colorPanel;

    [Tooltip("UI Джойстик")]
    [SerializeField] private GameObject joystick;

    [Tooltip("UI кнопки действия, атаки")]
    [SerializeField] private GameObject fireActButton;

    [Tooltip("Кнопка скила")]
    [SerializeField] private GameObject skillButton;

    [Tooltip("UI кнопки смены оружия")]
    [SerializeField] private GameObject swapWeaponButton;

    [Tooltip("UI кнопки счетчика ФПС")]
    [SerializeField] private GameObject fpsCounterButton;

    [Tooltip("Кнопка динамического джойстика")]
    [SerializeField] private Button dynamicJoystickButton;

    [Tooltip("Кнопка статического джойстика")]
    [SerializeField] private Button staticJoystickButton;

    [Tooltip("Текст подсказок джойстика")]
    [SerializeField] private TextMeshProUGUI hintsText;

    [Tooltip("Текст подсказок стандратных настроек, счетчика фпс")]
    [SerializeField] private TextMeshProUGUI hintsText2;
#pragma warning restore 0649

    private bool IsColorPanelState;
    //Скрипты
    private AudioManager audioManager;
    private SettingsInfo settingsInfo;
    private float hintTimer;
    private float hintTimer2;

    void Start()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        audioManager = FindObjectOfType<AudioManager>();
        hintTimer = float.MaxValue;
        hintTimer2 = float.MaxValue;
        if (MenuButtons.firstRun)
            SetStandart();
        else
            GetCurrentSave();
    }

    void Update()
    {
        if (Time.time > hintTimer)
        {
            hintsText.GetComponent<MovementUI>().MoveToStart();
            hintTimer = float.MaxValue;
        }

        if (Time.time > hintTimer2)
        {
            hintsText2.GetComponent<MovementUI>().MoveToStart();
            hintTimer2 = float.MaxValue;
        }
    }

    public void InterfaceSettingsPanelClose()
    {
        GetCurrentSave();
        audioManager.Play("ClickUI");
        InterfaceDrag.isDraging = false;
        IsColorPanelState = false;
        colorPanel.GetComponent<MovementUI>().MoveToStart();
        menuPanel.GetComponent<MovementUI>().MoveToStart();
        gameObject.GetComponent<MovementUI>().MoveToStart();
    }

    public void SaveSettings()
    {
        audioManager.Play("ClickUI");
        SetPosition();
        settingsInfo.SaveSettings();
        InterfaceSettingsPanelClose();
    }

    public void CounterFPSOnOff()
    {
        audioManager.Play("ClickUI");
        settingsInfo.fpsOn = !settingsInfo.fpsOn;
        if (settingsInfo.fpsOn)
            fpsCounterButton.GetComponent<Image>().color = Color.red;
        else
            fpsCounterButton.GetComponent<Image>().color = Color.white;

        hintsText2.GetComponent<MovementUI>().MoveToEnd();
        if (settingsInfo.fpsOn)
        {
            hintsText2.GetComponent<LocalizedText>().key = "OnFps";
            hintsText2.GetComponent<LocalizedText>().SetLocalization();
        }
        else
        {
            hintsText2.GetComponent<LocalizedText>().key = "OffFps";
            hintsText2.GetComponent<LocalizedText>().SetLocalization();
        }
        hintTimer2 = Time.time + 3f;
    }

    public void SetStandart()
    {
        audioManager.Play("ClickUI");

        SetCurrentColor(Color.white);
        settingsInfo.color = "white";

        joystick.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(SettingsInfo.startPositions["joystickPosition"][0] + 256,
                                SettingsInfo.startPositions["joystickPosition"][1] + 256);
        fireActButton.GetComponent<RectTransform>().anchoredPosition =
            new Vector3(SettingsInfo.startPositions["fireActButtonPosition"][0],
                            SettingsInfo.startPositions["fireActButtonPosition"][1]);
        swapWeaponButton.GetComponent<RectTransform>().anchoredPosition =
            new Vector3(SettingsInfo.startPositions["swapWeaponButtonPosition"][0],
                            SettingsInfo.startPositions["swapWeaponButtonPosition"][1]);
        skillButton.GetComponent<RectTransform>().anchoredPosition =
            new Vector3(SettingsInfo.startPositions["skillButtonPosition"][0],
                            SettingsInfo.startPositions["skillButtonPosition"][1]);

        hintsText2.GetComponent<MovementUI>().MoveToEnd();
        hintsText2.GetComponent<LocalizedText>().key = "StandartInterface";
        hintsText2.GetComponent<LocalizedText>().SetLocalization();
        hintTimer2 = Time.time + 3f;
    }

    public void ColorPanelOpenClose()
    {
        audioManager.Play("ClickUI");
        IsColorPanelState = !IsColorPanelState;
        if (IsColorPanelState)
        {
            hintsText2.GetComponent<MovementUI>().MoveToEnd();
            hintsText2.GetComponent<LocalizedText>().key = "ChangeColorTrancparency";
            hintsText2.GetComponent<LocalizedText>().SetLocalization();
            colorPanel.GetComponent<MovementUI>().MoveToEnd();
        }
        else
        {
            if (hintsText2.GetComponent<LocalizedText>().key == "ChangeColorTrancparency")
                hintsText2.GetComponent<MovementUI>().MoveToStart();
            colorPanel.GetComponent<MovementUI>().MoveToStart();
        }
        hintTimer2 = Time.time + 3f;
    }

    public void SetColor(string color)
    {
        settingsInfo.color = color;
        var curColor = StringToColor(color);
        SetCurrentColor(curColor);
    }

    public void SetCurrentColor(Color curColor)
    {
        joystick.GetComponent<Image>().color = curColor;
        joystick.transform.GetChild(0).GetComponent<Image>().color = curColor;
        fireActButton.GetComponent<Image>().color = curColor;
        swapWeaponButton.GetComponent<Image>().color = curColor;
        skillButton.GetComponent<Image>().color = curColor;

    }

    private void SetPosition()
    {
        audioManager.Play("ClickUI");
        settingsInfo.joystickPosition
            = new float[] { joystick.GetComponent<RectTransform>().anchoredPosition.x - 256,
                                joystick.GetComponent<RectTransform>().anchoredPosition.y - 256};
        settingsInfo.fireActButtonPosition
            = new float[] { fireActButton.GetComponent<RectTransform>().anchoredPosition.x,
                                fireActButton.GetComponent<RectTransform>().anchoredPosition.y};
        settingsInfo.swapWeaponButtonPosition
            = new float[] { swapWeaponButton.GetComponent<RectTransform>().anchoredPosition.x,
                                swapWeaponButton.GetComponent<RectTransform>().anchoredPosition.y};
        settingsInfo.skillButtonPosition
            = new float[] { skillButton.GetComponent<RectTransform>().anchoredPosition.x,
                                skillButton.GetComponent<RectTransform>().anchoredPosition.y};
    }

    private Color StringToColor(string color)
    {
        if (ColorUtility.TryParseHtmlString("#" + color, out Color newColor))
            return newColor;
        else
            return Color.white;
    }

    public void SetJoystick(string type)
    {
        audioManager.Play("ClickUI");
        hintsText.GetComponent<MovementUI>().MoveToEnd();
        if (type == "Dynamic")
        {
            hintsText.GetComponent<LocalizedText>().key = "HintDynamicJoystick";
            hintsText.GetComponent<LocalizedText>().SetLocalization();
            dynamicJoystickButton.GetComponent<Image>().color = Color.red;
            staticJoystickButton.GetComponent<Image>().color = Color.white;
            settingsInfo.joystickType = "Dynamic";
        }
        else if (type == "Static")
        {
            hintsText.GetComponent<LocalizedText>().key = "HintStaticJoystick";
            hintsText.GetComponent<LocalizedText>().SetLocalization();
            dynamicJoystickButton.GetComponent<Image>().color = Color.white;
            staticJoystickButton.GetComponent<Image>().color = Color.red;
            settingsInfo.joystickType = "Static";
        }
        hintTimer = Time.time + 3f;
    }

    private void GetCurrentSave()
    {
        settingsInfo.LoadSettings();
        SetCurrentColor(StringToColor(settingsInfo.color));
        joystick.GetComponent<RectTransform>().anchoredPosition =
            new Vector3(settingsInfo.joystickPosition[0] + 256,
                            settingsInfo.joystickPosition[1] + 256);
        fireActButton.GetComponent<RectTransform>().anchoredPosition =
            new Vector3(settingsInfo.fireActButtonPosition[0],
                            settingsInfo.fireActButtonPosition[1]);
        swapWeaponButton.GetComponent<RectTransform>().anchoredPosition =
            new Vector3(settingsInfo.swapWeaponButtonPosition[0],
                            settingsInfo.swapWeaponButtonPosition[1]);
        skillButton.GetComponent<RectTransform>().anchoredPosition =
            new Vector3(settingsInfo.skillButtonPosition[0],
                            settingsInfo.skillButtonPosition[1]);

        if (settingsInfo.joystickType == "Dynamic")
        {
            dynamicJoystickButton.GetComponent<Image>().color = Color.red;
            fpsCounterButton.GetComponent<Image>().color = Color.white;
        }
        else
        {
            staticJoystickButton.GetComponent<Image>().color = Color.red;
            dynamicJoystickButton.GetComponent<Image>().color = Color.white;
        }
            

        if (settingsInfo.fpsOn)
            fpsCounterButton.GetComponent<Image>().color = Color.red;
        else
            fpsCounterButton.GetComponent<Image>().color = Color.white;
    }
}
