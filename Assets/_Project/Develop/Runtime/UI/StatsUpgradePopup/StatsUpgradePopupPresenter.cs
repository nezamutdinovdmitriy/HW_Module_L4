using Assets._Project.Develop.Runtime.Gameplay.Features.StatsFeature;
using Assets._Project.Develop.Runtime.Meta.Features.StatsUpgrade;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Wallet;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.StatsUpgradePopup
{
    public class StatsUpgradePopupPresenter : PopupPresenterBase
    {
        private readonly StatsUpgradePopupView _view;
        private readonly ViewsFactory _viewFactory;
        private readonly ProjectPresentersFactory _projectPresentersFactory;
        private readonly StatsUpgradeService _statsUpgradeService;

        private List<UpgradableStatPresenter> _upgradableStatPresenters = new();
        private WalletPresenter _walletPresenter;

        public StatsUpgradePopupPresenter(
            ICoroutinesPerformer coroutinesPerformer,
            StatsUpgradePopupView view,
            ViewsFactory viewFactory, 
            ProjectPresentersFactory projectPresentersFactory,
            StatsUpgradeService statsUpgradeService)
            : base(coroutinesPerformer)
        {
            _view = view;
            _viewFactory = viewFactory;
            _projectPresentersFactory = projectPresentersFactory;
            _statsUpgradeService = statsUpgradeService;
        }

        protected override PopupViewBase PopupView => _view;

        public override void Initialize()
        {
            base.Initialize();

            _view.SetTitle("UPGRADE YOUR STATS");

            _walletPresenter = _projectPresentersFactory.CreateWalletPresenter(_view.CurrencyListView);
            _walletPresenter.Initialize();

            foreach (StatType statType in _statsUpgradeService.AvailableStats)
            {
                UpgradableStatView upgradableStatView = _viewFactory.Create<UpgradableStatView>(ViewIDs.UpgradableStatView);
                _view.UpgradableStatListView.Add(upgradableStatView);

                UpgradableStatPresenter upgradableStatPresenter = _projectPresentersFactory.CreateUpgradableStatPresenter(upgradableStatView, statType);
                _upgradableStatPresenters.Add(upgradableStatPresenter);
                upgradableStatPresenter.Initialize();
            }
        }

        protected override void OnPreHide()
        {
            base.OnPreHide();

            foreach (UpgradableStatPresenter presenter in _upgradableStatPresenters)
                presenter?.Dispose();
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach(UpgradableStatPresenter presenter in _upgradableStatPresenters)
            {
                presenter?.Dispose();
                _view.UpgradableStatListView.Remove(presenter.View);
                _viewFactory.Release(presenter.View);
            }

            _upgradableStatPresenters.Clear();

            _walletPresenter?.Dispose();
        }
    }
}