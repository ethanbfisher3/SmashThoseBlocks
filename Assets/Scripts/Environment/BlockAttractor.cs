using UnityEngine;
using System;
using System.Collections.Generic;
using Utils;
using Blocks;

namespace Environment
{

    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class BlockAttractor : MonoBehaviour
    {
        public bool linearForce = false;
        public float force = 10f;
        public float maxForce = 15f;
        public AttractionType attractionType;

        // the block type affected by the block attractor
        Type blockType;
        List<Block> affectedBlocks;

        void Start()
        {
            blockType = attractionType switch
            {
                AttractionType.Magnet => typeof(MagnetBlock),
                AttractionType.Repulsion => typeof(RepulsionBlock),
                _ => typeof(LightBlock),
            };
            affectedBlocks = new List<Block>();
        }

        void FixedUpdate()
        {
            Vector3 pos = transform.position;
            foreach (Block block in affectedBlocks)
            {
                if (block.gameObject.IsDestroyed())
                {
                    affectedBlocks.Remove(block);
                    return;
                }

                var blockOfType = block.FindChildOfType(blockType);

                Vector3 direction;
                if (!blockOfType || attractionType == AttractionType.Repulsion)
                {
                    direction = block.transform.position - pos;
                }
                else if (AttractionType.Magnet == attractionType)
                    direction = pos - block.transform.position;
                else
                {
                    direction = transform.up;
                }

                if (linearForce)
                    block.Rigidbody.AddForce(direction * force);
                else
                {
                    float sqrDistance = direction.sqrMagnitude;
                    float forceMagnitude = this.force / sqrDistance;
                    if (forceMagnitude > maxForce) forceMagnitude = maxForce;
                    Vector3 force = direction.normalized * forceMagnitude;
                    block.Rigidbody.AddForce(force);
                }
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Ghost")) return;

            if (collision.TryGetComponent(out Block b))
            {
                if (b.GetType() == blockType && !affectedBlocks.Contains(b))
                    affectedBlocks.Add(b);
                foreach (Block block in b.GetComponentsInChildren<Block>(true))
                    if (block.GetType() == blockType && !affectedBlocks.Contains(b))
                        affectedBlocks.Add(b);
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Block b))
                affectedBlocks.Remove(b);
        }
    }

    [Serializable]
    public enum AttractionType
    {
        Magnet, Repulsion, Wind
    }
}