using System.Runtime.CompilerServices;
using UnityEngine;

namespace Utils
{
    public class DestroyOnTouchCollider : MonoBehaviour
    {
        public bool IsDestroyed { get; private set; }

        void Start()
        {
            IsDestroyed = false;
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            var rb = GetComponent<Rigidbody2D>();
            if (rb.velocity.y < 0 && !IsDestroyed)
            {

                print(other.gameObject.name);
                IsDestroyed = true;
            }
        }

    }
}