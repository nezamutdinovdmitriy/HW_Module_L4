using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.Features.Enemies;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.Gameplay.States;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.AssetsManagment;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayContextRegistrations
    {
        private static GameplayInputArgs _inputArgs;

        public static void Process(DIContainer container, GameplayInputArgs args)
        {
            Debug.Log("Процесс регистрации сервисов на сцене геймплея");

            _inputArgs = args;

            container.RegisterAsSingle<IInputService>(CreateDesktopInput);

            container.RegisterAsSingle(CreateEntitiesLifeContext);
            container.RegisterAsSingle(CreateMonoEntitiesLifeContext);
            container.RegisterAsSingle(CreateAIBrainContext);
            container.RegisterAsSingle(CreateGameplayStatesContext);

            container.RegisterAsSingle(CreateEntitiesFactory);
            container.RegisterAsSingle(CreateBrainsFactory);
            container.RegisterAsSingle(CreateEnemiesFactory);
            container.RegisterAsSingle(CreateMainHeroFactory);
            container.RegisterAsSingle(CreateStagesFactory);
            container.RegisterAsSingle(CreateGameplayStatesFactory);

            container.RegisterAsSingle(CreateCollidersRegistryService);
            container.RegisterAsSingle(CreateStageProviderService);
            container.RegisterAsSingle(CreatePreperationTriggerService);

            container.RegisterAsSingle(CreateMonoEntitiesFactory).NonLazy();
            container.RegisterAsSingle(CreateScreenToWorldPositionConverter).NonLazy();
            container.RegisterAsSingle(CreateMainHeroHolderService).NonLazy();
        }

        private static GameplayStatesContext CreateGameplayStatesContext(DIContainer container) => new(
            container.Resolve<GameplayStatesFactory>().CreateGameplayStateMachine(_inputArgs));

        private static GameplayStatesFactory CreateGameplayStatesFactory(DIContainer container) => new(container);

        private static MainHeroHolderService CreateMainHeroHolderService(DIContainer container) => new(
            container.Resolve<EntitiesLifeContext>());
        
        private static PreperationTriggerService CreatePreperationTriggerService(DIContainer container) => new(
            container.Resolve<EntitiesFactory>(),
            container.Resolve<EntitiesLifeContext>());

        private static StageProviderService CreateStageProviderService(DIContainer container) => new(
                container.Resolve<ConfigsProviderService>().GetConfig<LevelsListConfig>().GetBy(_inputArgs.LevelNumber),
                container.Resolve<StagesFactory>());

        private static EnemiesFactory CreateEnemiesFactory(DIContainer container) => new(container);

        private static StagesFactory CreateStagesFactory(DIContainer container) => new(container);

        private static MainHeroFactory CreateMainHeroFactory(DIContainer container) => new(container);

        private static ScreenToWorldPositionConverter CreateScreenToWorldPositionConverter(DIContainer container) => new(Camera.main);

        private static DesktopInput CreateDesktopInput(DIContainer container) => new();

        private static AIBrainsContext CreateAIBrainContext(DIContainer container) => new();

        private static BrainsFactory CreateBrainsFactory(DIContainer container) => new(container);

        private static CollidersRegistryService CreateCollidersRegistryService(DIContainer container) => new();

        private static MonoEntitiesFactory CreateMonoEntitiesFactory(DIContainer container) => new(
                container.Resolve<ResourcesAssetsLoader>(),
                container.Resolve<MonoEntitiesLifeContext>(),
                container.Resolve<CollidersRegistryService>());

        private static MonoEntitiesLifeContext CreateMonoEntitiesLifeContext(DIContainer container) => new(container.Resolve<EntitiesLifeContext>());

        private static EntitiesLifeContext CreateEntitiesLifeContext(DIContainer container) => new();

        private static EntitiesFactory CreateEntitiesFactory(DIContainer container) => new(container);
    }
}
