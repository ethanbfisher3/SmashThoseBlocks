using Utils;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UIElements;
using Blocks;
using Unity.Mathematics;

namespace Environment
{

    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class StickySurface : MonoBehaviour
    {
        public bool stickRight;
        public bool stickLeft;
        public bool stickTop;
        public bool stickBottom;

        private bool ShouldStaySticky(Block block)
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(block.MousePositionAtLastThrow);
            var z = block.transform.eulerAngles.z % 90f;
            if (z > 1f && z < 89f)
            {
                print("z rotation: " + z);
                return false;
            }

            if (stickLeft)
            {
                if (mousePosition.x < block.PositionAtLastThrow.x)
                    return false;
            }

            if (stickRight)
            {
                if (mousePosition.x > block.PositionAtLastThrow.x)
                    return false;
            }

            return true;
        }

        public void StickBlock(Block b)
        {
            b.Rigidbody.isKinematic = true;
            b.Rigidbody.velocity = Vector2.zero;
            b.Rigidbody.freezeRotation = true;
        }

        public void UnstickBlock(Block b)
        {
            b.Rigidbody.isKinematic = false;
            b.Rigidbody.freezeRotation = false;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.TryGetComponent(out Block b))
            {
                if (b.TryGetBlockOfType(out StickyBlock _))
                {
                    if (ShouldStaySticky(b))
                        StickBlock(b);
                }
            }
        }
    }
}