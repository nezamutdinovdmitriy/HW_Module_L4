using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.GameplayServices
{
    public class GameplayDesktopInputHandler : IGameplayInput
    {
        private const KeyCode ConfirmKey = KeyCode.Space;

        public event Action<char> OnInput;
        public event Action OnConfirm;

        public void Update()
        {
            if(Input.GetKeyDown(ConfirmKey))
                OnConfirm?.Invoke();
            else
                foreach (char c in Input.inputString)
                    OnInput?.Invoke(c);
        }
    }
}