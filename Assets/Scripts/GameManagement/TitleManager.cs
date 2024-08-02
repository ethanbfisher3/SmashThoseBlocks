using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;
using System.Collections;
using Assets.Utils;

namespace GameManagement
{

    public class TitleManager : MonoBehaviour
    {
        public Button backButton;
        public Button audioButton;
        public Button playButton;

        public GameObject titleScreen;
        public GameObject levelScreen;

        public GameObject levelButtonsParent;
        public Slider loadSlider;

        void Awake()
        {
            SaveSystem.LoadAllData();

            audioButton.onClick.AddListener(() =>
            {
                if (AudioListener.volume == 0f)
                {
                    audioButton.image.sprite = Sprites.Instance.volumeOn;
                    AudioListener.volume = 1f;
                }
                else
                {
                    audioButton.image.sprite = Sprites.Instance.volumeOff;
                    AudioListener.volume = 0f;
                }
            });

            bool shouldDisable = false;
            levelButtonsParent.transform.ForEachChild((child, index) =>
            {
                var button = child.GetComponent<Button>();
                button.onClick.AddListener(() => LoadLevel(index));

                button.interactable = !shouldDisable;
                if ((bool)SaveSystem.LoadData("level-" + index + "-completed", false))
                {
                    button.GetComponent<Image>().color = Color.green;
                }
                else shouldDisable = true;

            });

            playButton.onClick.AddListener(() =>
            {
                backButton.gameObject.SetActive(true);
                levelScreen.SetActive(true);
                titleScreen.SetActive(false);
            });

            backButton.onClick.AddListener(() =>
            {
                backButton.gameObject.SetActive(false);
                levelScreen.SetActive(false);
                titleScreen.SetActive(true);
            });
        }

        void Start()
        {
            titleScreen.SetActive(true);
            levelScreen.SetActive(false);
            backButton.gameObject.SetActive(false);
            loadSlider.gameObject.SetActive(false);
        }

        void LoadLevel(int index)
        {
            loadSlider.gameObject.SetActive(true);
            loadSlider.value = 0;

            backButton.gameObject.SetActive(false);
            audioButton.gameObject.SetActive(false);

            InterSceneInfo.Instance.LevelToLoad = index;
            StartCoroutine(LoadLevelAsync());
        }

        IEnumerator LoadLevelAsync()
        {
            var op = SceneManager.LoadSceneAsync("Game");

            while (!op.isDone)
            {
                loadSlider.value = op.progress;
                yield return null;
            }

            loadSlider.value = 1;
        }
    }
}