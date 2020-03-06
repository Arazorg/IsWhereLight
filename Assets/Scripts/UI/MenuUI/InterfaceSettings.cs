using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceSettings : MonoBehaviour
{
    public GameObject interfaceSettings;
    public GameObject menu;
    public GameObject joystick;
    public GameObject fireActButton;
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
                new Vector3(settingsInfo.joystickPosition[0],
                                settingsInfo.joystickPosition[1]);
            fireActButton.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(settingsInfo.fireActButtonPosition[0],
                                settingsInfo.fireActButtonPosition[1]);
        }
    }

    public void InterfaceSettingsExit()
    {
        SetPosition();
        settingsInfo.SaveSettings();
        audioManager.Play("ClickUI");
        menu.SetActive(true);
        interfaceSettings.SetActive(false);
    }

    public void SetStandartPositions()
    {
        joystick.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(SettingsInfo.startPositions["joystickPosition"][0],
                                SettingsInfo.startPositions["joystickPosition"][1]);
        fireActButton.GetComponent<RectTransform>().anchoredPosition =
            new Vector3(SettingsInfo.startPositions["fireActButtonPosition"][0],
                            SettingsInfo.startPositions["fireActButtonPosition"][1]);
    }

    private void SetPosition()
    {
        settingsInfo.joystickPosition
            = new float[] { joystick.GetComponent<RectTransform>().anchoredPosition.x,
                                joystick.GetComponent<RectTransform>().anchoredPosition.y};
        settingsInfo.fireActButtonPosition
            = new float[] { fireActButton.GetComponent<RectTransform>().anchoredPosition.x,
                                fireActButton.GetComponent<RectTransform>().anchoredPosition.y};
    }
}
