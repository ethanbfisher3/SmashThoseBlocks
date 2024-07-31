using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Vector2 parallaxMultiplier;

    Vector3 prevCamPos;
    float width;
    Transform cam;

    void Start()
    {
        cam = Camera.main.transform;

        prevCamPos = cam.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        width = sprite.texture.width / sprite.pixelsPerUnit;
    }

    void LateUpdate()
    {
        Vector3 deltaPos = cam.position - prevCamPos;
        transform.position += new Vector3(deltaPos.x * parallaxMultiplier.x, deltaPos.y * parallaxMultiplier.y);
        prevCamPos = cam.position;

        if (Mathf.Abs(prevCamPos.x - transform.position.x) >= width * transform.localScale.x)
        {
            float offsetPosX = (prevCamPos.x - transform.position.x) % (width * transform.localScale.x);
            transform.position = new Vector3(prevCamPos.x + offsetPosX, transform.position.y);
        }
    }
}
