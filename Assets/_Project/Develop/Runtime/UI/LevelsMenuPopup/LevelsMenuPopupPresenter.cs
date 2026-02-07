using Assets._Project.Develop.Runtime.Gameplay;
using Assets._Project.Develop.Runtime.Gameplay.Configs;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.UI.LevelsMenuPopup
{
    public class LevelsMenuPopupPresenter : PopupPresenterBase
    {
        private const string TitleName = "LEVELS";

        private readonly ConfigsProviderService _configProvider;
        private readonly ProjectPresentersFactory _presentersFactory;
        private readonly ViewsFactory _viewsFactory;

        private readonly LevelsMenuPopupView _view;

        private readonly List<LevelTilePresenter> _presenters = new();

        public LevelsMenuPopupPresenter(
            ICoroutinesPerformer coroutinesPerformer,
            ConfigsProviderService configProvider,
            ProjectPresentersFactory presentersFactory,
            ViewsFactory viewsFactory,
            LevelsMenuPopupView view)
            : base(coroutinesPerformer)
        {
            _configProvider = configProvider;
            _presentersFactory = presentersFactory;
            _viewsFactory = viewsFactory;
            _view = view;
        }

        protected override PopupViewBase PopupView => _view;

        public override void Initialize()
        {
            base.Initialize();

            _view.SetTitle(TitleName);

            LevelsListConfigs levelsListConfigs = _configProvider.GetConfig<LevelsListConfigs>();

            foreach(LevelsListConfigs.Configs levelConfig in levelsListConfigs.Levels)
            {
                LevelTileView levelTileView = _viewsFactory.Create<LevelTileView>(ViewIDs.LevelTile);

                _view.LevelTilesListView.Add(levelTileView);

                LevelTilePresenter levelTilePresenter = _presentersFactory.CreateLevelTilePresenter(levelTileView, levelConfig.GameMode);

                levelTilePresenter.Initialize();

                _presenters.Add(levelTilePresenter);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (LevelTilePresenter levelTilePresenter in _presenters)
            {
                _view.LevelTilesListView.Remove(levelTilePresenter.View);
                _viewsFactory.Release(levelTilePresenter.View);

                levelTilePresenter.Dispose();
            }

            _presenters.Clear();
        }

        protected override void OnPreShow()
        {
            base.OnPreShow();

            foreach (LevelTilePresenter presenter in _presenters)
                presenter.Subscribe();
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            foreach (LevelTilePresenter presenter in _presenters)
                presenter.Unsubscribe();
        }
    }
}