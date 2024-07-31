using Environment;
using UnityEngine;

namespace GameManagement
{
    public class Prefabs : MonoBehaviour
    {

        public static Prefabs Instance { get; private set; }

        public Door door;

        void Awake()
        {
            Instance = this;
        }
    }
}