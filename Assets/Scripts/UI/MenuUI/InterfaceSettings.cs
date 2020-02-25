using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceSettings : MonoBehaviour
{
    public GameObject interfaceSettings;
    public GameObject menu;
    public GameObject joystick;
    public GameObject fireActButton;
    private AudioManager audioManager;
    private SettingsData data;
    private SettingsInfo settingsInfo;

    void Start()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void InterfaceSettingsExit()
    {
        settingsInfo.SaveSettings();
        audioManager.Play("ClickUI");
        menu.SetActive(true);
        interfaceSettings.SetActive(false);
        //settingsInfo.joystickPosition = joystick.transform;
        //settingsInfo.fireActButtonPosition = fireActButton.transform;
    }
}
