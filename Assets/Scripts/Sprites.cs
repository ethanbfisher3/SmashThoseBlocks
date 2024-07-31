using UnityEngine;

public class Sprites : MonoBehaviour
{
    public static Sprites Instance { get; private set; }

    public Sprite e, r, s, prev, next, home, letter, space, nextLevel, whitePixel,
        volumeOn, volumeOff;

    void Awake() => Instance = this;
}