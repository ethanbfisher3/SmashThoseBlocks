using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Blocks;

namespace Environment
{

    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class BlockPlatform : MonoBehaviour
    {
        public UnityEvent OnBlocksPlaced;
        public UnityEvent OnBlocksTouched;
        public SpriteRenderer image;
        public List<Block> allowedBlocks;
        public int requiredBlocks = 1;
        public bool destroyOnComplete = true;
        public float waitTime = 1f;
        public float dy = -0.2f;

        List<Block> enteredBlocks;
        bool blocksPlaced = false;

        protected virtual void Start()
        {
            enteredBlocks = new List<Block>();
            OnBlocksPlaced.AddListener(() => image.enabled = false);
        }

        protected virtual void Update()
        {
            if (enteredBlocks.Count < requiredBlocks) return;

            if (!BlocksMoving() && !blocksPlaced)
            {
                blocksPlaced = true;
                LeanTween.moveY(gameObject, transform.position.y + dy, 1f).setOnComplete(() =>
                {
                    OnBlocksPlaced?.Invoke();
                    if (destroyOnComplete)
                        Destroy(this);
                });
            }
        }

        private bool BlocksMoving()
        {
            foreach (Block block in enteredBlocks)
                if (!block.StillFor(waitTime)) return true;
            return false;
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name.StartsWith("Fuse") || collision.gameObject.tag == "Ghost") return;

            if (collision.TryGetComponent(out Block b) && allowedBlocks.Contains(b))
            {
                enteredBlocks.Add(b);
            }
        }

        protected void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Block b))
                enteredBlocks.Remove(b);
        }
    }
}