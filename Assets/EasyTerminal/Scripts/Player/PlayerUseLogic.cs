using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityStandardAssets.CrossPlatformInput;

namespace EasyTerminal
{
    public class PlayerUseLogic : MonoBehaviour
    {
        [SerializeField]
        protected GameObject _interactionCanvas;
        [SerializeField]
        protected MonoBehaviour _playerScript;
        [SerializeField]
        protected SmoothCameraMover _cameraMover;
        [SerializeField]
        protected float _useWaitTime = 1f;
        [SerializeField]
        protected float _playerReenableTime = 1f;

        IUseable _useableObject;
        GameObject _useableGameObject;
        IUseable _oldUseableObject;
        GameObject _interactionCanvasInstance;
        float _lastUseTime;
        bool _using;
        bool _externalUseEnd = false;

        void Start()
        {
            if (_interactionCanvas != null)
            {
                _interactionCanvasInstance = Instantiate(_interactionCanvas);
                _interactionCanvasInstance.SetActive(false);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_useableObject != null && Time.time >= _lastUseTime + _useWaitTime)
                {
                    _lastUseTime = Time.time;

                    if (_useableObject.CanBeUsed(this))
                    {
                        _useableObject.OnUse(this);
                        _externalUseEnd = _useableObject.HasExternalUseEnd();
                        _using = true;

                        if (_cameraMover != null)
                        {
                            _playerScript.enabled = false;
                            _cameraMover.ZoomCameraIn(_useableGameObject.transform);
                            _interactionCanvasInstance.SetActive(false);

                            Cursor.lockState = CursorLockMode.None;
                            Cursor.visible = true;
                        }
                    }
                    else if (_using && !_externalUseEnd)
                    {
                        _useableObject.OnEndUse(this);
                        EndUse();
                    }

                }
            }

        }

        public void EndUse()
        {
            _using = false;

            if (_cameraMover != null)
            {
                _cameraMover.ZoomCameraOut();
                StartCoroutine(EnablePlayerAfterDelay());

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        IEnumerator EnablePlayerAfterDelay()
        {
            yield return new WaitForSeconds(_playerReenableTime);
            _playerScript.enabled = true;
            _interactionCanvasInstance.SetActive(true);
        }

        void OnTriggerEnter(Collider other)
        {
            IUseable tempUseable = (IUseable)other.gameObject.GetComponentInChildren(typeof(IUseable));
            if (tempUseable != null && _oldUseableObject != tempUseable && tempUseable.CanBeUsed(this))
            {
                _oldUseableObject = tempUseable;
                _useableObject = (IUseable)other.gameObject.GetComponentInChildren(typeof(IUseable));
                _useableGameObject = other.gameObject;

                if (_interactionCanvas != null)
                {
                    _interactionCanvasInstance.SetActive(true);
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            IUseable tempUseable = (IUseable)other.gameObject.GetComponentInChildren(typeof(IUseable));
            if (tempUseable != null && _oldUseableObject == tempUseable)
            {
                _oldUseableObject = null;
                _useableObject = null;
                _useableGameObject = null;

                if (_interactionCanvas != null)
                {
                    _interactionCanvasInstance.SetActive(false);
                }
            }
        }


    }
}
