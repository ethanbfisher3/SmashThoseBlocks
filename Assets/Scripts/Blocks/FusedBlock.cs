using System.Collections;
using System.Collections.Generic;
using GameManagement;
using Levels;
using UnityEngine;

namespace Blocks
{
    public class FusedBlock : Block
    {
        private Block BlockOne { get; set; }
        private Block BlockTwo { get; set; }

        public SpriteRenderer imageOne;
        public SpriteRenderer imageTwo;

        public void SetFusedBlocks(Block blockOne, Block blockTwo)
        {
            BlockOne = blockOne;
            BlockTwo = blockTwo;

            imageOne.sprite = blockOne.innerImage;
            imageTwo.sprite = blockTwo.innerImage;

            blockOne.transform.parent = transform;
            blockTwo.transform.parent = transform;

            blockOne.gameObject.SetActive(false);
            blockTwo.gameObject.SetActive(false);

            blockOne.OnBecomeFusedTo(blockTwo);
            blockTwo.OnBecomeFusedTo(blockOne);
        }

        public void UnfuseBlocks()
        {
            var blockHandler = GameLevel.Current.blockHandler;

            BlockOne.transform.parent = blockHandler.transform;
            BlockTwo.transform.parent = blockHandler.transform;

            BlockOne.gameObject.SetActive(true);
            BlockTwo.gameObject.SetActive(true);

            Destroy(gameObject);

            // Vector3 move = new Vector2(0.75f * transform.localScale.x, 0f);
            // b.transform.position = transform.position - move;
            // transform.position += move;

            BlockOne.OnBreakApartFrom(BlockTwo);
            BlockTwo.OnBreakApartFrom(BlockOne);
        }

        public override bool CanFuseWith(Block other)
        {
            return base.CanFuseWith(other) && BlockOne == null && BlockTwo == null;
        }

        public override void SetSpriteRendererColor(Color color)
        {
            base.SetSpriteRendererColor(color);
            imageOne.color = color;
            imageTwo.color = color;
        }
    }
}