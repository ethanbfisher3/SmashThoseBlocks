using UnityEngine;

public class InterSceneInfo : MonoBehaviour
{
    public static InterSceneInfo Instance { get; private set; }

    public int LevelToLoad { get; set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}