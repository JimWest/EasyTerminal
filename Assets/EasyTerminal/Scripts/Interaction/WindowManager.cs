using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

namespace EasyTerminal
{
    public class WindowManager : MonoBehaviour, IUseable
    {

        [SerializeField]
        private Window _firstWindow;

        private List<Window> _windows;
        private Window _activeWindow;
        private Window _lastActiveWindow;
        private bool _used;
        PlayerUseLogic _currentUser;


        // Use this for initialization
        void Start()
        {
            _windows = new List<Window>(GetComponentsInChildren<Window>(true));

            if (_firstWindow)
            {
                SetWindowActive(_firstWindow);
            }
        }

        public void SetLastWindowActive()
        {
            SetWindowActive(_lastActiveWindow);
        }

        public void SetWindowActive(Window newActiveWindow)
        {
            if (!newActiveWindow)
            {
                Debug.LogError("Can't set active Window, newActiveWindow is null");
                return;
            }

            if (_activeWindow)
                _lastActiveWindow = _activeWindow;
            else
                _lastActiveWindow = newActiveWindow;

            _activeWindow = newActiveWindow;
            foreach (Window window in _windows)
            {
                if (window != newActiveWindow)
                {
                    window.gameObject.SetActive(false);
                }
            }

            newActiveWindow.gameObject.SetActive(true);
        }

        public void OnBackButton()
        {
            if (_activeWindow && (_activeWindow != _lastActiveWindow))

            {
                _activeWindow.OnBackPressed();
            }
            else
            {
                // there is no last window, so just exit the Window
                OnExitButton();
            }
        }

        public void OnExitButton()
        {
            // end use;
            _used = false;
            _activeWindow.OnEndUse(_currentUser);
            _currentUser.EndUse();
        }


        public IEnumerator FadeInCanvasGroup(CanvasGroup canvasGroup, float panelFadeFactor, Action onFinished = null)
        {
            canvasGroup.gameObject.SetActive(true);
            float alpha = canvasGroup.alpha;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / panelFadeFactor)
            {
                canvasGroup.alpha = Mathf.Lerp(alpha, 1, t);
                yield return null;
            }

            if (onFinished != null)
            {
                onFinished();
            }
        }

        public IEnumerator FadeOutCanvasGroup(CanvasGroup canvasGroup, float panelFadeFactor, Action onFinished = null)
        {
            float alpha = canvasGroup.alpha;
            for (float t = 1.0f; t > 0.0f; t -= Time.deltaTime / panelFadeFactor)
            {
                canvasGroup.alpha = Mathf.Lerp(alpha, 1, t);
                yield return null;
            }

            canvasGroup.gameObject.SetActive(false);

            if (onFinished != null)
            {
                onFinished();
            }
        }

        public IEnumerator FadeInAndOutCanvasGroup(CanvasGroup canvasGroup, float panelFadeFactor, float waitTime, Action onFinished = null)
        {
            canvasGroup.gameObject.SetActive(true);
            float alpha = canvasGroup.alpha;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * panelFadeFactor)
            {
                canvasGroup.alpha = Mathf.Lerp(alpha, 1, t);
                yield return null;
            }

            for (float t = 1.0f; t > 0.0f; t -= Time.deltaTime * panelFadeFactor)
            {
                canvasGroup.alpha = Mathf.Lerp(alpha, 1, t);
                yield return null;
            }

            canvasGroup.gameObject.SetActive(false);

            onFinished?.Invoke();
        }

        public bool HasExternalUseEnd()
        {
            return true;
        }

        public void OnUse(PlayerUseLogic user)
        {
            _currentUser = user;
            _used = true;
            _activeWindow.OnUse(user);
        }


        public void OnEndUse(PlayerUseLogic user)
        {
            _currentUser = null;
            _used = false;
        }

        public bool CanBeUsed(PlayerUseLogic user)
        {
            return !_used;
        }


    }
}