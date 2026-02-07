using Assets._Project.Develop.Runtime.Gameplay.GameplayServices;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features;
using Assets._Project.Develop.Runtime.UI.CommonView;
using Assets._Project.Develop.Runtime.UI.StatsInfo;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenPresenterFactory
    {
        private readonly DIContainer _container;

        public MainMenuScreenPresenterFactory(DIContainer container)
        {
            _container = container;
        }

        public StatsInfoPresenter CreateStatsInfoPresenter(TextView view)
        {
            return new StatsInfoPresenter(
                _container.Resolve<WinLossService>(),
                view);
        }

        public MainMenuScreenPresenter CreateMainMenuScreenPresenter(MainMenuScreenView view)
            => new(
                view,
                _container.Resolve<ProjectPresentersFactory>(),
                _container.Resolve<MainMenuPopupService>(),
                _container.Resolve<StatsResetPurchaseService>());
    }
}