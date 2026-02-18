using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explosion
{
    public class ExplosionStartSystem : IInitializableSystem, IDisposable
    {
        private ReactiveEvent _startRequest;
        private ReactiveEvent _startEvent;

        private ReactiveVariable<bool> _inProcess;

        private ICompositeCondition _startingCondition;

        private IDisposable _startRequestDisposable;

        public void OnInit(Entity entity)
        {
            _startRequest = entity.ExplosionStartRequest;
            _startEvent = entity.ExplosionStartEvent;

            _inProcess = entity.ExplosionInProcess;

            _startingCondition = entity.ExplosionCanStart;

            _startRequestDisposable = _startRequest.Subscribe(OnExplodeRequest);
        }

        private void OnExplodeRequest()
        {
            if (_startingCondition.Evaluate())
            {
                _inProcess.Value = true;
                _startEvent?.Invoke();

                Debug.Log("Started explosion process");
            }
            else
            {
                Debug.Log("Can't start explosion process");
            }
        }

        public void Dispose()
        {
            _startRequestDisposable.Dispose();
        }
    }
}