using UnityEngine;
using System;

namespace Utils
{
    public static class GameObjectAddons
    {
        public static void SetMaterialColor(this GameObject go, Color color)
        {
            foreach (Material material in go.GetComponent<Renderer>().materials)
                material.color = color;
        }

        public static void SetOnDestroy(this GameObject go, Action OnDestroy)
        {
            if (go.TryGetComponent(out DestroyAction da))
                da.OnDestruction = OnDestroy;
            else go.AddComponent<DestroyAction>().OnDestruction = OnDestroy;
        }

        public static void SetOnUpdate(this GameObject go, Action OnUpdate)
        {
            go.AddComponent<UpdateAction>().OnUpdate = OnUpdate;
        }

        public static void Dissolve(this GameObject go, float t)
        {
        }

        public static void Disappear(this GameObject go, float t)
        {
        }

        public static bool IsDestroyed(this GameObject gameObject)
        {
            // UnityEngine overloads the == opeator for the GameObject type
            // and returns null when the object has been destroyed, but 
            // actually the object is still there but has not been cleaned up yet
            // if we test both we can determine if the object has been destroyed.
            return gameObject == null && !ReferenceEquals(gameObject, null);
        }
    }
}