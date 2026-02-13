using Assets._Project.Develop.Runtime.Utilities.AssetsManagment;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono
{
    public class MonoEntitiesFactory
    {
        private readonly ResourcesAssetsLoader _resources;
        private readonly MonoEntitiesLifeContext _monoEntitiesLifeContext;

        private CollidersRegistryService _colllidersRegistryService;

        public MonoEntitiesFactory(
            ResourcesAssetsLoader resources,
            MonoEntitiesLifeContext monoEntitiesLifeContext,
            CollidersRegistryService colllidersRegistryService)
        {
            _resources = resources;
            _monoEntitiesLifeContext = monoEntitiesLifeContext;
            _colllidersRegistryService = colllidersRegistryService;
        }


        public MonoEntity Create(Entity entity, Vector3 position, string path)
        {
            MonoEntity prefab = _resources.Load<MonoEntity>(path);

            MonoEntity viewInstance = Object.Instantiate(prefab, position, Quaternion.identity, null);

            viewInstance.Initialize(_colllidersRegistryService);

            viewInstance.Link(entity);

            _monoEntitiesLifeContext.Add(entity, viewInstance);

            return viewInstance;
        }
    }
}