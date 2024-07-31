using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class Align : MonoBehaviour
{
    [MenuItem("Align/Align Selection Y")]
    public static void AlignVertical() => AlignObjects(true);
    [MenuItem("Align/Align Selection X")]
    public static void AlignHorizontal() => AlignObjects(false);
    [MenuItem("Distribute/Distribute Selection Y")]
    public static void DistributeVertical() => Distribute(true);
    [MenuItem("Distribute/Distribute Selection X")]
    public static void DistributeHorizontal() => Distribute(false);

    static void AlignObjects(bool vertical)
    {
        if (Selection.gameObjects.Length <= 1) return;

        List<GameObject> gameObjects = new List<GameObject>(Selection.gameObjects);
        float amount = 0f;
        gameObjects.ForEach(go => amount += vertical ? go.transform.position.y : go.transform.position.x);
        float avg = amount / gameObjects.Count;
        gameObjects.ForEach(go =>
        {
            var pos = go.transform.position;
            if (vertical)
                pos.y = avg;
            else
                pos.x = avg;
            go.transform.position = pos;
        });
    }

    static void Distribute(bool vertical)
    {
        if (Selection.gameObjects.Length <= 1) return;

        float most = -float.MaxValue;
        float least = float.MaxValue;
        List<GameObject> gameObjects = new List<GameObject>(Selection.gameObjects);
        gameObjects.ForEach(go =>
        {
            var pos = go.transform.position;
            if (vertical)
            {
                if (pos.y > most)
                    most = pos.y;
                else if (pos.y < least)
                    least = pos.y;
            }
            else
            {
                if (pos.x > most)
                    most = pos.x;
                else if (pos.x < least)
                    least = pos.x;
            }
        });

        gameObjects.Sort((one, two) => one.transform.GetSiblingIndex() - two.transform.GetSiblingIndex());

        for (float i = 0; i < gameObjects.Count; i++)
        {
            var go = gameObjects[(int)i];
            var pos = go.transform.position;
            if (vertical)
                pos.y = Mathf.Lerp(least, most, i / (gameObjects.Count - 1));
            else
                pos.x = Mathf.Lerp(least, most, i / (gameObjects.Count - 1));
            go.transform.position = pos;
        }
    }
}
