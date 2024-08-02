using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace Assets.Utils
{
    public class SaveSystem
    {
        public static Dictionary<string, object> SavedData { get; private set; } = new Dictionary<string, object>();
        static readonly BinaryFormatter formatter = new BinaryFormatter();
        static bool loaded = false;

        public static void SaveData(object serializableInfo, string path)
        {
            SavedData[path] = serializableInfo;
        }

        public static object LoadData(string path, object _default = null)
        {
            return SavedData.TryGetValue(path, out object info) ? info : _default;
            // if (File.Exists(path))
            // {
            //     var stream = new FileStream(path, FileMode.Open);
            //     object info;
            //     try
            //     {
            //         info = formatter.Deserialize(stream);
            //     }
            //     catch (Exception)
            //     {
            //         stream.Close();
            //         return _default;
            //     }
            //     stream.Close();
            //     return info;
            // }

            // return _default;
        }

        public static bool LoadData(string path, out object data)
        {
            data = LoadData(path, null);
            return data != null;
        }

        public static void DeleteData(string path)
        {
            SavedData.Remove(path);
        }

        public static void ClearData()
        {
            SavedData.Clear();
        }

        public static void SaveAllData()
        {
            var stream = new FileStream(Application.persistentDataPath + "/game-info.txt", FileMode.Create);

            formatter.Serialize(stream, SavedData);
            stream.Close();
        }

        public static void LoadAllData()
        {
            if (loaded) return;

            loaded = true;
            if (File.Exists(Application.persistentDataPath + "/game-info.txt"))
            {
                var stream = new FileStream(Application.persistentDataPath + "/game-info.txt", FileMode.Open);
                SavedData = (Dictionary<string, object>)formatter.Deserialize(stream);
                stream.Close();
            }
            else
            {
                SavedData.Clear();
            }
        }
    }
}