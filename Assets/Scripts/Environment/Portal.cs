using Blocks;
using GameManagement;
using Levels;
using UnityEngine;

namespace Environment
{

    public class Portal : MonoBehaviour
    {
        public Portal other;
        public Transform spawn;
        public Collider2D portalEntranceCollider;
        public Vector2 thrust;

        void Start()
        {
            // ignore all collisions with portal blocks, allowing them to enter
            var blocksParent = GameLevel.Current.blockHandler;
            foreach (var block in blocksParent.GetBlocksOfType<PortalBlock>())
            {
                var collider = block.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(portalEntranceCollider, collider, true);
            }

            GameLevel.Current.blockHandler.OnBlockFusion.AddListener(fusedBlock =>
            {
                var collider = fusedBlock.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(portalEntranceCollider, collider, true);
            });
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            var block = collider.transform.GetComponent<Block>();
            if (!block) return;

            var portalBlock = block.FindChildOfType<PortalBlock>();

            if (Time.time - portalBlock.LastTeleportTime < BlockConstants.Instance.waitTimeBetweenPortalMovement)
                return;

            block.transform.position = other.spawn.position;
            portalBlock.LastTeleportTime = Time.time;
            block.Rigidbody.velocity = other.thrust;
        }
    }
}
