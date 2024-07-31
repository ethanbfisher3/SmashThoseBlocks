using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Blocks;

[CustomEditor(typeof(Block), true), CanEditMultipleObjects]
public class BlockEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var objects = Selection.gameObjects;
        if (objects.Length <= 1) return;

        if (!new List<GameObject>(objects).Exists((obj) => !obj.TryGetComponent(out Block b)))
        {
            if (GUILayout.Button("Fuse All Blocks"))
            {
                var block = objects[0].GetComponent<Block>();
                for (int i = 1; i < objects.Length; i++)
                {
                    // block.FuseWith(objects[i].GetComponent<Block>());
                }
            }
        }
    }
}
