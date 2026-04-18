using Assets._Project.Develop.Runtime.Configs.Gameplay.Abilities;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Loot;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesDroppingFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.Features.Enemies;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.LevelUpFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.LootFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Gameplay.Features.PauseFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.Gameplay.States;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.UI;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay;
using Assets._Project.Develop.Runtime.Utilities.AssetsManagment;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
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

            container.RegisterAsSingle(CreateGameplayUIRoot).NonLazy();
            container.RegisterAsSingle(CreateGameplayScreenPresenter).NonLazy();
            container.RegisterAsSingle(CreateGameplayPresentersFactory);
            container.RegisterAsSingle(CreateGameplayPopupService);

            container.RegisterAsSingle(CreateAbilityFactory);

            container.RegisterAsSingle(CreateAbilityDroppingRulesService);
            container.RegisterAsSingle(CreateAbilityDropService);

            container.RegisterAsSingle<IPauseService>(CreateTimeScalePauseService);

            container.RegisterAsSingle(CreateDropAbilityOnMainHeroLevelUpService).NonLazy();

            container.RegisterAsSingle(CreateLootFactory);
            container.RegisterAsSingle(CreateDropLootService);
            container.RegisterAsSingle(CreateLootPullingService).NonLazy();
        }

        private static LootPullingService CreateLootPullingService(DIContainer container)
            => new(container.Resolve<EntitiesLifeContext>());

        private static DropLootService CreateDropLootService(DIContainer container)
            => new(
                container.Resolve<ConfigsProviderService>().GetConfig<LootListConfig>(),
                container.Resolve<LootFactory>());

        private static LootFactory CreateLootFactory(DIContainer container)
            => new(container);

        private static TimeScalePauseService CreateTimeScalePauseService(DIContainer container)
            => new();

        private static DropAbilityOnMainHeroLevelUpService CreateDropAbilityOnMainHeroLevelUpService(DIContainer container)
            => new(
                container.Resolve<MainHeroHolderService>(),
                container.Resolve<GameplayPopupService>(),
                container.Resolve<ICoroutinesPerformer>(),
                container.Resolve<IPauseService>());

        private static AbilityDroppingRulesService CreateAbilityDroppingRulesService(DIContainer container)
            => new();

        private static AbilityDropService CreateAbilityDropService(DIContainer container)
            => new(container.Resolve<AbilityDroppingRulesService>(),
                container.Resolve<ConfigsProviderService>().GetConfig<AbilitiesConfigsContainer>());

        private static AbilityFactory CreateAbilityFactory(DIContainer container)
            => new(container);

        private static GameplayPresentersFactory CreateGameplayPresentersFactory(DIContainer container)
            => new(container, _inputArgs);

        private static GameplayScreenPresenter CreateGameplayScreenPresenter(DIContainer container)
        {
            GameplayUIRoot root = container.Resolve<GameplayUIRoot>();

            GameplayScreenView view = container.Resolve<ViewsFactory>().Create<GameplayScreenView>(ViewIDs.GameplayScreen, root.HUDLayer);

            GameplayScreenPresenter presenter = container.Resolve<GameplayPresentersFactory>().CreateGameplayScreenPresenter(view);

            return presenter;
        }

        private static GameplayUIRoot CreateGameplayUIRoot(DIContainer container)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = container.Resolve<ResourcesAssetsLoader>();

            GameplayUIRoot gameplayUIRootPrefab = resourcesAssetsLoader.Load<GameplayUIRoot>("UI/Gameplay/GameplayUIRoot");

            return Object.Instantiate(gameplayUIRootPrefab);
        }

        private static GameplayPopupService CreateGameplayPopupService(DIContainer container)
        {
            return new(container.Resolve<ViewsFactory>(),
                container.Resolve<ProjectPresentersFactory>(),
                container.Resolve<GameplayUIRoot>(),
                container.Resolve<GameplayPresentersFactory>());
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
