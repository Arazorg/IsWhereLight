using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance;

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
        texts = GameObject.FindGameObjectsWithTag("Text");
    }

    public void LoadLocalizedText(string fileName)
    {
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }

            Debug.Log("Data loaded, dictionary contains: " + localizedText.Count + " entries");
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }
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
        foreach (var text in texts)
        {
            LocalizedText localizedText = text.GetComponent<LocalizedText>();
            localizedText.SetLocalization();
        }
    }
}
