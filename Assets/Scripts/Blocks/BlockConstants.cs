using UnityEngine;

namespace Blocks
{
    public class BlockConstants : MonoBehaviour
    {
        public static BlockConstants Instance;

        public Block[] blockPrefabs;
        public Color selectedColor = new Color(220f, 220f, 0f);
        public Color highlightedColor = new Color(255f, 255f, 0f);
        public Color highlightedAndSelectedColor = new Color(180f, 180f, 0f);
        public float distanceForMaxThrowForce = 175f;
        public float waitTimeBetweenThrows = 0.4f;
        public float waitTimeBetweenPortalMovement = 1f;
        [Tooltip("The maximum distance from the selected block that another block may be selected")]
        public float blockSelectDistance = 5f;
        public float blockThrowPower = 7.5f;

        void Awake()
        {
            Instance = this;
        }

        public Block GetBlockPrefab(string blockName)
        {
            foreach (var block in blockPrefabs)
            {
                if (block.gameObject.name.StartsWith(blockName))
                    return block;
            }
            return null;
        }
    }
}
