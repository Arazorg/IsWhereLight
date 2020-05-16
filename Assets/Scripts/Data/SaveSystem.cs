﻿using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static string CurrentCharFile = Application.persistentDataPath + "/player" + Application.version + ".bin";
    public static string CurrentSettingsFile = Application.persistentDataPath + "/settings" + Application.version + ".bin";
    public static string CurrentGameFile = Application.persistentDataPath + "/currentGame" + Application.version + ".bin";
    public static string ProgressFile = Application.persistentDataPath + "/progress" + Application.version + ".bin";

    public static void SaveChar(CharInfo charInfo)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = CurrentCharFile;
        FileStream stream = new FileStream(path, FileMode.Create);

        CharData charData = new CharData(charInfo);

        formatter.Serialize(stream, charData);
        stream.Close();
    }

    public static CharData LoadChar()
    {
        string path = CurrentCharFile;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            CharData charData = formatter.Deserialize(stream) as CharData;
            stream.Close();
            return charData;
        }
        else
        {
            Debug.Log("Save file not found in" + path);
            return null;
        }
    }

    public static void SaveSettings(SettingsInfo settingsInfo)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = CurrentSettingsFile;
        FileStream stream = new FileStream(path, FileMode.Create);

        SettingsData settingsData = new SettingsData(settingsInfo);

        formatter.Serialize(stream, settingsData);
        stream.Close();
    }

    public static SettingsData LoadSettings()
    {
        string path = CurrentSettingsFile;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SettingsData settingsData = formatter.Deserialize(stream) as SettingsData;
            stream.Close();
            return settingsData;
        }
        else
        {
            Debug.Log("Save file not found in" + path);
            return null;
        }
    }

    public static void SaveCurrentGame(CurrentGameInfo currentGameInfo)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = CurrentGameFile;
        FileStream stream = new FileStream(path, FileMode.Create);

        CurrentGameData currentGameData = new CurrentGameData(currentGameInfo);

        formatter.Serialize(stream, currentGameData);
        stream.Close();
    }

    public static CurrentGameData LoadCurrentGame()
    {
        string path = CurrentGameFile;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            CurrentGameData currentGameData = formatter.Deserialize(stream) as CurrentGameData;
            stream.Close();
            return currentGameData;
        }
        else
        {
            Debug.Log("Save file not found in" + path);
            return null;
        }
    }

    public static void SaveProgress(ProgressInfo progressInfo)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = ProgressFile;
        FileStream stream = new FileStream(path, FileMode.Create);

        ProgressData progressData = new ProgressData(progressInfo);

        formatter.Serialize(stream, progressData);
        stream.Close();
    }

    public static ProgressData LoadProgress()
    {
        string path = ProgressFile;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            ProgressData progressData = formatter.Deserialize(stream) as ProgressData;
            stream.Close();
            return progressData;
        }
        else
        {
            Debug.Log("Save file not found in" + path);
            return null;
        }
    }

    public static void DeleteCurrentGame()
    {
        File.Delete(CurrentGameFile);
        File.Delete(CurrentCharFile);
        RefreshEditorProjectWindow();
    }

    private static void RefreshEditorProjectWindow()
    {
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}
