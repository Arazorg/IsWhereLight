using UnityEngine;
using UnityEngine.UI;

public class ButtonImage : MonoBehaviour
{
    private enum Type
    {
        music,
        effects
    }

#pragma warning disable 0649
    [Tooltip("Спрайт выключенного состояния")]
    [SerializeField] private Sprite enableOffSprite;

    [Tooltip("Спрайт включенного состояния")]
    [SerializeField] private Sprite enableOnSprite;

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
                    GetComponent<Image>().sprite = enableOnSprite;
                else
                    GetComponent<Image>().sprite = enableOffSprite;
                break;
            case Type.music:
                if (settingsInfo.musicOn)
                    GetComponent<Image>().sprite = enableOnSprite;
                else
                    GetComponent<Image>().sprite = enableOffSprite;
                break;
        }
    }

    public void SetSprite(bool isEnable)
    {
        if (isEnable)
            GetComponent<Image>().sprite = enableOnSprite;
        else
            GetComponent<Image>().sprite = enableOffSprite;
    }
}
