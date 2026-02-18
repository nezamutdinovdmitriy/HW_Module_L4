using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Utilities.Optimization;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explosion
{
    public class DealDamageOnExplosionAreaSystem : IInitializableSystem, IDisposable
    {
        private ReactiveEvent _entitiesFilteredEvent;

        private Buffer<Entity> _contactsEntity;
        private ReactiveVariable<float> _damage;

        private IDisposable _entitiesFilteredEventDisposable;

        public void Dispose()
        {
            _entitiesFilteredEventDisposable.Dispose();
        }

        public void OnInit(Entity entity)
        {
            _entitiesFilteredEvent = entity.ExplosionEntitiesFilteredEvent;

            _contactsEntity = entity.AreaContactsEntitiesBuffer;

            _damage = entity.AreaContactDamage;

            _entitiesFilteredEventDisposable = _entitiesFilteredEvent.Subscribe(DealDamage);
        }

        public void DealDamage()
        {
            for (int i = 0; i < _contactsEntity.Count; i++)
            {
                Entity contactEntity = _contactsEntity.Items[i];

                if (contactEntity.HasComponent<TakeDamageRequest>())
                {
                    contactEntity.TakeDamageRequest.Invoke(_damage.Value);
                }
            }
        }
    }
}