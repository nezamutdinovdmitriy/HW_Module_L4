using Assets._Project.Develop.Runtime.Configs.Gameplay.LevelUp;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.Experience
{
    public class MainHeroExperiencePresenter : IPresenter
    {
        private BarWithText _view;

        private MainHeroHolderService _holder;
        private ExperienceForUpgradeLevelConfig _levelUpconfig;
        private ReactiveVariable<float> _experience;
        private ReactiveVariable<int> _currentLevel;

        private List<IDisposable> _disposables = new();

        public MainHeroExperiencePresenter(
            BarWithText view, 
            MainHeroHolderService holder, 
            ExperienceForUpgradeLevelConfig levelUpconfig)
        {
            _view = view;
            _holder = holder;
            _levelUpconfig = levelUpconfig;
        }

        public void Initialize()
            => _disposables.Add(_holder.HeroRegistered.Subscribe(OnMainHeroRegistered));

        public void Dispose()
        {
            foreach (IDisposable disposable in _disposables)
                disposable?.Dispose();

            _disposables.Clear();
        }

        private void OnMainHeroRegistered(Entity entity)
        {
            _experience = entity.Experience;
            _currentLevel = entity.Level;

            _disposables.Add(_experience.Subscribe(OnCurrentExperienceChanged));
            _disposables.Add(_currentLevel.Subscribe(OnLevelChanged));

            UpdateBarText(_currentLevel.Value);
            UpdateCurrentExperience(_experience.Value);
        }

        private void UpdateCurrentExperience(float value)
            => _view.UpdateValue(value / _levelUpconfig.GetExperienceFor(_currentLevel.Value));

        private void UpdateBarText(int level)
            => _view.UpdateText($"Lv.{level}");

        private void OnLevelChanged(int oldValue, int newValue)
            => UpdateBarText(newValue);

        private void OnCurrentExperienceChanged(float oldValue, float newValue)
            => UpdateCurrentExperience(newValue);
    }
}