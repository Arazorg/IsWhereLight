using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpawner : MonoBehaviour
{
    public static UISpawner instance;

#pragma warning disable 0649
    [Tooltip("Динамический джойстик")]
    [SerializeField] private GameObject dynamicJoystick;

    [Tooltip("Статический джойстик")]
    [SerializeField] private GameObject staticJoystick;

    [Tooltip("Кнопка стрельбы и действий")]
    [SerializeField] private Button fireActButton;

    [Tooltip("Кнопка смены оружия")]
    [SerializeField] private Button swapWeaponButton;
#pragma warning restore 0649

    private SettingsInfo settingsInfo;
    private GameObject joystick;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void SetUI()
    {
        CreateJoystick();
        SetPosition();
        SetColor();
    }

    private void CreateJoystick()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        if (settingsInfo.joystickType == "Dynamic")
            joystick = Instantiate(dynamicJoystick, GameObject.Find("Canvas").transform.Find("CharacterControlUI"));
        else
            joystick = Instantiate(staticJoystick, GameObject.Find("Canvas").transform.Find("CharacterControlUI"));
        joystick.name = "Joystick";
        joystick.transform.SetAsFirstSibling();
    }

    private void SetPosition()
    {
        var joystickPosition = new Vector2(settingsInfo.joystickPosition[0], settingsInfo.joystickPosition[1]);
        var joystickRectTransform = joystick.GetComponent<RectTransform>();
        joystickRectTransform.anchorMin = Vector2.zero;
        joystickRectTransform.anchorMax = Vector2.zero;
        if (settingsInfo.joystickType == "Dynamic")
            joystickRectTransform.anchoredPosition = joystickPosition;
        else
            joystickRectTransform.anchoredPosition = joystickPosition + new Vector2(256,256);
        fireActButton.GetComponent<RectTransform>().anchoredPosition
          = new Vector3(settingsInfo.fireActButtonPosition[0], settingsInfo.fireActButtonPosition[1]);
        swapWeaponButton.GetComponent<RectTransform>().anchoredPosition
          = new Vector3(settingsInfo.swapWeaponButtonPosition[0], settingsInfo.swapWeaponButtonPosition[1]);
    }

    private void SetColor()
    {
        ColorUtility.TryParseHtmlString("#" + settingsInfo.color, out Color newColor);

        foreach (var image in joystick.transform.GetComponentsInChildren<Image>())
            image.color = newColor;
        if (settingsInfo.joystickType == "Dynamic")
            joystick.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        fireActButton.GetComponent<Image>().color = newColor;
        swapWeaponButton.GetComponent<Image>().color = newColor;
    }
}
