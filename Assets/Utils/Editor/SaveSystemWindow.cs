using UnityEngine;
using UnityEditor;
using Assets.Utils;

public class SaveSystemWindow : EditorWindow
{
    enum ValueType
    {
        String,
        Int,
        Float,
        Double,
        Bool
    }

    string saveName;
    string saveValue;
    ValueType valueType;

    [MenuItem("Save System/Save System Window")]
    public static void ShowEditorWindow()
    {
        CreateWindow<SaveSystemWindow>();
        SaveSystem.LoadAllData();
    }

    void OnDestroy()
    {
        SaveSystem.SaveAllData();
    }

    public void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Save Name");
        GUILayout.Label("Save Value");
        GUILayout.Label("String Type");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        saveName = EditorGUILayout.TextField(saveName);
        saveValue = EditorGUILayout.TextField(saveValue);
        valueType = (ValueType)EditorGUILayout.EnumFlagsField(valueType);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        bool save = GUILayout.Button("Save");
        bool load = GUILayout.Button("Load");
        bool delete = GUILayout.Button("Delete");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        bool DeleteAll = GUILayout.Button("Delete All");
        bool loadAll = GUILayout.Button("Load All");
        bool printAll = GUILayout.Button("Print All");
        bool saveAll = GUILayout.Button("Save All");
        GUILayout.EndHorizontal();

        if (save)
        {
            SaveSystem.SaveData(ConvertToType(), saveName);
            Debug.Log("Saved " + saveName);
        }
        if (load)
        {
            Debug.Log(SaveSystem.LoadData(saveName));
        }
        if (delete)
        {
            SaveSystem.DeleteData(saveName);
            Debug.Log("Deleted " + saveName);
        }

        if (DeleteAll)
        {
            SaveSystem.ClearData();
            Debug.Log("Deleteed all data");
        }
        if (loadAll)
        {
            SaveSystem.LoadAllData();
            Debug.Log("Loaded all data");
        }
        if (printAll)
        {
            foreach (var item in SaveSystem.SavedData)
            {
                Debug.Log(item.Key + ": " + item.Value);
            }
        }
        if (saveAll)
        {
            SaveSystem.SaveAllData();
            Debug.Log("Saved all data");
        }
    }

    object ConvertToType()
    {
        return valueType switch
        {
            ValueType.String => saveValue,
            ValueType.Int => int.Parse(saveValue),
            ValueType.Float => float.Parse(saveValue),
            ValueType.Double => double.Parse(saveValue),
            ValueType.Bool => bool.Parse(saveValue),
            _ => saveValue,
        };
    }
}