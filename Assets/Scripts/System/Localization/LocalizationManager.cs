using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance;

    private SettingsInfo settingsInfo;
    private Dictionary<string, string> localizedText;
    private bool isReady = false;
    private readonly string missingStringText = "Localized text not found";
    GameObject[] texts;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        settingsInfo = GameObject.Find("SettingsHandler").GetComponent<SettingsInfo>();

    }

    public void LoadLocalizedText(string fileName)
    {
        localizedText = new Dictionary<string, string>();

        TextAsset localizationFile = Resources.Load<TextAsset>(Path.Combine("LocalizationFiles/", fileName));
        string dataAsJson = localizationFile.text;
        LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
        for (int i = 0; i < loadedData.items.Length; i++)
        {
            localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
        }
        settingsInfo.currentLocalization = fileName;
        settingsInfo.SaveSettings();
        //Debug.Log("Data loaded, dictionary contains: " + localizedText.Count + " entries");
        RefreshText();
        isReady = true;
    }

    public string GetLocalizedValue(string key)
    {
        string result = missingStringText;
        if (localizedText.ContainsKey(key))
            result = localizedText[key];

        return result;
    }

    public bool GetisReady()
    {
        return isReady;
    }

    public void RefreshText()
    {
        texts = GameObject.FindGameObjectsWithTag("Text");
        foreach (var text in texts)
        {
            LocalizedText localizedText = null;
            try
            {
                localizedText = text.GetComponent<LocalizedText>();
            }
            catch
            {
                Debug.Log(text.name);
            }
            if (localizedText != null)
                localizedText.SetLocalization();
        }
    }
}
