namespace PierreARNAUDET.TwitchUtilitary
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    using UnityEngine;

    using static PierreARNAUDET.TwitchUtilitary.ColorStringHelper;
    
    public static class DataHelper<T>
    {
        public static void SaveData(T data, string directory)
        {
            var binaryFormatter = new BinaryFormatter();
            var path = Application.persistentDataPath + directory;
            var fileStream = new FileStream(path, FileMode.Create);
            binaryFormatter.Serialize(fileStream, data);
            fileStream.Close();
        }

        public static T LoadData(string directory)
        {
            var path = Application.persistentDataPath + directory;
            if (File.Exists(path))
            {
                var binaryFormatter = new BinaryFormatter();
                var fileStream = new FileStream(path, FileMode.Open);
                var data = (T)binaryFormatter.Deserialize(fileStream);
                fileStream.Close();
                return data;
            }
            else
            {
                Debug.LogError($"{"Save file not found in".ColorString(ColorType.CommonConsole)} {path}");
                return default(T);
            }
        }
    }
}