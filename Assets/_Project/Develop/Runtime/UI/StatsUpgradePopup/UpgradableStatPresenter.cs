using Assets._Project.Develop.Runtime.Configs.Meta.Stats;
using Assets._Project.Develop.Runtime.Configs.Meta.Wallet;
using Assets._Project.Develop.Runtime.Gameplay.Features.StatsFeature;
using Assets._Project.Develop.Runtime.Meta.Features.StatsUpgrade;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Assets._Project.Develop.Runtime.Configs.Meta.Stats.StatsViewConfig;

namespace Assets._Project.Develop.Runtime.UI.StatsUpgradePopup
{
    public class UpgradableStatPresenter : IPresenter
    {
        private UpgradableStatView _view;
        private StatsViewConfig _statsViewConfig;
        private StatsUpgradeService _statsUpgradeService;
        private WalletService _walletService;
        private StatType _statType;
        private CurrencyIconsConfig _currencyIconsConfig;

        private List<IDisposable> _disposables = new();

        public UpgradableStatPresenter(
            UpgradableStatView view,
            StatsViewConfig statsViewConfig,
            StatsUpgradeService statsUpgradeService,
            WalletService walletService,
            StatType statType,
            CurrencyIconsConfig currencyIconsConfig)
        {
            _view = view;
            _statsViewConfig = statsViewConfig;
            _statsUpgradeService = statsUpgradeService;
            _walletService = walletService;
            _statType = statType;
            _currencyIconsConfig = currencyIconsConfig;
        }

        public UpgradableStatView View => _view;

        public void Initialize()
        {
            StatViewConfig statShowData = _statsViewConfig.GetStatViewData(_statType);

            _view.Initialize(statShowData.Name, statShowData.Sprite, GetStatValueText());

            UpdateBuyButtonState();

            _view.BuyButtonView.Clicked += OnBuyButtonClicked;

            IReadOnlyVariable<int> statLevel = _statsUpgradeService.GetStatLevelFor(_statType);
            _disposables.Add(statLevel.Subscribe(OnStatUpgradeLevelChanged));

            IReadOnlyVariable<int> currency = _walletService.GetCurrency(_statsUpgradeService.GetUpgradeCostTypeFor(_statType));
            _disposables.Add(currency.Subscribe(OnWalletChanged));
        }

        public void Dispose()
        {
            _view.BuyButtonView.Clicked -= OnBuyButtonClicked;

            foreach (IDisposable disposable in _disposables)
                disposable?.Dispose();
        }

        private void OnWalletChanged(int arg1, int arg2) => UpdateBuyButtonState();

        private void OnStatUpgradeLevelChanged(int arg1, int arg2) => _view.SetStatValueText(GetStatValueText());

        private void OnBuyButtonClicked()
        {
            if(_statsUpgradeService.TryGetUpgradeCostFor(_statType, out CurrencyTypes currencyType, out int cost))
            {
                if (_walletService.Enough(currencyType, cost))
                {
                    if (_statsUpgradeService.TryUpgradeStat(_statType) == false)
                        throw new InvalidOperationException();

                    _walletService.Spend(currencyType, cost);
                }
                else
                {
                    Debug.Log("Not enought currency!");
                }
            }
            else
            {
                Debug.Log("Already max!");
            }
        }

        private void UpdateBuyButtonState()
        {
            if(_statsUpgradeService.TryGetUpgradeCostFor(_statType, out CurrencyTypes currencyTypes, out int cost))
            {
                _view.BuyButtonView.SetPriceText(cost.ToString());
                _view.BuyButtonView.ShowIcon();
                _view.BuyButtonView.SetIcon(_currencyIconsConfig.GetSpriteFor(currencyTypes));

                if (_walletService.Enough(currencyTypes, cost))
                    _view.BuyButtonView.Unlock();
                else
                    _view.BuyButtonView.Lock();
            }
            else
            {
                _view.BuyButtonView.HideIcon();
                _view.BuyButtonView.Lock();
                _view.BuyButtonView.SetPriceText("MAX");
            }
        }

        private string GetStatValueText()
        {
            float statValue = _statsUpgradeService.GetCurrentStatValueFor(_statType);
            string result = statValue.ToString();

            if (_statsUpgradeService.TryGetStatValueForNextLevel(_statType, out float nextStatValue))
            {
                result += $"<color=green>>{nextStatValue}</color>";
            }

            return result;
        }
    }
}