﻿using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
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

    [Tooltip("UI кнопки смены оружия")]
    [SerializeField] private GameObject swapWeaponButton;
#pragma warning restore 0649

    private bool IsColorPanelState;
    //Скрипты
    private AudioManager audioManager;
    private SettingsInfo settingsInfo;

    void Start()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        audioManager = FindObjectOfType<AudioManager>();

        if (MenuButtons.firstRun)
            SetStandart();
        else
        {
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
        }
    }

    public void InterfaceSettingsPanelClose()
    {
        audioManager.Play("ClickUI");
        SetPosition();
        settingsInfo.SaveSettings();
        menuPanel.SetActive(true);
        gameObject.SetActive(false);
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
    }

    public void ColorPanelOpenClose()
    {
        audioManager.Play("ClickUI");
        IsColorPanelState = !IsColorPanelState;
        if (IsColorPanelState)
            colorPanel.GetComponent<MovementUI>().MoveToEnd();
        else
            colorPanel.GetComponent<MovementUI>().MoveToStart();
    }

    public void SetColor(string color)
    {
        audioManager.Play("ClickUI");
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
    }

    private Color StringToColor(string color)
    {
        if (ColorUtility.TryParseHtmlString("#" + color, out Color newColor))
            return newColor;
        else
            return Color.white;
    }

}
