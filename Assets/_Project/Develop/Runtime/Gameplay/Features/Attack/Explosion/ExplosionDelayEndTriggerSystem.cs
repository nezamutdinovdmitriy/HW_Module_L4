using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explosion
{
    public class ExplosionDelayEndTriggerSystem : IInitializableSystem, IDisposable
    {
        private ReactiveEvent _explosionStartEvent;
        private ReactiveEvent _delayEndEvent;

        private ReactiveVariable<float> _delay;
        private ReactiveVariable<float> _processCurrentTime;

        private IDisposable _explosionStartEventDisposable;
        private IDisposable _processCurrentTimeDisposable;

        private bool _alreadyExploded;

        public void OnInit(Entity entity)
        {
            _explosionStartEvent = entity.ExplosionStartEvent;
            _delayEndEvent = entity.ExplosionDelayEndEvent;

            _delay = entity.ExplosionDelayTime;
            _processCurrentTime = entity.ExplosionProcessCurrentTime;

            _explosionStartEventDisposable = _explosionStartEvent.Subscribe(OnExplosionStarted);
            _processCurrentTimeDisposable = _processCurrentTime.Subscribe(OnCurrentTimeChanged);
        }

        public void Dispose()
        {
            _explosionStartEventDisposable.Dispose();
            _processCurrentTimeDisposable.Dispose();
        }

        private void OnExplosionStarted()
        {
            _alreadyExploded = false;
        }

        private void OnCurrentTimeChanged(float arg1, float currentTime)
        {
            if (_alreadyExploded)
                return;

            if (currentTime >= _delay.Value)
            {
                Debug.Log("The delay before explosion is over.");

                _delayEndEvent?.Invoke();
                _alreadyExploded = true;
            }
        }
    }
}