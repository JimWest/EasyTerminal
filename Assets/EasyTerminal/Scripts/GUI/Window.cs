using System;
using UnityEngine;
using UnityEngine.UI;

namespace EasyTerminal
{
    [System.Serializable]
    public class Window : MonoBehaviour
    {

        protected WindowManager _windowManager;

        protected virtual void Start()
        {
            _windowManager = GetComponentInParent<WindowManager>();
        }

        public virtual void OnBackPressed()
        {
            _windowManager.SetLastWindowActive();
        }

        public void OnUse(PlayerUseLogic user) { }

        public void OnEndUse(PlayerUseLogic user) { }
    }
}
