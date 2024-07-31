using UnityEngine;

namespace Utils
{
    public class FloatingProgressBar : MonoBehaviour
    {
        public Transform background;
        public Transform foreground;
        [Range(0f, 1f)] public float progress = 0f;

        public void SetProgress(float progress)
        {
            float x = Mathf.Lerp(4.9f, 0f, progress);
            foreground.localPosition = new Vector3(x, 0.002f, 0f);
            foreground.localScale = new Vector3(background.localScale.x * progress, 1f, background.localScale.z);
        }

        void OnValidate()
        {
            if (background && foreground)
                SetProgress(progress);
        }
    }
}
