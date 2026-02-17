using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Teleportation
{
    public class TeleportationEndSystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveEvent _endEvent;

        private ReactiveVariable<bool> _inProcess;

        private ReactiveVariable<float> _currentTime;
        private ReactiveVariable<float> _initialTime;

        private IDisposable _initialTimeDisposable;

        private Transform _transform;

        public void OnInit(Entity entity)
        {
            _endEvent = entity.TeleportationEndEvent;
            _inProcess = entity.TeleportationInProcess;
            _currentTime = entity.TeleportationCurrentTime;
            _initialTime = entity.TeleportationInitialTime;

            _transform = entity.Transform;

            _initialTimeDisposable = _currentTime.Subscribe(OnCurrentTimeChanged);
        }

        public void OnDispose()
        {
            _initialTimeDisposable.Dispose();
        }

        private void OnCurrentTimeChanged(float arg1, float currentTime)
        {
            if(TimerIsDone(currentTime))
            {
                _transform.gameObject.SetActive(true);

                _inProcess.Value = false;
                _endEvent?.Invoke();

                Debug.Log("Teleportation ended!");
            }
        }

        private bool TimerIsDone(float currentTime) => currentTime >= _initialTime.Value;
    }
}