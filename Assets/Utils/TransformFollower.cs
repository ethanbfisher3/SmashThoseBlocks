using UnityEngine;

namespace Utils
{
    public class TransformFollower : MonoBehaviour
    {
        public Transform toFollow;
        public bool yAxisOnly = true;
        [Range(0f, 360f)]
        public float offset = 0f;
        public float maxUpdateDistance = 100f;

#if UNITY_EDITOR
        public bool autoUpdate = true;
#endif

        void Start() => Update();

        public void Update()
        {
            float distance = Vector3.Distance(transform.position, toFollow.position);
            if (distance > maxUpdateDistance) return;

            if (yAxisOnly)
            {
                float dx = toFollow.position.x - transform.position.x;
                float dz = transform.position.z - toFollow.position.z;
                float yRotation = Mathf.Rad2Deg * Mathf.Atan(-dx / dz);
                if (dz > 0) yRotation += 180f;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x,
                    yRotation + offset, transform.eulerAngles.z);
            }
            else
                transform.LookAt(toFollow);
        }

        void OnValidate()
        {
            if (toFollow == null && GameObject.FindGameObjectWithTag("Player") != null)
                toFollow = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}