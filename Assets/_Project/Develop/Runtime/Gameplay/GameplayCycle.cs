using Assets._Project.Develop.Runtime.Gameplay.GameplayServices;
using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Meta.Features;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay
{
    public class GameplayCycle : IDisposable
    {
        private readonly IGameplayInput _input;
        
        private readonly SequenceGameplay _gameplay;
        
        private readonly SceneSwitcherService _sceneSwitcherService;
        private readonly GameplayInputArgs _args;
        
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        
        private readonly WalletService _walletService;
        private readonly PlayerDataProvider _playerDataProvider;
        private readonly WinLossService _winLossService;

        private bool _awaitingConfirm;
        private GameplayResult _result;

        public GameplayCycle(
            IGameplayInput input,
            SceneSwitcherService sceneSwitcherService,
            SequenceGameplay gameplay,
            GameplayInputArgs args,
            WalletService walletService,
            ICoroutinesPerformer coroutinesPerformer,
            PlayerDataProvider playerDataProvider,
            WinLossService winLossService)
        {
            _input = input;
            _gameplay = gameplay;
            _walletService = walletService;

            _args = args;
            _coroutinesPerformer = coroutinesPerformer;

            _sceneSwitcherService = sceneSwitcherService;

            _playerDataProvider = playerDataProvider;
            _winLossService = winLossService;
        }

        public void StartGame()
        {
            _gameplay.Finished += OnGameplayFinished;
            _input.OnConfirm += HandleConfirm;

            _gameplay.Start();
        }

        public void Dispose()
        {
            _gameplay.Finished -= OnGameplayFinished;
            _input.OnConfirm -= HandleConfirm;
        }

        public void Update() => _input.Update();

        private void OnGameplayFinished(GameplayResult result)
        {
            _awaitingConfirm = true;

            _result = result;

            Debug.Log("Для продолжения нажмите SPACE!");
        }

        private void HandleConfirm()
        {
            if (_awaitingConfirm == false)
                return;

            switch (_result)
            {
                case GameplayResult.Win:
                    _walletService.Add(CurrencyType.Gold, _gameplay.Config.WinReward);
                    
                    _coroutinesPerformer.StartPerform(_sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu));

                    _winLossService.AddWins();

                    Debug.Log($"Вы получили {_gameplay.Config.WinReward} монет");
                    break;

                case GameplayResult.Lose:
                    if (_walletService.Enough(CurrencyType.Gold, _gameplay.Config.DefeatPenalty))
                    {
                        _walletService.Spend(CurrencyType.Gold, _gameplay.Config.DefeatPenalty);
                        Debug.Log($"Вы потеряли {_gameplay.Config.DefeatPenalty} монет");
                        _coroutinesPerformer.StartPerform(_sceneSwitcherService.ProcessSwitchTo(Scenes.Gameplay, _args));
                    }
                    else
                    {
                        _walletService.Spend(CurrencyType.Gold, _walletService.GetCurrency(CurrencyType.Gold).Value);
                        Debug.Log("Недостаточно монет для продолжения игры");
                        _coroutinesPerformer.StartPerform(_sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu));
                    }

                    _winLossService.AddLosses();

                    break;
            }

            _coroutinesPerformer.StartPerform(_playerDataProvider.Save());
        }
    }
}