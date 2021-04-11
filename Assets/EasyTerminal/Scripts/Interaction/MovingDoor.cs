using UnityEngine;
using System.Collections;

namespace EasyTerminal.Entities
{

    public class MovingDoor : MonoBehaviour
    {
        public enum OpeningDirection
        {
            Up,
            Down,
            Left,
            Right
        }

        [SerializeField]
        protected OpeningDirection _openingDirection;
        [SerializeField]
        protected float _movingAmount = 3f;
        [SerializeField]
        protected float _speed = 1f;
        [SerializeField]
        protected bool _locked = false;

        protected Vector3 _movementOffst;
        protected Vector3 _closedPosition;
        protected bool _open = false;
        protected BoxCollider _box;
        private Coroutine _currentCoroutine;

        // Use this for initialization
        void Start()
        {
            _closedPosition = transform.position;
            _box = this.GetComponent<BoxCollider>();

            switch (_openingDirection)
            {
                case OpeningDirection.Up:
                    _movementOffst = Vector3.up;
                    break;
                case OpeningDirection.Down:
                    _movementOffst = Vector3.down;
                    break;
                case OpeningDirection.Left:
                    _movementOffst = Vector3.left;
                    break;
                case OpeningDirection.Right:
                    _movementOffst = Vector3.right;
                    break;
            }

        }

        IEnumerator MoveDoor(Vector3 endPos)
        {
            float t = 0f;
            Vector3 startPos = transform.position;
            while (t < 1f)
            {
                t += Time.deltaTime * _speed;
                transform.position = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }
        }

        public void Open()
        {
            if (_locked)
                return;

            _open = true;
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }

            Vector3 endpos = _closedPosition + (_movementOffst * _movingAmount);
            _currentCoroutine = StartCoroutine(MoveDoor(endpos));
        }

        public void Close()
        {
            _open = false;
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }
            _currentCoroutine = StartCoroutine(MoveDoor(_closedPosition));
        }

        public void Lock()
        {
            _locked = true;
        }

        public void Unlock()
        {
            _locked = false;
        }
    }
}