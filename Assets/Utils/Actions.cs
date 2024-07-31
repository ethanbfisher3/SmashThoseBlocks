using UnityEngine;
using System;

namespace Utils
{
    public class DestroyAction : MonoBehaviour
    {
        public Action OnDestruction;

        void OnDestroy() => OnDestruction?.Invoke();
    }

    public class UpdateAction : MonoBehaviour
    {
        public Action OnUpdate;

        void Update() => OnUpdate?.Invoke();
    }
}