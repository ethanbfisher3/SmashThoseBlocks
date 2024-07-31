using System;
using System.Collections;
using Blocks;
using GameManagement;
using Levels;
using UnityEngine;

namespace Environment
{
    public class BlockDestroyer : MonoBehaviour
    {
        public float waitTimeBeforeDestroying = 0.75f;

        private bool WillDestroy { get; set; }

        void Start()
        {
            WillDestroy = false;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (!WillDestroy && collision.TryGetComponent(out Block b) && !collision.CompareTag("Ghost"))
            {
                WillDestroy = true;
                StartCoroutine(WaitAndDestroy(b));
            }
        }

        IEnumerator WaitAndDestroy(Block b)
        {
            yield return new WaitForSeconds(waitTimeBeforeDestroying);

            if (b.ContainsFlaggedBlock())
                GameManager.Instance.RestartLevel();
            else
                Destroy(b.gameObject);

            if (GameLevel.Current.SelectedBlock == b)
                GameLevel.Current.blockHandler.DeselectBlock();
        }

    }
}