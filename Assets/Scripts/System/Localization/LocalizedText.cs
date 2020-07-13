using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
    public bool isNotRefresh;
    public string key;
    TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        if(GetComponentInParent<Weapon>() == null && !isNotRefresh)
            text.text = LocalizationManager.instance.GetLocalizedValue(key);
    }

    private void OnEnable()
    {
        if(text != null)
        {
            text = GetComponent<TextMeshProUGUI>();
            if (GetComponentInParent<Weapon>() == null && !isNotRefresh) ;
               text.text = LocalizationManager.instance.GetLocalizedValue(key);
        }
    }

    public void SetLocalization()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = LocalizationManager.instance.GetLocalizedValue(key);
    }

    public static string SetLocalization(string key)
    {
        return LocalizationManager.instance.GetLocalizedValue(key);
    }
}
