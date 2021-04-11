using System.Collections;
using UnityEngine;

namespace EasyTerminal
{
    public class SmoothCameraMover : MonoBehaviour
    {

        [SerializeField]
        private Camera _playerCamera;

        [SerializeField]
        private float _speed = 1f;

        [SerializeField]
        private Vector3 _offsetVector;

        [SerializeField]
        private Vector3 _offsetRotation;


        Vector3 _oldCameraPosition;
        Quaternion _oldCameraRot;
        Transform _oldCameraParent;
        Quaternion _tempRotation = Quaternion.identity;
        bool _zommedIn;

        void Start()
        {
        }


        void FixedUpdate()
        {
        }

        public void ZoomCameraIn(Transform targetCameraTransform)
        {
            if (!_zommedIn)
            {
                _zommedIn = true;
                _oldCameraPosition = _playerCamera.transform.position;
                _oldCameraRot = _playerCamera.transform.rotation;
                _oldCameraParent = _playerCamera.transform.parent;
                _playerCamera.transform.SetParent(null);
                _tempRotation.eulerAngles = _offsetRotation;

                StartCoroutine(MoveCamera(_playerCamera.transform, targetCameraTransform.position + _offsetVector, targetCameraTransform.rotation * _tempRotation));
            }
        }

        public void ZoomCameraOut()
        {
            if (_zommedIn)
            {
                StartCoroutine(MoveCamera(_playerCamera.transform, _oldCameraPosition, _oldCameraRot));
                _zommedIn = false;
            }
        }


        IEnumerator MoveCamera(Transform cameraTransform, Vector3 endPos, Quaternion endRot)
        {
            Vector3 startPos = cameraTransform.position;
            Quaternion startRot = cameraTransform.rotation;

            float i = 0;
            while (i < 1)
            {
                i += Time.deltaTime * _speed;
                cameraTransform.position = Vector3.Slerp(startPos, endPos, i);
                cameraTransform.rotation = Quaternion.Slerp(startRot, endRot, i);
                yield return null;
            }

            _playerCamera.transform.SetParent(_oldCameraParent);
        }


    }
}
