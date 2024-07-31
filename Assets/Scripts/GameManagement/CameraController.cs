using System;
using System.Collections;
using Blocks;
using Levels;
using UnityEngine;
using UnityEngine.Events;

namespace GameManagement
{
    public enum CameraControllerBehavior { Default, CutScene }

    public class CameraController : MonoBehaviour
    {
        public float minYPos = -2f;
        public float minOrthographicSize = 2f;
        public float maxOrthographicSize = 10f;
        public float lerpRate = 2.5f;
        public float yOffset = 1f;

        private CameraControllerBehavior Behavior { get; set; }

        private Vector2 Origin { get; set; }
        private Vector2 Difference { get; set; }

        private bool Drag { get; set; }
        private bool ShouldLerpToAverageBlockPosition { get; set; }
        private bool ShouldLerpToSelectedBlockPosition { get; set; }

        private Block PrevSelectedBlock { get; set; }

        void Start()
        {
            Drag = false;
            ShouldLerpToAverageBlockPosition = true;
            ShouldLerpToSelectedBlockPosition = false;

            Behavior = CameraControllerBehavior.Default;

            GameManager.Instance.OnLevelRestart.AddListener(() =>
            {
                Behavior = CameraControllerBehavior.Default;
                Drag = false;
                ShouldLerpToAverageBlockPosition = true;
                ShouldLerpToSelectedBlockPosition = false;
            });
        }

        void FixedUpdate()
        {
            if (!GameManager.Instance.Playing) return;

            if (Behavior == CameraControllerBehavior.Default)
                UpdateDefaultBehavior();
        }

        private void UpdateDefaultBehavior()
        {
            // deal with camera movement
            var cameraTransform = Camera.main.transform;

            var selectedBlock = GameLevel.Current.blockHandler != null ? GameLevel.Current.blockHandler.SelectedBlock : null;

            // mouse right click
            if (Input.GetMouseButton(1))
            {
                Difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - cameraTransform.position;
                if (!Drag)
                {
                    Drag = true;
                    ShouldLerpToAverageBlockPosition = false;
                    ShouldLerpToSelectedBlockPosition = false;
                    Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    GameLevel.Current.blockHandler.DeselectBlock();
                }
            }

            // otherwise, figure out if it should lerp to the blocks
            else
            {
                Drag = false;

                if (!selectedBlock)
                {
                    if (PrevSelectedBlock)
                    {
                        ShouldLerpToAverageBlockPosition = true;
                        ShouldLerpToSelectedBlockPosition = false;
                    }
                    PrevSelectedBlock = null;
                }
                else if (!PrevSelectedBlock)
                {
                    ShouldLerpToSelectedBlockPosition = true;
                    ShouldLerpToAverageBlockPosition = false;
                }
            }

            if (Drag)
            {
                cameraTransform.position = Origin - Difference;
            }
            else if (ShouldLerpToAverageBlockPosition)
            {
                LerpToAverageBlockPosition();
            }
            else if (ShouldLerpToSelectedBlockPosition)
            {
                if (selectedBlock)
                {
                    var pos = selectedBlock.transform.position;
                    pos.y += yOffset;
                    transform.position = Vector3.Lerp(transform.position, pos, Time.fixedDeltaTime * lerpRate);
                }
            }


            PrevSelectedBlock = selectedBlock;

            var position = cameraTransform.position;
            position.z = -10;
            cameraTransform.position = position;
        }

        void Update()
        {
            // scroll to zoom
            if (Behavior == CameraControllerBehavior.Default)
            {
                var scroll = Input.GetAxis("Mouse ScrollWheel") * 0.5f;
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - scroll, minOrthographicSize, maxOrthographicSize);
            }
        }

        IEnumerator MoveToLevelCompleter(UnityEvent OnEnd)
        {
            GameManager.Instance.Pause();

            var level = GameLevel.Current;
            var levelCompleter = level.levelCompleter;
            var blockHandler = level.blockHandler;
            var seconds = level.cutsceneSeconds;
            var moveInOrOutSeconds = (seconds - 2f) / 2f;

            var initCamPos = Camera.main.transform.position;
            var levelCompleterPos = levelCompleter.transform.position;
            levelCompleterPos.y += 3f;
            levelCompleterPos.z = -10f;

            float distance = Vector2.Distance(initCamPos, levelCompleterPos);
            float time = distance / 10f;
            time = Mathf.Min(time, 3f);

            var moveToLevelCompleter = LeanTween.move(gameObject, levelCompleterPos, moveInOrOutSeconds)
                .setDelay(moveInOrOutSeconds);

            var moveToInitialPosition = LeanTween.move(gameObject, initCamPos, moveInOrOutSeconds)
                .setDelay(moveInOrOutSeconds * 2f + 1f);

            GameUI.Instance.AddGameOption(() =>
            {
                StopAllCoroutines();

                // make the camera controller go back to the flagged block
                LeanTween.move(gameObject, initCamPos, 0.5f).setOnComplete(() =>
                {
                    OnEnd?.Invoke();
                    GameManager.Instance.UnPause();
                    moveToLevelCompleter.pause();
                    moveToInitialPosition.pause();
                });
            }, "Skip", KeyCode.S);

            yield return new WaitForSeconds(seconds);

            GameUI.Instance.RemoveGameOption(KeyCode.S);
            GameManager.Instance.UnPause();
            OnEnd?.Invoke();
        }

        public void OnCutscene(UnityEvent OnEnd)
        {
            Behavior = CameraControllerBehavior.CutScene;

            var avgPos = GameLevel.Current.blockHandler.GetAverageBlockPosition();
            avgPos.y += yOffset;
            transform.position = avgPos;

            StartCoroutine(MoveToLevelCompleter(OnEnd));

            Behavior = CameraControllerBehavior.Default;
        }

        public LTDescr LeanTweenToFlag(float time = 0.5f)
        {
            Drag = false;
            ShouldLerpToAverageBlockPosition = false;
            ShouldLerpToSelectedBlockPosition = false;
            var pos = GameLevel.Current.blockHandler.FlaggedBlock.transform.position;
            pos.z = -10f;
            pos.y += yOffset;
            return LeanTween.move(gameObject, pos, time);
        }

        private void LerpToAverageBlockPosition()
        {
            Block[] blocks = GameLevel.Current.blockHandler.ChildBlocks;
            if (blocks.Length == 0) return;

            float totalX = 0f, totalY = 0f;
            foreach (Block block in blocks)
            {
                Vector2 blockPos = block.transform.position;
                totalX += blockPos.x;
                totalY += blockPos.y;
            }

            Vector3 pos = new Vector3(totalX / blocks.Length, totalY / blocks.Length, -10f);
            pos.y += yOffset;
            if (pos.y < minYPos) pos.y = minYPos;
            transform.position = Vector3.Lerp(transform.position, pos, Time.fixedDeltaTime * lerpRate);
        }
    }
}