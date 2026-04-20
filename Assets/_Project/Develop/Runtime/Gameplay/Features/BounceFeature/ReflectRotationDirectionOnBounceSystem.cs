using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.BounceFeature
{
    public class ReflectRotationDirectionOnBounceSystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveVariable<Vector3> _rotationDirection;
        private ReactiveEvent<RaycastHit> _bounceEvent;

        private IDisposable _bounceEventDisposable;

        public void OnInit(Entity entity)
        {
            _rotationDirection = entity.RotationDirection;
            _bounceEvent = entity.BounceEvent;

            _bounceEventDisposable = _bounceEvent.Subscribe(OnBounce);
        }

        public void OnDispose() => _bounceEventDisposable?.Dispose();

        private void OnBounce(RaycastHit hit)
            => _rotationDirection.Value = Vector3.Reflect(_rotationDirection.Value, hit.normal);
    }
}