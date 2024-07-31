using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utils
{
    public class SaveSystem
    {
        static readonly BinaryFormatter formatter = new BinaryFormatter();

        public static void SaveData(object serializableInfo, string path)
        {
            var stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, serializableInfo);
            stream.Close();
        }

        public static object LoadData(string path, object _default = null)
        {
            if (File.Exists(path))
            {
                var stream = new FileStream(path, FileMode.Open);
                object info;
                try
                {
                    info = formatter.Deserialize(stream);
                }
                catch (Exception)
                {
                    stream.Close();
                    return _default;
                }
                stream.Close();
                return info;
            }

            Debug.Log("No data found at " + path);
            return _default;
        }

        public static bool LoadData(string path, out object data)
        {
            data = LoadData(path, null);
            return data != null;
        }

        public static void DeleteData(string path) => File.Delete(path);
    }
}