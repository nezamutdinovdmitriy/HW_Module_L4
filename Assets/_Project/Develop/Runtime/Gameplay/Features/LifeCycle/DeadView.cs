using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle
{
    [RequireComponent(typeof(Animator))]
    public class DeadView : MonoEntityView
    {
        [SerializeField] private string _isDeadParameterName;

        private int _isDeadParameterHash;

        private Animator _animator;

        private IReadOnlyVariable<bool> _isDead;

        private IDisposable _isDeadChangedDisposable;

        private void Awake()
        {
            if (string.IsNullOrEmpty(_isDeadParameterName))
                throw new InvalidOperationException("Animation name is empty!");

            _isDeadParameterHash = Animator.StringToHash(_isDeadParameterName);
            _animator = GetComponent<Animator>();
        }

        protected override void OnEntityInitialized(Entity entity)
        {
            _isDead = entity.IsDead;

            _isDeadChangedDisposable = _isDead.Subscribe(OnIsDeadChanged);

            UpdateIsDead(_isDead.Value);
        }

        private void UpdateIsDead(bool value) => _animator.SetBool(_isDeadParameterHash, value);

        private void OnIsDeadChanged(bool arg1, bool isDead) => UpdateIsDead(isDead);

        public override void Cleanup(Entity entity)
        {
            base.Cleanup(entity);

            _isDeadChangedDisposable?.Dispose();
        }
    }
}