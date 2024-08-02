using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Environment;
using Blocks;
using GameManagement;

namespace Levels
{
    public class GameLevel : MonoBehaviour
    {
        public static GameLevel Current { get; private set; }
        public Block SelectedBlock { get => blockHandler.SelectedBlock; }
        public CameraController CameraController { get; private set; }

        public UnityEvent OnLevelBegin;
        public UnityEvent OnLevelEnd;
        public UnityEvent OnCutSceneEnd;
        public LevelCompleter levelCompleter;
        public BlockHandler blockHandler;
        public Transform environment;
        [Tooltip("The amount of time it takes to run the cutscene")]
        public float cutsceneSeconds = 5f;
        public bool shouldRecalculatePhysicsScene = true;
        public float physicsRecalculationTime = 2f;
        public int number;

        bool cutscenePlayed = false;

        protected virtual void Awake()
        {
            OnLevelBegin?.Invoke();
            CameraController = Camera.main.transform.GetComponent<CameraController>();
            Current = this;
        }

        protected virtual void Start()
        {
            var multipleBlocks = blockHandler.ChildBlocks.Length > 1;
            GameManager.Instance.nextBlockButton.gameObject.SetActive(multipleBlocks);
            GameManager.Instance.prevBlockButton.gameObject.SetActive(multipleBlocks);

            TrajectoryLine.Instance.CreatePhysicsScene();

            PlayCutScene();
        }

        public void PlayCutScene()
        {
            if (cutscenePlayed) return;
            cutscenePlayed = true;

            if (!GameManager.Instance.skipAllCutscenes)
                CameraController.OnCutscene(OnCutSceneEnd);
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (!levelCompleter)
            {
                levelCompleter = transform.GetComponentInChildren<LevelCompleter>();
                if (!levelCompleter)
                {
                    GameObject levelCompleter = new GameObject("LevelCompleter");
                    levelCompleter.transform.parent = transform;
                    SpriteRenderer sr = levelCompleter.AddComponent<SpriteRenderer>();
                    sr.sprite = FindObjectOfType<Sprites>().whitePixel;
                    BoxCollider2D bc = levelCompleter.AddComponent<BoxCollider2D>();
                    bc.isTrigger = true;
                    levelCompleter.AddComponent<BoxCollider2D>();
                    this.levelCompleter = levelCompleter.AddComponent<LevelCompleter>();
                }
            }
            if (!blockHandler)
            {
                blockHandler = transform.GetComponentInChildren<BlockHandler>();
                if (!blockHandler)
                {
                    GameObject blocks = new GameObject("Blocks");
                    blocks.transform.parent = transform;
                    this.blockHandler = blocks.AddComponent<BlockHandler>();
                }
            }
            if (!environment)
            {
                if (transform.Find("Environment"))
                    environment = transform.Find("Environment");

                else
                {
                    GameObject env = new GameObject("Environment");
                    env.transform.parent = transform;

                    GameObject box = new GameObject("Box");
                    SpriteRenderer sr = box.AddComponent<SpriteRenderer>();
                    sr.sprite = Sprites.Instance.whitePixel;

                    box.AddComponent<BoxCollider2D>();
                    box.transform.parent = env.transform;
                    box.transform.localScale = new Vector2(3.5f, 3.5f);

                    environment = env.transform;
                }
            }
        }
#endif
    }
}