using Blocks;
using Environment;
using GameManagement;
using UnityEngine;

namespace Levels
{
    public class Level1 : GameLevel
    {
        public GameObject heavyBlockPointer;
        public GameObject flaggedBlockPointer;
        public GameObject platformPointer;
        public GameObject levelCompleterPointer;
        public Door door;

        protected override void Start()
        {
            base.Start();

            heavyBlockPointer.SetActive(false);
            flaggedBlockPointer.SetActive(false);
            platformPointer.SetActive(false);
            levelCompleterPointer.SetActive(false);
        }

        void Update()
        {
            if (!GameManager.Instance.Playing) return;

            var doorIsOpen = door.Opened;
            var existsSelectedBlock = SelectedBlock != null;
            var flaggedBlockSelected = false;
            var heavyBlockSelected = false;

            if (existsSelectedBlock)
            {
                flaggedBlockSelected = SelectedBlock.name.StartsWith("Flag");
                heavyBlockSelected = SelectedBlock.TryGetBlockOfType(out WeightBlock _);
            }

            var heavyBlockActive = (!doorIsOpen) && !heavyBlockSelected;
            var flaggedBlockActive = doorIsOpen && !flaggedBlockSelected;
            var platformActive = heavyBlockSelected && !doorIsOpen;
            var levelCompleterActive = doorIsOpen && flaggedBlockSelected && !GameManager.Instance.IsAboutToWin;

            heavyBlockPointer.SetActive(heavyBlockActive);
            flaggedBlockPointer.SetActive(flaggedBlockActive);
            platformPointer.SetActive(platformActive);
            levelCompleterPointer.SetActive(levelCompleterActive);
        }
    }
}