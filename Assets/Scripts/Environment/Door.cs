using System.ComponentModel;
using GameManagement;
using Levels;
using UnityEngine;
using Utils;

namespace Environment
{

    public class Door : MonoBehaviour
    {
        public float dy = 3f;
        public bool Opened { get; private set; }

        void Start()
        {
            Opened = false;
        }

        public void Open()
        {
            if (Opened) return;

            AudioManager.Instance.Play("DoorOpen");
            Opened = true;
            LeanTween.moveY(gameObject, transform.position.y + dy, 3f);
        }

        public void Close()
        {
            if (!Opened) return;

            Opened = false;
            LeanTween.moveY(gameObject, transform.position.y - dy, 3f);
        }
    }
}