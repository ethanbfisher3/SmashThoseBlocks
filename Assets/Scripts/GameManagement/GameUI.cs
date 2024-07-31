using UnityEngine;
using System.Collections.Generic;
using System;
using Levels;
using UnityEngine.UI;

namespace GameManagement
{

    public class GameUI : MonoBehaviour
    {
        public static GameUI Instance { get; private set; }
        public enum NotificationType { Normal, Warning, Error }

        public GameOption gameOptionPrefab;
        public Animator winPageAnimator;
        public Button selectPrevBlock;
        public Button selectNextBlock;

        List<GameOption> gameOptions;

        void Awake()
        {
            Instance = this;

            gameOptions = new List<GameOption>();
        }

        void Update()
        {
            var blockCount = GameLevel.Current.blockHandler.ChildBlocks.Length;

            selectNextBlock.gameObject.SetActive(GameManager.Instance.Playing && blockCount > 1);
            selectPrevBlock.gameObject.SetActive(GameManager.Instance.Playing && blockCount > 1);

            for (int i = 0; i < gameOptions.Count; i++)
            {
                GameOption go = gameOptions[i];
                if (Input.GetKeyDown(go.keyCode))
                {
                    go.OnComplete();
                    if (go.destroyOnUse)
                    {
                        gameOptions.Remove(go);
                        Destroy(go.gameObject);
                        UpdateGameOptions();
                        i--;
                    }
                }
            }
        }

        void UpdateGameOptions()
        {
            for (int i = 0; i < gameOptions.Count; i++)
            {
                RectTransform rt = gameOptions[i].GetComponent<RectTransform>();

                // set pivot to the left side
                rt.anchorMin = new Vector2(0f, 0.5f);
                rt.anchorMax = new Vector2(0f, 0.5f);
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.position = new Vector2(rt.position.x, 400f - 75f * i);
            }
        }

        public void CreateNotification(string text, NotificationType type = NotificationType.Normal)
        {
            switch (type)
            {
                case NotificationType.Normal:
                    Debug.Log(text);
                    break;
                case NotificationType.Error:
                    Debug.LogError(text);
                    break;
                case NotificationType.Warning:
                    Debug.LogWarning(text);
                    break;
            }
        }

        public void AddGameOption(Action OnComplete, string text = null,
            KeyCode code = KeyCode.None, bool destroyOnUse = true)
        {
            foreach (GameOption option in gameOptions)
                if (option.keyCode == code)
                    return;

            GameOption go = Instantiate(gameOptionPrefab, transform);

            if (code != KeyCode.None)
                go.image.sprite = GameOption.SpriteFromKeyCode(code);
            go.OnComplete = OnComplete;
            go.text.text = text;
            go.destroyOnUse = destroyOnUse;
            go.keyCode = code;

            gameOptions.Add(go);
            UpdateGameOptions();
        }

        public void RemoveGameOption(KeyCode keyCode)
        {
            for (int i = 0; i < gameOptions.Count; i++)
                if (gameOptions[i].keyCode == keyCode)
                {
                    Destroy(gameOptions[i].gameObject);
                    gameOptions.RemoveAt(i);
                    UpdateGameOptions();
                    return;
                }
        }
        public void RemoveAllGameOptions()
        {
            for (int i = Instance.gameOptions.Count - 1; i >= 0; i--)
            {
                Destroy(gameOptions[i].gameObject);
                gameOptions.RemoveAt(i);
            }
        }

        public void OpenWinPage()
        {
            winPageAnimator.gameObject.SetActive(true);
            winPageAnimator.Play("In");
        }
        public void CloseWinPage() => winPageAnimator.Play("Out");
    }
}