using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Gameplay.Features.PauseFeature;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.UI.Gameplay;
using Assets._Project.Develop.Runtime.UI.Gameplay.AbilitySelectPopup;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.LevelUpFeature
{
    public class DropAbilityOnMainHeroLevelUpService : IInitializable, IDisposable
    {
        private readonly MainHeroHolderService _mainHeroHolder;
        private readonly GameplayPopupService _popupService;
        private readonly ICoroutinesPerformer _performer;

        private readonly Queue<int> _levelUpRequests = new();
        private readonly List<IDisposable> _disposables = new();

        private readonly IPauseService _pauseService;

        private AbilitySelectPopupPresenter _popup;
        private Coroutine _selectAbilityProcess;


        public DropAbilityOnMainHeroLevelUpService(
            MainHeroHolderService mainHeroHolder,
            GameplayPopupService popupService,
            ICoroutinesPerformer performer,
            IPauseService pauseService)
        {
            _mainHeroHolder = mainHeroHolder;
            _popupService = popupService;
            _performer = performer;
            _pauseService = pauseService;
        }

        private bool PopupIsOpened => _popup != null;

        public void Initialize()
            => _disposables.Add(_mainHeroHolder.HeroRegistered.Subscribe(OnMainHeroRegistred));

        public void Dispose()
        {
            foreach (IDisposable disposable in _disposables)
                disposable?.Dispose();
        }

        private void OnMainHeroRegistred(Entity entity)
            => _disposables.Add(entity.Level.Subscribe(OnHeroLevelChanged));

        private void OnHeroLevelChanged(int arg1, int currentLevel)
        {
            _levelUpRequests.Enqueue(currentLevel);

            if (_selectAbilityProcess != null)
                return;

            _selectAbilityProcess = _performer.StartPerform(SelectAbilityProcess());
        }

        private IEnumerator SelectAbilityProcess()
        {
            while (_levelUpRequests.Count > 0)
            {
                int level = _levelUpRequests.Dequeue();

                _pauseService.Pause();

                _popup = _popupService.OpenAbilitySelectPopup(
                    _mainHeroHolder.MainHero,
                    level,
                    () =>
                {
                    _pauseService.Unpause();
                    _popup = null;
                });

                yield return new WaitUntil(() => PopupIsOpened == false);
            }

            _selectAbilityProcess = null;
        }
    }
}