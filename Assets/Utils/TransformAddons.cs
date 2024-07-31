using UnityEngine;
using System.Collections.Generic;

namespace Utils
{
    public static class TransformAddons
    {
        /// <summary>
        /// Destroys a component of type T of the transform
        /// </summary>
        public static void DestroyComponent<T>(this Transform transform) where T : Component
        {
            Object.Destroy(transform.GetComponent<T>());
        }

        /// <summary>
        /// Destroys all of the components of type T in this transform's children
        /// </summary>
        public static void DestroyComponentsInChildren<T>(this Transform transform) where T : Component
        {
            foreach (T component in transform.GetComponentsInChildren<T>())
                Object.Destroy(component);
        }

        /// <summary>
        /// Destroys all the children of this transform
        /// </summary>
        public static void DestroyAllChildren(this Transform transform)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
                Object.DestroyImmediate(transform.GetChild(i).gameObject);
        }

        /// <summary>
        /// Completes the action for each of the children of the transform
        /// </summary>
        public static void ForEachChild(this Transform t, System.Action<Transform> Action)
        {
            for (int i = 0; i < t.childCount; i++)
                Action(t.GetChild(i));
        }

        /// <summary>
        /// Completes the action for each of the children of the transform
        /// </summary>
        public static void ForEachChild(this Transform t, System.Action<Transform, int> Action)
        {
            for (int i = 0; i < t.childCount; i++)
                Action(t.GetChild(i), i);
        }

        /// <summary>
        /// Returns all of the components in this transform and in its children
        /// </summary>
        public static List<T> GetComponentsInThisAndChildren<T>(this Transform t) where T : Component
        {
            return new List<T>(t.GetComponentsInChildren<T>())
            {
                t.GetComponent<T>()
            };
        }

        /// <summary>
        /// Returns the world scale of this transform
        /// </summary>
        public static Vector3 GetWorldScale(this Transform t)
        {
            Vector3 scale = t.localScale;
            while (t = t.parent)
                scale = scale.Multiply(t.localScale);
            return scale;
        }

        /// <summary>
        /// Sets the world scale of this transform
        /// </summary>
        public static void SetWorldScale(this Transform t, Vector3 scale)
        {
            Vector3 worldScaleFactor = scale.Divide(GetWorldScale(t));
            t.localScale = t.localScale.Multiply(worldScaleFactor);
        }
    }
}
