using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.AssetsManagment;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayContextRegistrations
    {
        public static void Process(DIContainer container, GameplayInputArgs args)
        {
            Debug.Log("Процесс регистрации сервисов на сцене геймплея");

            container.RegisterAsSingle(CreateEntitiesFactory);

            container.RegisterAsSingle(CreateEntitiesLifeContext);
            container.RegisterAsSingle(CreateMonoEntitiesLifeContext);
            container.RegisterAsSingle(CreateCollidersRegistryService);

            container.RegisterAsSingle(CreateMonoEntitiesFactory).NonLazy();
        }

        private static CollidersRegistryService CreateCollidersRegistryService(DIContainer container)
            => new CollidersRegistryService();

        private static MonoEntitiesFactory CreateMonoEntitiesFactory(DIContainer container)
        {
            return new MonoEntitiesFactory(
                container.Resolve<ResourcesAssetsLoader>(),
                container.Resolve<MonoEntitiesLifeContext>(),
                container.Resolve<CollidersRegistryService>());
        }

        private static MonoEntitiesLifeContext CreateMonoEntitiesLifeContext(DIContainer container)
        {
            return new MonoEntitiesLifeContext(
                container.Resolve<EntitiesLifeContext>());
        }

        private static EntitiesLifeContext CreateEntitiesLifeContext(DIContainer container)
            => new();

        private static EntitiesFactory CreateEntitiesFactory(DIContainer container)
            => new(container);
    }
}
