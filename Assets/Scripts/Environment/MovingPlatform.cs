using UnityEngine;

namespace Environment {

    public class MovingPlatform : MonoBehaviour
    {
        public LeanTweenType movementType = LeanTweenType.easeInOutBack;
        public Transform start;
        public Transform end;
        public float moveTime;
        public bool moveOnGameStart;
        public bool loop;

        Vector3 startPosition;
        Vector3 endPosition;

        void Start()
        {
            startPosition = start.position;
            endPosition = end.position;

            if (moveOnGameStart)
                MoveToEnd();
        }

        public void MoveToEnd()
        {
            transform.position = startPosition;
            var move = LeanTween.move(gameObject, endPosition, moveTime).setEase(movementType);
            if (loop)
                move.setOnComplete(MoveToStart);
        }

        public void MoveToStart()
        {
            transform.position = endPosition;
            var move = LeanTween.move(gameObject, startPosition, moveTime).setEase(movementType);
            if (loop)
                move.setOnComplete(MoveToEnd);
        }
    }
}