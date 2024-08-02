using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Assets.Utils;

public class GameInfoWindow : EditorWindow
{
    int levelNumber;

    [MenuItem("Save System/Game Info Window")]
    public static void ShowEditorWindow()
    {
        CreateWindow<GameInfoWindow>();
        SaveSystem.LoadAllData();
    }

    public void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Complete Levels Up to:"))
        {
            for (int i = 0; i < levelNumber; i++)
            {
                SaveSystem.SaveData(true, "level-" + i + "-completed");
            }
            SaveSystem.SaveAllData();
            Debug.Log("Completed levels up to " + levelNumber);
        }
        levelNumber = EditorGUILayout.IntField(levelNumber);
        GUILayout.EndHorizontal();
    }
}