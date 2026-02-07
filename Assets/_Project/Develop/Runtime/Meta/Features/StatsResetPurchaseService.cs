using Assets._Project.Develop.Runtime.Gameplay.GameplayServices;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Features
{
    public class StatsResetPurchaseService
    {
        private readonly WalletService _walletService;
        private readonly WinLossService _winLossService;
        private readonly PlayerDataProvider _dataProvider;
        private readonly ICoroutinesPerformer _coroutinesPerformer;

        private readonly int _price;

        public StatsResetPurchaseService
            (WalletService walletService, 
            WinLossService winLossService, 
            PlayerDataProvider dataProvider, 
            ICoroutinesPerformer coroutinesPerformer,
            int config)
        {
            _walletService = walletService;
            _winLossService = winLossService;
            _dataProvider = dataProvider;
            _coroutinesPerformer = coroutinesPerformer;
            _price = config;
        }

        public void Reset()
        {
            if(_walletService.Enough(CurrencyType.Gold, _price))
            {
                _walletService.Spend(CurrencyType.Gold, _price);

                _winLossService.Reset();

                Debug.Log("Статистика сброшена");

                _coroutinesPerformer.StartPerform(_dataProvider.Save());
            }
            else
            {
                Debug.Log("Не достаточно монет для сброса статистики");
            }
        }
    }
}