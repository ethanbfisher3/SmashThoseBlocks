using UnityEditor;
using UnityEngine;

namespace Utils
{ 
    [CustomEditor(typeof(TransformFollower))]
    public class TransformFollowerEditor : Editor
    {
        public override void OnInspectorGUI()
        { 
            DrawDefaultInspector();

            if (((TransformFollower)target).autoUpdate || GUILayout.Button("Update"))
                ((TransformFollower)target).Update();
        }
    }
}
