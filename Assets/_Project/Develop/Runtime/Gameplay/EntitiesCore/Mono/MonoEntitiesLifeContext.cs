using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono
{
    public class MonoEntitiesLifeContext
    {
        private readonly EntitiesLifeContext _entitiesLifeContext;
        private readonly Dictionary<Entity, MonoEntity> _entityToMono = new();

        public MonoEntitiesLifeContext(EntitiesLifeContext entitiesLifeContext)
        {
            _entitiesLifeContext = entitiesLifeContext;
            _entitiesLifeContext.Removed += Remove;
        }   

        public void Add(Entity entity, MonoEntity monoEntity)
        {
            _entityToMono.Add(entity, monoEntity);
        }

        public void Dispose()
        {
            _entitiesLifeContext.Removed -= Remove;

            foreach (Entity entity in _entityToMono.Keys)
                CleanupFor(entity);

            _entityToMono.Clear();
        }

        private void Remove(Entity entity)
        {
            CleanupFor(entity);

            _entityToMono.Remove(entity);
        }

        private void CleanupFor(Entity entity)
        {
            MonoEntity monoEntity = _entityToMono[entity];
            monoEntity.Cleanup(entity);
            Object.Destroy(monoEntity.gameObject);
        }
    }
}