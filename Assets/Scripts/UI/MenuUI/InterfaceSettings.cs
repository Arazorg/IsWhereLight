using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InterfaceSettings : MonoBehaviour
{
#pragma warning disable 0649
    [Tooltip("UI панели меню")]
    [SerializeField] private GameObject menuPanel;

    [Tooltip("UI Джойстик")]
    [SerializeField] private GameObject joystick;

    [Tooltip("UI кнопки действия, атаки")]
    [SerializeField] private GameObject fireActButton;
#pragma warning restore 0649

    //Скрипты
    private AudioManager audioManager;
    private SettingsInfo settingsInfo;

    void Start()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        audioManager = FindObjectOfType<AudioManager>();

        if (MenuButtons.firstRun)
        {
            SetStandartPositions();
        }
        else
        {
            joystick.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(settingsInfo.joystickPosition[0] + 256,
                                settingsInfo.joystickPosition[1] + 256);
            fireActButton.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(settingsInfo.fireActButtonPosition[0],
                                settingsInfo.fireActButtonPosition[1]);
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

    public void SetStandartPositions()
    {
        audioManager.Play("ClickUI");
        joystick.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(SettingsInfo.startPositions["joystickPosition"][0] + 256,
                                SettingsInfo.startPositions["joystickPosition"][1] + 256);
        fireActButton.GetComponent<RectTransform>().anchoredPosition =
            new Vector3(SettingsInfo.startPositions["fireActButtonPosition"][0],
                            SettingsInfo.startPositions["fireActButtonPosition"][1]);
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
    }
}
