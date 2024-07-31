using GameManagement;
using UnityEditor;
using UnityEngine;

public class ConfigWindow : EditorWindow
{
    [MenuItem("Config/Config Window")]
    public static void ShowWindow()
    {
        CreateWindow<ConfigWindow>();
    }

    public void OnGUI()
    {
        var config = new Config();
        config.name = EditorGUILayout.TextField(config.name);

        SaveSystem.SaveData(config, "config.config");
    }
}

[System.Serializable]
public class Config
{
    public string name;
}