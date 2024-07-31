using Blocks;
using GameManagement;
using UnityEngine;

namespace Environment
{

    public class LevelCompleter : BlockPlatform
    {
        protected override void Start()
        {
            base.Start();
            OnBlocksPlaced.AddListener(GameManager.Instance.Win);
        }

        protected void OnValidate()
        {
            if (allowedBlocks.Count == 0)
                allowedBlocks.Add(FindObjectOfType<BlockHandler>().transform.Find("FlaggedBlock").GetComponent<Block>());
        }

        protected void OnCollisionEnter2D(Collision2D other)
        {
            print("Entered Collision: " + other.gameObject.name);
            var block = other.gameObject.GetComponent<Block>();
            if (block.gameObject.name == "FlaggedBlock")
                GameManager.Instance.IsAboutToWin = true;
        }
    }
}