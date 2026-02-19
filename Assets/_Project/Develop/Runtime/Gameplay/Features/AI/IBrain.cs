using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI
{
    public interface IBrain : IDisposable
    {
        public void Enable();
        public void Disable();
        public void Update(float deltaTime);
    }
}