using System;
using System.Text;
using UnityEngine;

public static class NewSaveSystem
{

    public static void Save(string key, string saveString)
    {
        if (key == $"progress{Application.version}")
            PlayerPrefs.SetString(key + Application.version, Common.AES.Encrypt(saveString, "11qz3x15el"));
        else
        {
            PlayerPrefs.SetString(key + Application.version, Common.B64X.Encode(saveString));
        }
            
    }

    public static string Load(string key)
    {
        if (PlayerPrefs.HasKey(key + Application.version))
        {
            var saveString = Common.B64X.Decode(PlayerPrefs.GetString(key + Application.version));
            if (key == $"progress{Application.version}")
                saveString = Common.AES.Decrypt(saveString, "11qz3x15el");
            return saveString;
        }
        else
            return null;
    }

    public static void Delete(string key)
    {
        if (PlayerPrefs.HasKey(key + Application.version))
            PlayerPrefs.DeleteKey(key + Application.version);
    }

    private static string ToUtf8(string myString)
    {
        var utf8 = Encoding.UTF8;
        var utfBytes = utf8.GetBytes(myString);
        return utf8.GetString(utfBytes, 0, utfBytes.Length);
    }
}
