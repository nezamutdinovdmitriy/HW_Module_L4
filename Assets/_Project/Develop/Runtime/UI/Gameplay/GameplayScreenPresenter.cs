using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay.Experience;
using Assets._Project.Develop.Runtime.UI.Gameplay.HealthDisplay;
using Assets._Project.Develop.Runtime.UI.Gameplay.Stages;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public sealed class GameplayScreenPresenter : IPresenter
    {
        private readonly GameplayScreenView _view;
        private readonly List<IPresenter> _childPresenters = new();
        private readonly GameplayPresentersFactory _presentersFactory;
        private EntitiesHealthDisplayPresenter _entitiesHealthDisplayPresenter;

        public GameplayScreenPresenter(
            GameplayScreenView view,
            GameplayPresentersFactory presentersFactory)
        {
            _view = view;
            _presentersFactory = presentersFactory;
        }

        public void Initialize()
        {
            CreateStageNumber();
            CreateEntitiesHealthDisplayPresenter();
            CreateMainHeroExperiencePresenter();

            foreach (IPresenter presenter in _childPresenters)
                presenter.Initialize();
        }

        public void Dispose()
        {
            foreach (IPresenter presenter in _childPresenters)
                presenter.Dispose();

            _childPresenters.Clear();
        }

        public void LateUpdate()
        {
            _entitiesHealthDisplayPresenter.LateUpdate();
        }

        private void CreateStageNumber()
        {
            StagePresenter presenter = _presentersFactory.CreateStagePresenter(_view.StageNumberView);

            _childPresenters.Add(presenter);
        }

        private void CreateEntitiesHealthDisplayPresenter()
        {
            _entitiesHealthDisplayPresenter = _presentersFactory.CreateEntitiesHealthDisplayPresenter(_view.EntitiesHealthDisplay);

            _childPresenters.Add(_entitiesHealthDisplayPresenter);
        }

        private void CreateMainHeroExperiencePresenter()
        {
            MainHeroExperiencePresenter presenter = _presentersFactory.CreateMainHeroExperiencePresenter(_view.ExperienceBarView);

            _childPresenters.Add(presenter);
        }
    }
}