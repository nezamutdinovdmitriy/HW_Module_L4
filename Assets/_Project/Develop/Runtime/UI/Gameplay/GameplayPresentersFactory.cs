using Assets._Project.Develop.Runtime.Configs.Gameplay.Abilities;
using Assets._Project.Develop.Runtime.Configs.Gameplay.LevelUp;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesDroppingFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay.AbilitySelectPopup;
using Assets._Project.Develop.Runtime.UI.Gameplay.Experience;
using Assets._Project.Develop.Runtime.UI.Gameplay.HealthDisplay;
using Assets._Project.Develop.Runtime.UI.Gameplay.ResultsPopups;
using Assets._Project.Develop.Runtime.UI.Gameplay.Stages;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public sealed class GameplayPresentersFactory
    {
        private readonly DIContainer _container;
        private readonly GameplayInputArgs _gameplayInputArgs;

        public GameplayPresentersFactory(DIContainer container, GameplayInputArgs gameplayInputArgs)
        {
            _container = container;
            _gameplayInputArgs = gameplayInputArgs;
        }

        public MainHeroExperiencePresenter CreateMainHeroExperiencePresenter(BarWithText view)
            => new(
                view,
                _container.Resolve<MainHeroHolderService>(),
                _container.Resolve<ConfigsProviderService>().GetConfig<ExperienceForUpgradeLevelConfig>());

        public AbilitySelectPopupPresenter CreateAbilitySelectPopupPresenter(
            AbilitySelectPopupView view,
            Entity entity,
            int level)
            => new(
                _container.Resolve<ICoroutinesPerformer>(),
                view,
                entity,
                this,
                _container.Resolve<AbilityDropService>(),
                _container.Resolve<ViewsFactory>(),
                level);

        public SelectableAbilityPresenter CreateSelectableAbilityPresenter(
            AbilityConfig config,
            SelectableAbilityView view,
            Entity entity)
            => new(config, _container.Resolve<AbilityFactory>(), view, entity);

        public WinPopupPresenter CreateWinPopupPresenter(WinPopupView view)
            => new(
                view,
                _container.Resolve<SceneSwitcherService>(),
                _container.Resolve<ICoroutinesPerformer>());

        public DefeatPopupPresenter CreateDefeatPopupPresenter(DefeatPopupView view)
            => new(
                _container.Resolve<ICoroutinesPerformer>(),
                view,
                _container.Resolve<SceneSwitcherService>(),
                _gameplayInputArgs);

        public GameplayScreenPresenter CreateGameplayScreenPresenter(GameplayScreenView view)
            => new(
                view, 
                _container.Resolve<GameplayPresentersFactory>(),
                _container.Resolve<ProjectPresentersFactory>(),
                _container.Resolve<MainHeroHolderService>());

        public StagePresenter CreateStagePresenter(IconTextView view)
            => new(view, _container.Resolve<StageProviderService>());

        public EntityHealthPresenter CreateEntityHealthPresenter(Entity entity, BarWithText view)
            => new(view, entity);

        public EntitiesHealthDisplayPresenter CreateEntitiesHealthDisplayPresenter(EntitiesHealthDisplay view)
            => new(
                view,
                _container.Resolve<EntitiesLifeContext>(),
                this,
                _container.Resolve<ViewsFactory>());
    }
}