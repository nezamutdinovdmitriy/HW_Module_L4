using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay.Experience;
using Assets._Project.Develop.Runtime.UI.Gameplay.HealthDisplay;
using Assets._Project.Develop.Runtime.UI.Gameplay.Stages;
using Assets._Project.Develop.Runtime.UI.Wallet;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public sealed class GameplayScreenPresenter : IPresenter
    {
        private readonly GameplayScreenView _view;
        private readonly List<IPresenter> _childPresenters = new();
        private readonly GameplayPresentersFactory _presentersFactory;
        private EntitiesHealthDisplayPresenter _entitiesHealthDisplayPresenter;

        private ProjectPresentersFactory _projectPresentersFactory;
        private MainHeroHolderService _mainHeroHolderService;
        private IDisposable _mainHeroHolderServiceDisposable;
        private CurrencyPresenter _currencyPresenter;

        public GameplayScreenPresenter(
            GameplayScreenView view,
            GameplayPresentersFactory presentersFactory,
            ProjectPresentersFactory projectPresentersFactory,
            MainHeroHolderService mainHeroHolderService)
        {
            _view = view;
            _presentersFactory = presentersFactory;
            _projectPresentersFactory = projectPresentersFactory;
            _mainHeroHolderService = mainHeroHolderService;
        }

        public void Initialize()
        {
            CreateStageNumber();
            CreateEntitiesHealthDisplayPresenter();
            CreateMainHeroExperiencePresenter();

            _mainHeroHolderServiceDisposable = _mainHeroHolderService.HeroRegistered.Subscribe(OnHeroRegistred);

            foreach (IPresenter presenter in _childPresenters)
                presenter.Initialize();
        }

        public void Dispose()
        {
            _mainHeroHolderServiceDisposable?.Dispose();
            _currencyPresenter?.Dispose();

            foreach (IPresenter presenter in _childPresenters)
                presenter.Dispose();

            _childPresenters.Clear();
        }

        public void LateUpdate()
        {
            _entitiesHealthDisplayPresenter.LateUpdate();
        }

        private void OnHeroRegistred(Entity entity)
        {
            Debug.Log("CURRENCY_PRESENTER+++");
            _currencyPresenter = _projectPresentersFactory.CreateCurrencyPresenter(_view.CoinsView, entity.Coins, CurrencyTypes.Gold);
            _currencyPresenter.Initialize();
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