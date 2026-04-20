using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Shoot
{
    [RequireComponent(typeof(Animator))]
    public class InstantAttckAnimationSpeedView : MonoEntityView
    {
        [SerializeField] private string _attackMultiplierParameterName;

        private ReactiveVariable<float> _attackProcessInitialTime;
        private ReactiveVariable<float> _attackProcessModifiedTime;

        private int _attackMultiplierParameterHash;

        private Animator _animator;

        private IDisposable _attackProcessTimeChangedDisposable;

        private void Awake()
        {
            if (string.IsNullOrEmpty(_attackMultiplierParameterName))
                throw new InvalidOperationException("Animation name is empty!");

            _animator = GetComponent<Animator>();
            _attackMultiplierParameterHash = Animator.StringToHash(_attackMultiplierParameterName);
        }

        protected override void OnEntityInitialized(Entity entity)
        {
            _attackProcessInitialTime = entity.AttackProcessInitialTime;
            _attackProcessModifiedTime = entity.AttackProcessModifiedTime;

            _attackProcessTimeChangedDisposable = _attackProcessModifiedTime.Subscribe(OnAttackProcessTimeChanged);

            OnAttackProcessTimeChanged(0, _attackProcessModifiedTime.Value);
        }

        public override void Cleanup(Entity entity)
        {
            base.Cleanup(entity);

            _attackProcessTimeChangedDisposable?.Dispose();
        }

        private void OnAttackProcessTimeChanged(float arg1, float currentAttackProcessTime)
            => _animator.SetFloat(
                _attackMultiplierParameterHash,
                _attackProcessInitialTime.Value / currentAttackProcessTime);
    }
}