using Assets._Project.Develop.Runtime.Configs.Gameplay.LevelUp;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.LevelUpFeature
{
    public class LevelUpSystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveVariable<float> _experience;
        private ReactiveVariable<int> _level;

        private ExperienceForUpgradeLevelConfig _config;

        private IDisposable _disposable;

        public LevelUpSystem(ExperienceForUpgradeLevelConfig config)
            => _config = config;

        public float CurrentLimitForExperience => _config.GetExperienceFor(_level.Value);

        public void OnInit(Entity entity)
        {
            _experience = entity.Experience;
            _level = entity.Level;

            _disposable = _experience.Subscribe(OnExperienceChanged);
        }

        public void OnDispose() => _disposable?.Dispose();

        private void OnExperienceChanged(float arg1, float newExperience)
        {
            while (newExperience >= CurrentLimitForExperience
            && _level.Value < _config.MaxLevel)
            {
                newExperience -= CurrentLimitForExperience;
                _level.Value++;
            }

            _experience.Value = newExperience;
        }

    }
}