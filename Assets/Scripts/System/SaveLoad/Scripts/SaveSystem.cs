using UnityEngine;

public static class NewSaveSystem
{

    public static void Save(string key, string saveString)
    {
        PlayerPrefs.SetString(key, saveString);
        //PlayerPrefs.Save();
    }

    public static string Load(string key)
    {

        if (PlayerPrefs.HasKey(key))
        {
            var saveString = PlayerPrefs.GetString(key);
            Debug.Log(saveString);
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

}
