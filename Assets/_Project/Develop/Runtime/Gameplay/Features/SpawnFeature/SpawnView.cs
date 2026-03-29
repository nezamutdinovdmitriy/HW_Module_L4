using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.SpawnFeature
{
    [RequireComponent(typeof(Animator))]
    public class SpawnView : MonoEntityView
    {
        [SerializeField] private string _spawnInProcessParameterName;
        [SerializeField] private ParticleSystem _spawnEffectPrefab;
        private int _spawnInProcessParameterHash;

        private Animator _animator;

        private ReactiveVariable<bool> _spawnInProcess;
        private Transform _entityTransform;

        private IDisposable _spawnInProcessChangedDisposable;

        private void Awake()
        {
            if (string.IsNullOrEmpty(_spawnInProcessParameterName))
                throw new InvalidOperationException("Animation name is empty!");

            _animator = GetComponent<Animator>();

            _spawnInProcessParameterHash = Animator.StringToHash(_spawnInProcessParameterName);
        }

        protected override void OnEntityInitialized(Entity entity)
        {
            _spawnInProcess = entity.SpawnInProcess;
            _entityTransform = entity.Transform;

            _spawnInProcessChangedDisposable = _spawnInProcess.Subscribe(OnSpawnProcessChanged);

            UpdateSpawnProcess(_spawnInProcess.Value);
        }

        public override void Cleanup(Entity entity)
        {
            base.Cleanup(entity);

            _spawnInProcessChangedDisposable?.Dispose();
        }

        private void UpdateSpawnProcess(bool value)
        {
            _animator.SetBool(_spawnInProcessParameterHash, value);

            if (value)
                Instantiate(
                    _spawnEffectPrefab, 
                    _entityTransform.position, 
                    _spawnEffectPrefab.transform.rotation,
                    null);
        }

        private void OnSpawnProcessChanged(bool arg1, bool value) => UpdateSpawnProcess(value);
    }
}