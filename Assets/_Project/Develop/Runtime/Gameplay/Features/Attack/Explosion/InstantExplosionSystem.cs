using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explosion
{
    public class InstantExplosionSystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveEvent _exploded;
        private ReactiveEvent _attackDelayEndEvent;

        private ReactiveVariable<float> _damage;


        private IDisposable _attackDelayEndDisposable;

        public void OnInit(Entity entity)
        {
            _exploded = entity.ExplosionAreaEvent;
            _attackDelayEndEvent = entity.AttackDelayEndEvent;

            _damage = entity.ExplosionAreaDamage;

            _attackDelayEndDisposable = _attackDelayEndEvent.Subscribe(OnExploded);
        }

        public void OnDispose()
        {
            _attackDelayEndDisposable.Dispose();
        }

        private void OnExploded()
        {
            _exploded?.Invoke();

            Debug.Log("Boom");
        }
    }
}