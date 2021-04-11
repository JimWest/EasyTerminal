using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using System;

namespace EasyTerminal
{
    public class LoginWindow : Window
    {
        [Header("Objects")]
        [SerializeField]
        private Window _windowAfterLogin;
        [SerializeField]
        private Text _digitText;
        [SerializeField]
        private CanvasGroup _accessDeniedPanel;
        [SerializeField]
        private CanvasGroup _accessGrantedPanel;

        [Header("Options")]
        [SerializeField]
        private string _loginCode;
        [SerializeField]
        private int _maxDigits = 4;
        [SerializeField]
        private float _panelFadeFactor = 1.5f;
        [SerializeField]
        private float _panelWaitTime = 1f;

        private StringBuilder _currentDigits;

        // Use this for initialization
        new void Start()
        {
            base.Start();
            _currentDigits = new StringBuilder();

            // make sure the access panels are invisible
            _accessGrantedPanel.alpha = 0.0f;
            _accessDeniedPanel.alpha = 0.0f;
        }

        public void CheckCode()
        {
            CanvasGroup tempGroup = _accessDeniedPanel;
            Action loggedInCallback = null;
             
            if (_loginCode.Equals(_currentDigits.ToString()))
            {
                tempGroup = _accessGrantedPanel;
                loggedInCallback = delegate () 
                {
                    OnSuccessfullyLoggedIn();
                };
            }
            StartCoroutine(_windowManager.FadeInAndOutCanvasGroup(tempGroup, _panelFadeFactor, _panelWaitTime, loggedInCallback));
        }


        public void AddDigit(string digit)
        {
            if (_currentDigits.Length < _maxDigits)
            {
                _currentDigits.Insert(_currentDigits.Length, digit);
                _digitText.text = _currentDigits.ToString();
            }
        }

        public void ClearDigit()
        {
            if (_currentDigits.Length > 0)
            {
                _currentDigits.Remove(_currentDigits.Length - 1, 1);
                _digitText.text = _currentDigits.ToString();
            }
        }

        public void ClearAllDigits()
        {
            _currentDigits.Length = 0;
            _digitText.text = _currentDigits.ToString();
        }

        public void OnSuccessfullyLoggedIn()
        {
            if (_windowAfterLogin)
                _windowManager.SetWindowActive(_windowAfterLogin);
        }
    }
}
