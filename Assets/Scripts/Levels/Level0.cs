using GameManagement;
using UnityEngine;

namespace Levels
{
    public class Level0 : GameLevel
    {
        public GameObject blockPointer;
        public GameObject platformPointer;

        protected override void Start()
        {
            base.Start();

            blockPointer.SetActive(false);
            platformPointer.SetActive(false);
        }

        void Update()
        {
            if (!GameManager.Instance.Playing) return;

            if (GameManager.Instance.IsAboutToWin)
                platformPointer.SetActive(false);
            else
                platformPointer.SetActive(SelectedBlock != null);
            blockPointer.SetActive(SelectedBlock == null);
        }
    }
}