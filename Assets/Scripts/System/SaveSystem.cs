using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveChar(CharInfo charInfo)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.bin";
        FileStream stream = new FileStream(path, FileMode.Create);

        CharData charData = new CharData(charInfo);

        formatter.Serialize(stream, charData);
        stream.Close();
    }

    public static CharData LoadChar()
    {
        string path = Application.persistentDataPath + "/player.bin";
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
}
