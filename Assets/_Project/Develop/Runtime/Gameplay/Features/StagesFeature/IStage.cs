using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature
{
    public interface IStage : IDisposable
    {
        public IReadOnlyEvent Completed { get; }

        public void Start();
        public void Update(float deltaTime);
        public void Cleanup();
    }
}