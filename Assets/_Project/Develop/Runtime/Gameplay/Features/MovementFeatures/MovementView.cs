using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeatures
{
    [RequireComponent(typeof(Animator))]
    public class MovementView : MonoEntityView
    {
        [SerializeField] private string _isMovingParameterName;
        
        private int _isMovingParameterHash;
        
        private Animator _animator;

        private IReadOnlyVariable<bool> _isMoving;

        private IDisposable _isMovingChangedDisposable;

        private void Awake()
        {
            if (string.IsNullOrEmpty(_isMovingParameterName))
                throw new InvalidOperationException("Animation name is empty!");

            _animator = GetComponent<Animator>();
            _isMovingParameterHash = Animator.StringToHash(_isMovingParameterName);
        }

        protected override void OnEntityInitialized(Entity entity)
        {
            _isMoving = entity.IsMoving;

            _isMovingChangedDisposable = _isMoving.Subscribe(OnIsMovingChanged);

            UpdateIsMoving(_isMoving.Value);
        }

        public override void Cleanup(Entity entity)
        {
            base.Cleanup(entity);

            _isMovingChangedDisposable?.Dispose();
        }

        private void UpdateIsMoving(bool value) => _animator.SetBool(_isMovingParameterHash, value);

        private void OnIsMovingChanged(bool args, bool isMoving) => UpdateIsMoving(isMoving);
    }
}