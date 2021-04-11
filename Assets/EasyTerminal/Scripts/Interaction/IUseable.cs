using UnityEngine;
using System.Collections;

namespace EasyTerminal
{
    public interface IUseable
    {
        bool HasExternalUseEnd();
        void OnUse(PlayerUseLogic user);
        void OnEndUse(PlayerUseLogic user);
        bool CanBeUsed(PlayerUseLogic user);
    }
}
