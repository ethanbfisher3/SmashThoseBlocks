using UnityEngine;

namespace Utils
{
    [CreateAssetMenu()]
    public class Sound : ScriptableObject
    {
        public AudioClip clip;
        [HideInInspector]
        public AudioSource source;

        [Range(0f, 1f)]
        public float volume = .5f;
        [Range(.1f, 3f)]
        public float pitch = 1f;
        public bool loop;
    }
}