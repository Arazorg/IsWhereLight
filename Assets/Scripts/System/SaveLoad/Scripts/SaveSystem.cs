using System;
using System.Text;
using UnityEngine;

public static class NewSaveSystem
{

    public static void Save(string key, string saveString)
    {
        if (key == "progress")
            PlayerPrefs.SetString(key, Common.AES.Encrypt(saveString, "11qz3x15el"));
        else
        {
            PlayerPrefs.SetString(key, saveString);
        }
            
    }

    public static string Load(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            var saveString = PlayerPrefs.GetString(key);
            if (key == "progress")
                saveString = Common.AES.Decrypt(saveString, "11qz3x15el");
            return saveString;
        }
        else
            return null;
    }

    public static void Delete(string key)
    {
        if (PlayerPrefs.HasKey(key))
            PlayerPrefs.DeleteKey(key);
    }

    private static string ToUtf8(string myString)
    {
        var utf8 = Encoding.UTF8;
        var utfBytes = utf8.GetBytes(myString);
        return utf8.GetString(utfBytes, 0, utfBytes.Length);
    }
}
