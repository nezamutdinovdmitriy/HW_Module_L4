using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Optimization;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explosion
{
    public class ExplosionAreaEntitiesFilterSystem : IInitializableSystem, IDisposable
    {
        private ReactiveEvent _delayEndEvent;
        private ReactiveEvent _entitiesFilteredEvent;

        private Buffer<Collider> _contactsColliders;
        private Buffer<Entity> _contactsEntities;

        private readonly CollidersRegistryService _colllidersRegistryService;

        private IDisposable _delayEndEventDisposable;

        public ExplosionAreaEntitiesFilterSystem(CollidersRegistryService colllidersRegistryService)
        {
            _colllidersRegistryService = colllidersRegistryService;
        }

        public void OnInit(Entity entity)
        {
            _delayEndEvent = entity.ExplosionDelayEndEvent;
            _entitiesFilteredEvent = entity.ExplosionEntitiesFilteredEvent;

            _contactsColliders = entity.AreaContactsCollidersBuffer;
            _contactsEntities = entity.AreaContactsEntitiesBuffer;

            _delayEndEventDisposable = _delayEndEvent.Subscribe(OnFiltered);
        }

        public void Dispose()
        {
            _delayEndEventDisposable.Dispose();
        }

        public void OnFiltered()
        {
            _contactsEntities.Count = 0;

            for (int i = 0; i < _contactsColliders.Count; i++)
            {
                Collider collider = _contactsColliders.Items[i];

                Entity contactEntity = _colllidersRegistryService.GetBy(collider);

                if (contactEntity != null)
                {
                    _contactsEntities.Items[_contactsEntities.Count] = contactEntity;
                    _contactsEntities.Count++;
                }
            }

            _entitiesFilteredEvent?.Invoke();
        }
    }
}