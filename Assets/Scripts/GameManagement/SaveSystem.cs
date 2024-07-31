using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GameManagement {

    public class SaveSystem
    {
        static readonly BinaryFormatter formatter = new BinaryFormatter();

        public static bool SaveData(object serializableInfo, string path)
        {
            if (GameManager.Instance)
            {
                if (!GameManager.Instance.saveData)
                    return false;
            }

            var stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, serializableInfo);
            stream.Close();

            return true;
        }

        public static object LoadData(string path, object _default = null)
        {
            if (GameManager.Instance)
            {
                if (!GameManager.Instance.loadData)
                    return _default;
            }

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
            if (!GameManager.Instance || !GameManager.Instance.loadData)
            {
                data = null;
                return false;
            }

            data = LoadData(path, null);
            return data != null;
        }

        public static void DeleteData(string path) => File.Delete(path);
    }
}