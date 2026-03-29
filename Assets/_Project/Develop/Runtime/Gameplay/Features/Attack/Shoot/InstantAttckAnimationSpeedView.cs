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
        [SerializeField] private AnimationClip _animationClip;

        private int _attackMultiplierParameterHash;

        private Animator _animator;

        private ReactiveVariable<float> _attackProcessTime;

        private void Awake()
        {
            if (string.IsNullOrEmpty(_attackMultiplierParameterName))
                throw new InvalidOperationException("Animation name is empty!");

            _animator = GetComponent<Animator>();
            _attackMultiplierParameterHash = Animator.StringToHash(_attackMultiplierParameterName);
        }

        protected override void OnEntityInitialized(Entity entity)
        {
            _attackProcessTime = entity.AttackProcessInitialTime;

            _animator.SetFloat(_attackMultiplierParameterHash, _animationClip.length / _attackProcessTime.Value);
        }
    }
}