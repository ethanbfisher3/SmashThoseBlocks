using System.Collections.Generic;
using GameManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Blocks
{
    public class BlockFusionEvent : UnityEvent<FusedBlock> { }

    public class BlockHandler : MonoBehaviour
    {
        public Block HighlightedBlock { get; private set; }
        public Block SelectedBlock { get; private set; }
        public int FusionsCompleted { get; private set; }
        public Block[] ChildBlocks { get; private set; }
        public BlockFusionEvent OnBlockFusion { get; private set; }

        bool firstSelection = false;

        void Awake()
        {
            ChildBlocks = GetComponentsInChildren<Block>(true);

            HighlightedBlock = null;
            SelectedBlock = null;

            OnBlockFusion = new BlockFusionEvent();
        }

        void OnTransformChildrenChanged()
        {
            ChildBlocks = GetComponentsInChildren<Block>();
        }

        void Update()
        {
            if (!GameManager.Instance.Playing) return;

            // use arrow keys to select blocks
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                SelectPrevBlock();
            if (Input.GetKeyDown(KeyCode.RightArrow))
                SelectNextBlock();

            // stuff about selecting blocks
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (HighlightedBlock)
                {
                    if (SelectedBlock == HighlightedBlock)
                    {
                        HighlightedBlock.SetSpriteRendererColor(BlockConstants.Instance.highlightedColor);
                        SelectedBlock = null;
                        GameUI.Instance.RemoveGameOption(KeyCode.Space);
                    }
                    else
                    {
                        SetSelectedBlock(HighlightedBlock);
                        if (SelectedBlock)
                            SelectedBlock.SetSpriteRendererColor(BlockConstants.Instance.highlightedAndSelectedColor);
                    }
                }

                else if (!HighlightedBlock && SelectedBlock)
                    SelectedBlock.TryThrow();
            }
        }

        void UpdateSelectedBlock()
        {
            if (!SelectedBlock) return;

            // game option to break apart a fused block
            if (SelectedBlock.TryGetComponent(out FusedBlock _))
                GameUI.Instance.AddGameOption(UnfuseBlocks, text: "Break Apart", KeyCode.Space);
            else
                GameUI.Instance.RemoveGameOption(KeyCode.Space);

            // if there is a highlighted block that isn't the currently selected
            // block and they can fuse together, add game option allowing it
            if (HighlightedBlock && SelectedBlock != HighlightedBlock
                && SelectedBlock.CanFuseWith(HighlightedBlock)
                && HighlightedBlock.CanFuseWith(SelectedBlock)
                && CloseEnough(HighlightedBlock.transform, SelectedBlock.transform))
            {
                GameUI.Instance.AddGameOption(() =>
                {
                    FusionsCompleted++;
                    Fuse(SelectedBlock, HighlightedBlock);
                    UpdateSelectedBlock();
                }, text: "Fuse", KeyCode.E);
            }
            else
                GameUI.Instance.RemoveGameOption(KeyCode.E);
        }

        public void PauseBlocks()
        {
            foreach (Block block in ChildBlocks)
            {
                var rb = block.Rigidbody;
                rb.isKinematic = true;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }

        public void UnPauseBlocks()
        {
            foreach (Block block in ChildBlocks)
            {
                var rb = block.Rigidbody;
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints2D.None;
            }
        }

        public void DeselectBlock()
        {
            if (SelectedBlock == null) return;

            if (HighlightedBlock == SelectedBlock)
                SelectedBlock.SetSpriteRendererColor(BlockConstants.Instance.highlightedColor);
            else
                SelectedBlock.SetSpriteRendererColor(Color.white);
            SelectedBlock = null;
        }

        public void HighlightBlock(Block block)
        {
            if (HighlightedBlock && HighlightedBlock != block) UnHighlightBlock(HighlightedBlock);
            HighlightedBlock = block;

            if (SelectedBlock == HighlightedBlock)
                SelectedBlock.SetSpriteRendererColor(BlockConstants.Instance.highlightedAndSelectedColor);
            else
            {
                HighlightedBlock.SetSpriteRendererColor(BlockConstants.Instance.highlightedColor);
                UpdateSelectedBlock();
            }
        }

        public void UnHighlightBlock(Block block)
        {
            if (SelectedBlock && block == SelectedBlock)
                SelectedBlock.SetSpriteRendererColor(BlockConstants.Instance.selectedColor);
            else
                block.SetSpriteRendererColor(Color.white);

            HighlightedBlock = null;
            UpdateSelectedBlock();
        }

        public void SelectNextBlock()
        {
            if (!SelectedBlock) return;

            Block closestBlock = null;

            float sx = SelectedBlock ? SelectedBlock.transform.position.x : Camera.main.transform.position.x;
            foreach (Block block in ChildBlocks)
            {
                float x = block.transform.position.x;
                if (x > sx && (!closestBlock || x < closestBlock.transform.position.x))
                    closestBlock = block;
            }

            SetSelectedBlock(closestBlock);
        }

        public void SelectPrevBlock()
        {
            if (!SelectedBlock) return;

            Block closestBlock = null;

            float sx = SelectedBlock ? SelectedBlock.transform.position.x : Camera.main.transform.position.x;
            foreach (Block block in ChildBlocks)
            {
                float x = block.transform.position.x;
                if (x < sx && (!closestBlock || x > closestBlock.transform.position.x))
                    closestBlock = block;
            }

            SetSelectedBlock(closestBlock);
        }

        // public void SelectClosestBlock()
        // {
        //     Block closestBlock = null;

        //     float sx = SelectedBlock ? SelectedBlock.transform.position.x : Camera.main.transform.position.x;
        //     foreach (Block block in ChildBlocks)
        //     {
        //         if (block == SelectedBlock) continue;

        //         float x = block.transform.position.x;
        //         if (closestBlock == null || Mathf.Abs(sx - x) < Mathf.Abs(sx - closestBlock.transform.position.x))
        //             closestBlock = block;
        //     }
        //     SetSelectedBlock(closestBlock);
        // }

        public void SetSelectedBlock(Block block)
        {
            if (GameManager.Instance.requireCloseBlocks)
            {
                if (firstSelection && (!block || block == SelectedBlock ||
                    !CloseEnough(block.transform, SelectedBlock ?
                    SelectedBlock.transform : Camera.main.transform)))
                {
                    GameUI.Instance.CreateNotification("Block cannot be selected", GameUI.NotificationType.Warning);
                    return;
                }
            }

            if (SelectedBlock)
                SelectedBlock.SetSpriteRendererColor(Color.white);
            SelectedBlock = block;
            SelectedBlock.SetSpriteRendererColor(BlockConstants.Instance.selectedColor);
            UpdateSelectedBlock();
            firstSelection = true;
        }

        public void Fuse(Block one, Block two)
        {
            FusedBlock fusedBlock = (FusedBlock)Instantiate(BlockConstants.Instance.GetBlockPrefab("FusedBlock"));
            var middlePosition = (one.transform.position + two.transform.position) * 0.5f;
            fusedBlock.transform.parent = transform;
            fusedBlock.transform.position = middlePosition;

            fusedBlock.SetFusedBlocks(one, two);

            SetSelectedBlock(fusedBlock);

            OnBlockFusion.Invoke(fusedBlock);
        }

        public void UnfuseBlocks()
        {
            var fusedBlock = (FusedBlock)SelectedBlock;
            fusedBlock.UnfuseBlocks();
        }

        bool CloseEnough(Transform one, Transform two) =>
            Vector2.SqrMagnitude(one.position -
                two.position) < BlockConstants.Instance.blockSelectDistance *
                BlockConstants.Instance.blockSelectDistance;

        public Block[] GetBlocksOfType<T>() where T : Block
        {
            var blocksOfType = new List<T>();

            foreach (var block in ChildBlocks)
            {
                if (block.TryGetBlockOfType<T>(out T b))
                    blocksOfType.Add(b);
            }

            return blocksOfType.ToArray();
        }

        public Block FlaggedBlock
        {
            get
            {
                foreach (var block in ChildBlocks)
                    if (block.gameObject.name.StartsWith("Flag"))
                        return block;
                return null;
            }
        }

        public Vector3 GetAverageBlockPosition()
        {
            if (ChildBlocks.Length == 0) return Vector3.zero;

            float totalX = 0f, totalY = 0f;
            foreach (Block block in ChildBlocks)
            {
                Vector2 blockPos = block.transform.position;
                totalX += blockPos.x;
                totalY += blockPos.y;
            }

            Vector3 pos = new Vector3(totalX / ChildBlocks.Length, totalY / ChildBlocks.Length, -10f);
            return pos;
        }
    }
}