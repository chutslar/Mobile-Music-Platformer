using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveData {

    public static string GetSaveDataPath()
    {
        return Application.persistentDataPath + "data.bin";
    }

    public static void Save(List<int> data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(GetSaveDataPath());
        bf.Serialize(file, data);
        file.Close();
    }

    public static List<int> Load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (!File.Exists(GetSaveDataPath()))
            return new List<int>();
        FileStream file = File.Open(GetSaveDataPath(), FileMode.Open);
        List<int> data = (List<int>)bf.Deserialize(file);
        file.Close();
        return data;
    }
}
