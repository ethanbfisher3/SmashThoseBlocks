using UnityEngine;
using System.Collections.Generic;

namespace Utils
{
    public class CutScene : MonoBehaviour
    {
        public bool destroyOnComplete = true;
        public float startWaitTime;
        public CutSceneTransition beginTransition;
        public CutSceneTransition endTransition;
        public List<CutSceneEvent> events;

#if UNITY_EDITOR
        void OnValidate()
        {
            foreach (CutSceneEvent e in events)
                if (e.camera != null && e.waitTime == 0)
                {
                    Animation anim = e.camera.GetComponent<Animation>();
                    if (anim != null)
                        e.waitTime = anim.clip.length;
                }
        }
#endif
    }

    [System.Serializable]
    public class CutSceneEvent
    {
        public GameObject camera;
        public float waitTime;
    }

    [System.Serializable]
    public enum CutSceneTransition
    {
        Fade
    }
}
