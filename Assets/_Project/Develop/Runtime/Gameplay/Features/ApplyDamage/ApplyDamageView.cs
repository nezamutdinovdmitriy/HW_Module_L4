using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage
{
    public class ApplyDamageView : MonoEntityView
    {
        [SerializeField] private Transform _vfxSpawnPoint;
        [SerializeField] private ParticleSystem _vfxPrefab;

        private ReactiveEvent<float> _takeDamageEvent;
        private IDisposable _takeDamageEventDisposable;

        protected override void OnEntityInitialized(Entity entity)
        {
            _takeDamageEvent = entity.TakeDamageEvent;

            _takeDamageEventDisposable = _takeDamageEvent.Subscribe(OnDamaged);
        }

        public override void Cleanup(Entity entity)
        {
            base.Cleanup(entity);

            _takeDamageEventDisposable.Dispose();
        }

        private void OnDamaged(float obj) => Instantiate(_vfxPrefab, _vfxSpawnPoint.position, Quaternion.identity);
    }
}