using Blocks;
using GameManagement;
using UnityEngine;

namespace Levels
{
    public class Level2 : GameLevel
    {
        public GameObject flaggedBlockPointer;
        public GameObject magnetPointer;
        public GameObject levelCompleterPointer;
        public Checkpoint passedMagnetCheckpoint;

        protected override void Start()
        {
            base.Start();

            flaggedBlockPointer.SetActive(false);
            magnetPointer.SetActive(false);
            levelCompleterPointer.SetActive(false);
        }

        void Update()
        {
            if (!GameManager.Instance.Playing) return;

            var existsSelectedBlock = SelectedBlock != null;
            var flaggedBlockSelected = false;
            var magnetBlockSelected = false;
            var blocksHaveFused = blockHandler.GetComponentInChildren<FusedBlock>();
            var passedMagnet = passedMagnetCheckpoint.reached;

            if (existsSelectedBlock)
            {
                flaggedBlockSelected = SelectedBlock.name.StartsWith("Flag");
                magnetBlockSelected = SelectedBlock.TryGetBlockOfType(out MagnetBlock _);
            }

            var flaggedBlockActive = (!existsSelectedBlock || (existsSelectedBlock && magnetBlockSelected)) && !passedMagnet;
            var magnetBlockActive = (!existsSelectedBlock || (existsSelectedBlock && flaggedBlockSelected)) && !passedMagnet;
            var levelCompleterActive = blocksHaveFused && !GameManager.Instance.IsAboutToWin;

            flaggedBlockPointer.SetActive(flaggedBlockActive);
            magnetPointer.SetActive(magnetBlockActive);
            levelCompleterPointer.SetActive(levelCompleterActive);
        }
    }
}