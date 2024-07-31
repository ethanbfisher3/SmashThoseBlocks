using Blocks;
using GameManagement;
using Levels;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public Sprite cursorImage;

    void Awake()
    {
        Vector2 offset = new Vector2(cursorImage.rect.width / 2f, cursorImage.rect.height / 2f);
        UnityEngine.Cursor.SetCursor(cursorImage.texture, offset, CursorMode.Auto);
    }

    void Update()
    {
        if (!GameManager.Instance.Playing) return;

        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(pos.x, pos.y, 0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.Instance.Playing) return;

        if (collision.TryGetComponent(out Block block))
            GameLevel.Current.blockHandler.HighlightBlock(block);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!GameManager.Instance.Playing) return;

        if (collision.TryGetComponent(out Block block) && GameLevel.Current.blockHandler.HighlightedBlock == block)
            GameLevel.Current.blockHandler.UnHighlightBlock(block);
    }
}
