using Assets._Project.Develop.Runtime.Gameplay.Configs;
using Assets._Project.Develop.Runtime.Gameplay.GameplayServices;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay;
using Assets._Project.Develop.Runtime.UI.MainMenu;
using Assets._Project.Develop.Runtime.Utilities.AssetsManagment;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayContextRegistrations
    {
        private static GameplayInputArgs _args;

        public static void Process(DIContainer container, GameplayInputArgs args)
        {
            _args = args;

            Debug.Log("Процесс регистрации сервисов на сцене геймплея!");

            container.RegisterAsSingle(CreateSequenceGeneratorService);
            container.RegisterAsSingle(CreateGameplayInputService);

            container.RegisterAsSingle(CreateGameplayCycle);
            container.RegisterAsSingle(CreateSequenceGameplay);
            container.RegisterAsSingle(CreateGameplayPresentersFactory);

            container.RegisterAsSingle(CreateGameplayUIRoot).NonLazy();
            container.RegisterAsSingle(CreateGameplaySequencePresenter).NonLazy();
        }

        private static GameplayPresentersFactory CreateGameplayPresentersFactory(DIContainer c)
            => new GameplayPresentersFactory(c);

        private static GameplaySequencePresenter CreateGameplaySequencePresenter(DIContainer c)
        {
            GameplayUIRoot root = c.Resolve<GameplayUIRoot>();

            GameplayView view = c.Resolve<ViewsFactory>().Create<GameplayView>(ViewIDs.GameplaySequensView, root.HUDLayer);

            GameplaySequencePresenter presenter = c.Resolve<GameplayPresentersFactory>().CreateGameplaySequencePresenter(view);

            presenter.Initialize();

            return presenter;
        }

        private static GameplayCycle CreateGameplayCycle(DIContainer c)
        {
            IGameplayInput input = c.Resolve<GameplayDesktopInputHandler>();
            SequenceGameplay sequenceGameplay = c.Resolve<SequenceGameplay>();
            WalletService walletService = c.Resolve<WalletService>();
            SceneSwitcherService sceneSwitcherService = c.Resolve<SceneSwitcherService>();
            ICoroutinesPerformer coroutinesPerformer = c.Resolve<ICoroutinesPerformer>();
            PlayerDataProvider playerDataProvider = c.Resolve<PlayerDataProvider>();
            WinLossService winLossService = c.Resolve<WinLossService>();

            return new GameplayCycle(input, sceneSwitcherService, sequenceGameplay, _args, walletService, coroutinesPerformer, playerDataProvider, winLossService);
        }

        private static GameplayUIRoot CreateGameplayUIRoot(DIContainer c)
        {
            return Object.Instantiate(c.Resolve<ResourcesAssetsLoader>().Load<GameplayUIRoot>("UI/Gameplay/GameplayUIRoot"));
        }

        private static SequenceGeneratorService CreateSequenceGeneratorService(DIContainer c)
        {
            LevelConfig levelConfig = c.Resolve<ConfigsProviderService>().GetConfig<LevelsListConfigs>().GetLevelConfigBy(_args.GameMode);

            return new SequenceGeneratorService(levelConfig);
        }

        private static GameplayDesktopInputHandler CreateGameplayInputService(DIContainer c)
        {
            return new GameplayDesktopInputHandler();
        }

        private static SequenceGameplay CreateSequenceGameplay(DIContainer c)
        {
            IGameplayInput input = c.Resolve<GameplayDesktopInputHandler>();
            SequenceGeneratorService sequenceGenerator = c.Resolve<SequenceGeneratorService>();

            return new SequenceGameplay(input, sequenceGenerator);
        }
    }
}