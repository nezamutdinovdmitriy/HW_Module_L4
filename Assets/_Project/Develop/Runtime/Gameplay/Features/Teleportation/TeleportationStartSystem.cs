using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Teleportation
{
    public class TeleportationStartSystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveEvent _startRequest;
        private ReactiveEvent _startEvent;

        private ReactiveVariable<bool> _inProcess;

        private ICompositeCondition _canStart;

        private IDisposable _startRequestDisposable;

        private ReactiveVariable<float> _cost;
        private ReactiveVariable<float> _currentEnergy;

        private ReactiveVariable<float> _radius;

        private Transform _transform;

        public void OnInit(Entity entity)
        {
            _startEvent = entity.TeleportationStartEvent;
            _startRequest = entity.TeleportationStartRequest;

            _inProcess = entity.TeleportationInProcess;

            _canStart = entity.TeleportationCanStart;

            _cost = entity.TeleportationCost;
            _currentEnergy = entity.CurrentEnergy;

            _transform = entity.Transform;

            _radius = entity.TeleportationRadiusArea;

            _startRequestDisposable = _startRequest.Subscribe(OnStartRequest);
        }

        public void OnDispose()
        {
            _startRequestDisposable.Dispose();
        }

        private void OnStartRequest()
        {
            if (_canStart.Evaluate())
            {
                _currentEnergy.Value -= _cost.Value;

                _inProcess.Value = true;
                _startEvent?.Invoke();

                _transform.gameObject.SetActive(false);

                Vector2 randomPoint = Random.insideUnitCircle * _radius.Value;
                _transform.position = _transform.position + new Vector3(randomPoint.x, 0f, randomPoint.y);

                Debug.Log("Teleportation started");
            }
            else
            {
                Debug.Log("Don't teleportation");
            }
        }
    }
}