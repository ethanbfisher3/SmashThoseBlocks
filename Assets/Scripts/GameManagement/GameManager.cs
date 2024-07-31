using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Utils;
using UnityEngine.Events;
using Levels;
using Blocks;

namespace GameManagement
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public bool Playing { get; private set; }
        public bool IsAboutToWin { get; set; }
        public UnityEvent OnLevelRestart { get; private set; }

        public Button nextBlockButton;
        public Button prevBlockButton;
        [Tooltip("If this number is negative, the saved level number will be loaded. Otherwise this value will be used")]
        public int startLevelIndex;
        public bool skipAllCutscenes;
        public bool requireCloseBlocks;
        public bool saveData = true;
        public bool loadData = true;

        void Awake()
        {
            Instance = this;
            OnLevelRestart = new UnityEvent();
        }

        void Start()
        {
            IsAboutToWin = false;
            if (startLevelIndex < 0)
            {
                int level = (int)SaveSystem.LoadData("level-to-load", 0);
                LoadLevel(level);
            }
            else LoadLevel(startLevelIndex);
        }

        public void Win()
        {
            AudioManager.Instance.Play("Win", pauseTheme: true);
            GameLevel.Current.OnLevelEnd?.Invoke();
            Playing = false;
            GameLevel.Current.blockHandler.DeselectBlock();
            GameUI.Instance.RemoveAllGameOptions();
            if (!Resources.Load($"Levels/Level{GameLevel.Current.number + 1}"))
                LoadHomepage();
            else
                GameUI.Instance.OpenWinPage();
        }

        public void Pause()
        {
            GameUI.Instance.RemoveGameOption(KeyCode.R);
            Playing = false;
            GameLevel.Current.blockHandler.PauseBlocks();
        }

        public void UnPause()
        {
            GameUI.Instance.AddGameOption(RestartLevel, "Restart", KeyCode.R, false);
            Playing = true;
            GameLevel.Current.blockHandler.UnPauseBlocks();
        }

        public void RestartLevel()
        {
            LoadLevel(GameLevel.Current.number, playCutScene: false);
        }

        public void LoadNextLevel() => LoadLevel(GameLevel.Current.number + 1);
        public void LoadLevel(int index, bool playCutScene = true)
        {
            OnLevelRestart.Invoke();

            IsAboutToWin = false;
            GameUI.Instance.RemoveAllGameOptions();

            if (GameLevel.Current?.gameObject)
                Destroy(GameLevel.Current.gameObject);

            var level = Resources.Load<GameLevel>($"Levels/Level{index}");
            if (level)
            {
                Playing = false;
                GameUI.Instance.AddGameOption(RestartLevel, "Restart", KeyCode.R, false);
                level = Instantiate(level);
                if (playCutScene && !skipAllCutscenes)
                {
                    level.OnCutSceneEnd.AddListener(() => Playing = true);
                    level.PlayCutScene();
                }
                else Playing = true;
            }
            else
                LoadHomepage();
        }
        public void LoadHomepage() => SceneManager.LoadScene("Title");

        public void SelecteNextBlock() => GameLevel.Current.blockHandler.SelectNextBlock();
        public void SelectPreviousBlock() => GameLevel.Current.blockHandler.SelectPrevBlock();
    }
}