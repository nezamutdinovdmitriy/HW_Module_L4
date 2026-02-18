using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explosion
{
    public class ExplosionEndSystem : IInitializableSystem, IDisposable
    {
        private ReactiveEvent _endEvent;

        private ReactiveVariable<bool> _inProcess;

        private ReactiveVariable<float> _processInitialTime;
        private ReactiveVariable<float> _processCurrentTime;

        private IDisposable _processCurrentTimeDisposable;

        public void OnInit(Entity entity)
        {
            _endEvent = entity.ExplosionEndEvent;

            _inProcess = entity.ExplosionInProcess;

            _processInitialTime = entity.ExplosionProcessInitialTime;
            _processCurrentTime = entity.ExplosionProcessCurrentTime;

            _processCurrentTimeDisposable = _processCurrentTime.Subscribe(OnCurrentTimeChanged);
        }

        public void Dispose()
        {
            _processCurrentTimeDisposable.Dispose();
        }

        private void OnCurrentTimeChanged(float arg1, float currentTime)
        {
            if (TimerIsDone(currentTime))
            {
                _inProcess.Value = false;
                _endEvent?.Invoke();


                Debug.Log("Explosion ended");
            }
        }

        private bool TimerIsDone(float currentTime) => currentTime >= _processInitialTime.Value;
    }
}