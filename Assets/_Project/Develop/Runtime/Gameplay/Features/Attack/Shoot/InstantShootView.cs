using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Shoot
{


    public class InstantShootView : MonoEntityView
    {
        [SerializeField] private string _inAttackParameterName;

        private int _inAttackParameterHash;

        private Animator _animator;

        private IReadOnlyVariable<bool> _inAttackProcess;

        private IDisposable _inAttackProcessChangedDisposable;

        private void Awake()
        {
            if (string.IsNullOrEmpty(_inAttackParameterName))
                throw new InvalidOperationException("Animation name is empty!");

            _animator = GetComponent<Animator>();
            _inAttackParameterHash = Animator.StringToHash(_inAttackParameterName);
        }

        protected override void OnEntityInitialized(Entity entity)
        {
            _inAttackProcess = entity.InAttackProcess;

            _inAttackProcessChangedDisposable = _inAttackProcess.Subscribe(AttackProcessChanged);

            UpdateInAttack(_inAttackProcess.Value);
        }

        public override void Cleanup(Entity entity)
        {
            base.Cleanup(entity);

            _inAttackProcessChangedDisposable.Dispose();
        }

        private void UpdateInAttack(bool value) => _animator.SetBool(_inAttackParameterHash, value);

        private void AttackProcessChanged(bool arg1, bool value) => UpdateInAttack(value);
    }
}