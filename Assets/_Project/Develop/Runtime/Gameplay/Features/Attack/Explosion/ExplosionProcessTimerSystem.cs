using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explosion
{
    public class ExplosionProcessTimerSystem : IInitializableSystem, IDisposable, IUpdatableSystem
    {
        private ReactiveEvent _startEvent;

        private ReactiveVariable<float> _processCurrentTime;
        private ReactiveVariable<bool> _inProcess;

        private IDisposable _startEventDisposable;

        public void OnInit(Entity entity)
        {
            _processCurrentTime = entity.ExplosionProcessCurrentTime;

            _inProcess = entity.ExplosionInProcess;

            _startEvent = entity.ExplosionStartEvent;

            _startEventDisposable = _startEvent.Subscribe(OnStartedExplosion);
        }

        public void Dispose()
        {
            _startEventDisposable.Dispose();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_inProcess.Value == false)
                return;

            _processCurrentTime.Value += deltaTime;
        }

        private void OnStartedExplosion()
        {
            _processCurrentTime.Value = 0;
        }
    }
}