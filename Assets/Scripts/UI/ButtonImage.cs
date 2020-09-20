using UnityEngine;
using UnityEngine.UI;

public class ButtonImage : MonoBehaviour
{
    private enum Type
    {
        music,
        effects,
        vibration
    }
    
#pragma warning disable 0649
    [Tooltip("Спрайт выключенного состояния")]
    [SerializeField] private Sprite enableSoundsOffSprite;

    [Tooltip("Спрайт включенного состояния")]
    [SerializeField] private Sprite enableSoundsOnSprite;

    [Tooltip("Спрайт включенного состояния")]
    [SerializeField] private Sprite enableVibrationOnSprite;

    [Tooltip("Спрайт включенного состояния")]
    [SerializeField] private Sprite enableVibrationOffSprite;

    [Tooltip("Тип спрайта")]
    [SerializeField] private Type type;
#pragma warning restore 0649

    private SettingsInfo settingsInfo;

    void Start()
    {
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();
        switch(type)
        {
            case Type.effects:
                if (settingsInfo.effectsOn)
                    GetComponent<Image>().sprite = enableSoundsOnSprite;
                else
                    GetComponent<Image>().sprite = enableSoundsOffSprite;
                break;
            case Type.music:
                if (settingsInfo.musicOn)
                    GetComponent<Image>().sprite = enableSoundsOnSprite;
                else
                    GetComponent<Image>().sprite = enableSoundsOffSprite;
                break;
            case Type.vibration:
                if (settingsInfo.isVibration)
                    GetComponent<Image>().sprite = enableVibrationOnSprite;
                else
                    GetComponent<Image>().sprite = enableVibrationOffSprite;
                break;
        }
    }

    public void SetSoundsSprite(bool isEnable)
    {
        if (isEnable)
            GetComponent<Image>().sprite = enableSoundsOnSprite;
        else
            GetComponent<Image>().sprite = enableSoundsOffSprite;
    }

    public void SetVibrationSprite(bool isEnable)
    {
        if (isEnable)
            GetComponent<Image>().sprite = enableVibrationOnSprite;
        else
            GetComponent<Image>().sprite = enableVibrationOffSprite;
    }
}
