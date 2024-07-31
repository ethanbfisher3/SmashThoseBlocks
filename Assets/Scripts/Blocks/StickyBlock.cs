using UnityEngine;

namespace Blocks
{

    public class StickyBlock : Block
    {
        // public override bool TryThrow(bool playAudio = true)
        // {
        //     if (CanBeThrown())
        //     {
        //         Rigidbody.isKinematic = false;
        //         Rigidbody.freezeRotation = false;
        //     }
        //     return base.TryThrow(playAudio);
        // }

        public override bool CanFuseWith(Block other) => !other.ContainsBlockOfType<StickyBlock>();
    }
}