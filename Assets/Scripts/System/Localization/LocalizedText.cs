using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    public bool isNotRefresh;
    public bool isAdd;
    public string addable;
    public string key;

    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        if(GetComponentInParent<Weapon>() == null && !isNotRefresh)
        {
            if (isAdd)
                text.text = LocalizationManager.instance.GetLocalizedValue(key) + addable;
            else
                text.text = LocalizationManager.instance.GetLocalizedValue(key);
        }
            
    }

    private void OnEnable()
    {
        if(text != null)
        {
            text = GetComponent<TextMeshProUGUI>();
            if (GetComponentInParent<Weapon>() == null && !isNotRefresh)
            {
                if (isAdd)
                    text.text = LocalizationManager.instance.GetLocalizedValue(key) + addable;
                else
                    text.text = LocalizationManager.instance.GetLocalizedValue(key);
            }
        }
    }

    public void SetLocalization()
    {
        text = GetComponent<TextMeshProUGUI>();
        if (isAdd)
            text.text = LocalizationManager.instance.GetLocalizedValue(key) + addable;
        else
            text.text = LocalizationManager.instance.GetLocalizedValue(key);
    }

    public static string SetLocalization(string key)
    {
        return LocalizationManager.instance.GetLocalizedValue(key);
    }
}
