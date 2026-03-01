using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Teleportation
{
    public class TeleportationStartSystem : IInitializableSystem, IDisposable
    {
        private ReactiveEvent<Vector3> _startRequest;
        private ReactiveEvent _startEvent;

        private ReactiveVariable<bool> _inProcess;

        private ICompositeCondition _canStart;

        private IDisposable _startRequestDisposable;

        private ReactiveVariable<float> _radius;

        private Transform _transform;

        public void OnInit(Entity entity)
        {
            _startEvent = entity.TeleportationStartEvent;
            _startRequest = entity.TeleportationStartRequest;

            _inProcess = entity.TeleportationInProcess;

            _canStart = entity.TeleportationCanStart;

            _radius = entity.TeleportationRadiusArea;

            _transform = entity.Transform;

            _startRequestDisposable = _startRequest.Subscribe(OnStartRequest);
        }

        public void Dispose()
        {
            _startRequestDisposable.Dispose();
        }

        private void OnStartRequest(Vector3 position)
        {
            if (_canStart.Evaluate())
            {
                _inProcess.Value = true;
                _startEvent?.Invoke();

                _transform.gameObject.SetActive(false);

                if (IsInside(position))
                {
                    _transform.position = position;
                    Debug.Log("Target Inside area");
                }
                else
                {
                    Vector3 direciton = (position - _transform.position).normalized;

                    _transform.position += direciton * _radius.Value;

                    Debug.Log("Target Outside area");
                }
            }
        }

        private bool IsInside(Vector3 position) => (position - _transform.position).magnitude <= _radius.Value;
    }
}