using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack
{
    public class CurrentTargetView : MonoEntityView
    {
        [SerializeField] private ParticleSystem _backlightPrefab;
        
        private ParticleSystem _backlight;

        private ReactiveVariable<Entity> _currentTarget;
        private Transform _currentTargetTransform;

        private IDisposable _currentTargetChangedDisposable;

        protected override void OnEntityInitialized(Entity entity)
        {
            _currentTarget = entity.CurrentTarget;

            _backlight = Instantiate(_backlightPrefab);

            _currentTargetChangedDisposable = _currentTarget.Subscribe(OnCurrentTargetChanged);

            UpdateBlacklightFor(_currentTarget.Value);
        }

        public override void Cleanup(Entity entity)
        {
            base.Cleanup(entity);

            _currentTargetChangedDisposable?.Dispose();
            Destroy(_backlight.gameObject);
        }

        private void LateUpdate()
        {
            if (_currentTargetTransform == null)
                return;

            _backlight.transform.position = _currentTargetTransform.position;
        }

        private void UpdateBlacklightFor(Entity currentTarget)
        {
            if(currentTarget == null)
            {
                _backlight.gameObject.SetActive(false);
                _currentTargetTransform = null;
                return;
            }

            _backlight.gameObject.SetActive(true);
            _currentTargetTransform = currentTarget.Transform;
        }

        private void OnCurrentTargetChanged(Entity entity1, Entity currentTarget) => UpdateBlacklightFor(currentTarget);
    }
}