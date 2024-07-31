using System;
using GameManagement;
using UnityEngine;
using Utils;

namespace Blocks
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Block : MonoBehaviour
    {
        public float throwPower = 5f;
        public Vector2 MousePositionAtLastThrow { get; private set; }
        public Vector2 PositionAtLastThrow { get; private set; }

        //[HideInInspector] public int fusionLevel = 0;

        public Rigidbody2D Rigidbody { get; set; }
        public SpriteRenderer ImageOverlay { get; set; }
        public Sprite innerImage;

        float startThrowableTime;

        private SpriteRenderer SpriteRenderer { get; set; }

        void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();

            startThrowableTime = Time.time;
        }

        void Update()
        {
            if (IsMoving())
                startThrowableTime = Time.time;
        }

        public bool IsMoving() => Rigidbody.velocity.sqrMagnitude > 0.01f;
        public bool StillFor(float time) => Time.time - startThrowableTime > time;

        public virtual bool CanFuseWith(Block other) => true;
        public virtual void OnBecomeFusedTo(Block other) { }

        public virtual void OnBreakApartFrom(Block other) { }

        public T FindChildOfType<T>() where T : Block
        {
            return this is T ? this as T : GetComponentInChildren<T>(true);
        }

        public Block FindChildOfType(System.Type type)
        {
            if (GetType() == type)
                return this;
            var children = GetComponentsInChildren<Block>(true);
            foreach (var child in children)
                if (child.GetType() == type)
                    return child;
            return null;
        }

        protected bool CanBeThrown()
        {
            bool still = StillFor(BlockConstants.Instance.waitTimeBetweenThrows);
            return still;
        }

        // throws the block if able
        // returns true if the block was thrown, false if not thrown
        public virtual bool TryThrow(bool playAudio = true)
        {
            if (!CanBeThrown()) return false;
            ForceThrow(playAudio);
            return true;
        }

        public virtual void ForceThrow(bool playAudio = true)
        {
            if (playAudio)
                AudioManager.Instance.Play("Whoosh");

            Vector2 mouseOffset = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            MousePositionAtLastThrow = Input.mousePosition;
            PositionAtLastThrow = transform.position;
            float distance = mouseOffset.magnitude;
            var direction = mouseOffset.normalized;
            float forceMultiplier = Math.Min(distance / BlockConstants.Instance.distanceForMaxThrowForce, 1f);

            Rigidbody.isKinematic = false;
            Rigidbody.freezeRotation = false;

            Rigidbody.AddForce(direction * forceMultiplier * BlockConstants.Instance.blockThrowPower, ForceMode2D.Impulse);
        }

        public virtual void SetSpriteRendererColor(Color color)
        {
            SpriteRenderer.color = color;
        }

        public bool ContainsBlockOfType<T>() where T : Block
        {
            return this is T || GetComponentInChildren<T>(true) != null;
        }

        public bool TryGetBlockOfType<T>(out T block) where T : Block
        {
            if (this is T)
            {
                block = this as T;
                return true;
            }
            block = GetComponentInChildren<T>(true);
            return block != null;
        }

        public bool ContainsFlaggedBlock()
        {
            if (name.Equals("FlaggedBlock")) return true;
            foreach (Block block in GetComponentsInChildren<Block>(true))
                if (block.name.Equals("FlaggedBlock")) return true;
            return false;
        }

        void OnValidate()
        {
            if (GetComponent<SpriteRenderer>().sortingOrder == 0)
                GetComponent<SpriteRenderer>().sortingOrder = 1;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, FindObjectOfType<BlockConstants>().blockSelectDistance);
        }
    }
}