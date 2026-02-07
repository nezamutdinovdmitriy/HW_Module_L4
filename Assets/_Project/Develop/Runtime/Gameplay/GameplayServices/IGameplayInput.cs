using System;

namespace Assets._Project.Develop.Runtime.Gameplay.GameplayServices
{
    public interface IGameplayInput
    {
        public event Action<char> OnInput;
        public event Action OnConfirm;

        public void Update();
    }
}