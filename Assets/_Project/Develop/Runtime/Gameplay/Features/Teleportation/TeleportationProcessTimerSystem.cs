using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Teleportation
{
    public class TeleportationProcessTimerSystem : IInitializableSystem, IDisposableSystem, IUpdatableSystem
    {
        private ReactiveEvent _startEvent;
        private ReactiveVariable<bool> _inProcess;

        private ReactiveVariable<float> _currentTime;

        private IDisposable _startEventDisposable;

        public void OnInit(Entity entity)
        {
            _startEvent = entity.TeleportationStartEvent;
            _inProcess = entity.TeleportationInProcess;

            _currentTime = entity.TeleportationCurrentTime;

            _startEventDisposable = _startEvent.Subscribe(OnStartEvent);
        }

        public void OnDispose()
        {
            _startEventDisposable.Dispose();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_inProcess.Value == false)
                return;

            _currentTime.Value += deltaTime;
        }

        private void OnStartEvent()
        {
            _currentTime.Value = 0;
        }
    }
}