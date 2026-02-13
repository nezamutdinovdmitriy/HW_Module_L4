using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack
{
    public class AttackProcessTimerSystem : IInitializableSystem, IDisposable, IUpdatableSystem
    {
        private ReactiveVariable<float> _currentTime;

        private ReactiveVariable<bool> _inAttackProcess;

        private ReactiveEvent _startAttackEvent;

        private IDisposable _startAttackEventDisposable;

        public void OnInit(Entity entity)
        {
            _currentTime = entity.AttackProcessCurrentTime;
            _inAttackProcess = entity.InAttackProcess;
            _startAttackEvent = entity.StartAttackEvent;

            _startAttackEventDisposable = _startAttackEvent.Subscribe(OnStartAttackProcess);
        }

        public void Dispose()
        {
            _startAttackEventDisposable.Dispose();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_inAttackProcess.Value == false)
                return;

            _currentTime.Value += deltaTime;
        }

        private void OnStartAttackProcess()
        {
            _currentTime.Value = 0;
        }
    }
}